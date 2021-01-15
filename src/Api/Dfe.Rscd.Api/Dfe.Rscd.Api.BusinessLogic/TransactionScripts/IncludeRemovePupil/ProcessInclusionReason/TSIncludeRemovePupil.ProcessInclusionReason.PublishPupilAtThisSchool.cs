using System.Collections.Generic;
using Web09.Checking.Business.Logic.Common;
using Web09.Checking.Business.Logic.Entities;
using Web09.Checking.DataAccess;
using Web09.Common.Exceptions;

namespace Web09.Checking.Business.Logic.TransactionScripts
{
    public partial class TSIncludeRemovePupil
    {

        public static AdjustmentPromptAnalysis ProcessInclusionPromptResponses_PublishPupilAtThisSchool(int studentId, int inclusionReasonId, PromptAnswerList answers)
        {
            if (!answers.HasPromptAnswer(600))
            {
                List<Prompts> furtherPrompts = new List<Prompts>();
                furtherPrompts.Add(GetPromptByPromptID(600));
                return new AdjustmentPromptAnalysis(furtherPrompts);
            }
            else if(answers.HasPromptAnswer(600) && IsPromptAnswerComplete(answers, 600))
            {
                return new AdjustmentPromptAnalysis( new CompletedStudentAdjustment(studentId,
                    inclusionReasonId,
                    answers,
                    Contants.SCRUTINY_REASON_ADD_DUAL_REG_PUPIL,
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
