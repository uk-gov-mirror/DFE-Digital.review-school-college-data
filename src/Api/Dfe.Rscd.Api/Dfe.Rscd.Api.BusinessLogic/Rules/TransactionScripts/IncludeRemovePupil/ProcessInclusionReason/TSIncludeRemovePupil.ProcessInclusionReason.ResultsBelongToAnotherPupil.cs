using System.Collections.Generic;
using Web09.Checking.Business.Logic.Common;
using Web09.Checking.Business.Logic.Entities;
using Web09.Checking.DataAccess;
using Web09.Common.Exceptions;

namespace Web09.Checking.Business.Logic.TransactionScripts
{
    public partial class TSIncludeRemovePupil
    {

        private static AdjustmentPromptAnalysis ProcessInclusionPromptResponses_ResultsBelongToAnotherPupil(Students student, int inclusionReasonId, PromptAnswerList answers)
        {
            if (student == null || student.Cohorts == null)
                throw Web09Exception.GetBusinessException(Web09MessageList.InsufficientStudentDetails);

            int forvusIndexNumberPrompt;

            if (student.Cohorts.KeyStage == 2)
                forvusIndexNumberPrompt = Contants.PROMPT_ID_RESULTS_BELONG_TO_ANOTHER_PUPIL_KS2;
            else if (student.Cohorts.KeyStage == 3)
                forvusIndexNumberPrompt = Contants.PROMPT_ID_RESULTS_BELONG_TO_ANOTHER_PUPIL_KS3;
            else if (student.Cohorts.KeyStage == 4)
                forvusIndexNumberPrompt = Contants.PROMPT_ID_RESULTS_BELONG_TO_ANOTHER_PUPIL_KS4;
            else //key stage == 5
                forvusIndexNumberPrompt = Contants.PROMPT_ID_RESULTS_BELONG_TO_ANOTHER_PUPIL_KS5;

            if (!answers.HasPromptAnswer(forvusIndexNumberPrompt))
            {
                List<Prompts> furtherPrompts = new List<Prompts>();
                furtherPrompts.Add(GetPromptByPromptID(forvusIndexNumberPrompt));
                return new AdjustmentPromptAnalysis(furtherPrompts);
            }
            else if (answers.HasPromptAnswer(forvusIndexNumberPrompt) &&
                IsPromptAnswerComplete(answers, forvusIndexNumberPrompt))
            {
                return new AdjustmentPromptAnalysis(new CompletedStudentAdjustment(
                    student.StudentID,
                    inclusionReasonId,
                    answers,
                    Contants.SCRUTINY_REASON_MERGE_PUPILS,
                    null,
                    Contants.SCRUTINY_STATUS_PENDINGFORVUS,
                    null));

            }
            else
            {
                throw Web09Exception.GetBusinessException(Web09MessageList.InsufficientStudentAdjustmentPromptAnswerProvided);
            }
        }

    }
}
