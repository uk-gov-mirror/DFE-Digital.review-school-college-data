namespace Dfe.Rscd.Api.Domain.Entities
{
    public class RemovePupilAmendment : Amendment
    {
        public const string FIELD_ReasonCode = "ReasonCode";
        public const string FIELD_SubReason = "SubReason";
        public const string FIELD_Detail = "Detail";
        public const string FIELD_ScrutinyReasonCode = "ScrutinyReasonCode";
        public const string FIELD_AmdFlag = "AmdFlag";

        public RemovePupilAmendment()
        {
            AmendmentType = AmendmentType.RemovePupil;
            AmendmentDetail.AddField(FIELD_ReasonCode, default(int));
            AmendmentDetail.AddField(FIELD_SubReason, string.Empty);
            AmendmentDetail.AddField(FIELD_Detail, string.Empty);
            AmendmentDetail.AddField(FIELD_ScrutinyReasonCode, string.Empty);
            AmendmentDetail.AddField(FIELD_AmdFlag, string.Empty);
        }
    }
}