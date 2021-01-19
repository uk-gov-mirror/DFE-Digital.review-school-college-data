
namespace Dfe.Rscd.Api.BusinessLogic.Entities
{
    public class CompletedNonStudentAdjustment
    {
        public string RequestCompletionDisplayMessage;
        public OutcomeStatus OutcomeStatus;

        public CompletedNonStudentAdjustment(string completionMessage, OutcomeStatus outcomeStatus=OutcomeStatus.AutoReject)
        {
            RequestCompletionDisplayMessage = completionMessage;
            OutcomeStatus = outcomeStatus;
        }
    }
}
