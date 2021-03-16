namespace Dfe.Rscd.Web.Application.Models.Common
{
    public class Constants
    {
        public const string APP_TITLE = "Review my school or college data";

        // Session keys
        public const string AMENDMENT_SESSION_KEY = "current-amendment";
        public const string NEW_AMENDMENT_ID = "new-amendment-id";
        public const string NEW_REFERENCE_ID = "new-reference-id";
        public const string PROMPT_QUESTIONS = "new-promptquestions-ref";
        public const string PROMPT_ANSWERS = "new-promptanswers-ref";


        public class AddPupil
        {
            public const string PriorAttainmentResults = "PriorAttainmentResults";
            public const string AddReason = "AddReason";
            public const string PreviousSchoolLAEstab = "PreviousSchoolLAEstab";
            public const string PreviousSchoolURN = "PreviousSchoolURN";
        }

        public class RemovePupil
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
            public const string FIELD_UserProvidedDetails = "UserProvidedDetails";
        }
    }
}
