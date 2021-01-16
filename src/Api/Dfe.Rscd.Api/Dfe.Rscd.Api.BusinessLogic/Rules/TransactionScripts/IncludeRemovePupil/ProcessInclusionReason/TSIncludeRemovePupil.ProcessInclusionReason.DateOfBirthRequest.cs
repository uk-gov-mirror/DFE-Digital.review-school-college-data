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

        internal static AdjustmentPromptAnalysis ProcessDOBAdjustmentRequest(Students student, int? inclusionReasonId, PromptAnswerList answers)
        {
            if(student.Cohorts == null)
                throw Web09Exception.GetBusinessException(Web09MessageList.KeystageInvalid);

            switch (student.Cohorts.KeyStage)
            {
                case(2):
                    return ProcessKS2DOBAdjustmentRequest(student, answers);
                case(3):
                    throw Web09Exception.GetBusinessException(Web09MessageList.InvalidKS3StudentAdjustmentRequest);
                case (4):
                    return ProcessKS4DOBAdjustmentRequest(student, inclusionReasonId, answers);
                case (5) :
                    return ProcessKS5DOBAdjustmentRequest(student, inclusionReasonId, answers);
                default:
                    throw Web09Exception.GetBusinessException(Web09MessageList.KeystageInvalid);
            }
        }

        private static AdjustmentPromptAnalysis ProcessKS2DOBAdjustmentRequest(Students student, PromptAnswerList answers)
        {
            if (student.PINCLs == null || student.PINCLs.P_INCL == null)
                throw Web09Exception.GetBusinessException(Web09MessageList.InsufficientStudentDetails);

            if (!answers.HasPromptAnswer(Contants.PROMPT_ID_DOB_KS2))
            {
                List<Prompts> furtherPrompts = new List<Prompts>();
                furtherPrompts.Add(GetPromptByPromptID(Contants.PROMPT_ID_DOB_KS2));
                return new AdjustmentPromptAnalysis(furtherPrompts);
            }
            else if (answers.HasPromptAnswer(Contants.PROMPT_ID_DOB_KS2) && IsPromptAnswerComplete(answers, Contants.PROMPT_ID_DOB_KS2))
            {

                string studentPincl = student.PINCLs.P_INCL;

                if (studentPincl.Equals("201"))
                {
                    return new AdjustmentPromptAnalysis(new CompletedNonStudentAdjustment(GetInfoPromptText(21510)));
                }
                else if (studentPincl.Equals("202"))
                {
                    return new AdjustmentPromptAnalysis(new CompletedNonStudentAdjustment(GetInfoPromptText(21520)));
                }
                else
                {
                    return new AdjustmentPromptAnalysis(new CompletedNonStudentAdjustment("No request has been generated for this DOB change."));
                }

            }
            else //Prompt provided, no answer has been given.
            {
                throw Web09Exception.GetBusinessException(Web09MessageList.InsufficientStudentAdjustmentPromptAnswerProvided);
            }

        }

        private static AdjustmentPromptAnalysis ProcessKS4DOBAdjustmentRequest(Students student, int? inclusionReasonId, PromptAnswerList answers)
        {

            if (student.PINCLs == null || student.PINCLs.P_INCL == null)
                throw Web09Exception.GetBusinessException(Web09MessageList.InsufficientStudentDetails);

            if(!answers.HasPromptAnswer(Contants.PROMPT_ID_DOB_KS4))
            {
                List<Prompts> furtherPrompts = new List<Prompts>();
                furtherPrompts.Add(GetPromptByPromptID(Contants.PROMPT_ID_DOB_KS4));
                return new AdjustmentPromptAnalysis(furtherPrompts);
            }
            else if(answers.HasPromptAnswer(Contants.PROMPT_ID_DOB_KS4) && IsPromptAnswerComplete(answers, Contants.PROMPT_ID_DOB_KS4))
            {

                DateTime newDob = answers.GetPromptAnswerByPromptID(Contants.PROMPT_ID_DOB_KS4).PromptDateTimeAnswer.Value;
                string studentPincl = student.PINCLs.P_INCL;
            
                int studentAgeInYears = TSStudent.CalculateStudentAge(newDob);
                if( studentPincl.Equals("403") && studentAgeInYears != 16)
                {
                    return new AdjustmentPromptAnalysis(new CompletedStudentAdjustment(
                        student.StudentID,
                        inclusionReasonId,
                        answers,
                        Contants.SCRUTINY_REASON_APPEAL_ADD_BACK,
                        null,
                        Contants.SCRUTINY_STATUS_PENDINGFORVUS,
                        GetInfoPromptText(1510))
                        );
                }
                else if ((studentPincl.Equals("406") || studentPincl.Equals("424")) && studentAgeInYears != 15)
                {    
                    return new AdjustmentPromptAnalysis(new CompletedStudentAdjustment(
                        student.StudentID,
                        inclusionReasonId,
                        answers,
                        Contants.SCRUTINY_REASON_DOB_CHANGE,
                        null,
                        Contants.SCRUTINY_STATUS_ACCEPTEDAUTOMATICALLY,
                        GetInfoPromptText(1520))
                        );
                }
                else
                {
                    return new AdjustmentPromptAnalysis(new CompletedNonStudentAdjustment("No request has been generated for this DOB change."));
                }

            }
            else //Prompt provided, no answer has been given.
            {
                throw Web09Exception.GetBusinessException(Web09MessageList.InsufficientStudentAdjustmentPromptAnswerProvided);
            }

        }

        private static AdjustmentPromptAnalysis ProcessKS5DOBAdjustmentRequest(Students student, int? inclusionReasonId, PromptAnswerList answers)
        {

            if (!answers.HasPromptAnswer(Contants.PROMPT_ID_DOB_KS5))
            {
                List<Prompts> furtherPrompts = new List<Prompts>();
                furtherPrompts.Add(GetPromptByPromptID(Contants.PROMPT_ID_DOB_KS5));
                return new AdjustmentPromptAnalysis(furtherPrompts);
            }
            else if (answers.HasPromptAnswer(Contants.PROMPT_ID_DOB_KS5) && IsPromptAnswerComplete(answers, Contants.PROMPT_ID_DOB_KS5))
            {

                DateTime newDob = answers.GetPromptAnswerByPromptID(Contants.PROMPT_ID_DOB_KS5).PromptDateTimeAnswer.Value;
                int studentAgeInYears = TSStudent.CalculateStudentAge(newDob);

                if (studentAgeInYears >= 19)
                {
                    return new AdjustmentPromptAnalysis(new CompletedStudentAdjustment(
                        student.StudentID,
                        inclusionReasonId,
                        answers,
                        Contants.SCRUTINY_REASON_DOB_CHANGE,
                        null,
                        Contants.SCRUTINY_STATUS_ACCEPTEDAUTOMATICALLY,
                        GetInfoPromptText(5310))
                        );
                }
                else if (studentAgeInYears < 16)
                {
                    return new AdjustmentPromptAnalysis(new CompletedStudentAdjustment(
                        student.StudentID,
                        inclusionReasonId,
                        answers,
                        Contants.SCRUTINY_REASON_DOB_CHANGE,
                        null,
                        Contants.SCRUTINY_STATUS_ACCEPTEDAUTOMATICALLY,
                        GetInfoPromptText(5320))
                        );
                }
                else
                {
                    return new AdjustmentPromptAnalysis(new CompletedNonStudentAdjustment(null));
                }

            }
            else //Prompt provided, no answer has been given.
            {
                throw Web09Exception.GetBusinessException(Web09MessageList.InsufficientStudentAdjustmentPromptAnswerProvided);
            }
        }
    }
}
