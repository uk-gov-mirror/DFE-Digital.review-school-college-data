#nullable disable

namespace Dfe.Rscd.Api.BusinessLogic.Entities
{
    public class InclusionAdjustmentReason
    {
        public short IncAdjReasonId { get; set; }
        public string IncAdjReasonDescription { get; set; }
        public bool InJuneChecking { get; set; }
        public bool CanCancel { get; set; }
        public bool IsInclusion { get; set; }
        public bool IsNewStudentReason { get; set; }
        public int? ListOrder { get; set; }
    }
}
