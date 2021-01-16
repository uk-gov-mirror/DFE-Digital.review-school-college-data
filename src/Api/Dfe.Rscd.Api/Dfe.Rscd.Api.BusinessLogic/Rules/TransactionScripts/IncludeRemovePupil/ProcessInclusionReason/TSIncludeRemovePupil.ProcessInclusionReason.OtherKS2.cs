using Web09.Checking.Business.Logic.Common;
using Web09.Checking.Business.Logic.Entities;
using Web09.Common.Exceptions;

namespace Web09.Checking.Business.Logic.TransactionScripts
{
    public partial class TSIncludeRemovePupil
    {

        /// <summary>
        /// Process the answer provided for inclusion/removal reason of Other for a key stage 2 PINCL
        /// </summary>
        /// <param name="inclusionReasonId">inclusionReasonId</param>
        /// <param name="promptAnswers">promptAnswers. List of PromptAnswers</param>
        /// <param name="studentId">studentId</param>
        /// <throws>ArgumentException. However, logically impossible for valid date to throw exception</throws>
        /// <returns>Either: List of Prompts<see cref="Web09.Checking.DataAccess.Prompts"/> to be completed by UI or a CompletedStudentAdjustment</returns>
        public static AdjustmentPromptAnalysis ProcessInclusionPromptResponses_OtherKS2(int inclusionReasonId, PromptAnswerList promptAnswers, int studentId)
        {
            // Collect descriptive text answer from prompt
            if (promptAnswers.HasPromptAnswer(21900) && IsPromptAnswerComplete(promptAnswers, 21900))
            {
                return new AdjustmentPromptAnalysis(new CompletedStudentAdjustment(studentId,
                    inclusionReasonId,
                    promptAnswers,
                    Contants.SCRUTINY_REASON_OTHER,
                    null,
                    Contants.SCRUTINY_STATUS_PENDINGFORVUS,
                    null)
                    );

            }
            else
            {
                //Error with answer not found.
                throw Web09Exception.GetBusinessException(Web09MessageList.InsufficientStudentAdjustmentPromptAnswerProvided);
            }
        }
    }
}
