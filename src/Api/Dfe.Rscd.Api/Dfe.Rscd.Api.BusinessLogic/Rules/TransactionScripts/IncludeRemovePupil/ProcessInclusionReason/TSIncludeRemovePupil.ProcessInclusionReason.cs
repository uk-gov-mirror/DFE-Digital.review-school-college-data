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

        /// <summary>
        /// Take in a set of adjustment prompt answers and analyse to investigate
        /// whether the adjustment request is complete or requires further input.
        /// </summary>
        /// <param name="inclusionReason">The adjustment reason id</param>
        /// <param name="promptAnswers">The answers provided to prompts</param>
        /// <param name="studentId">The ID of the pupil whom is the subject of the adjustment</param>
        /// <returns>Prompts. If the processing is complete, the prompts list will be empty</returns>
        public static AdjustmentPromptAnalysis ProcessInclusionPromptResponses(int dfesNumber, Students student, int inclusionReasonId, PromptAnswerList promptAnswers)
        {
            List<Prompts> FurtherPrompts = new List<Prompts>();
            AdjustmentPromptAnalysis adjPromptAnalysis_Out;

            switch(inclusionReasonId)
            {

                //Reason 3
                case((int)ReasonsForAdjustment.CancelAddBack):
                    adjPromptAnalysis_Out = ProcessInclusionPromptResponses_CancelAddBack(student.StudentID, inclusionReasonId, promptAnswers);
                    break;

                //Reason 4
                case ((int)ReasonsForAdjustment.CancelAdjustmentForAdmissionFollowingPermanentExclusion):
                    adjPromptAnalysis_Out = ProcessInclusionPromptResponses_CancelAdjustmentForAdmissionFollowingPermanentExclusion(student.StudentID, inclusionReasonId, promptAnswers);
                    break;

                //Reason 5
                case ((int)ReasonsForAdjustment.RemovePupilCompletelyFromAllData):
                    adjPromptAnalysis_Out = ProcessInclusionPromptResponses_RemovePupilCompletelyFromAllData(student.StudentID, inclusionReasonId, promptAnswers);
                    break;

                //Reason 6
                case ((int)ReasonsForAdjustment.PublishPupilAtThisSchool):
                    adjPromptAnalysis_Out = ProcessInclusionPromptResponses_PublishPupilAtThisSchool(student.StudentID, inclusionReasonId, promptAnswers);
                    break;

                //Reason 7
                case ((int)ReasonsForAdjustment.ReinstateThePupilKS4):
                    adjPromptAnalysis_Out = ProcessInclusionPromptResponses_ReinstateThePupil(student, inclusionReasonId, promptAnswers);
                    break;

                //Reason 8
                case((int)ReasonsForAdjustment.AdmittedFromAbroad):
                    adjPromptAnalysis_Out = ProcessInclusionPromptResponses_AdmittedFromAbroad(dfesNumber, student, inclusionReasonId, promptAnswers);
                    break;

                //Reason 9
                case ((int)ReasonsForAdjustment.ContingencyKS4):
                    adjPromptAnalysis_Out = ProcessSingularFurtherPrompt(
                        900,
                        student.StudentID,
                        inclusionReasonId,
                        promptAnswers,
                        Contants.SCRUTINY_REASON_CONTINGENCY,
                        null,
                        Contants.SCRUTINY_STATUS_PENDINGFORVUS,
                        null);
                    break;

                //Reason 10
                case ((int)ReasonsForAdjustment.AdmittedFollowingPermanentExclusionFromMaintainedSchool):
                    adjPromptAnalysis_Out = ProcessInclusionPromptResponses_AdmittedFollowingPermanentExclusionFromMaintainedSchool(student, inclusionReasonId, promptAnswers);
                    break;

                //Reason 11
                case ((int)ReasonsForAdjustment.PermanentlyLeftEngland):
                    adjPromptAnalysis_Out = ProcessInclusionPromptResponses_PermanentlyLeftEngland(student, inclusionReasonId, promptAnswers); 
                    break;

                //Reason 12
                case((int)ReasonsForAdjustment.Deceased):
                    adjPromptAnalysis_Out = ProcessInclusionPromptResponses_Deceased(inclusionReasonId, promptAnswers, student.StudentID);
                    break;

                //Reason 13
                case ((int)ReasonsForAdjustment.PupilNotAtEndOfKeyStage4):
                    adjPromptAnalysis_Out = ProcessInclusionPromptResponses_PupilNotAtEndOfKeyStage4(student, inclusionReasonId, promptAnswers);
                    break;

                //Reason 17
                case((int)ReasonsForAdjustment.PupilAtEndOfKeyStage4):
                    adjPromptAnalysis_Out = ProcessKS4NCYearGroupAdjustment(student, inclusionReasonId, promptAnswers, 1700);
                    break;

                //Reason 18
                case ((int)ReasonsForAdjustment.LeftSchoolRollBeforeExamsKS4):
                    adjPromptAnalysis_Out = ProcessInclusionPromptResponses_LeftSchoolRollBeforeTests(student, inclusionReasonId, promptAnswers);
                    break;

                //Reason 19
                case ((int)ReasonsForAdjustment.KS4Other):
                    adjPromptAnalysis_Out = ProcessInclusionPromptResponses_OtherKS4(student, inclusionReasonId, promptAnswers);
                    break;

                //Reason 21
                case ((int)ReasonsForAdjustment.AddPupilToAchievementAndAttainmentTablesKS4):
                    adjPromptAnalysis_Out = ProcessInclusionPromptResponses_AddPupilToAchievementAndAttainmentTablesKS4(student, inclusionReasonId, promptAnswers);
                    break;

                //Reason 23
                case ((int)ReasonsForAdjustment.ResultsBelongToAnotherPupilKS4):
                    adjPromptAnalysis_Out = ProcessInclusionPromptResponses_ResultsBelongToAnotherPupil(student, inclusionReasonId, promptAnswers);
                    break;

                //Reason 54
                case ((int)ReasonsForAdjustment.NotAtEndOfAdvancedStudy):
                    adjPromptAnalysis_Out = ProcessInclusionPromptResponses_NotAtEndOfAdvancedStudy(student, inclusionReasonId, promptAnswers);
                    break;

                //Reason 55
                case ((int)ReasonsForAdjustment.LeftBeforeExamsKS5):
                    adjPromptAnalysis_Out = ProcessInclusionPromptResponses_LeftBeforeExamsKS5();
                    break;

                //Reason 56
                case ((int)ReasonsForAdjustment.KS5Other):
                    adjPromptAnalysis_Out = ProcessInclusionPromptResponses_OtherKS5(student.StudentID, inclusionReasonId, promptAnswers);
                    break;

                //Reason 57
                case ((int)ReasonsForAdjustment.AddPupilToAchievementAndAttainmentTablesKS5):
                    adjPromptAnalysis_Out = ProcessInclusionPromptResponses_AddPupilToAchievementAndAttainmentTablesKS5(student, inclusionReasonId, promptAnswers);
                    break;

                //Reason 58
                case ((int)ReasonsForAdjustment.ResultsBelongToAnotherPupilKS5):
                    adjPromptAnalysis_Out = ProcessInclusionPromptResponses_ResultsBelongToAnotherPupil(student, inclusionReasonId, promptAnswers);
                    break;

                //Reason 59
                case ((int)ReasonsForAdjustment.ReinstatePupilKS5):
                    adjPromptAnalysis_Out = ProcessInclusionPromptResponses_ReinstateThePupil(student, inclusionReasonId, promptAnswers);
                    break;

                //Reason 92
                case ((int)ReasonsForAdjustment.AddUnlistedPupilToAATKS2):
                    adjPromptAnalysis_Out = AddUnlistedPupilToAATKS2(student, inclusionReasonId, promptAnswers);
                    break;

                //Reason 212
                case ((int)ReasonsForAdjustment.ContingencyKS2):
                    adjPromptAnalysis_Out = ProcessSingularFurtherPrompt(
                        21200,
                        student.StudentID,
                        inclusionReasonId,
                        promptAnswers,
                        Contants.SCRUTINY_REASON_CONTINGENCY,
                        null,
                        Contants.SCRUTINY_STATUS_PENDINGFORVUS,
                        null);
                    break;

                //Reason 213
                case((int)ReasonsForAdjustment.PupilNotAtEndOfKeyStage2InAllSubjects):
                    adjPromptAnalysis_Out = ProcessInclusionPromptResponses_PupilNotAtEndOfKeyStage2(student, inclusionReasonId, promptAnswers);
                    break;

                //Reason 218
                case ((int)ReasonsForAdjustment.LeftSchoolRollBeforeTestsKS2):
                    adjPromptAnalysis_Out = ProcessInclusionPromptResponses_LeftSchoolRollBeforeTestsKS2(inclusionReasonId, promptAnswers, student.StudentID);
                    break;

                //Reason 219
                case ((int)ReasonsForAdjustment.KS2Other):
                    adjPromptAnalysis_Out = ProcessInclusionPromptResponses_OtherKS2(inclusionReasonId, promptAnswers, student.StudentID);
                    break;

                //Reason 221
                case ((int)ReasonsForAdjustment.AddPupilToAchievementAndAttainmentTablesKS2):
                    adjPromptAnalysis_Out = ProcessInclusionPromptResponses_AddPupilToAchievementAndAttainmentTablesKS2(student, inclusionReasonId, promptAnswers);
                    break;

                //Reason 223
                case ((int)ReasonsForAdjustment.ResultsBelongToAnotherPupilKS2):
                    adjPromptAnalysis_Out = ProcessInclusionPromptResponses_ResultsBelongToAnotherPupil(student, inclusionReasonId, promptAnswers);
                    break;

                //Reason 224
                case ((int)ReasonsForAdjustment.CancelAddBackKS5):
                    adjPromptAnalysis_Out = ProcessInclusionPromptResponses_CancelAddBackKS5(student.StudentID, inclusionReasonId, promptAnswers);
                    break;
               
                default:
                    adjPromptAnalysis_Out = new AdjustmentPromptAnalysis(new List<Prompts>());
                    break;
            }

            return adjPromptAnalysis_Out;
        }

        #region Private nethods useful for ProcessInclusionPromptResponses

        private static DateTime GetAnnualSchoolCensusDate(Web09_Entities context)
        {
            string annualSchoolCensusDateStr = context.CohortConfiguration
                .Where(cc => cc.ConfigurationCode == Contants.ANNUAL_SCHOOL_CENSUS_DATE_LOOKUP_CODE)
                .Select(cc => cc.ConfigurationValue)
                .ToList()[0];

            return Convert.ToDateTime(annualSchoolCensusDateStr);
        }

        private static int GetCurrentAcademicYear(Web09_Entities context)
        {
            return int.Parse(context.CohortConfiguration
                .Where(cc => cc.ConfigurationCode == Contants.COHORT_CONFIG_LOOKUP_CODE_TABLE_YEAR)
                .Select(cc => cc.ConfigurationValue)
                .First()) - 1;
        }

        private static int GetCurrentYear(Web09_Entities context)
        {
            return int.Parse(context.CohortConfiguration
                .Where(cc => cc.ConfigurationCode == Contants.COHORT_CONFIG_LOOKUP_CODE_TABLE_YEAR)
                .Select(cc => cc.ConfigurationValue)
                .First());
        }

        private static DateTime GetASCDate(Web09_Entities context)
        {
            return DateTime.Parse(context.CohortConfiguration
                .Where(cc => cc.ConfigurationCode == Contants.ANNUAL_SCHOOL_CENSUS_DATE_LOOKUP_CODE)
                .Select(cc => cc.ConfigurationValue)
                .First());
        }
        
        /// <summary>
        /// Collects KS3 exam start date from Web09 database
        /// </summary>
        /// <param name="context"></param>
        /// <returns>DateTime: KS3 start date from database: Web09.CohortConfiguration</returns>
        private static DateTime GetKS3TestStartDate(Web09_Entities context)
        {
            string ks3TestStartDateStr = context.CohortConfiguration
                .Where(cc => cc.ConfigurationCode == Contants.KS3_TEST_START_DATE_LOOKUP_CODE && cc.KeyStage == 3)
                .Select(cc => cc.ConfigurationValue)
                .ToList()[0];

            return Convert.ToDateTime(ks3TestStartDateStr);
        }

        private static DateTime GetKS3TestEndDate(Web09_Entities context)
        {
            string ks3TestEndDateStr = context.CohortConfiguration
                .Where(cc => cc.ConfigurationCode == Contants.KS3_TEST_END_DATE_LOOKUP_CODE && cc.KeyStage == 3)
                .Select(cc => cc.ConfigurationValue)
                .ToList()[0];

            return Convert.ToDateTime(ks3TestEndDateStr);
        }

        /// <summary>
        /// Collects KS2 exam start date from Web09 database
        /// </summary>
        /// <param name="context"></param>
        /// <returns>DateTime: KS2 start date from database: Web09.CohortConfiguration</returns>
        private static DateTime GetKS2TestStartDate(Web09_Entities context)
        {
            string ks2TestStartDateStr = context.CohortConfiguration
                .Where(cc => cc.ConfigurationCode == Contants.KS3_TEST_START_DATE_LOOKUP_CODE && cc.KeyStage == 2)
                .Select(cc => cc.ConfigurationValue)
                .ToList()[0];

            return Convert.ToDateTime(ks2TestStartDateStr);
        }

        private static DateTime GetKS2TestEndDate(Web09_Entities context)
        {
            string ks2TestEndDateStr = context.CohortConfiguration
                   .Where(cc => cc.ConfigurationCode == Contants.KS2_TEST_END_DATE_LOOKUP_CODE && cc.KeyStage == 2)
                   .Select(cc => cc.ConfigurationValue)
                   .ToList()[0];

            return Convert.ToDateTime(ks2TestEndDateStr);
        }

        internal static bool IsPromptAnswerComplete(PromptAnswerList promptAnswerList, int promptId)
        {
            bool isPromptAnswerComplete = false;

            if (promptAnswerList.HasPromptAnswer(promptId))
            {
                PromptAnswer answer = promptAnswerList.GetPromptAnswerByPromptID(promptId);
                isPromptAnswerComplete = IsPromptAnswerComplete(answer);
            }
            else
            {
                isPromptAnswerComplete = false;
            }

            return isPromptAnswerComplete;

        }

        internal static bool IsPromptAnswerComplete(PromptAnswer answer)
        {
            bool isPromptAnswerComplete = false;
            bool promptAllowsNulls = (GetPromptByPromptID(answer.PromptID)).AllowNulls;

            switch (answer.PromptAnswerType)
            {
                case (PromptAnswer.PromptAnswerTypeEnum.Info):
                    if ((answer.PromptAcknowledgeInfoSightAnswer.HasValue && answer.PromptAcknowledgeInfoSightAnswer.Value) || promptAllowsNulls)
                        isPromptAnswerComplete = true;
                    else
                        isPromptAnswerComplete = false;
                    break;

                case (PromptAnswer.PromptAnswerTypeEnum.Text):
                    if (!String.IsNullOrEmpty(answer.PromptStringAnswer) || promptAllowsNulls)
                        isPromptAnswerComplete = true;
                    else
                        isPromptAnswerComplete = false;
                    break;

                case (PromptAnswer.PromptAnswerTypeEnum.Date):
                    if (answer.PromptDateTimeAnswer != null || promptAllowsNulls)
                        isPromptAnswerComplete = true;
                    else
                        isPromptAnswerComplete = false;
                    break;

                case (PromptAnswer.PromptAnswerTypeEnum.Integer):
                    if (answer.PromptIntegerAnswer.HasValue)
                        isPromptAnswerComplete = true;
                    else
                        isPromptAnswerComplete = false;
                    break;

                case (PromptAnswer.PromptAnswerTypeEnum.ListBox):
                    if (!String.IsNullOrEmpty(answer.PromptSelectedValueAnswer) || promptAllowsNulls)
                        isPromptAnswerComplete = true;
                    else
                        isPromptAnswerComplete = false;
                    break;

                case (PromptAnswer.PromptAnswerTypeEnum.YesNo):
                    if (answer.PromptYesNoAnswer.HasValue || promptAllowsNulls)
                        isPromptAnswerComplete = true;
                    else
                        isPromptAnswerComplete = false;
                    break;

                default:
                    isPromptAnswerComplete = false;
                    break;
            }

            return isPromptAnswerComplete;
        }


        /// <summary>
        /// A common routine to process the Exceptional Circumstances prompts which is referenced by
        /// multiple inclusion/removal reason prompts.
        /// </summary>
        /// <param name="context">Web09_Entities context</param>
        /// <param name="studentId">The id of the student for whom the adjustment is being created</param>
        /// <param name="inclusionReasonId">The inclusion reason</param>
        /// <param name="answers">A list of completed prompt answers</param>
        /// <param name="scrutinyReasonId">The scrutiny reason</param>
        /// <param name="previousPromptId">The id of the prompt that references exceptional circumstances prompt (2000)</param>
        /// <returns></returns>
        private static AdjustmentPromptAnalysis ProcessExceptionalCircumstancesResponse(Web09_Entities context, 
            int studentId, 
            int inclusionReasonId, 
            PromptAnswerList answers, 
            int scrutinyReasonId,
            int previousPromptId)
        {
            
            //If the previous prompt that displays prior to the exceptional prompt
            //has not been included, return that and the exceptional circumstances prompt.
            if(!answers.HasPromptAnswer(previousPromptId))
            {
                List<Prompts> promptsOut = new List<Prompts>();
                promptsOut.Add(GetPromptByPromptID(previousPromptId));
                promptsOut.Add(GetPromptByPromptID(Contants.PROMPT_ID_EXCEPTIONALCIRCUMSTANCES));
                return new AdjustmentPromptAnalysis(promptsOut);
            }
            else if(answers.HasPromptAnswer(previousPromptId) && IsPromptAnswerComplete(answers, previousPromptId))
            {
                if (answers.HasPromptAnswer(Contants.PROMPT_ID_EXCEPTIONALCIRCUMSTANCES) &&
                !String.IsNullOrEmpty(answers.GetPromptAnswerByPromptID(Contants.PROMPT_ID_EXCEPTIONALCIRCUMSTANCES).PromptStringAnswer))
                {
                    //process submission with given reason and a Scrutiny Status of pending. Include
                    //info prompt 2020 as reqest completion message
                    return new AdjustmentPromptAnalysis(new CompletedStudentAdjustment(
                        studentId,
                        inclusionReasonId,
                        answers,
                        scrutinyReasonId,
                        null,
                        Contants.SCRUTINY_STATUS_PENDINGFORVUS,
                        GetInfoPromptText(context, 2020))
                        );
                }
                else if (answers.HasPromptAnswer(Contants.PROMPT_ID_EXCEPTIONALCIRCUMSTANCES) && String.IsNullOrEmpty(answers.GetPromptAnswerByPromptID(Contants.PROMPT_ID_EXCEPTIONALCIRCUMSTANCES).PromptStringAnswer))
                {
                    //process submission with given reason and a Scrutiny Status of pending. Include
                    //info prompt 2010 as reqest completion message
                    return new AdjustmentPromptAnalysis(new CompletedStudentAdjustment(
                        studentId,
                        inclusionReasonId,
                        answers,
                        scrutinyReasonId,
                        Contants.REJECTION_REASON_NO_EXPLANATION,
                        Contants.SCRUTINY_STATUS_REJECT,
                        GetInfoPromptText(context, 2010))
                        );
                }
                else
                {
                    List<Prompts> promptsOut = new List<Prompts>();
                    promptsOut.Add(GetPromptByPromptID(Contants.PROMPT_ID_EXCEPTIONALCIRCUMSTANCES));
                    return new AdjustmentPromptAnalysis(promptsOut);
                }
            }
            else //The previous prompt is included but no answer is given, throw error.
            {
                throw Web09Exception.GetBusinessException(Web09MessageList.InsufficientStudentAdjustmentPromptAnswerProvided);
            }
        }


        /// <summary>
        /// A common routine to process the Exceptional Circumstances prompts which is referenced by
        /// multiple inclusion/removal reason prompts. Overloaded so not to require a Web09_Entities
        /// parameter.
        /// </summary>
        /// <param name="studentId">The id of the student for whom the adjustment is being created</param>
        /// <param name="inclusionReasonId">The inclusion reason</param>
        /// <param name="answers">A list of completed prompt answers</param>
        /// <param name="scrutinyReasonId">The scrutiny reason</param>
        /// <param name="previousPromptId">The id of the prompt that references exceptional circumstances prompt (2000)</param>
        /// <returns></returns>
        private static AdjustmentPromptAnalysis ProcessExceptionalCircumstancesResponse(int studentId,
            int inclusionReasonId,
            PromptAnswerList answers,
            int scrutinyReasonId,
            int previousPromptId)
        {
            using (EntityConnection conn = new EntityConnection("name=Web09_Entities"))
            {
                conn.Open();

                using (Web09_Entities context = new Web09_Entities(conn))
                {
                    return ProcessExceptionalCircumstancesResponse(context,
                        studentId,
                        inclusionReasonId,
                        answers,
                        scrutinyReasonId,
                        previousPromptId);
                }

            }
       
        }

        /// <summary>
        /// A common routine for when a single further prompt needs to be analysed for a response. If
        /// a response has been provided, then the adjustment is completed. If the prompt has not been
        /// provided to the user, then the prompt is returned for display.
        /// </summary>
        /// <param name="furtherPromptId">The id of the single prompt to be analysed</param>
        /// <param name="studentId">The id of the student for whom the adjustment is being created</param>
        /// <param name="inclusionReasonId">The selected adjustment reason</param>
        /// <param name="answers">Any previous prompt answers provided.</param>
        /// <param name="scrutinyReasonId">The ultimate scrutiny reason that will be used if adjustment is complete</param>
        /// <param name="rejectionReasonCode">If the reason is to be rejected, then rejected reason</param>
        /// <param name="scrutinyStatusCode">The ultimate scrutiny status code to be used if the adjustment is complete</param>
        /// <param name="completionMessage">The completion message to be displayed if adjustment is complete</param>
        /// <returns>AdjustmentPromptAnalsysis containing either a completed adjustment or further prompt.</returns>
        private static AdjustmentPromptAnalysis ProcessSingularFurtherPrompt(int furtherPromptId,
            int studentId,
            int? inclusionReasonId,
            PromptAnswerList answers,
            int scrutinyReasonId,
            int? rejectionReasonCode,
            string scrutinyStatusCode,
            string completionMessage)
        {


            if (!answers.HasPromptAnswer(furtherPromptId))
            {
                List<Prompts> furtherPrompts = new List<Prompts>();
                furtherPrompts.Add(GetPromptByPromptID(furtherPromptId));
                return new AdjustmentPromptAnalysis(furtherPrompts);
            }
            else if (answers.HasPromptAnswer(furtherPromptId) && IsPromptAnswerComplete(answers, furtherPromptId))
            {
                return new AdjustmentPromptAnalysis(new CompletedStudentAdjustment(studentId,
                    inclusionReasonId,
                    answers,
                    scrutinyReasonId,
                    rejectionReasonCode,
                    scrutinyStatusCode,
                    completionMessage)
                    );
            }
            else
            {
                throw Web09Exception.GetBusinessException(Web09MessageList.InsufficientStudentAdjustmentPromptAnswerProvided);
            }
        }

        private static string GetInfoPromptText(Web09_Entities context, int promptID)
        {
            var prompt = context.Prompts
                .FirstOrDefault(p => p.PromptID == promptID);

            if(prompt != null)
            {
                return prompt.PromptText;
            }

            return "Prompt not found!";
        }

        /// <summary>
        /// Overloaded to perform the get without a context object.
        /// </summary>
        /// <param name="promptID">The id of the prompt containing the information</param>
        /// <returns></returns>
        internal static string GetInfoPromptText(int promptId)
        {
            using(EntityConnection conn = new EntityConnection("name=Web09_Entities"))
            {
                conn.Open();

                using (Web09_Entities context = new Web09_Entities(conn))
                {
                    return GetInfoPromptText(context, promptId);
                }
            }
        }

        //private static bool IsSelectedCountryOther(int promptId, int selectedAnswer)
        //{
        //    using(EntityConnection conn = new EntityConnection("name=Web09_Entities"))
        //    {
        //        conn.Open();

        //        using (Web09_Entities context = new Web09_Entities(conn))
        //        {
        //            string selectedCountry = context.PromptResponses
        //                .Where(pr => pr.PromptID == promptId && pr.ListOrder == selectedAnswer)
        //                .Select(pr => pr.ListValue)
        //                .FirstOrDefault();

        //            if (selectedCountry.Equals("Other"))
        //                return true;
        //            else
        //                return false;
        //        }
        //    }

        //}


        #endregion

        

        #region Private Members
        private static DateTime AnnualSchoolCensusDate
        {
            get
            {

                using (EntityConnection conn = new EntityConnection("name=Web09_Entities"))
                {
                    conn.Open();

                    using (Web09_Entities context = new Web09_Entities(conn))
                    {
                        return GetAnnualSchoolCensusDate(context);
                    }
                }
            }
        }


        private static int CurrentAcademicYear
        {
            get
            {
                using (EntityConnection conn = new EntityConnection("name=Web09_Entities"))
                {
                    conn.Open();

                    using (Web09_Entities context = new Web09_Entities(conn))
                    {
                        return GetCurrentAcademicYear(context);
                    }
                }
            }
        }

        private static int CurrentYear
        {
            get
            {
                using (EntityConnection conn = new EntityConnection("name=Web09_Entities"))
                {
                    conn.Open();

                    using (Web09_Entities context = new Web09_Entities(conn))
                    {
                        return GetCurrentYear(context);
                    }
                }
            }
        }


        private static DateTime ACSDate
        {
            get
            {
                using (EntityConnection conn = new EntityConnection("name=Web09_Entities"))
                {
                    conn.Open();

                    using (Web09_Entities context = new Web09_Entities(conn))
                    {
                        return GetASCDate(context);
                    }
                }
            }
        }

        /// <summary>
        /// Collects KS3Test Start date from Web09 database
        /// </summary>
        private static DateTime KS3TestStartDate
        {
            get
            {
                using (EntityConnection conn = new EntityConnection("name=Web09_Entities"))
                {
                    conn.Open();

                    using (Web09_Entities context = new Web09_Entities(conn))
                    {
                        return GetKS3TestStartDate(context);
                    }
                }
            }
        }

        private static DateTime KS3TestEndDate
        {
            get
            {
                using (EntityConnection conn = new EntityConnection("name=Web09_Entities"))
                {
                    conn.Open();

                    using (Web09_Entities context = new Web09_Entities(conn))
                    {
                        return GetKS3TestEndDate(context);
                    }
                }
            }
        }

        /// <summary>
        /// Collects KS2Test Start date from Web09 database
        /// </summary>
        private static DateTime KS2TestStartDate
        {
            get
            {
                using (EntityConnection conn = new EntityConnection("name=Web09_Entities"))
                {
                    conn.Open();

                    using (Web09_Entities context = new Web09_Entities(conn))
                    {
                        return GetKS2TestStartDate(context);
                    }
                }
            }
        }

        private static DateTime KS2TestEndDate
        {
            get
            {
                using (EntityConnection conn = new EntityConnection("name=Web09_Entities"))
                {
                    conn.Open();

                    using (Web09_Entities context = new Web09_Entities(conn))
                    {
                        return GetKS2TestEndDate(context);
                    }
                }
            }
        }

        

        #endregion

        
    }
}