using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Dfe.CspdAlpha.Web.Application.Application.Helpers
{
    public class ClaimsHelper
    {
        public static string GetURN(ClaimsPrincipal user)
        {
            var urn = user.Claims.FirstOrDefault(c => c.Type == "https://sa.education.gov.uk/idp/org/establishment/uRN");

            return urn == null ? "136028" : urn.Value;
        }
    }
}
