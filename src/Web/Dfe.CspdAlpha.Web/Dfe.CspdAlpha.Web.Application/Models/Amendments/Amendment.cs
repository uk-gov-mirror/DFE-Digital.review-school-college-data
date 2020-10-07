using Dfe.CspdAlpha.Web.Application.Models.Amendments.AmendmentTypes;
using Dfe.CspdAlpha.Web.Application.Models.Common;

namespace Dfe.CspdAlpha.Web.Application.Models.Amendments
{
    public class Amendment<T>  where T : BaseAmendmentType
    {
        public string URN { get; set; }
        public CheckingWindow CheckingWindow { get; set; }
        public T AmendmentDetail { get; set; }
    }
}
