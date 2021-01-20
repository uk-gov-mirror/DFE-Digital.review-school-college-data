using System.Collections.Generic;

namespace Dfe.Rscd.Api.BusinessLogic.Entities
{
    public class CompletedStudentAdjustment
    {
        public List<ValidationFailure> ErrorMessageList;
        public int ForvusId;
        public int? InclusionReasonID;

        public List<ValidationFailure> InformationMessageList;
        public List<PromptAnswer> PromptAnswerList;
        public OutcomeStatus OutcomeStatus;
        public int? RejectionReasonCode;
        public string RequestCompletionDisplayMessage;
        public int ScrutinyReasonID;
        public string ScrutinyStatusCode;
        public string ScrutinyStatusDescription;
        public string StudentID;
        public int? StudentRequestID;
        public List<ValidationFailure> WarningMessageList;

        public CompletedStudentAdjustment(string studentId,
            int? inclusionReasonId,
            List<PromptAnswer> promptAnswerList,
            int scrutinyReasonId,
            int? rejectionReasonCode,
            string scrutinyStatusCode,
            string completionMessage,
            OutcomeStatus outcomeStatus)
        {
            StudentID = studentId;
            InclusionReasonID = inclusionReasonId;
            PromptAnswerList = promptAnswerList;
            ScrutinyReasonID = scrutinyReasonId;
            RejectionReasonCode = rejectionReasonCode;
            ScrutinyStatusCode = scrutinyStatusCode;
            RequestCompletionDisplayMessage = completionMessage;
            OutcomeStatus = outcomeStatus;
        }
    }
}