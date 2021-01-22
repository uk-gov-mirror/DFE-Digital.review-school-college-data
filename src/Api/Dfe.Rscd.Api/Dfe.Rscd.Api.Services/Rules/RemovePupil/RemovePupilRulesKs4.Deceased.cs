using System;
using System.Collections.Generic;
using Dfe.Rscd.Api.BusinessLogic.Common;
using Dfe.Rscd.Api.BusinessLogic.Entities;

namespace Dfe.Rscd.Api.Services.Rules
{
    public partial class RemovePupilRulesKs4
    {
        
        private static AmendmentOutcome ProcessInclusionPromptResponses_Deceased(int inclusionReasonId, List<PromptAnswer> promptAnswers, string studentId)
        {
            //PromptAnswer answer = promptAnswers.GetPromptAnswerByPromptId(Contants.PROMPT_ID_DATE_OF_DEATH);
            
            //if (answer.PromptDateTimeAnswer.HasValue)
            //{

            //    DateTime dateOfDeath = answer.PromptDateTimeAnswer.Value;

            //    if (dateOfDeath < AnnualSchoolCensusDate)
            //    {
            //        return ProcessSingularFurtherPrompt(1210,
            //            studentId,
            //            inclusionReasonId,
            //            promptAnswers,
            //            Contants.SCRUTINY_REASON_DEATH,
            //            null,
            //            Contants.SCRUTINY_STATUS_PENDINGFORVUS,
            //            null);
            //    }
            //    else if (dateOfDeath <= DateTime.Now && dateOfDeath >= AnnualSchoolCensusDate)
            //    {
            //        //The adjustment reason is complete, submit to database with a scrutiny status of accept.
            //        return new AmendmentOutcome(new CompletedStudentAdjustment(
            //            studentId,
            //            inclusionReasonId,
            //            promptAnswers,
            //            Contants.SCRUTINY_REASON_DEATH,
            //            null,
            //            Contants.SCRUTINY_STATUS_ACCEPTEDAUTOMATICALLY,
            //            null)
            //            );
            //    }
            //    else
            //    {
            //        //Reject the request it is after the current date
            //        return new AmendmentOutcome(new CompletedStudentAdjustment(
            //            studentId,
            //            inclusionReasonId,
            //            promptAnswers,
            //            Contants.SCRUTINY_REASON_DEATH,
            //            Contants.REJECTION_REASON_OTHER,
            //            Contants.SCRUTINY_STATUS_REJECT,
            //            null)
            //            );
            //    }
            //}
            //else
            //{
            //    throw Web09Exception.GetBusinessException(Web09MessageList.InvalidDateTimeAdjustmentPromptAnswer);
            //}


            return new AmendmentOutcome(
                new CompletedStudentAdjustment(studentId, 
                    inclusionReasonId, 
                    promptAnswers, 
                    2, 
                    null, 
                    Constants.SCRUTINY_STATUS_ACCEPTEDAUTOMATICALLY, 
                    "Accepted Automatically", 
                    OutcomeStatus.AutoAccept));
        }
    }
}
