namespace Dfe.Rscd.Api.Domain.Entities
{
    public class AmendmentReason
    {
        public short ReasonId { get; set; }
        public string Description { get; set; }
        public bool InJune { get; set; }
        public bool CanCancel { get; set; }
        public bool IsInclusion { get; set; }
        public bool IsNew { get; set; }
        public int? Order { get; set; }
    }

    public enum AmendmentReasonCode
    {
        AdmittedFromAbroadWithEnglishNotFirstLanguageCode = 8
    }
}
