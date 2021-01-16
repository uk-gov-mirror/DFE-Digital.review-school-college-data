using System.Collections.Generic;
using Web09.Checking.Business.Logic.Common;
using Web09.Checking.Business.Logic.Entities;
using Web09.Checking.DataAccess;
using Web09.Common.Exceptions;

namespace Web09.Checking.Business.Logic.TransactionScripts
{
    public partial class TSIncludeRemovePupil
    {
        public static AdjustmentPromptAnalysis ProcessInclusionPromptResponses_ReinstateThePupil(Students student, int inclusionReasonId, PromptAnswerList answers)
        {
            if (student == null || student.Cohorts == null)
                throw Web09Exception.GetBusinessException(Web09MessageList.InsufficientStudentDetails);

            int reinstateExplanationPromptId;

            if (student.Cohorts.KeyStage == 4)
                reinstateExplanationPromptId = Contants.PROMPT_ID_REINSTATE_PUPIL_EXPLANATION_KS4;
            else //key stage 5
                reinstateExplanationPromptId = Contants.PROMPT_ID_REINSTATE_PUPIL_EXPLANATION_KS5;

            if (!answers.HasPromptAnswer(reinstateExplanationPromptId))
            {
                List<Prompts> furtherPrompts = new List<Prompts>();
                furtherPrompts.Add(GetPromptByPromptID(700));
                return new AdjustmentPromptAnalysis(furtherPrompts);
            }
            else if (answers.HasPromptAnswer(reinstateExplanationPromptId) && IsPromptAnswerComplete(answers, reinstateExplanationPromptId))
            {
                return new AdjustmentPromptAnalysis( new CompletedStudentAdjustment(student.StudentID,
                    inclusionReasonId,
                    answers,
                    Contants.SCRUTINY_REASON_REINSTATE_PUPIL,
                    null,
                    Contants.SCRUTINY_STATUS_PENDINGFORVUS,
                    null)
                    );
            }
            else
            {
                throw Web09Exception.GetBusinessException(Web09MessageList.InsufficientStudentAdjustmentPromptAnswerProvided);
            }
        }
    }
}
