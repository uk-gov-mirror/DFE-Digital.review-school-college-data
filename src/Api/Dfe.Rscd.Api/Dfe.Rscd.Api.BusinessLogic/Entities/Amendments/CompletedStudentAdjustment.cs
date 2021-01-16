using System.Collections.Generic;

namespace Dfe.Rscd.Api.BusinessLogic.Entities
{
    public class CompletedStudentAdjustment
    {
        public List<ValidationFailure> ErrorMessageList;
        public int ForvusId;
        public int? InclusionReasonID;

        public List<ValidationFailure> InformationMessageList;
        public PromptAnswerList PromptAnswerList;
        public int? RejectionReasonCode;
        public string RequestCompletionDisplayMessage;
        public int ScrutinyReasonID;
        public string ScrutinyStatusCode;
        public string ScrutinyStatusDescription;
        public int StudentID;
        public int? StudentRequestID;
        public List<ValidationFailure> WarningMessageList;

        public CompletedStudentAdjustment(int studentId,
            int? inclusionReasonId,
            PromptAnswerList promptAnswerList,
            int scrutinyReasonId,
            int? rejectionReasonCode,
            string scrutinyStatusCode,
            string completionMessage)
        {
            StudentID = studentId;
            InclusionReasonID = inclusionReasonId;
            PromptAnswerList = promptAnswerList;
            ScrutinyReasonID = scrutinyReasonId;
            RejectionReasonCode = rejectionReasonCode;
            ScrutinyStatusCode = scrutinyStatusCode;
            RequestCompletionDisplayMessage = completionMessage;
        }
    }
}