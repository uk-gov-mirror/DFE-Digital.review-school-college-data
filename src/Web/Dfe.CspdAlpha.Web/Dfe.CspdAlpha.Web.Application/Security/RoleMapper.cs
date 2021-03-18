using System.Collections.Generic;
using static Dfe.Rscd.Web.Application.Security.Constants;

namespace Dfe.Rscd.Web.Application.Security
{
    public static class RoleMapper
    {
        private static Dictionary<string, Role> map = new Dictionary<string, Role>
        {
            { RoleNames.EstablishmentUser, Role.EstablishmentUser },
            { RoleNames.LocalAuthorityUser, Role.LocalAuthorityUser },
            { RoleNames.DfEUser, Role.DfeUser }
        };

        public static Role? MapToEnum(string roleName)
        {
            if (map.TryGetValue(roleName, out var role))
            {
                return role;
            }

            return null;
        }
    }
}
