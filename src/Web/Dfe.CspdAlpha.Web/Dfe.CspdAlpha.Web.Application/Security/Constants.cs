namespace Dfe.Rscd.Web.Application.Security
{
    public static class Constants
    {
        public static class Claims
        {
            public const string Organisation = "organisation";
            public const string Role = "role";
            public const string Subject = "sub";
            public const string Urn = "urn";
            public const string Laestab = "laestab";
            public const string Email = "email";
            public const string GivenName = "given_name";
            public const string FamilyName = "family_name";
        }

        /// <remarks>
        /// The role names must match what is returned from DSI.
        /// </remarks>
        public static class RoleNames
        {
            public const string EstablishmentUser = "Establishment User";
            public const string LocalAuthorityUser = "LA User";
            public const string DfEUser = "DfE User";
        }
    }
}
