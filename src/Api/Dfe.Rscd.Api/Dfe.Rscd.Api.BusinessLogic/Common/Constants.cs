
namespace Dfe.Rscd.Api.BusinessLogic.Common
{
    public class Constants
    {
        public const int CHANGE_TYPE_ID_SCHOOL_AMENDMENT = 2;
        public const int DATA_ORIGIN_ID_USER_ADDITION = 3;
        public const int NORFLAGE_ID_PUPIL_ADDED = 8;


        #region PINCL Codes for unlisted pupils with an Add Pupil Request

        public const string PINCL_UNLISTED_PUPIL_WITH_ADD_PUPIL_REQUEST_PLASC_KS2 = "299";
        public const string PINCL_UNLISTED_PUPIL_WITH_ADD_PUPIL_REQUEST_NON_PLASC_KS4 = "498";
        public const string PINCL_UNLISTED_PUPIL_WITH_ADD_PUPIL_REQUEST_PLASC_KS4 = "499";
        public const string PINCL_UNLISTED_PUPIL_WITH_ADD_PUPIL_REQUEST_PLASC_KS5 = "599";

        #endregion


        #region StudentStatus

        public const int STUDENT_STATUS_ID_UNAMENDED = 1;
        public const int STUDENT_STATUS_ID_AMENDED = 2;
        public const int STUDENT_STATUS_ID_ADJUSTMENT_REQUESTED = 3;
        public const int STUDENT_STATUS_ID_UNDECIDED = 4;
        public const int STUDENT_STATUS_ID_FORCE_ACCEPTED = 5;
        public const int STUDENT_STATUS_ID_ACCEPTED = 6;
        public const int STUDENT_STATUS_ID_REJECTED = 7;
        public const int STUDENT_STATUS_ID_ADDED = 8;
        public const int STUDENT_STATUS_ID_CANCELLED = 9;

        #endregion

        #region Prompt Constants

        //Non KS SPCIFIC
        public const int PROMPT_ID_LANGUAGE = 801;
        public const int PROMPT_ID_COUNTRY = 802;
        public const int PROMPT_ID_DATE_OF_ARRIVAL = 803;
        public const int PROMPT_ID_LANGUAGE_OTHER = 804;
        public const int PROMPT_ID_COUNTRY_OTHER = 805;

        public const int PROMPT_ID_ADMISSION_FOLLOWING_PERMANENT_EXCLUSION= 1001;
        public const int PROMPT_ID_REMOVE_DUAL_REGISTERED = 1909; 

        public const int PROMPT_ID_COUNTRY_WHERE_LIVING_NOW = 1101;
        public const int PROMPT_ID_DATE_LEFT_ENGLAND = 1102;

        public const int PROMPT_ID_DATE_OF_DEATH = 1201;
        public const int PROMPT_ID_EXCEPTIONALCIRCUMSTANCES = 2000;

        public const int PROMPT_ID_JOIN_ROLL_DATE = 810;
        public const int PROMPT_ID_REVISED_ADMISSION_DATE = 820;
        public const int PROMPT_ID_REVISED_ADMISSION_DATE_IF_AVAILABLE = 830;

        //KS4 SPECIFIC PROMPTS
        public const int PROMPT_ID_REINSTATE_PUPIL_EXPLANATION_KS4 = 700;
        public const int PROMPT_ID_EXCLUSION_DATE = 1040;
        public const int PROMPT_ID_NC_YEAR_GROUP_KS4 = 1400;
        public const int PROMPT_ID_DOB_KS4 = 1500;
        public const int PROMPT_ID_ADMISSION_DATE_FOR_PUPIL_EDIT_KS4 = 1600;
        public const int PROMPT_ID_DATE_OF_ROLL_REMOVAL_KS4 = 1801;
        public const int PROMPT_ID_OTHER_KS4 = 1900;
        public const int PROMPT_ID_ADMISSION_DATE_KS4 = 2200;
        public const int PROMPT_ID_RESULTS_BELONG_TO_ANOTHER_PUPIL_KS4 = 2300;

        //KS5 SPECIFIC PROMPTS
        public const int PROMPT_ID_NC_YEAR_GROUP_KS5 = 5200;
        public const int PROMPT_ID_DOB_KS5 = 5300;
        public const int PROMPT_ID_OTHER_KS5 = 5600;
        public const int PROMPT_ID_RESULTS_BELONG_TO_ANOTHER_PUPIL_KS5 = 5800;
        public const int PROMPT_ID_REINSTATE_PUPIL_EXPLANATION_KS5 = 5900;
        public const int PROMPT_ID_ADMISSION_DATE_KS5 = 5700;

        //KS2 SPECIFIC PROMPTS
        public const int PROMPT_ID_NC_YEAR_GROUP_KS2 = 21400;
        public const int PROMPT_ID_DOB_KS2 = 21500;
        public const int PROMPT_ID_ADMISSION_DATE_FOR_PUPIL_EDIT_KS2 = 21600;
        public const int PROMPT_ID_DATE_OF_ROLL_REMOVAL_KS2 = 21801;
        public const int PROMPT_ID_OTHER_KS2 = 21900;
        public const int PROMPT_ID_ADMISSION_DATE_KS2 = 22200;
        public const int PROMPT_ID_PUPIL_OMISSION_KS2 = 22210;
        public const int PROMPT_ID_PUPIL_ADDITION_REVIEW_INFO_KS2 = 22220;
        public const int PROMPT_ID_PUBLISH_PUPIL_EXPLANATION_KS2 = 22230;
        public const int PROMPT_ID_RESULTS_BELONG_TO_ANOTHER_PUPIL_KS2 = 22300;

        //KS3 SPECIFIC PROMPTS
        public const int PROMPT_ID_NC_YEAR_GROUP_KS3 = 31400;
        public const int PROMPT_ID_DOB_KS3 = 31500;
        public const int PROMPT_ID_DATE_OF_ROLL_REMOVAL_KS3 = 31801;
        public const int PROMPT_ID_OTHER_KS3 = 31900;
        public const int PROMPT_ID_ADMISSION_DATE_KS3 = 32200;
        public const int PROMPT_ID_PUPIL_OMISSION_KS3 = 32210;
        public const int PROMPT_ID_PUPIL_ADDITION_REVIEW_INFO_KS3 = 32220;
        public const int PROMPT_ID_PUBLISH_PUPIL_EXPLANATION_KS3 = 32230;
        public const int PROMPT_ID_RESULTS_BELONG_TO_ANOTHER_PUPIL_KS3 = 32300;

        public const int INCLUSION_REASON_ID_EDIT_PUPIL = 30;

        #endregion

        #region Constants for Inclusion Adjustment Reasons
        public const int INCLUSION_ADJUSTMENT_REASON_WAS_PUPIL_ON_ROLL_ON_ANNUAL_CENSUS_DATE = 1;
        public const int INCLUSION_ADJUSTMENT_REASON_WAS_PUPIL_EVER_ENROLLED = 2;
        public const int INCLUSION_ADJUSTMENT_REASON_WAS_PUPIL_WAS_ADMITTED_FROM_ABROAD_WITH_ENGLISH_NOT_FIRST_LANGUAGE = 8;
        public const int INCLUSION_ADJUSTMENT_REASON_PUPIL_WAS_NOT_ON_ROLL_AT_CENSUS_DATE = 51;

        #endregion

        #region Constants for Request Reasons

        public const int SCRUTINY_REASON_ADMITTED_INTO_6TH_FORM_FROM_ABROAD = 1;
        public const int SCRUTINY_REASON_RECENTLY_FROM_ABROAD = 2;
        public const int SCRUTINY_REASON_EMIGRATED = 3;
        public const int SCRUTINY_REASON_DEATH = 4;
        public const int SCRUTINY_REASON_MERGE_PUPILS = 5;
        public const int SCRUTINY_REASON_NOT_ON_ROLL = 6;
        public const int SCRUTINY_REASON_NEVER_ON_ROLL = 7;
        public const int SCRUTINY_REASON_APPEAL_ADD_BACK = 8;
        public const int SCRUTINY_REASON_CANCEL_EXCLUSION_ADJUSTMENT = 9;
        public const int SCRUTINY_REASON_REMOVE_COMPLETELY = 10;
        public const int SCRUTINY_REASON_ADD_DUAL_REG_PUPIL = 11;
        public const int SCRUTINY_REASON_REINSTATE_PUPIL = 12;
        public const int SCRUTINY_REASON_ADMISSION_FOLLOWING_PERMANENT_EXCLUSION = 13;
        public const int SCRUTINY_REASON_NOT_AT_END_OF_KEY_STAGE = 14;
        public const int SCRUTINY_REASON_NC_YEAR_CHANGE = 15;
        public const int SCRUTINY_REASON_NC_YEAR_AMENDED = 16;
        public const int SCRUTINY_REASON_ADD_BACK_IN_2008 = 17;
        public const int SCRUTINY_REASON_YEAR_GROUP_ALGORITHM = 18;
        public const int SCRUTINY_REASON_DOB_CHANGE = 19;
        public const int SCRUTINY_REASON_DOB_AMENDED = 20;
        public const int SCRUTINY_REASON_ADMISSION_DATE_CHANGE = 21;
        public const int SCRUTINY_REASON_JOINED_AFTER_ASC = 22;
        public const int SCRUTINY_REASON_ADMISSION_DATE_AMENDED = 23;
        public const int SCRUTINY_REASON_LEFT_SCHOOL_BEFORE_ASC = 24;
        public const int SCRUTINY_REASON_LEFT_SCHOOL_AFTER_ASC = 25;
        public const int SCRUTINY_REASON_ILLNESS = 26;
        public const int SCRUTINY_REASON_HOME_TUITION = 27;
        public const int SCRUTINY_REASON_FUNDING_FOLLOWED = 28;
        public const int SCRUTINY_REASON_ATTENDANCE = 29;
        public const int SCRUTINY_REASON_SPECIAL_NEEDS = 30;
        public const int SCRUTINY_REASON_REMOVE_DUAL_REGISTERED = 31;
        public const int SCRUTINY_REASON_PRISON = 32;
        public const int SCRUTINY_REASON_PUPIL_NOT_KNOWN = 33;
        public const int SCRUTINY_REASON_TRAVELLER = 34;
        public const int SCRUTINY_REASON_CONTINGENCY = 35;
        public const int SCRUTINY_REASON_OTHER = 36;
        public const int SCRUTINY_REASON_ADD_PUPIL = 37;
        public const int SCRUTINY_REASON_ONE_YEAR_COURSE = 38;
        public const int SCRUTINY_REASON_WORK_BASED_LEARNER = 39;
        public const int SCRUTINY_REASON_PART_TIME = 40;
        public const int SCRUTINY_REASON_NOT_AT_END_OF_KEY_STAGE_IN_ALL_SUBJECTS = 41;
        public const int SCRUTINY_REASON_LEFT_SCHOOL_AFTER_ASC_AND_BEFORE_TESTS = 42;
        public const int SCRUTINY_REASON_LEFT_SCHOOL_AFTER_TEST_WEEK = 43;
        public const int SCRUTINY_REASON_LEFT_SCHOOL_DURING_TESTS = 44;
        public const int SCRUTINY_REASON_JOINED_AFTER_TEST_WEEK = 45;
        public const int SCRUTINY_REASON_LEFT_SCHOOL_BEFORE_TESTS = 46;
        public const int SCRUTINY_REASON_MULTIPLE_REQUESTS = 47;
        public const int SCRUTINY_REASON_ADD_UNLISTED_PUPIL = 75;
        public const int SCRUTINY_REASON_ADD_UNLISTED_OVERSEAS_PUPIL = 93;

        #endregion

        #region Constants for scrutiny status

        public const string SCRUTINY_STATUS_ACCEPT = "A";
        public const string SCRUTINY_STATUS_ACCEPTEDAUTOMATICALLY = "AA";
        public const string SCRUTINY_STATUS_CANCELLED = "C";
        public const string SCRUTINY_STATUS_PENDINGDCSF = "PD";
        public const string SCRUTINY_STATUS_PENDINGFORVUSEXPERT = "PE";
        public const string SCRUTINY_STATUS_PENDINGFORVUS = "PF";
        public const string SCRUTINY_STATUS_REJECT = "R";
        public const string SCRUTINY_STATUS_AWAITINGEVIDENCE = "W";

        #endregion

        #region Constants for rejection reasons

        public const int REJECTION_REASON_LEFT_SCHOOL_AFTER_ASC = 25;
        public const int REJECTION_REASON_ILLNESS = 26;
        public const int REJECTION_REASON_HOME_TUITION = 27;
        public const int REJECTION_REASON_FUNDING_FOLLOWED = 28;
        public const int REJECTION_REASON_SPECIAL_NEEDS = 30;
        public const int REJECTION_REASON_PUPIL_NOT_KNOWN = 33;
        public const int REJECTION_REASON_TRAVELLER = 34;
        public const int REJECTION_REASON_OTHER = 36;
        public const int REJECTION_REASON_DUAL_REGISTRATION = 48;
        public const int REJECTION_REASON_WORKING_IN_DIFFERENT_YEARGROUPS = 50;
        public const int REJECTION_REASON_NEVER_ON_ROLL = 54;
        public const int REJECTION_REASON_DOUBLE_LISTED = 55;
        public const int REJECTION_REASON_LEFT_BEFORE_ASC = 57;
        public const int REJECTION_REASON_OVERSEAS_PUPIL_CONDITIONS_NOT_MET = 58;
        public const int REJECTION_REASON_ON_ROLL = 59;
        public const int REJECTION_REASON_INSUFFICIENT_INFO_PROVIDED = 60;
        public const int REJECTION_REASON_DUPLICATE_REQUEST = 63;
        public const int REJECTION_REASON_EXCLUDED_PUPIL = 66;
        public const int REJECTION_REASON_ADD_BACK = 67;
        public const int REJECTION_REASON_NON_ATTENDANT = 68;
        public const int REJECTION_REASON_AGE_CRITERIA_NOT_MET = 69;
        public const int REJECTION_REASON_REPORTED_IN_PREVIOUS_YEAR_TABLES = 70;
        public const int REJECTION_REASON_CONDITIONS_NOT_MET_FOR_EXCLUDED_PUPIL = 71;
        public const int REJECTION_REASON_SEN_PUPIL_MUST_BE_INCLUDED = 72;
        public const int REJECTION_REASON_ON_ROLL_DURING_TEST_WEEK = 73;
        public const int REJECTION_REASON_NO_EXPLANATION = 74;
        public const int REJECTION_REASON_AT_END_OF_KS4 = 76;
        public const int REJECTION_REASON_NOT_AT_END_OF_KS4 = 77;
        




        #endregion

        #region Cohort configuration lookup codes
        public const string ANNUAL_SCHOOL_CENSUS_DATE_LOOKUP_CODE = "ASCDate";

        public const string KS3_TEST_START_DATE_LOOKUP_CODE = "TestStartDate";
        public const string KS3_TEST_END_DATE_LOOKUP_CODE = "TestEndDate";
        public const string KS2_TEST_START_DATE_LOOKUP_CODE = "TestStartDate";
        public const string KS2_TEST_END_DATE_LOOKUP_CODE = "TestEndDate";
        public const string COHORT_CONFIG_LOOKUP_CODE_TABLE_YEAR = "TableYear";
        
        #endregion

        #region Student Value Type IDs

        public const int STUDENT_VALUE_TYPE_ID_LEV2EM = 27;

        #endregion

        #region Constants for regular expressions for validations

        public const string REGEX_PATTERN_NUMERIC = @"\d";
        public const string REGEX_PATTERN_ONE_AND_TWO_LETTER_WORDS = @"(?<=(?:\s|\G|\A))\w{1,2}(?=(?:\s|\Z|\.|\?|\!))(?x)";
        public const string REGEX_PATTERN_NON_ALPHA = @"[^a-zA-Z -]";
        public const string REGEX_PATTERN_NAME = @"[a-zA-Z][a-zA-Z\'\- ]*[a-zA-Z]?";

        #endregion
    }
}
