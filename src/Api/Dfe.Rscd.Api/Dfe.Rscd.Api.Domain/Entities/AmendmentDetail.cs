using Dfe.Rscd.Api.Domain.Interfaces;

namespace Dfe.Rscd.Api.Domain.Entities
{
    public class AmendmentDetail : IAmendmentDetail
    {
        public int ReasonCode { get; set; }
        public string SubReason { get; set; }
        public string Detail { get; set; }
    }
}