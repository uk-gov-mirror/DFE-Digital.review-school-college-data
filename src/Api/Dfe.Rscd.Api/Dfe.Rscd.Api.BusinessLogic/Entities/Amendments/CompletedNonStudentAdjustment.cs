
namespace Dfe.Rscd.Api.BusinessLogic.Entities
{
    public class CompletedNonStudentAdjustment
    {
        public string RequestCompletionDisplayMessage;
        public OutcomeStatus AdjustmentOutcome;

        public CompletedNonStudentAdjustment(string completionMessage, OutcomeStatus adjustmentOutcome=OutcomeStatus.AutoReject)
        {
            RequestCompletionDisplayMessage = completionMessage;
            AdjustmentOutcome = adjustmentOutcome;
        }
    }
}
