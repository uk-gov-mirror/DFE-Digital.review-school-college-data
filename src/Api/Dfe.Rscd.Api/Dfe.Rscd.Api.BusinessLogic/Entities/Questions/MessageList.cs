using System;
using System.Collections.Generic;
using System.Reflection;
using System.Resources;
using System.Text;

namespace Dfe.Rscd.Api.BusinessLogic.Entities
{
    public enum MessageList : int
    {
        FAQInvalidID = 0,
        FAQInvalidCategoryID = 1,
        FAQIncompleteInformation = 2,
        
        DCSFNumberInvalid = 3,        
        KeystageInvalid = 4,
        ApplicationLevelError=5,
        SchoolGroupInvalid = 6,

        HelpInvalidID=7,
        HelpInvalidPageID=8,
        HelpIncompleteInformation=9,

        MessageInvalidID = 10,
        MessageInvalidMessageTypeID = 11,
        MessageIncompleteInformation = 12,

        ResultInvalidID = 13,
        QANInvalid = 14,
        SyllabusInvalid = 15,
        AwardingBodyInvalid = 16,
        ExamYearInvalid = 17,
        ExamSeasonInvalid = 18,

        StudentInvalidID=19,
        AdjustmentReasonInvalidID=20,
        PromptInvalidID=21,

        TransactionDataConcurrencyError=22,

        MarksNotInRange = 23,
        AddingWithdrawnResult = 24,
        DuplicateResult = 25,
        MarksNotInRangeResubmit=26,
        ConflictsWithRRBelow=27,
        LRMatchesUnamended=28,
        MatchingLRNotRejected=29,
        ConflictsWithLRAbove=30,
        MatchesLRAbove=31,
        InsufficientStudentDetails=32,
        InsufficientStudentAdjustmentPromptAnswerProvided=33,
        InvalidStudentIncludeRemoveRequest = 34,
        InvalidStudentEditRequest = 35,
        ErrorAttachingEvidence = 36,
        OutstandingStudentAdjustmentExists = 37,
        InvalidKS5AdmissionDateRequest = 38,
        InvalidKS3StudentAdjustmentRequest = 39,
        StudentValidationFailureList = 40,
        EditStudentOutstandingAdjustmentExists = 41,
        NoStudentChangeDetected = 42,
        InvalidStudent = 43,
        StudentNotSaved = 44,
        ErrorUpdatingSchoolNOR = 45,
        AnotherNORRequestExists = 46,
        AddedResultExists = 47,
        AmendedResultExists = 48,
        UnamendedResultExists = 49,
        LateResultExists = 50,
        StudentRequestNotFound = 51,
        InvalidDateTimeAdjustmentPromptAnswer = 52,
        SchoolNotFound = 53,
        InvalidKeyStageForPupilAdd = 54,
        StudentNotFound = 55,
        AlreadyLoggedEvidence = 56,
        DuplicateResultExists = 57,
        StudentWontBeIncludedInKS5Publication = 58,
        YearsAbove13CannotBeAccepted = 59,
        StudentWasDeferredLastYear = 60,
        StudentTooOldToDefer = 61
    }

    public class MessageFactory
    {
        public static string GetMessage(MessageList res)
        {
            ResourceManager rm = new ResourceManager("Dfe.Rscd.Api.BusinesLogic.MessageFactory", Assembly.GetExecutingAssembly());
            int val =(int) res;
            return rm.GetString("MSG" + val.ToString("0000"));
        }
    }
}
