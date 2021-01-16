using System;
using System.Collections.Generic;
using System.Data.EntityClient;
using System.Linq;
using Web09.Checking.Business.Logic.Common;
using Web09.Checking.Business.Logic.Entities;
using Web09.Checking.DataAccess;
using Web09.Common.Exceptions;

namespace Web09.Checking.Business.Logic.TransactionScripts
{
    public partial class TSIncludeRemovePupil : Logic.TSBase
    {

        private static AdjustmentPromptAnalysis ProcessInclusionPromptResponses_AdmittedFromAbroad(int dfesNumber, Students student, int inclusionReasonId, PromptAnswerList promptAnswers)
        {
            // TFS 28853
            int scrutinyReasonID = Contants.SCRUTINY_REASON_RECENTLY_FROM_ABROAD;       
            if (student != null && student.Cohorts != null && student.PINCLs != null &&
                student.Cohorts.KeyStage == 2 && 
                student.PINCLs.P_INCL.Equals(Contants.PINCL_UNLISTED_PUPIL_WITH_ADD_PUPIL_REQUEST_PLASC_KS2) &&
                inclusionReasonId == Contants.INCLUSION_ADJUSTMENT_REASON_WAS_PUPIL_WAS_ADMITTED_FROM_ABROAD_WITH_ENGLISH_NOT_FIRST_LANGUAGE)
            {
                scrutinyReasonID = Contants.SCRUTINY_REASON_ADD_UNLISTED_OVERSEAS_PUPIL;
            }

            List<Prompts> FurtherPrompts = new List<Prompts>();
            
            using (EntityConnection conn = new EntityConnection("name=Web09_Entities"))
            {
                conn.Open();

                using (Web09_Entities context = new Web09_Entities())
                {

                    DateTime? admissionDate = null;
                    short keyStage = 0;
                    
                    if (IsPromptAnswerComplete(promptAnswers, Contants.PROMPT_ID_LANGUAGE) &&
                            IsPromptAnswerComplete(promptAnswers, Contants.PROMPT_ID_COUNTRY))
                    {

                        //If the user selected a country or language of other, ensure they have been prompted to type in which
                        //language and/or country.
                        bool promptForVerboseCountryOrLanguage = false;
                        if (IsSelectedCountryOrLanguageOther(context, promptAnswers.GetPromptAnswerByPromptID(Contants.PROMPT_ID_LANGUAGE)))
                        {
                            if (!promptAnswers.HasPromptAnswer(Contants.PROMPT_ID_LANGUAGE_OTHER))
                            {
                                FurtherPrompts.Add(GetPromptByPromptID(Contants.PROMPT_ID_LANGUAGE_OTHER));
                                promptForVerboseCountryOrLanguage = true;
                            }
                            else if (!IsPromptAnswerComplete(promptAnswers, Contants.PROMPT_ID_LANGUAGE_OTHER))
                            {
                                Web09Exception.GetBusinessException(Web09MessageList.InsufficientStudentAdjustmentPromptAnswerProvided);
                            }
                        }
                        if (IsSelectedCountryOrLanguageOther(context, promptAnswers.GetPromptAnswerByPromptID(Contants.PROMPT_ID_COUNTRY)))
                        {
                            if (!promptAnswers.HasPromptAnswer(Contants.PROMPT_ID_COUNTRY_OTHER))
                            {
                                FurtherPrompts.Add(GetPromptByPromptID(Contants.PROMPT_ID_COUNTRY_OTHER));
                                promptForVerboseCountryOrLanguage = true;
                            }
                            else if (!IsPromptAnswerComplete(promptAnswers, Contants.PROMPT_ID_COUNTRY_OTHER))
                            {
                                Web09Exception.GetBusinessException(Web09MessageList.InsufficientStudentAdjustmentPromptAnswerProvided);
                            }
                        }
                        //If further prompts are required for a language and/or country selection of other,
                        //return those new prompts.
                        if (promptForVerboseCountryOrLanguage)
                        {
                            return new AdjustmentPromptAnalysis(FurtherPrompts);
                        }

                        //Get the response to Admission Date
                        int admissionDatePromptID = 0;
                        DateTime KSTestEndDate = new DateTime();

                        if (student.Schools == null || student.Cohorts == null)
                            throw Web09Exception.GetBusinessException(Web09MessageList.InsufficientStudentDetails);

                        if (TSSchool.IsSchoolNonPlasc(student.Schools.DFESNumber))
                        {
                            admissionDatePromptID = Contants.PROMPT_ID_JOIN_ROLL_DATE;
                        }
                        else
                        {
                            if (student.Cohorts.KeyStage == 2 && student.PINCLs.P_INCL != Contants.PINCL_UNLISTED_PUPIL_WITH_ADD_PUPIL_REQUEST_PLASC_KS2)
                            {
                                admissionDatePromptID = Contants.PROMPT_ID_REVISED_ADMISSION_DATE_IF_AVAILABLE;
                            }
                        }
                            

                        switch (student.Cohorts.KeyStage)
                        {
                            case (2):
                                keyStage = 2;
                                KSTestEndDate = GetKS2TestEndDate(context);
                                break;
                            case (3):
                                keyStage = 3;
                                KSTestEndDate = GetKS3TestEndDate(context);
                                break;
                            case (4):
                                keyStage = 4;
                                break;
                            default:
                                //Should not occur, this reason only available for KS2, KS3 and KS4
                                throw new Exception("Invalid PINCL");
                        }

                        //Ensure admision date has been collected. No analysis on this date is required.
                        if (admissionDatePromptID > 0)
                        {
                            if (!promptAnswers.HasPromptAnswer(admissionDatePromptID))
                            {
                                var furtherPrompts = new List<Prompts> {GetPromptByPromptID(admissionDatePromptID)};
                                return new AdjustmentPromptAnalysis(furtherPrompts);
                            }
                            if (IsPromptAnswerComplete(promptAnswers, admissionDatePromptID) &&
                                (promptAnswers.GetPromptAnswerByPromptID(admissionDatePromptID)).PromptDateTimeAnswer.
                                    HasValue)
                            {
                                admissionDate = promptAnswers.GetPromptAnswerByPromptID(admissionDatePromptID).PromptDateTimeAnswer;
                            }
                            else if (!(GetPromptByPromptID(admissionDatePromptID)).AllowNulls)
                            {
                                //The prompt does not allow nulls, but no answer has been provided.
                                throw Web09Exception.GetBusinessException(
                                    Web09MessageList.InsufficientStudentAdjustmentPromptAnswerProvided);
                            }
                        }

                        //Retrieve required existing DB values for the student.
                        DateTime? plascAdmissionDate = TSStudent.GetCurrentStudentAdmissionDate(context, student.StudentID);

                        if (!admissionDate.HasValue)
                        {
                            admissionDate = plascAdmissionDate;
                        }

                        Languages plascFirstLanguage = TSStudent.GetCurrentFirstLanguage(context, student.StudentID);

                        if(!IsSelectedCountryOrLanguageAcceptable(context, promptAnswers.GetPromptAnswerByPromptID(Contants.PROMPT_ID_COUNTRY)))
                        {
                            return new AdjustmentPromptAnalysis(new CompletedStudentAdjustment(student.StudentID,
                                inclusionReasonId,
                                promptAnswers,
                               scrutinyReasonID,
                                null,
                                Contants.SCRUTINY_STATUS_PENDINGFORVUS,
                                "The country not on the accept list. ")
                                );
                        }
                        if (admissionDate.HasValue && (admissionDate.Value < new DateTime(CurrentYear - 2, 6, 1)))
                        {
                            return new AdjustmentPromptAnalysis(new CompletedStudentAdjustment(student.StudentID,
                                                                                               inclusionReasonId,
                                                                                               promptAnswers,
                                                                                               scrutinyReasonID,
                                                                                               null,
                                                                                               Contants.SCRUTINY_STATUS_PENDINGFORVUS,
                                                                                                "Admission date is before 1st June " + (CurrentYear - 2).ToString())
                                );
                        }
                        if (admissionDate.HasValue && keyStage == 2 && admissionDate.Value > KS2TestStartDate)
                        {
                            return new AdjustmentPromptAnalysis(new CompletedStudentAdjustment(student.StudentID,
                                                                                               inclusionReasonId,
                                                                                               promptAnswers,
                                                                                               scrutinyReasonID,
                                                                                               null,
                                                                                               Contants.SCRUTINY_STATUS_PENDINGFORVUS,
                                                                                               "The admission date is after the start of tests. ")
                                );
                        }
                        if (admissionDate.HasValue && keyStage == 4 && admissionDate.Value > AnnualSchoolCensusDate)
                        {
                            return new AdjustmentPromptAnalysis(new CompletedStudentAdjustment(student.StudentID,
                                                                                               inclusionReasonId,
                                                                                               promptAnswers,
                                                                                               scrutinyReasonID,
                                                                                               null,
                                                                                               Contants.SCRUTINY_STATUS_PENDINGFORVUS,
                                                                                               "The admission date is after January census. ")
                                );
                        }
                        if (!IsSelectedCountryOrLanguageAcceptable(context, promptAnswers.GetPromptAnswerByPromptID(Contants.PROMPT_ID_LANGUAGE)))
                        {
                            return new AdjustmentPromptAnalysis(new CompletedStudentAdjustment(student.StudentID,
                                                                                               inclusionReasonId,
                                                                                               promptAnswers,
                                                                                               scrutinyReasonID,
                                                                                               null,
                                                                                               Contants.SCRUTINY_STATUS_PENDINGFORVUS,
                                                                                               "The language not on the accept list. ")
                                );
                        }

                        
                       
                        if ((promptAnswers.GetPromptAnswerByPromptID(Contants.PROMPT_ID_DATE_OF_ARRIVAL)).PromptDateTimeAnswer.HasValue &&
                            (promptAnswers.GetPromptAnswerByPromptID(Contants.PROMPT_ID_DATE_OF_ARRIVAL)).PromptDateTimeAnswer.Value < ACSDate.AddYears(-2)) // TFS 22667
                        {
                            return new AdjustmentPromptAnalysis(new CompletedStudentAdjustment(student.StudentID,
                                                                                               inclusionReasonId,
                                                                                               promptAnswers,
                                                                                               scrutinyReasonID,
                                                                                               null,
                                                                                               Contants.SCRUTINY_STATUS_PENDINGFORVUS,
                                                                                               "UK Arrival Date more than two years before ASC date. ")
                                );
                        }

                       
                        if (TSStudent.HasPriorResults(context, student.StudentID, (short)keyStage))
                        {
                            return new AdjustmentPromptAnalysis(new CompletedStudentAdjustment(student.StudentID,
                                                                                               inclusionReasonId,
                                                                                               promptAnswers,
                                                                                               scrutinyReasonID,
                                                                                               null,
                                                                                               Contants.SCRUTINY_STATUS_PENDINGFORVUS,
                                                                                               "Prior key stage test results found. "
                                                                    ));
                        }
                        if (TSStudent.IsCurrentAttainmentLev2ForEnglishAndMaths(context, student.StudentID))
                        {
                            return new AdjustmentPromptAnalysis(new CompletedStudentAdjustment(student.StudentID,
                                                                                               inclusionReasonId,
                                                                                               promptAnswers,
                                                                                               scrutinyReasonID,
                                                                                               null,
                                                                                               Contants.SCRUTINY_STATUS_PENDINGFORVUS,
                                                                                               "The current attainment is at level 2 including English and Maths. ")
                                );

                        }
                        
                        if (plascFirstLanguage != null && (new String[] { "ENB", "ENG" }).Contains(plascFirstLanguage.LanguageCode))
                        {
                            //The currently assigned first language is either ENG or ENB => Refer to DCSf
                            return new AdjustmentPromptAnalysis(new CompletedStudentAdjustment(student.StudentID,
                                                                                               inclusionReasonId,
                                                                                               promptAnswers,
                                                                                               scrutinyReasonID,
                                                                                               null,
                                                                                               Contants.SCRUTINY_STATUS_PENDINGDCSF,
                                                                                               "First Language Code is ENG or ENB. ")
                                );
                        }
                        if (plascAdmissionDate.HasValue && plascAdmissionDate.Value < new DateTime(CurrentYear - 2, 6, 1))
                        {
                            //Admission date currently assigned in the database (not entered as a prompt) 
                            //is prior to 1 July ([table year]-2)
                            //THEREFORE: Refer to DCSF
                            return new AdjustmentPromptAnalysis(new CompletedStudentAdjustment(student.StudentID,
                                                                                               inclusionReasonId,
                                                                                               promptAnswers,
                                                                                               scrutinyReasonID,
                                                                                               null,
                                                                                               Contants.SCRUTINY_STATUS_PENDINGDCSF,
                                                                                               "Census admission date is before 1st June " + (CurrentYear - 2).ToString() )
                                );
                        }
                        if (!TSStudent.IsStudentListed(student.StudentID))
                        {
                            //Refer to RM
                            return new AdjustmentPromptAnalysis(new CompletedStudentAdjustment(student.StudentID,
                                                                                               inclusionReasonId,
                                                                                               promptAnswers,
                                                                                               scrutinyReasonID,
                                                                                               null,
                                                                                               Contants.SCRUTINY_STATUS_PENDINGFORVUS,
                                                                                               "Request will be considered. ")
                                );
                        }
//All reject conditions have been checked. The rest go to forvus scrutiny for consideration
                        //Refer to DCSF
                        return new AdjustmentPromptAnalysis(new CompletedStudentAdjustment(student.StudentID,
                                                                                           inclusionReasonId,
                                                                                           promptAnswers,
                                                                                           scrutinyReasonID,
                                                                                           null,
                                                                                           Contants.SCRUTINY_STATUS_ACCEPTEDAUTOMATICALLY,
                                                                                           null)
                            );
                    }
                    else
                    {
                        //Insufficient answers provided.
                        throw Web09Exception.GetBusinessException(Web09MessageList.InsufficientStudentAdjustmentPromptAnswerProvided);
                    }
                }
            }


        }

               
        #region Private methods

        private static bool HasResultForOrPriorTo2005(Web09_Entities context, int studentId)
        {
            int minExamYear = context.Results
                .Where(r => r.Students.StudentID == studentId)
                .Select(r => r.ExamYear)
                .Min();

            return (minExamYear <= 2005) ? true : false;
        }

        private static bool IsSelectedCountryOrLanguageAcceptable(Web09_Entities context, PromptAnswer answer)
        {
            int selectedListItem = Convert.ToInt32(answer.PromptSelectedValueAnswer);
            return !(context.PromptResponses
                .Where(pr => pr.PromptID == answer.PromptID && pr.ListOrder == selectedListItem)
                .Select(pr => pr.Rejected)
                .First()
                );
        }


        private static bool IsSelectedCountryOrLanguageOther(PromptAnswer answer)
        {
            using (EntityConnection conn = new EntityConnection("name=Web09_Entities"))
            {
                conn.Open();

                using (Web09_Entities context = new Web09_Entities(conn))
                {
                    return IsSelectedCountryOrLanguageOther(context, answer);
                }
            }
        }

        private static bool IsSelectedCountryOrLanguageOther(Web09_Entities context, PromptAnswer answer)
        {
            int selectedListItem = Convert.ToInt32(answer.PromptSelectedValueAnswer);
            string selectedAnswerValue = context.PromptResponses
                .Where(pr => pr.PromptID == answer.PromptID && pr.ListOrder == selectedListItem)
                .Select(pr => pr.ListValue)
                .FirstOrDefault();

            if (selectedAnswerValue.ToLower().Equals("other"))
                return true;
            else
                return false;
        }

        #endregion 
    }
}
