using Dfe.Rscd.Web.Application.Security.DTO;
using System;
using System.Collections.Generic;
using System.Security.Claims;

namespace Dfe.Rscd.Web.Application.Security
{
    public abstract class UserInfoHelperBase
    {
        public UserInfo MapPrincipalToUserInfo(ClaimsPrincipal principal)
        {
            return new UserInfo()
            {
                Email = principal.FindFirst(Constants.Claims.Email).Value,
                FirstName = principal.FindFirst(Constants.Claims.GivenName).Value,
                LastName = principal.FindFirst(Constants.Claims.FamilyName).Value,
                Role = RoleMapper.MapToEnum(principal.FindFirst(Constants.Claims.Role).Value).Value,
                UserId = principal.FindFirst(Constants.Claims.Subject).Value,
                Urn = principal.FindFirst(Constants.Claims.Urn)?.Value,
                LaEstab = principal.FindFirst(Constants.Claims.Laestab)?.Value
            };
        }

        protected IEnumerable<Claim> BuildAdditionalClaims(
            string userId, IEnumerable<Claim> initialClaims, Organisation org, string roleName)
        {
            var role = RoleMapper.MapToEnum(roleName);

            if (role == null)
            {
                throw new NotSupportedException(
                    $"Role for user ID {userId} not supported. " +
                    $"Received role: '{roleName}'.");
            }

            var newClaims = new List<Claim>(initialClaims);

            newClaims.Add(new Claim(Constants.Claims.Role, roleName));

            if (role == Role.EstablishmentUser)
            {
                newClaims.Add(new Claim(Constants.Claims.Urn, org.urn));
                newClaims.Add(new Claim(
                    Constants.Claims.Laestab, $"{org.localAuthority.code}{org.establishmentNumber}"));
            }            

            return newClaims;
        }
    }
}
