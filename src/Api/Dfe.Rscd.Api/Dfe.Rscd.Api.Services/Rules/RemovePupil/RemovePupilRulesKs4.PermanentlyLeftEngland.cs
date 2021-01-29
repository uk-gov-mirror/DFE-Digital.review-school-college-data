using System.Collections.Generic;
using Dfe.Rscd.Api.BusinessLogic.Common;
using Dfe.Rscd.Api.BusinessLogic.Entities;

namespace Dfe.Rscd.Api.Services.Rules
{
    public partial class RemovePupilRulesKs4
    {
        private static AmendmentOutcome ProcessInclusionPromptResponses_PermanentlyLeftEngland(int inclusionReasonId, List<PromptAnswer> promptAnswers, string studentId)
        {
            
            //if (student == null || student.Cohorts == null)
            //    throw Web09Exception.GetBusinessException(Web09MessageList.InsufficientStudentDetails);

            //DateTime offRoleDate;

            //if(!answers.HasPromptAnswer(1102) || !IsPromptAnswerComplete(answers, 1102))
            //{
            //    throw Web09Exception.GetBusinessException(Web09MessageList.InsufficientStudentAdjustmentPromptAnswerProvided);
            //}
            //else 
            //{
            //    offRoleDate = answers.GetPromptAnswerByPromptID(1102).PromptDateTimeAnswer.Value;

            //    //If selected country is other, prompt for the name:
            //    if(IsSelectedCountryOrLanguageOther(answers.GetPromptAnswerByPromptID(1101)))
            //    {
            //         if (!answers.HasPromptAnswer(1103))
            //            {
            //                List<Prompts> furtherPrompts = new List<Prompts>();
            //                furtherPrompts.Add(GetPromptByPromptID(1103));
            //                return new AdjustmentPromptAnalysis(furtherPrompts);
            //            }
            //            else if (answers.HasPromptAnswer(804) && !IsPromptAnswerComplete(answers, 804))
            //            {
            //                throw Web09Exception.GetBusinessException(Web09MessageList.InsufficientStudentAdjustmentPromptAnswerProvided);
            //            }
            //    }

            //    if (offRoleDate < AnnualSchoolCensusDate)
            //    {
            //        return ProcessSingularFurtherPrompt(1120,
            //            student.StudentID,
            //            inclusionReasonId,
            //            answers,
            //            Contants.SCRUTINY_REASON_EMIGRATED,
            //            null,
            //            Contants.SCRUTINY_STATUS_PENDINGFORVUS,
            //            null);
            //    }
            //    else if (!TSStudent.IsStudentListed(student.StudentID))
            //    {
            //        return new AdjustmentPromptAnalysis(new CompletedStudentAdjustment(
            //            student.StudentID,
            //            inclusionReasonId,
            //            answers,
            //            Contants.SCRUTINY_REASON_EMIGRATED,
            //            null,
            //            Contants.SCRUTINY_STATUS_PENDINGFORVUS,
            //            GetInfoPromptText(1130))
            //            );
            //    }
            //    else //other => Accepted
            //    {
            //        return new AdjustmentPromptAnalysis(new CompletedStudentAdjustment(
            //            student.StudentID,
            //            inclusionReasonId,
            //            answers,
            //            Contants.SCRUTINY_REASON_EMIGRATED,
            //            null,
            //            Contants.SCRUTINY_STATUS_ACCEPTEDAUTOMATICALLY,
            //            null)
            //            );
            //    }
                


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
