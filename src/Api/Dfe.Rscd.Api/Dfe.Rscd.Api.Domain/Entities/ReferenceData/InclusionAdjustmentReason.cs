namespace Dfe.Rscd.Api.Domain.Entities.ReferenceData
{
    public class InclusionAdjustmentReason
    {
        public int IncAdjReasonID { get; set; }
        public string IncAdjReasonDescription { get; set; }
        public bool InJuneChecking { get; set; }
        public bool CanCancel { get; set; }
        public bool IsInclusion { get; set; }
        public bool IsNewStudentReason { get; set; }
        public int ListOrder { get; set; }
    }
}