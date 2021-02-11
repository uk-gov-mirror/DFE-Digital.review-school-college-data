namespace Dfe.Rscd.Api.Domain.Entities
{
    public class RemovePupilAmendment : Amendment
    {
        public const string FIELD_ReasonCode = "ReasonCode";
        public const string FIELD_ReasonDescription = "ReasonDescription";
        public const string FIELD_OutcomeDescription = "OutcomeDescription";
        public const string FIELD_CountryOfOrigin = "CountryOfOrigin";
        public const string FIELD_NativeLanguage = "NativeLanguage";
        public const string FIELD_DateOfArrivalUk = "DateOfArrivalUk";
        public const string FIELD_LAESTABNumber = "LAESTABNumber";
        public const string FIELD_ExclusionDate = "PupilExclusionDate";
        public const string FIELD_DateOffRoll = "PupilDateOffRoll";

        public RemovePupilAmendment()
        {
            AmendmentType = AmendmentType.RemovePupil;
            AmendmentDetail.AddField(FIELD_ReasonCode, default(int));
            AmendmentDetail.AddField(FIELD_OutcomeDescription, string.Empty);
            AmendmentDetail.AddField(FIELD_ReasonDescription, string.Empty);
            AmendmentDetail.AddField(FIELD_CountryOfOrigin, string.Empty);
            AmendmentDetail.AddField(FIELD_NativeLanguage, string.Empty);
            AmendmentDetail.AddField(FIELD_DateOfArrivalUk, string.Empty);
            AmendmentDetail.AddField(FIELD_LAESTABNumber, string.Empty);
            AmendmentDetail.AddField(FIELD_ExclusionDate, string.Empty);
            AmendmentDetail.AddField(FIELD_DateOffRoll, string.Empty);
        }
    }
}