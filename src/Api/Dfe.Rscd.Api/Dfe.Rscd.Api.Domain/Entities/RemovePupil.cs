using Dfe.Rscd.Api.Domain.Interfaces;

namespace Dfe.Rscd.Api.Domain.Entities
{
    public class RemovePupil : IAmendmentType
    {
        public string  Reason { get; set; }
        public string SubReason { get; set; }
        public string Detail { get; set; }
        public string AllocationYear { get; set; }
    }
}
