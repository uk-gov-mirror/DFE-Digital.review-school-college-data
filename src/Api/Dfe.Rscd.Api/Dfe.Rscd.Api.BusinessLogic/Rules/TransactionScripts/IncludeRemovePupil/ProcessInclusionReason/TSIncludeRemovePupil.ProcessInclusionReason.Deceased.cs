using System;
using Web09.Checking.Business.Logic.Common;
using Web09.Checking.Business.Logic.Entities;
using Web09.Common.Exceptions;

namespace Web09.Checking.Business.Logic.TransactionScripts
{
    public partial class TSIncludeRemovePupil
    {
        
        private static AdjustmentPromptAnalysis ProcessInclusionPromptResponses_Deceased(int inclusionReasonId, PromptAnswerList promptAnswers, int studentId)
        {
            PromptAnswer answer = promptAnswers.GetPromptAnswerByPromptID(Contants.PROMPT_ID_DATE_OF_DEATH);
            
            if (answer.PromptDateTimeAnswer.HasValue)
            {

                DateTime dateOfDeath = answer.PromptDateTimeAnswer.Value;

                if (dateOfDeath < AnnualSchoolCensusDate)
                {
                    return ProcessSingularFurtherPrompt(1210,
                        studentId,
                        inclusionReasonId,
                        promptAnswers,
                        Contants.SCRUTINY_REASON_DEATH,
                        null,
                        Contants.SCRUTINY_STATUS_PENDINGFORVUS,
                        null);
                }
                else if (dateOfDeath <= DateTime.Now && dateOfDeath >= AnnualSchoolCensusDate)
                {
                    //The adjustment reason is complete, submit to database with a scrutiny status of accept.
                    return new AdjustmentPromptAnalysis(new CompletedStudentAdjustment(
                        studentId,
                        inclusionReasonId,
                        promptAnswers,
                        Contants.SCRUTINY_REASON_DEATH,
                        null,
                        Contants.SCRUTINY_STATUS_ACCEPTEDAUTOMATICALLY,
                        null)
                        );
                }
                else
                {
                    //Reject the request it is after the current date
                    return new AdjustmentPromptAnalysis(new CompletedStudentAdjustment(
                        studentId,
                        inclusionReasonId,
                        promptAnswers,
                        Contants.SCRUTINY_REASON_DEATH,
                        Contants.REJECTION_REASON_OTHER,
                        Contants.SCRUTINY_STATUS_REJECT,
                        null)
                        );
                }
            }
            else
            {
                throw Web09Exception.GetBusinessException(Web09MessageList.InvalidDateTimeAdjustmentPromptAnswer);
            }

        }
    }
}
