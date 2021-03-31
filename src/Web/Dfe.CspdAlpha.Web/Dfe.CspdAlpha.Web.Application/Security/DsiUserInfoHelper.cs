using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Net.Http;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.IdentityModel.Tokens;
using System.Text.Json;
using Newtonsoft.Json.Linq;
using Microsoft.Extensions.Options;
using Dfe.Rscd.Web.Application.Security.DTO;
using Microsoft.Extensions.Logging;

namespace Dfe.Rscd.Web.Application.Security
{
    public class DsiUserInfoHelper : UserInfoHelperBase, IUserInfoHelper
    {
        private readonly DfeSignInSettings _settings;
        private readonly HttpClient _httpClient;
        private readonly ILogger<DsiUserInfoHelper> _logger;

        public DsiUserInfoHelper(
            IOptions<DfeSignInSettings> settings,
            IHttpClientFactory httpClientFactory,
            ILogger<DsiUserInfoHelper> logger)
        {
            _settings = settings.Value;
            _httpClient = httpClientFactory.CreateClient("DfeSignIn");
            _logger = logger;
        }

        public static string CreateApiToken(DfeSignInSettings settings)
        {
            var secretKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(settings.ApiSecret));
            var signinCredentials = new SigningCredentials(secretKey, SecurityAlgorithms.HmacSha256);
            var tokenOptions = new JwtSecurityToken(
                issuer: settings.Issuer,
                audience: settings.Audience,
                signingCredentials: signinCredentials
            );

            return new JwtSecurityTokenHandler().WriteToken(tokenOptions);
        }

        public async Task<ClaimsPrincipal> HydrateUserClaimsAsync(ClaimsPrincipal principal)
        {
            var userId = principal.FindFirst(Constants.Claims.Subject).Value;
            var organisationClaim = JObject.Parse(principal.FindFirst(Constants.Claims.Organisation).Value);
            var organisationId = organisationClaim["id"].ToString();

            var orgTask = GetOrganisation(organisationId, userId);
            var userRolesTask = GetUserRoles(organisationId, userId);

            await Task.WhenAll(orgTask, userRolesTask);

            var organisation = orgTask.Result;
            var userRoles = userRolesTask.Result;

            if (userRoles.Count == 0)
            {
                throw new InvalidOperationException($"No roles found for user ID {userId}");
            }

            if (userRoles.Count > 1)
            {
                throw new NotSupportedException(
                    $"More than one role returned for user ID: {userId} " +
                    $"(Roles returned: '{string.Join(", ", userRoles)}').");
            }

            var roleName = userRoles.Single();

            var newClaims = BuildAdditionalClaims(userId, principal.Claims, organisation, roleName);

            _logger.LogInformation("Log in event - User ID: {userId}, Role: {roleName}", userId, roleName);

            return new ClaimsPrincipal(new ClaimsIdentity(newClaims, "Dfe Sign In"));
        }

        private async Task<Organisation> GetOrganisation(string organisationId, string userId)
        {
            var endpoint = $"/users/{userId}/organisations";

            var response = await _httpClient.GetAsync(endpoint);
            response.EnsureSuccessStatusCode();

            var responseJson = await response.Content.ReadAsStringAsync();

            return JsonSerializer.Deserialize<Organisation[]>(responseJson).Single(org => org.id == organisationId);
        }

        private async Task<IReadOnlyCollection<string>> GetUserRoles(string organisationId, string userId)
        {
            var endpoint = $"/services/{_settings.ServiceId}/organisations/{organisationId}/users/{userId}";

            var response = await _httpClient.GetAsync(endpoint);
            response.EnsureSuccessStatusCode();

            var responseJson = await response.Content.ReadAsStringAsync();

            return JObject.Parse(responseJson)
                .SelectToken("roles")
                .Select(role => role.SelectToken("name").ToString())
                .ToList();
        }
    }
}
