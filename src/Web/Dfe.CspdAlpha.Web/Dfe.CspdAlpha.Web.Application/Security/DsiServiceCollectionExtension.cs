using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;

namespace Dfe.Rscd.Web.Application.Security
{
    public static class DsiServiceCollectionExtension
    {
        public static void AddDfeSignIn(this IServiceCollection services, DfeSignInSettings settings)
        {
            if (settings.UseStubIdp)
            {
                services.AddSingleton<IUserInfoHelper, StubUserInfoHelper>();
            }
            else
            {
                services.AddSingleton<IUserInfoHelper, DsiUserInfoHelper>();

                services.AddHttpClient("DfeSignIn", client =>
                {
                    client.BaseAddress = new Uri(settings.ApiBaseUri);
                    client.DefaultRequestHeaders.Add(
                        "Authorization", $"Bearer {DsiUserInfoHelper.CreateApiToken(settings)}");
                });
            }

            services.AddScoped<UserInfo>((serviceProvider) =>
            {
                var userInfoHelper = serviceProvider.GetRequiredService<IUserInfoHelper>();
                var httpContextAccessor = serviceProvider.GetRequiredService<IHttpContextAccessor>();

                return userInfoHelper.MapPrincipalToUserInfo(httpContextAccessor.HttpContext.User);
            });

            var overallSessionTimeout = TimeSpan.FromMinutes(20);

            services.AddAuthentication(options =>
            {
                options.DefaultSignInScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                options.DefaultAuthenticateScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = OpenIdConnectDefaults.AuthenticationScheme;
            })
            .AddCookie(options =>
            {
                options.Cookie.Name = "rscd-auth-cookie";
                options.Cookie.SecurePolicy = CookieSecurePolicy.Always;
                options.SlidingExpiration = true;
                options.ExpireTimeSpan = overallSessionTimeout;
                options.LoginPath = "/Account/Login/";
            })
            .AddOpenIdConnect(options =>
            {
                options.SignInScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                options.MetadataAddress = settings.MetadataAddress;
                options.CallbackPath = settings.CallbackPath;
                options.SignedOutCallbackPath = settings.SignedOutCallbackPath;
                options.ClientId = settings.ClientId;
                options.ClientSecret = settings.ClientSecret;
                options.ResponseType = OpenIdConnectResponseType.Code;

                options.Scope.Clear();
                options.Scope.Add("openid");
                options.Scope.Add("email");
                options.Scope.Add("profile");

                // Retrieving the organisation claim from DSI results in a large ID token
                // payload that causes a 431 error when instructing DSI to sign out the user. Instead
                // we must only request the org ID and then retrieve the org details from the DSI API.
                // The 431 error does not occur with our stub IdP, so we can simply request the whole
                // organisation claim.
                if (settings.UseStubIdp)
                {
                    options.Scope.Add("organisation");
                }
                else
                {
                    options.Scope.Add("organisationid");
                }

                options.SaveTokens = true;
                options.GetClaimsFromUserInfoEndpoint = true;
                options.SecurityTokenValidator = new JwtSecurityTokenHandler()
                {
                    // Remove default mapping to ensure we receive unmodified claims.
                    InboundClaimTypeMap = new Dictionary<string, string>(),
                    TokenLifetimeInMinutes = (int)overallSessionTimeout.TotalMinutes,
                    SetDefaultTimesOnTokenCreation = true
                };

                // When we expire the session, ensure user is prompted to sign in again at DfE Sign In
                options.MaxAge = overallSessionTimeout;

                options.ProtocolValidator = new OpenIdConnectProtocolValidator
                {
                    RequireSub = true,
                    RequireStateValidation = false,
                    NonceLifetime = overallSessionTimeout
                };

                options.Events = new OpenIdConnectEvents
                {
                    OnTokenValidated = async ctx =>
                    {
                        ctx.Properties.IsPersistent = true;
                        ctx.Properties.ExpiresUtc = DateTime.UtcNow.Add(overallSessionTimeout);

                        var userInfoHelper = ctx.HttpContext.RequestServices.GetService<IUserInfoHelper>();

                        ctx.Principal = await userInfoHelper.HydrateUserClaimsAsync(ctx.Principal);
                    }
                };

                options.DisableTelemetry = true;
            });
        }
    }
}
