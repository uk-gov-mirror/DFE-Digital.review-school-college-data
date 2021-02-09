namespace Dfe.Rscd.Api.Domain.Entities
{
    public class RemovePupilAmendment : Amendment
    {
        public const string FIELD_ReasonCode = "ReasonCode";
        public const string FIELD_OutcomeReasonDescription = "SubReason";
        public const string FIELD_ReasonDescription = "ScrutinyReasonCode";
        public const string FIELD_CountryOfOrigin = "CountryOfOrigin";
        public const string FIELD_NativeLanguage = "NativeLanguage";
        public const string FIELD_DateOfArrivalUk = "DateOfArrivalUk";

        public RemovePupilAmendment()
        {
            AmendmentType = AmendmentType.RemovePupil;
            AmendmentDetail.AddField(FIELD_ReasonCode, default(int));
            AmendmentDetail.AddField(FIELD_OutcomeReasonDescription, string.Empty);
            AmendmentDetail.AddField(FIELD_ReasonDescription, string.Empty);
            AmendmentDetail.AddField(FIELD_CountryOfOrigin, string.Empty);
            AmendmentDetail.AddField(FIELD_NativeLanguage, string.Empty);
            AmendmentDetail.AddField(FIELD_DateOfArrivalUk, string.Empty);
        }
    }
}