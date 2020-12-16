namespace Dfe.CspdAlpha.Web.Application.Models.Common
{
    public class Constants
    {
        public const string APP_TITLE = "Review my school and college data";

        // Session keys
        public const string AMENDMENT_SESSION_KEY = "current-amendment";
        public const string NEW_AMENDMENT_ID = "new-amendment-id";

        // Remove reason codes
        public const int NOT_AT_END_OF_16_TO_18_STUDY = 325;
        public const int INTERNATIONAL_STUDENT = 326;
        public const int DECEASED = 327;
        public const int NOT_ON_ROLL = 328;
        public const int OTHER_WITH_EVIDENCE = 329;
        public const int OTHER_EVIDENCE_NOT_REQUIRED = 330;

        public class AddPupil
        {
            public const string PriorAttainmentResults = "PriorAttainmentResults";
            public const string AddReason = "AddReason";
            public const string PreviousSchoolLAEstab = "PreviousSchoolLAEstab";
            public const string PreviousSchoolURN = "PreviousSchoolURN";
        }

        public class RemovePupil
        {
            public const string ReasonCode = "ReasonCode";
            public const string Detail = "Detail";
        }
    }
}
