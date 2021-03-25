using System;
using System.Security.Claims;
using System.Text.Json;
using System.Threading.Tasks;
using Dfe.Rscd.Web.Application.Security.DTO;
using Microsoft.Extensions.Logging;
using static Dfe.Rscd.Web.Application.Security.Constants;

namespace Dfe.Rscd.Web.Application.Security
{
    public class StubUserInfoHelper : UserInfoHelperBase, IUserInfoHelper
    {
        private readonly ILogger<DsiUserInfoHelper> _logger;

        public StubUserInfoHelper(
            ILogger<DsiUserInfoHelper> logger)
        {
            _logger = logger;
        }

        public Task<ClaimsPrincipal> HydrateUserClaimsAsync(ClaimsPrincipal principal)
        {
            var userId = principal.FindFirst(Claims.Subject).Value;
            var organisation = JsonSerializer.Deserialize<Organisation>(principal.FindFirst(Claims.Organisation).Value);

            var roleName = GetRoleFromOrgClaim(organisation, userId);

            var newClaims = BuildAdditionalClaims(userId, principal.Claims, organisation, roleName);

            _logger.LogInformation("Log in event - User ID: {userId}, Role: {roleName}", userId, roleName);

            return Task.FromResult(new ClaimsPrincipal(new ClaimsIdentity(newClaims, "Dfe Sign In")));
        }

        private string GetRoleFromOrgClaim(Organisation org, string userId)
        {
            if (org.category?.id == "001")
            {
                return RoleNames.EstablishmentUser;
            }

            if (org.category?.id == "002")
            {
                if (org.establishmentNumber == "001")
                {
                    return RoleNames.DfEUser;
                }

                return RoleNames.LocalAuthorityUser;
            }

            throw new InvalidOperationException($"User role could not be determined from organisation claim for user {userId}");
        }
    }
}
