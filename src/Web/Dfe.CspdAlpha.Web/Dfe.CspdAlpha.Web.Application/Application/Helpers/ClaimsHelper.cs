using System.Linq;
using System.Security.Claims;
using System.Text.Json;

namespace Dfe.Rscd.Web.Application.Application.Helpers
{
    public class ClaimsHelper
    {
        public static string GetLAESTAB(ClaimsPrincipal user)
        {
            var org = GetOrganisationClaim(user);

            if (org.localAuthority == null)
            {
                return null;
            }

            return $"{org.localAuthority.code}{org.establishmentNumber}";
        }

        public static string GetURN(ClaimsPrincipal user)
        {
            return GetOrganisationClaim(user).urn;
        }

        public static string GetUserId(ClaimsPrincipal user)
        {
            return user.Claims.FirstOrDefault(
                c => c.Type == "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier")?.Value;
        }

        private static Organisation GetOrganisationClaim(ClaimsPrincipal user)
        {
            var orgClaim = user.Claims.FirstOrDefault(c => c.Type == "organisation")?.Value;

            return JsonSerializer.Deserialize<Organisation>(orgClaim);
        }

        private class Organisation
        {
            public string id { get; set; }
            public string name { get; set; }
            public Category category { get; set; }
            public Type type { get; set; }
            public string urn { get; set; }
            public object uid { get; set; }
            public string ukprn { get; set; }
            public string establishmentNumber { get; set; }
            public Status status { get; set; }
            public object closedOn { get; set; }
            public string address { get; set; }
            public string telephone { get; set; }
            public Region region { get; set; }
            public Localauthority localAuthority { get; set; }
            public Phaseofeducation phaseOfEducation { get; set; }
            public int? statutoryLowAge { get; set; }
            public int? statutoryHighAge { get; set; }
            public string legacyId { get; set; }
            public object companyRegistrationNumber { get; set; }
        }

        private class Category
        {
            public string id { get; set; }
            public string name { get; set; }
        }

        private class Type
        {
            public string id { get; set; }
            public string name { get; set; }
        }

        private class Status
        {
            public int id { get; set; }
            public string name { get; set; }
        }

        private class Region
        {
            public string id { get; set; }
            public string name { get; set; }
        }

        private class Localauthority
        {
            public string id { get; set; }
            public string name { get; set; }
            public string code { get; set; }
        }

        private class Phaseofeducation
        {
            public int id { get; set; }
            public string name { get; set; }
        }
    }
}
