using System;
using Web09.Checking.Business.Logic.Common;
using Web09.Checking.Business.Logic.Entities;
using Web09.Common.Exceptions;

namespace Web09.Checking.Business.Logic.TransactionScripts
{
    public partial class TSIncludeRemovePupil
    {
        private static AdjustmentPromptAnalysis ProcessInclusionPromptResponses_CancelAddBack(int studentId, int inclusionReasonId, PromptAnswerList answerList)
        {
            if (answerList.HasPromptAnswer(300) || String.IsNullOrEmpty(answerList.GetPromptAnswerByPromptID(300).PromptStringAnswer))
            {
                //A reason is provided, therefore the request may be completed.
                return new AdjustmentPromptAnalysis( new CompletedStudentAdjustment(
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
