using System.Collections.Generic;
using Web09.Checking.Business.Logic.Common;
using Web09.Checking.Business.Logic.Entities;
using Web09.Checking.DataAccess;
using Web09.Common.Exceptions;

namespace Web09.Checking.Business.Logic.TransactionScripts
{
    public partial class TSIncludeRemovePupil
    {
        public static AdjustmentPromptAnalysis ProcessInclusionPromptResponses_RemovePupilCompletelyFromAllData(int studentId, int inclusionReasonId, PromptAnswerList answers)
        {
            if (!answers.HasPromptAnswer(500))
            {
                List<Prompts> furtherPrompts = new List<Prompts>();
                furtherPrompts.Add(GetPromptByPromptID(500));
                return new AdjustmentPromptAnalysis(furtherPrompts);
            }
            else if( answers.HasPromptAnswer(500) && IsPromptAnswerComplete(answers, 500))
            {
                return new AdjustmentPromptAnalysis( new CompletedStudentAdjustment(studentId,
                    inclusionReasonId,
                    answers,
                    Contants.SCRUTINY_REASON_REMOVE_COMPLETELY,
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
