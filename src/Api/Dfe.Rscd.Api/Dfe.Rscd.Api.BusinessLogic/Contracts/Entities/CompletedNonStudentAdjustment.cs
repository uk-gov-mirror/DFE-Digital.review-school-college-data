
namespace Dfe.Rscd.Api.BusinessLogic.Contracts.Entities
{
    public class CompletedNonStudentAdjustment
    {
        public string RequestCompletionDisplayMessage;

        public CompletedNonStudentAdjustment(string completionMessage)
        {
            RequestCompletionDisplayMessage = completionMessage;
        }
    }
}
