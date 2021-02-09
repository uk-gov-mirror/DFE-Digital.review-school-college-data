using System.Linq;
using System.Security.Claims;

namespace Dfe.Rscd.Web.Application.Application.Helpers
{
    public class ClaimsHelper
    {
        public static string GetLAESTAB(ClaimsPrincipal user)
        {
            var urn = user.Claims.FirstOrDefault(c => c.Type == "https://sa.education.gov.uk/idp/org/establishment/dfeNumber");

            return urn == null ? "136028" : urn.Value;
        }

        public static string GetURN(ClaimsPrincipal user)
        {
            var urn = user.Claims.FirstOrDefault(c => c.Type == "https://sa.education.gov.uk/idp/org/establishment/uRN");

            return urn == null ? "136028" : urn.Value;
        }

        public static string GetUserId(ClaimsPrincipal user)
        {
            var userId = user.Claims.FirstOrDefault(c => c.Type == "urn:oid:0.9.2342.19200300.100.1.1");
            //var urn = user.Claims.FirstOrDefault(c => c.Type == "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier");

            return userId == null ? "136028" : userId.Value;
        }

    }
}
