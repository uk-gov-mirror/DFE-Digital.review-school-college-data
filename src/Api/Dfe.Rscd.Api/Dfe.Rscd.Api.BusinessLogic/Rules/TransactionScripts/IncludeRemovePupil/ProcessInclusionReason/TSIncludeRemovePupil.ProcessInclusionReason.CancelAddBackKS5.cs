using Web09.Checking.Business.Logic.Common;
using Web09.Checking.Business.Logic.Entities;
using Web09.Common.Exceptions;

namespace Web09.Checking.Business.Logic.TransactionScripts
{
    public partial class TSIncludeRemovePupil
    {                
        private static AdjustmentPromptAnalysis ProcessInclusionPromptResponses_CancelAddBackKS5(int studentId, int inclusionReasonId, PromptAnswerList answerList)
        {
            if (answerList.HasPromptAnswer(22400) || answerList.HasPromptAnswer(22500) || answerList.HasPromptAnswer(22600) )
            {
                //A reason is provided, therefore the request may be completed.
                return new AdjustmentPromptAnalysis(new CompletedStudentAdjustment(
                        studentId,
                        inclusionReasonId,
                        answerList,
                        Contants.SCRUTINY_REASON_APPEAL_ADD_BACK,
                        null,
                        Contants.SCRUTINY_STATUS_PENDINGFORVUS,
                        null)
                        );
            }

            throw Web09Exception.GetBusinessException(Web09MessageList.InsufficientStudentAdjustmentPromptAnswerProvided);
        }
    }


}
