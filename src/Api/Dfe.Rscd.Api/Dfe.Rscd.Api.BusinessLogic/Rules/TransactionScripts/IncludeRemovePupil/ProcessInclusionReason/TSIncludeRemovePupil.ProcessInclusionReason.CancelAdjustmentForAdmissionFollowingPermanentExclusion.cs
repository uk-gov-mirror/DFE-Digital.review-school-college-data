using System.Collections.Generic;
using Web09.Checking.Business.Logic.Common;
using Web09.Checking.Business.Logic.Entities;
using Web09.Checking.DataAccess;
using Web09.Common.Exceptions;

namespace Web09.Checking.Business.Logic.TransactionScripts
{
    public partial class TSIncludeRemovePupil
    {
        private static AdjustmentPromptAnalysis ProcessInclusionPromptResponses_CancelAdjustmentForAdmissionFollowingPermanentExclusion(int studentId, int inclusionReasonId, PromptAnswerList answers)
        {
            if (!answers.HasPromptAnswer(400))
            {
                List<Prompts> furtherPrompts = new List<Prompts>();
                furtherPrompts.Add(GetPromptByPromptID(400));
                return new AdjustmentPromptAnalysis(furtherPrompts);
            }
            if (answers.HasPromptAnswer(400) && IsPromptAnswerComplete(answers, 400))
            {
                return new AdjustmentPromptAnalysis(new CompletedStudentAdjustment(studentId,
                                                                                   inclusionReasonId,
                                                                                   answers,
                                                                                   Contants.SCRUTINY_REASON_CANCEL_EXCLUSION_ADJUSTMENT,
                                                                                   null,
                                                                                   Contants.SCRUTINY_STATUS_PENDINGFORVUS,
                                                                                   null)
                    );
            }
            throw Web09Exception.GetBusinessException(Web09MessageList.InsufficientStudentAdjustmentPromptAnswerProvided);
        }
    }
}
