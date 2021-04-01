using Dfe.Rscd.Api.Domain.Entities.Amendments;

namespace Dfe.Rscd.Api.Domain.Entities
{
    public class RemovePupilAmendment : Amendment
    {
        public const string FIELD_ReasonCode = "ReasonCode";
        public const string FIELD_ReasonDescription = "ReasonDescription";
        public const string FIELD_SubReasonDescription = "SubReasonDescription";
        public const string FIELD_OutcomeDescription = "OutcomeDescription";
        public const string FIELD_CountryOfOrigin = "CountryOfOrigin";
        public const string FIELD_CountryLeftEnglandFor = "CountryLeftEnglandFor";
        public const string FIELD_Detail = "Detail";
        public const string FIELD_EvidenceFolder = "EvidenceFolder";
        public const string FIELD_NativeLanguage = "NativeLanguage";
        public const string FIELD_DateOfArrivalUk = "DateOfArrivalUk";
        public const string FIELD_LAESTABNumber = "LAESTABNumber";
        public const string FIELD_ExclusionDate = "PupilExclusionDate";
        public const string FIELD_DateOffRoll = "PupilDateOffRoll";
        public const string FIELD_DateOnRoll = "PupilDateOnRoll";

        public RemovePupilAmendment()
        {
            AmendmentType = AmendmentType.RemovePupil;
        }
    }
}