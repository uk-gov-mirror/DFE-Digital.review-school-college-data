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
    public partial class TSIncludeRemovePupil
    {


        internal static AdjustmentPromptAnalysis ProcessNCYearGroup(Web09_Entities context,
            Students student,
            int? inclusionReasonId,
            PromptAnswerList answers,
            int? previousPromptId)
        {
            if (student.Cohorts == null)
                throw Web09Exception.GetBusinessException(Web09MessageList.InsufficientStudentDetails);

            switch (student.Cohorts.KeyStage)
            {
                case (2):
                    return ProcessKS2NCYearGroupAdjustment(context, student, inclusionReasonId, answers, previousPromptId);
                case (3):
                    return ProcessKS3NCYearGroupAdjustment(context, student, inclusionReasonId, answers, previousPromptId);
                case (4):
                    return ProcessKS4NCYearGroupAdjustment(context, student, inclusionReasonId, answers, previousPromptId);
                case (5):
                    return ProcessKS5NCYearGroupAdjustment(context, student, inclusionReasonId, answers, previousPromptId);
                default:
                    throw Web09Exception.GetBusinessException(Web09MessageList.KeystageInvalid);
            }
        }


        /// <summary>
        /// Analyse the answers to determine if they have a complete set of answers for the NC Year group for key stage 2.
        /// </summary>
        /// <param name="context">The Web09 context object.</param>
        /// <param name="student">The student for whom the adjustment is being created</param>
        /// <param name="inclusionReasonId">The selected inclusion reason</param>
        /// <param name="answers">The answers provided so far in the adjustment cycle</param>
        /// <param name="previousPromptId">The prompt that preluded the NC Year Group prompt</param>
        /// <returns></returns>
        internal static AdjustmentPromptAnalysis ProcessKS2NCYearGroupAdjustment(Web09_Entities context,
            Students student,
            int? inclusionReasonId,
            PromptAnswerList answers,
            int? previousPromptId)
        {
            if (previousPromptId.HasValue && !answers.HasPromptAnswer(previousPromptId.Value) )
            {
                List<Prompts> promptsOut = new List<Prompts>();
                promptsOut.Add(GetPromptByPromptID(previousPromptId.Value));
                promptsOut.Add(GetPromptByPromptID(Contants.PROMPT_ID_NC_YEAR_GROUP_KS2));
                return new AdjustmentPromptAnalysis(promptsOut);
            }
            else if (answers.HasPromptAnswer(Contants.PROMPT_ID_NC_YEAR_GROUP_KS2) &&
                IsPromptAnswerComplete(answers, Contants.PROMPT_ID_NC_YEAR_GROUP_KS2))
            {

                if (student == null ||
                    student.StudentChanges.Count == 0 ||
                    student.StudentChanges.First().YearGroups == null ||
                    student.Schools == null ||
                    String.IsNullOrEmpty(student.StudentChanges.First().DOB) ||
                    student.PINCLs == null || student.PINCLs.P_INCL == null)
                    throw Web09Exception.GetBusinessException(Web09MessageList.InsufficientStudentDetails);

                StudentChanges studentChange = student.StudentChanges.First();
                bool isSchoolPlasc = !TSSchool.IsSchoolNonPlasc(student.Schools.DFESNumber);
                int newStudentNCYearGroup = answers.GetPromptAnswerByPromptID(Contants.PROMPT_ID_NC_YEAR_GROUP_KS2).PromptIntegerAnswer.Value;

                int originalStudentNCYearGroup;
                string originalStudentNCYearGroupStr = context.StudentChanges
                    .Where(sc => sc.StudentID == student.StudentID && sc.DateEnd == null)
                    .Select(sc => sc.YearGroups.YearGroupCode)
                    .FirstOrDefault();

                DateTime studentDOB = TSStudent.GetStudentDOBDateTime(studentChange.DOB);
                int studentAgeInYears = TSStudent.CalculateStudentAge(studentDOB);

                int newNCYearGroup = answers.GetPromptAnswerByPromptID(Contants.PROMPT_ID_NC_YEAR_GROUP_KS2).PromptIntegerAnswer.Value;

                if (!int.TryParse(originalStudentNCYearGroupStr, out originalStudentNCYearGroup))
                {
                    originalStudentNCYearGroup = newNCYearGroup;
                }

                if (originalStudentNCYearGroup < 6 && newNCYearGroup == 6)
                {

                    if (student.PINCLs.P_INCL == "202")
                    {
                        return new AdjustmentPromptAnalysis(new CompletedStudentAdjustment(
                           student.StudentID,
                           inclusionReasonId,
                           answers,
                           Contants.SCRUTINY_REASON_NC_YEAR_CHANGE,
                           null,
                           Contants.SCRUTINY_STATUS_ACCEPTEDAUTOMATICALLY,
                           GetInfoPromptText(21410))
                           );
                    }
                    else
                    {
                        return new AdjustmentPromptAnalysis(new CompletedStudentAdjustment(
                           student.StudentID,
                           inclusionReasonId,
                           answers,
                           Contants.SCRUTINY_REASON_NC_YEAR_CHANGE,
                           null,
                           Contants.SCRUTINY_STATUS_ACCEPTEDAUTOMATICALLY,
                           GetInfoPromptText(21420))
                           );
                    }
                }
                
                else if(originalStudentNCYearGroup == 6 && newNCYearGroup < 6)
                {
                    if (TSStudent.AreAllKS2ResultsUndiscountedMResults(student.StudentID))
                    {
                        return new AdjustmentPromptAnalysis(new CompletedStudentAdjustment(
                           student.StudentID,
                           inclusionReasonId,
                           answers,
                           Contants.SCRUTINY_REASON_NC_YEAR_CHANGE,
                           null,
                           Contants.SCRUTINY_STATUS_ACCEPTEDAUTOMATICALLY,
                           GetInfoPromptText(21430))
                           );
                    }
                    else // => Not all undiscounted results are M results
                    {
                        return new AdjustmentPromptAnalysis(new CompletedNonStudentAdjustment(GetInfoPromptText(21440)));
                    }
                }
                else if (originalStudentNCYearGroup == 6 && newNCYearGroup > 6)
                {
                    return new AdjustmentPromptAnalysis(new CompletedNonStudentAdjustment(GetInfoPromptText(21450)));
                }
                else if (originalStudentNCYearGroup > 6 && newNCYearGroup == 6)
                {
                    return new AdjustmentPromptAnalysis(new CompletedNonStudentAdjustment(GetInfoPromptText(21460)));
                }
                else
                {
                    return new AdjustmentPromptAnalysis(new CompletedNonStudentAdjustment("No request generated for this year group change."));
                }
            }
            else
            {
                throw Web09Exception.GetBusinessException(Web09MessageList.InsufficientStudentAdjustmentPromptAnswerProvided);
            }
        }

        /// <summary>
        /// Overloaded method to process NC year group prompt for KS2 without a context object.
        /// </summary>
        /// <param name="student">Student for whom adjustment is being performed on</param>
        /// <param name="inclusionReasonId">The adjustment reason</param>
        /// <param name="answers">Any prompt answers given so far throughout the adjustment creation process.</param>
        /// <param name="previousPromptId">The preluding prompt that initiates the NC Year group prompt.</param>
        /// <returns>AdjustmentPromptAnalysis object to indicate if adjustment is complete or not</returns>
        internal static AdjustmentPromptAnalysis ProcessKS2NCYearGroupAdjustment(Students student,
            int? inclusionReasonId,
            PromptAnswerList answers,
            int? previousPromptId)
        {
            using (EntityConnection conn = new EntityConnection("name=Web09_Entities"))
            {
                conn.Open();

                using (Web09_Entities context = new Web09_Entities(conn))
                {
                    return ProcessKS2NCYearGroupAdjustment(context,
                        student,
                        inclusionReasonId,
                        answers,
                        previousPromptId);
                }
            }
        }

        


        /// <summary>
        /// Analyse the answers to determine if they have a complete set of answers for the NC Year group.
        /// </summary>
        /// <param name="context">The Web09 context object.</param>
        /// <param name="student">The student for whom the adjustment is being created</param>
        /// <param name="inclusionReasonId">The selected inclusion reason</param>
        /// <param name="answers">The answers provided so far in the adjustment cycle</param>
        /// <param name="previousPromptId">The prompt that preluded the NC Year Group prompt</param>
        /// <returns></returns>
        internal static AdjustmentPromptAnalysis ProcessKS3NCYearGroupAdjustment(Web09_Entities context,
            Students student,
            int? inclusionReasonId,
            PromptAnswerList answers,
            int? previousPromptId)
        {
            if (previousPromptId.HasValue && !answers.HasPromptAnswer(previousPromptId.Value))
            {
                List<Prompts> promptsOut = new List<Prompts>();
                promptsOut.Add(GetPromptByPromptID(previousPromptId.Value));
                promptsOut.Add(GetPromptByPromptID(Contants.PROMPT_ID_NC_YEAR_GROUP_KS3));
                return new AdjustmentPromptAnalysis(promptsOut);
            }
            else if (answers.HasPromptAnswer(Contants.PROMPT_ID_NC_YEAR_GROUP_KS3) &&
                IsPromptAnswerComplete(answers, Contants.PROMPT_ID_NC_YEAR_GROUP_KS3))
            {

                if (student == null ||
                    student.StudentChanges.Count == 0 ||
                    student.StudentChanges.First().YearGroups == null ||
                    student.Schools == null ||
                    !String.IsNullOrEmpty(student.StudentChanges.First().DOB))
                    throw Web09Exception.GetBusinessException(Web09MessageList.InsufficientStudentDetails);

                StudentChanges studentChange = student.StudentChanges.First();
                bool isSchoolPlasc = !TSSchool.IsSchoolNonPlasc(student.Schools.DFESNumber);
                int newStudentNCYearGroup = answers.GetPromptAnswerByPromptID(Contants.PROMPT_ID_NC_YEAR_GROUP_KS3).PromptIntegerAnswer.Value;
                int originalStudentNCYearGroup;
                DateTime studentDOB = TSStudent.GetStudentDOBDateTime(studentChange.DOB);
                int studentAgeInYears = TSStudent.CalculateStudentAge(studentDOB);

                int newNCYearGroup = answers.GetPromptAnswerByPromptID(Contants.PROMPT_ID_NC_YEAR_GROUP_KS3).PromptIntegerAnswer.Value;

                if (int.TryParse(student.StudentChanges.First().YearGroups.YearGroupCode, out originalStudentNCYearGroup))
                {
                    if (originalStudentNCYearGroup < 9 && newNCYearGroup == 9)
                    {

                        if (studentChange.AMDFlag.ToLower() == "z")
                        {
                            return new AdjustmentPromptAnalysis(new CompletedStudentAdjustment(
                               student.StudentID,
                               inclusionReasonId,
                               answers,
                               Contants.SCRUTINY_REASON_NC_YEAR_CHANGE,
                               null,
                               Contants.SCRUTINY_STATUS_ACCEPTEDAUTOMATICALLY,
                               GetInfoPromptText(31410))
                               );
                        }
                        else
                        {
                            return new AdjustmentPromptAnalysis(new CompletedStudentAdjustment(
                               student.StudentID,
                               inclusionReasonId,
                               answers,
                               Contants.SCRUTINY_REASON_NC_YEAR_CHANGE,
                               null,
                               Contants.SCRUTINY_STATUS_ACCEPTEDAUTOMATICALLY,
                               GetInfoPromptText(31420))
                               );
                        }
                    }
                    else if (originalStudentNCYearGroup == 9 && newNCYearGroup > 9)
                    {
                        return new AdjustmentPromptAnalysis(new CompletedStudentAdjustment(
                              student.StudentID,
                              inclusionReasonId,
                              answers,
                              Contants.SCRUTINY_REASON_NC_YEAR_CHANGE,
                              null,
                              Contants.SCRUTINY_STATUS_ACCEPTEDAUTOMATICALLY,
                              GetInfoPromptText(31450))
                              );
                    }
                    else if (originalStudentNCYearGroup > 9 && newNCYearGroup == 9)
                    {
                        return new AdjustmentPromptAnalysis(new CompletedStudentAdjustment(
                              student.StudentID,
                              inclusionReasonId,
                              answers,
                              Contants.SCRUTINY_REASON_NC_YEAR_CHANGE,
                              null,
                              Contants.SCRUTINY_STATUS_ACCEPTEDAUTOMATICALLY,
                              GetInfoPromptText(31460))
                              );
                    }
                    else
                    {
                        throw Web09Exception.GetBusinessException(Web09MessageList.InvalidStudentIncludeRemoveRequest);
                    }
                }
                else
                {
                    throw Web09Exception.GetBusinessException(Web09MessageList.InsufficientStudentAdjustmentPromptAnswerProvided);
                }
            }
            else
            {
                throw Web09Exception.GetBusinessException(Web09MessageList.InsufficientStudentAdjustmentPromptAnswerProvided);
            }

        }

        /// <summary>
        /// Overloaded method to process NC year group prompt without a context object.
        /// </summary>
        /// <param name="student">Student for whom adjustment is being performed on</param>
        /// <param name="inclusionReasonId">The adjustment reason</param>
        /// <param name="answers">Any prompt answers given so far throughout the adjustment creation process.</param>
        /// <param name="previousPromptId">The preluding prompt that initiates the NC Year group prompt.</param>
        /// <returns>AdjustmentPromptAnalysis object to indicate if adjustment is complete or not</returns>
        internal static AdjustmentPromptAnalysis ProcessKS3NCYearGroupAdjustment(Students student,
            int? inclusionReasonId,
            PromptAnswerList answers,
            int previousPromptId)
        {
            using (EntityConnection conn = new EntityConnection("name=Web09_Entities"))
            {
                conn.Open();

                using (Web09_Entities context = new Web09_Entities(conn))
                {
                    return ProcessKS3NCYearGroupAdjustment(context,
                        student,
                        inclusionReasonId,
                        answers,
                        previousPromptId);
                }
            }
        }

        /// <summary>
        /// Process a NC Year Group adjustment for a KS4 student
        /// </summary>
        /// <param name="context">Web09 Entity Framework object context</param>
        /// <param name="student">The student for whom the adjustment is being created</param>
        /// <param name="inclusionReasonId"></param>
        /// <param name="answers">Any prompt answers that have been captured as part of the adjustment request</param>
        /// <param name="previousPromptId"></param>
        /// <returns></returns>
        internal static AdjustmentPromptAnalysis ProcessKS4NCYearGroupAdjustment(Web09_Entities context, 
            Students student, 
            int? inclusionReasonId, 
            PromptAnswerList answers,
            int? previousPromptId)
        {

            if (previousPromptId.HasValue && !answers.HasPromptAnswer(previousPromptId.Value))
            {
                List<Prompts> promptsOut = new List<Prompts>();
                promptsOut.Add(GetPromptByPromptID(previousPromptId.Value));
                promptsOut.Add(GetPromptByPromptID(Contants.PROMPT_ID_NC_YEAR_GROUP_KS4));
                return new AdjustmentPromptAnalysis(promptsOut);
            }
            if (answers.HasPromptAnswer(Contants.PROMPT_ID_NC_YEAR_GROUP_KS4) &&
                IsPromptAnswerComplete(answers, Contants.PROMPT_ID_NC_YEAR_GROUP_KS4))
            {
                if (student == null ||
                    student.StudentChanges.Count == 0 ||
                    student.StudentChanges.First().YearGroups == null ||
                    student.Schools == null ||
                    String.IsNullOrEmpty(student.StudentChanges.First().DOB))
                    throw Web09Exception.GetBusinessException(Web09MessageList.InsufficientStudentDetails);

                StudentChanges studentChange = student.StudentChanges.First();
                bool isSchoolPlasc = !TSSchool.IsSchoolNonPlasc(student.Schools.DFESNumber);
                int newStudentNCYearGroup = answers.GetPromptAnswerByPromptID(Contants.PROMPT_ID_NC_YEAR_GROUP_KS4).PromptIntegerAnswer.Value;
                int? originalStudentNCYearGroup = TSStudent.GetStudentCurrentNCYearGroup(context, student.StudentID);
                DateTime studentDOB = TSStudent.GetStudentDOBDateTime(studentChange.DOB);
                int studentAgeInYears = TSStudent.CalculateStudentAge(studentDOB);

                if (originalStudentNCYearGroup.HasValue)
                {
                    if (isSchoolPlasc)
                    {
                        if (originalStudentNCYearGroup == newStudentNCYearGroup)
                        {
                            return new AdjustmentPromptAnalysis(new CompletedNonStudentAdjustment("No request has been made."));
                        }
                        if (originalStudentNCYearGroup < 11 && newStudentNCYearGroup == 11 && studentAgeInYears >= 14 && studentAgeInYears <= 16)
                        {
                            return new AdjustmentPromptAnalysis(new CompletedStudentAdjustment(
                                                                    student.StudentID,
                                                                    inclusionReasonId,
                                                                    answers,
                                                                    Contants.SCRUTINY_REASON_NC_YEAR_CHANGE,
                                                                    null,
                                                                    Contants.SCRUTINY_STATUS_ACCEPTEDAUTOMATICALLY,
                                                                    GetInfoPromptText(context, 1410))
                                );
                        }
                        if (originalStudentNCYearGroup == 11 && newStudentNCYearGroup >= 12)
                        {
                            //Process if text box answer has been provided for prompt 1420
                            return ProcessSingularFurtherPrompt(1420, student.StudentID, inclusionReasonId, answers,
                                                                Contants.SCRUTINY_REASON_NC_YEAR_CHANGE,
                                                                null,
                                                                Contants.SCRUTINY_STATUS_PENDINGFORVUS,
                                                                null);
                        }
                        if (originalStudentNCYearGroup == 11 && newStudentNCYearGroup <= 10 && studentAgeInYears == 14)
                        {
                            return new AdjustmentPromptAnalysis(new CompletedStudentAdjustment(
                                                                    student.StudentID,
                                                                    inclusionReasonId,
                                                                    answers,
                                                                    Contants.SCRUTINY_REASON_NC_YEAR_CHANGE,
                                                                    null,
                                                                    Contants.SCRUTINY_STATUS_ACCEPTEDAUTOMATICALLY,
                                                                    GetInfoPromptText(context, 1430))
                                );
                        }
                        if (originalStudentNCYearGroup == 11 && newStudentNCYearGroup <= 10 && studentAgeInYears == 15)
                        {
                            return new AdjustmentPromptAnalysis(new CompletedStudentAdjustment(
                                                                    student.StudentID,
                                                                    inclusionReasonId,
                                                                    answers,
                                                                    Contants.SCRUTINY_REASON_ADD_BACK_IN_2008,
                                                                    null,
                                                                    Contants.SCRUTINY_STATUS_ACCEPTEDAUTOMATICALLY,
                                                                    GetInfoPromptText(context, 1440))
                                );
                        }
                        if (isSchoolPlasc && !(newStudentNCYearGroup >= (studentAgeInYears - 5) && newStudentNCYearGroup <= (studentAgeInYears - 3)))
                        {
                            return ProcessSingularFurtherPrompt(1445,
                                                                student.StudentID,
                                                                inclusionReasonId,
                                                                answers,
                                                                Contants.SCRUTINY_REASON_NC_YEAR_CHANGE,
                                                                null,
                                                                Contants.SCRUTINY_STATUS_PENDINGFORVUS,
                                                                null);
                        }
                        return new AdjustmentPromptAnalysis(new CompletedStudentAdjustment(
                                                                student.StudentID,
                                                                inclusionReasonId,
                                                                answers,
                                                                Contants.SCRUTINY_REASON_NC_YEAR_CHANGE,
                                                                null,
                                                                Contants.SCRUTINY_STATUS_PENDINGFORVUS,
                                                                GetInfoPromptText(context, 1450))
                            );
                    }
                    //Non-Plasc school options
                    //if it's a proxy school then use different results.
                    var isProxy = TSSchool.GetSchoolValue(student.Schools.DFESNumber, "INDNORPROX", 4) == "1";

                    if (!(newStudentNCYearGroup >= (studentAgeInYears - 5) && newStudentNCYearGroup <= (studentAgeInYears - 3)))
                    {
                        return ProcessSingularFurtherPrompt(isProxy ? 1456 : 1455,
                                                            student.StudentID,
                                                            inclusionReasonId,
                                                            answers,
                                                            Contants.SCRUTINY_REASON_YEAR_GROUP_ALGORITHM,
                                                            null,
                                                            Contants.SCRUTINY_STATUS_PENDINGFORVUS,
                                                            null);
                    }
                    if (originalStudentNCYearGroup == 11 && newStudentNCYearGroup != 11)
                    {
                        return new AdjustmentPromptAnalysis(new CompletedStudentAdjustment(
                                                                student.StudentID,
                                                                inclusionReasonId,
                                                                answers,
                                                                Contants.SCRUTINY_REASON_YEAR_GROUP_ALGORITHM,
                                                                null,
                                                                Contants.SCRUTINY_STATUS_ACCEPTEDAUTOMATICALLY,
                                                                GetInfoPromptText(context, isProxy ? 1461 : 1460))
                            );
                    }
                    if (originalStudentNCYearGroup != 11 && newStudentNCYearGroup == 11)
                    {
                        return new AdjustmentPromptAnalysis(new CompletedStudentAdjustment(
                                                                student.StudentID,
                                                                inclusionReasonId,
                                                                answers,
                                                                Contants.SCRUTINY_REASON_YEAR_GROUP_ALGORITHM,
                                                                null,
                                                                Contants.SCRUTINY_STATUS_ACCEPTEDAUTOMATICALLY,
                                                                GetInfoPromptText(context, isProxy ? 1471 : 1470))
                            );
                    }
                    return new AdjustmentPromptAnalysis(new CompletedStudentAdjustment(
                                                            student.StudentID,
                                                            inclusionReasonId,
                                                            answers,
                                                            Contants.SCRUTINY_REASON_YEAR_GROUP_ALGORITHM,
                                                            null,
                                                            Contants.SCRUTINY_STATUS_ACCEPTEDAUTOMATICALLY,
                                                            GetInfoPromptText(context, isProxy ? 1481 : 1480))
                        );
                }
            }
            throw Web09Exception.GetBusinessException(Web09MessageList.InsufficientStudentAdjustmentPromptAnswerProvided);
        }

        /// <summary>
        /// Overloaded method to process KS4 NC year group prompt without a context object.
        /// </summary>
        /// <param name="student">Student for whom adjustment is being performed on</param>
        /// <param name="inclusionReasonId">The adjustment reason</param>
        /// <param name="answers">Any prompt answers given so far throughout the adjustment creation process.</param>
        /// <param name="previousPromptId">The preluding prompt that initiates the NC Year group prompt.</param>
        /// <returns>AdjustmentPromptAnalysis object to indicate if adjustment is complete or not</returns>
        internal static AdjustmentPromptAnalysis ProcessKS4NCYearGroupAdjustment(Students student,
            int? inclusionReasonId,
            PromptAnswerList answers,
            int previousPromptId)
        {
            using (EntityConnection conn = new EntityConnection("name=Web09_Entities"))
            {
                conn.Open();

                using (Web09_Entities context = new Web09_Entities(conn))
                {
                    return ProcessKS4NCYearGroupAdjustment(context,
                        student,
                        inclusionReasonId,
                        answers,
                        previousPromptId);
                }
            }
        }

        
        /// <summary>
        /// Obtain new KS5 NC Year group and process the response.
        /// </summary>
        /// <param name="context">Web09 Entity Context.</param>
        /// <param name="student"></param>
        /// <param name="inclusionReasonId"></param>
        /// <param name="answers"></param>
        /// <param name="previousPromptId"></param>
        /// <returns></returns>
        internal static AdjustmentPromptAnalysis ProcessKS5NCYearGroupAdjustment(Web09_Entities context,
            Students student,
            int? inclusionReasonId,
            PromptAnswerList answers,
            int? previousPromptId)
        {
            string[] KS5AddBackPincls = {"504","505","525"};

            if (previousPromptId.HasValue && !answers.HasPromptAnswer(previousPromptId.Value))
            {
                List<Prompts> promptsOut = new List<Prompts>();
                promptsOut.Add(GetPromptByPromptID(previousPromptId.Value));
                promptsOut.Add(GetPromptByPromptID(Contants.PROMPT_ID_NC_YEAR_GROUP_KS5));
                return new AdjustmentPromptAnalysis(promptsOut);
            }
            else if (answers.HasPromptAnswer(Contants.PROMPT_ID_NC_YEAR_GROUP_KS5) &&
                IsPromptAnswerComplete(answers, Contants.PROMPT_ID_NC_YEAR_GROUP_KS5))
            {
                if (student == null ||
                    student.StudentChanges.Count == 0 ||
                    student.StudentChanges.First().YearGroups == null ||
                    String.IsNullOrEmpty(student.StudentChanges.First().DOB))
                    throw Web09Exception.GetBusinessException(Web09MessageList.InsufficientStudentDetails);

                // defect 2009
                // this already has amended value of 12, same value compares to itself from prompt value 
                // in following conditions not logical 
                //StudentChanges studentChange = student.StudentChanges.First(); 
                StudentChanges studentChange = context.StudentChanges
                        .Include("YearGroups")
                        .Where(sc => sc.StudentID == student.StudentID && sc.DateEnd == null)
                        .First();
                int newStudentNCYearGroup = answers.GetPromptAnswerByPromptID(Contants.PROMPT_ID_NC_YEAR_GROUP_KS5).PromptIntegerAnswer.Value;
                int originalStudentNCYearGroup;
               
                if (int.TryParse(studentChange.YearGroups.YearGroupCode, out originalStudentNCYearGroup))
                {
                    if (originalStudentNCYearGroup != 13 && newStudentNCYearGroup == 13)
                    {
                        return new AdjustmentPromptAnalysis(new CompletedStudentAdjustment(
                                student.StudentID,
                                inclusionReasonId,
                                answers,
                                Contants.SCRUTINY_REASON_NC_YEAR_CHANGE,
                                null,
                                Contants.SCRUTINY_STATUS_ACCEPTEDAUTOMATICALLY,
                                GetInfoPromptText(context, 5210))
                                );
                    }
                    else if (originalStudentNCYearGroup == 13 && newStudentNCYearGroup < 12)   // TFS 22200, June 2013
                    {
                        return new AdjustmentPromptAnalysis(new CompletedStudentAdjustment(
                                student.StudentID,
                                inclusionReasonId,
                                answers,
                                Contants.SCRUTINY_REASON_NC_YEAR_CHANGE,
                                null,
                                Contants.SCRUTINY_STATUS_ACCEPTEDAUTOMATICALLY,
                                GetInfoPromptText(context, 5220))
                                );
                    }
                    // CC14-016 / TFS 25282 - added Age and PINCL checks to this condition
                    else if (originalStudentNCYearGroup == 13 && newStudentNCYearGroup == 12
                             && ( student.StudentChanges.First().Age < 18 )
                             && (! KS5AddBackPincls.Contains( student.PINCLs.P_INCL ))  )  // TFS 22200, June 2013
                    {
                        return new AdjustmentPromptAnalysis(new CompletedStudentAdjustment(
                                student.StudentID,
                                inclusionReasonId,
                                answers,
                                Contants.SCRUTINY_REASON_NC_YEAR_CHANGE,
                                null,
                                Contants.SCRUTINY_STATUS_ACCEPTEDAUTOMATICALLY,
                                GetInfoPromptText(context, 5230))
                                );
                    }
                    // CC14-016 / TFS 25282 - added this condition
                    else if (originalStudentNCYearGroup == 13 && newStudentNCYearGroup == 12
                             && (student.StudentChanges.First().Age < 18)
                             && (KS5AddBackPincls.Contains(student.PINCLs.P_INCL)))  // TFS 22200, June 2013
                    {
                        throw Web09Exception.GetBusinessException(Web09MessageList.StudentWasDeferredLastYear);
                    }
                    // CC14-016 / TFS 25282 - added this condition
                    else if (originalStudentNCYearGroup == 13 && newStudentNCYearGroup == 12
                             && (student.StudentChanges.First().Age == 18) )  // TFS 22200, June 2013
                    {
                        throw Web09Exception.GetBusinessException(Web09MessageList.StudentTooOldToDefer);
                    }
                    else if (originalStudentNCYearGroup == 13 && newStudentNCYearGroup > 13)  // TFS 22200, June 2013
                    {                        
                        throw Web09Exception.GetBusinessException(Web09MessageList.YearsAbove13CannotBeAccepted);
                    }
                    else //Non-plasc other
                    {
                        return new AdjustmentPromptAnalysis(new CompletedNonStudentAdjustment(null));                    
                    }

                }
                else
                {
                    throw Web09Exception.GetBusinessException(Web09MessageList.InsufficientStudentAdjustmentPromptAnswerProvided);
                }

            }
            else
            {
                throw Web09Exception.GetBusinessException(Web09MessageList.InsufficientStudentAdjustmentPromptAnswerProvided);
            }
        }       

        /// <summary>
        /// Overloaded method to process KS5 NC year group prompt without a context object.
        /// </summary>
        /// <param name="student">Student for whom adjustment is being performed on</param>
        /// <param name="inclusionReasonId">The adjustment reason</param>
        /// <param name="answers">Any prompt answers given so far throughout the adjustment creation process.</param>
        /// <param name="previousPromptId">The preluding prompt that initiates the NC Year group prompt.</param>
        /// <returns>AdjustmentPromptAnalysis object to indicate if adjustment is complete or not</returns>
        internal static AdjustmentPromptAnalysis ProcessKS5NCYearGroupAdjustment(Students student,
            int? inclusionReasonId,
            PromptAnswerList answers,
            int previousPromptId)
        {
            using (EntityConnection conn = new EntityConnection("name=Web09_Entities"))
            {
                conn.Open();

                using (Web09_Entities context = new Web09_Entities(conn))
                {
                    return ProcessKS5NCYearGroupAdjustment(context,
                        student,
                        inclusionReasonId,
                        answers,
                        previousPromptId);
                }
            }
        }

    }
}
