using System;
using System.Collections.Generic;
using System.Data.EntityClient;
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
        /// <param name="inclusionReasonId">inclusionReasonId</param>
        /// <param name="promptAnswers">promptAnswers. List of PromptAnswers</param>
        /// <param name="studentId">studentId</param>
        /// <throws>ArgumentException. However, logically impossible for valid date to throw exception</throws>
        /// <returns>Either: List of Prompts<see cref="Web09.Checking.DataAccess.Prompts"/> to be completed by UI or a CompletedStudentAdjustment</returns>
        private static AdjustmentPromptAnalysis ProcessInclusionPromptResponses_LeftSchoolRollBeforeTests(Students student, int inclusionReasonId, PromptAnswerList promptAnswers)
        {

            
            if (student == null ||
                student.Cohorts == null ||
                student.Schools == null)
                throw Web09Exception.GetBusinessException(Web09MessageList.InsufficientStudentDetails);

            short keyStage = student.Cohorts.KeyStage; 


            //Context required for exceptional circumstances propmts
            using (EntityConnection conn = new EntityConnection("name=Web09_Entities"))
            {
                conn.Open();


                using (Web09_Entities context = new Web09_Entities(conn))
                {

                    List<Prompts> furtherPrompts = new List<Prompts> { };

                    int dateOffRollPromptID;
                    DateTime KSTestStartDate;
                    DateTime KSTestEndDate;

                    if (keyStage == 2)
                    {
                        dateOffRollPromptID = (int)Contants.PROMPT_ID_DATE_OF_ROLL_REMOVAL_KS2;
                        KSTestStartDate = KS2TestStartDate;
                        KSTestEndDate = KS2TestEndDate;
                    }
                    else if (keyStage == 3)
                    {
                        dateOffRollPromptID = (int)Contants.PROMPT_ID_DATE_OF_ROLL_REMOVAL_KS3;
                        KSTestStartDate = KS3TestStartDate;
                        KSTestEndDate = KS3TestEndDate;
                    }
                    else if (keyStage == 4)
                    {
                        dateOffRollPromptID = (int)Contants.PROMPT_ID_DATE_OF_ROLL_REMOVAL_KS4;
                        if (!promptAnswers.HasPromptAnswer(1801))
                        {
                            furtherPrompts.Add(GetPromptByPromptID(1801));                            
                            return new AdjustmentPromptAnalysis(furtherPrompts);
                        }
                        else if (IsPromptAnswerComplete(promptAnswers, dateOffRollPromptID))
                        {
                            DateTime dateOffRollRemoval = promptAnswers.GetPromptAnswerByPromptID(dateOffRollPromptID).PromptDateTimeAnswer.Value;

                            if (dateOffRollRemoval <= AnnualSchoolCensusDate)
                            {
                                if ((TSSchool.IsSchoolNonPlasc(student.Schools.DFESNumber)))
                                {
                                    return new AdjustmentPromptAnalysis(new CompletedNonStudentAdjustment(GetInfoPromptText(1815)));
                                }
                                else
                                { 
                                    //  non PLASC school
                                    if (!IsPromptAnswerComplete(promptAnswers, 1810))
                                    {
                                        furtherPrompts.Add(GetPromptByPromptID(1810));
                                        furtherPrompts.Add(GetPromptByPromptID(1811));
                                        return new AdjustmentPromptAnalysis(furtherPrompts);
                                    }
                                    else
                                    {
                                        return new AdjustmentPromptAnalysis(new CompletedStudentAdjustment(student.StudentID,
                                        inclusionReasonId,
                                        promptAnswers,
                                        Contants.SCRUTINY_REASON_LEFT_SCHOOL_BEFORE_ASC,
                                        null,
                                        Contants.SCRUTINY_STATUS_PENDINGFORVUS,
                                        null)
                                        );
                                        //return ProcessSingularFurtherPrompt(
                                        //    1810,
                                        //    student.StudentID,
                                        //    inclusionReasonId,
                                        //    promptAnswers,
                                        //    Contants.SCRUTINY_REASON_LEFT_SCHOOL_BEFORE_ASC,
                                        //    null,
                                        //    Contants.SCRUTINY_STATUS_PENDINGFORVUS,
                                        //    null);
                                    }
                                }
                            }
                            else // date of removal is greater than ACS
                            {
                                return ProcessExceptionalCircumstancesResponse(context,
                                   student.StudentID,
                                   inclusionReasonId,
                                   promptAnswers,
                                   Contants.SCRUTINY_REASON_LEFT_SCHOOL_AFTER_ASC,
                                   1820);
                            }
                        }
                        else
                        {
                            // dateOffRoll hasn't been provided.
                            throw Web09Exception.GetBusinessException(Web09MessageList.InsufficientStudentAdjustmentPromptAnswerProvided);
                        }
                    }
                    else
                    {
                        throw new Exception("Invalid key stage for student under adjustment.");
                    }

                    

                    // Collect Date from prompt
                    if (IsPromptAnswerComplete(promptAnswers, dateOffRollPromptID))
                    {
                        DateTime dateOffRollRemoval = promptAnswers.GetPromptAnswerByPromptID(dateOffRollPromptID).PromptDateTimeAnswer.Value;

                        //Evaluate for condition 318.10/218.10 from specs
                        if (dateOffRollRemoval <= KSTestStartDate && dateOffRollRemoval > AnnualSchoolCensusDate)
                        {
                            //The adjustment reason is complete, submit to database with a scrutiny status of accept.
                            //Include text info from prompt 31810 for KS3 and 21810 for KS2 as request completion message.
                            int infoPromptId;
                            if (keyStage == 2)
                                infoPromptId = 21810;
                            else
                                infoPromptId = 31810;

                            return new AdjustmentPromptAnalysis(new CompletedStudentAdjustment(
                                student.StudentID,
                                inclusionReasonId,
                                promptAnswers,
                                Contants.SCRUTINY_REASON_LEFT_SCHOOL_AFTER_ASC_AND_BEFORE_TESTS,
                                null,
                                Contants.SCRUTINY_STATUS_ACCEPTEDAUTOMATICALLY,
                                GetInfoPromptText(context, infoPromptId))
                                );
                        }
                        else if (dateOffRollRemoval <= AnnualSchoolCensusDate)
                        {
                            //if prompt 318.20 for KS3 or 218.20 for KS2 has been completed
                            int explanationPromptId;
                            if (keyStage == 2)
                                explanationPromptId = 21820;
                            else
                                explanationPromptId = 31820;

                            if (promptAnswers.HasPromptAnswer(explanationPromptId) && promptAnswers.GetPromptAnswerByPromptID(explanationPromptId).PromptStringAnswer != null)
                            {
                                //The adjustment reason is complete, submit to database with a scrutiny status of pending.
                                return new AdjustmentPromptAnalysis(new CompletedStudentAdjustment(
                                student.StudentID,
                                inclusionReasonId,
                                promptAnswers,
                                Contants.SCRUTINY_REASON_LEFT_SCHOOL_BEFORE_ASC,
                                null, //Rejection reason code not populated as adjustment accepted
                                Contants.SCRUTINY_STATUS_PENDINGFORVUS,
                                null)
                                );
                            }
                            else
                            {
                                //return text box prompt for 31820
                                furtherPrompts.Add(GetPromptByPromptID(explanationPromptId));
                                return new AdjustmentPromptAnalysis(furtherPrompts);
                            }
                        }
                        else if (dateOffRollRemoval > KSTestEndDate)
                        {
                            //Evaluate for condition 318.30 from specs
                            return ProcessExceptionalCircumstancesResponse(context, student.StudentID, inclusionReasonId, promptAnswers, Contants.SCRUTINY_REASON_LEFT_SCHOOL_AFTER_TEST_WEEK, 31830);


                        }
                        else //Date off role is greater than test start date and less than or equal to test end date.
                        {
                            //Evaluate for condition 318.40 from specs

                            //if prompt 318.20 for KS3 or 218.20 for KS2 has been completed
                            int explanationPromptId;
                            if (keyStage == 2)
                                explanationPromptId = 21840;
                            else
                                explanationPromptId = 31840;

                            //if prompt 318.40 has been completed
                            if (promptAnswers.HasPromptAnswer(explanationPromptId) && !String.IsNullOrEmpty(promptAnswers.GetPromptAnswerByPromptID(explanationPromptId).PromptStringAnswer))
                            {
                                //The adjustment reason is complete, submit to database with a scrutiny status of pending.
                                return new AdjustmentPromptAnalysis(new CompletedStudentAdjustment(
                                student.StudentID,
                                inclusionReasonId,
                                promptAnswers,
                                Contants.SCRUTINY_REASON_LEFT_SCHOOL_DURING_TESTS,
                                null,
                                Contants.SCRUTINY_STATUS_PENDINGFORVUS,
                                null)
                                );
                            }
                            else
                            {
                                //return text box prompt for 31840
                                furtherPrompts.Add(GetPromptByPromptID(31840));
                                return new AdjustmentPromptAnalysis(furtherPrompts);
                            }
                        }
                    }
                    else
                    {
                        // dateOfRollRemoval will be evalutated into either 318.10/218.10, 318.20/218.20, 318.30/218.30 or 318.40/218.40
                        throw Web09Exception.GetBusinessException(Web09MessageList.InsufficientStudentAdjustmentPromptAnswerProvided);
                    }

                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="inclusionReasonId"></param>
        /// <param name="promptAnswers"></param>
        /// <param name="studentId"></param>
        /// <returns></returns>
        public static AdjustmentPromptAnalysis ProcessInclusionPromptResponses_LeftSchoolRollBeforeTestsKS2(int inclusionReasonId, PromptAnswerList promptAnswers, int studentId)
        {
            int dateOffRollPromptID = (int)Contants.PROMPT_ID_DATE_OF_ROLL_REMOVAL_KS2;
            DateTime KSTestStartDate = KS2TestStartDate;
            DateTime KSTestEndDate = KS2TestEndDate;

            // Collect Date from prompt
            if (IsPromptAnswerComplete(promptAnswers, dateOffRollPromptID))
            {
                DateTime dateOffRollRemoval = promptAnswers.GetPromptAnswerByPromptID(dateOffRollPromptID).PromptDateTimeAnswer.Value;

                if (!TSStudent.IsStudentListed(studentId))
                {
                    return new AdjustmentPromptAnalysis(new CompletedStudentAdjustment(
                        studentId,
                        inclusionReasonId,
                        promptAnswers,
                        Contants.SCRUTINY_REASON_LEFT_SCHOOL_BEFORE_TESTS,
                        null,
                        Contants.SCRUTINY_STATUS_PENDINGFORVUS,
                        null)
                        );
                }

                //Evaluate for condition 318.20/218.20 from specs
                if (dateOffRollRemoval <= KSTestStartDate )//&& dateOffRollRemoval > AnnualSchoolCensusDate)
                {
                    return new AdjustmentPromptAnalysis(new CompletedStudentAdjustment(
                        studentId,
                        inclusionReasonId,
                        promptAnswers,
                        Contants.SCRUTINY_REASON_LEFT_SCHOOL_BEFORE_TESTS,
                        null,
                        Contants.SCRUTINY_STATUS_PENDINGFORVUS,
                        GetInfoPromptText(21820))
                        );
                }
                else if (dateOffRollRemoval > KSTestEndDate)
                {
                    return ProcessExceptionalCircumstancesResponse(studentId,
                        inclusionReasonId,
                        promptAnswers,
                        Contants.SCRUTINY_REASON_LEFT_SCHOOL_AFTER_TEST_WEEK,
                        21830);
                }
                else //Date off role is greater than test start date and less than or equal to test end date.
                {
                    return ProcessSingularFurtherPrompt(
                        21840,
                        studentId,
                        inclusionReasonId,
                        promptAnswers,
                        Contants.SCRUTINY_REASON_LEFT_SCHOOL_DURING_TESTS,
                        null,
                        Contants.SCRUTINY_STATUS_PENDINGFORVUS,
                        null);
                }
            }
            else
            {
                // dateOfRollRemoval will be evalutated into either 318.10/218.10, 318.20/218.20, 318.30/218.30 or 318.40/218.40
                throw Web09Exception.GetBusinessException(Web09MessageList.InsufficientStudentAdjustmentPromptAnswerProvided);
            }
        }

        private static AdjustmentPromptAnalysis ProcessInclusionPromptResponses_LeftBeforeExamsKS5(int studentId, int inclusionReasonid, PromptAnswer answers)
        {
            return new AdjustmentPromptAnalysis(new CompletedNonStudentAdjustment(GetInfoPromptText(5600)));
        }
    }
}
