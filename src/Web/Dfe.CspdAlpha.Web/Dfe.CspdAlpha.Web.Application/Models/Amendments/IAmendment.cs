using Dfe.CspdAlpha.Web.Application.Models.Common;

namespace Dfe.CspdAlpha.Web.Application.Models.Amendments
{
    public class IAmendment
    {
         string URN { get; set; }
         CheckingWindow CheckingWindow { get; set; }
         IAmendmentType AmendmentDetail { get; set; }
    }
}
