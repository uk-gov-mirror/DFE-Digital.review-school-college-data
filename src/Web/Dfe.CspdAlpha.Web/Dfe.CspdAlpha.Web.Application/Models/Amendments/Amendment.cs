using Dfe.CspdAlpha.Web.Application.Models.Common;

namespace Dfe.CspdAlpha.Web.Application.Models.Amendments
{
    public class Amendment<T>  where T : IAmendmentType
    {
        public string URN { get; set; }
        public CheckingWindow CheckingWindow { get; set; }
        public T AmendmentDetail { get; set; }
    }
}
