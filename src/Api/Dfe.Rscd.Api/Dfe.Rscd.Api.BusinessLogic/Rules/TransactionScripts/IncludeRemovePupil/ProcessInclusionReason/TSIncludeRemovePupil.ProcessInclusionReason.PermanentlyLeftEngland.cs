using System;
using System.Collections.Generic;
using Web09.Checking.Business.Logic.Common;
using Web09.Checking.Business.Logic.Entities;
using Web09.Checking.DataAccess;
using Web09.Common.Exceptions;

namespace Web09.Checking.Business.Logic.TransactionScripts
{
    public partial class TSIncludeRemovePupil
    {
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public static AdjustmentPromptAnalysis ProcessInclusionPromptResponses_PermanentlyLeftEngland(Students student, int inclusionReasonId, PromptAnswerList answers)
        {
            if (student == null || student.Cohorts == null)
                throw Web09Exception.GetBusinessException(Web09MessageList.InsufficientStudentDetails);

            DateTime offRoleDate;

            if(!answers.HasPromptAnswer(1102) || !IsPromptAnswerComplete(answers, 1102))
            {
                throw Web09Exception.GetBusinessException(Web09MessageList.InsufficientStudentAdjustmentPromptAnswerProvided);
            }
            else 
            {
                offRoleDate = answers.GetPromptAnswerByPromptID(1102).PromptDateTimeAnswer.Value;

                //If selected country is other, prompt for the name:
                if(IsSelectedCountryOrLanguageOther(answers.GetPromptAnswerByPromptID(1101)))
                {
                     if (!answers.HasPromptAnswer(1103))
                        {
                            List<Prompts> furtherPrompts = new List<Prompts>();
                            furtherPrompts.Add(GetPromptByPromptID(1103));
                            return new AdjustmentPromptAnalysis(furtherPrompts);
                        }
                        else if (answers.HasPromptAnswer(804) && !IsPromptAnswerComplete(answers, 804))
                        {
                            throw Web09Exception.GetBusinessException(Web09MessageList.InsufficientStudentAdjustmentPromptAnswerProvided);
                        }
                }

                //  ITEM 11.10 REMOVED FROM REQUIREMENTS
                //if (student.Cohorts.KeyStage == 2 && offRoleDate > KS2TestStartDate)
                //{
                //    return ProcessSingularFurtherPrompt(1110, 
                //        student.StudentID, 
                //        inclusionReasonId, 
                //        answers,
                //        Contants.SCRUTINY_REASON_EMIGRATED,
                //        null,
                //        Contants.SCRUTINY_STATUS_PENDINGFORVUS,
                //        null);
                //}

                if (offRoleDate < AnnualSchoolCensusDate)
                {
                    return ProcessSingularFurtherPrompt(1120,
                        student.StudentID,
                        inclusionReasonId,
                        answers,
                        Contants.SCRUTINY_REASON_EMIGRATED,
                        null,
                        Contants.SCRUTINY_STATUS_PENDINGFORVUS,
                        null);
                }
                else if (!TSStudent.IsStudentListed(student.StudentID))
                {
                    return new AdjustmentPromptAnalysis(new CompletedStudentAdjustment(
                        student.StudentID,
                        inclusionReasonId,
                        answers,
                        Contants.SCRUTINY_REASON_EMIGRATED,
                        null,
                        Contants.SCRUTINY_STATUS_PENDINGFORVUS,
                        GetInfoPromptText(1130))
                        );
                }
                else //other => Accepted
                {
                    return new AdjustmentPromptAnalysis(new CompletedStudentAdjustment(
                        student.StudentID,
                        inclusionReasonId,
                        answers,
                        Contants.SCRUTINY_REASON_EMIGRATED,
                        null,
                        Contants.SCRUTINY_STATUS_ACCEPTEDAUTOMATICALLY,
                        null)
                        );
                }
                
            }
        }

    }
}
