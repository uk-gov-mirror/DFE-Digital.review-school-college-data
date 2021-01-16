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
        /// <param name="inclusionReasonId">inclusionReasonId</param>
        /// <param name="promptAnswers">promptAnswers. List of PromptAnswers</param>
        /// <param name="studentId">studentId</param>
        /// <throws>ArgumentException. However, logically impossible for valid date to throw exception</throws>
        /// <returns>Either: List of Prompts<see cref="Web09.Checking.DataAccess.Prompts"/> to be completed by UI or a CompletedStudentAdjustment</returns>
        public static AdjustmentPromptAnalysis ProcessInclusionPromptResponses_OtherKS3(Students student, int inclusionReasonId, PromptAnswerList promptAnswers)
        {
            PromptAnswer answer = promptAnswers.GetPromptAnswerByPromptID(Contants.PROMPT_ID_OTHER_KS3);
            List<Prompts> furtherPrompts = new List<Prompts> { };

            int promptAnswerID;

            // Collect Dropdown answer from prompt
            if (int.TryParse(answer.PromptSelectedValueAnswer, out promptAnswerID))
            {
                
                //31901
                if (promptAnswerID == 31901)
                {

                    return ProcessNonConditionalRequiredTextBoxPrompt(
                        student.StudentID,                                              //Student for whom request is generated
                        inclusionReasonId,                                      //Inclusion reason
                        promptAnswers,                                          //Prompt Answers provided
                        31901,                                                  //Text Box Prompt ID
                        Contants.SCRUTINY_REASON_ILLNESS,         //The scrutiny reason if answer is accepted
                        null,                                                   //No rejection code
                        Contants.SCRUTINY_STATUS_PENDINGFORVUS,   //Scrutiny status if message provided
                        null                                                    //Completion message
                        );
                    
                }

                //31902
                else if (promptAnswerID == 31902)
                {
                    //Check for answer to Exceptional Circumstances prompt
                    return ProcessExceptionalCircumstancesResponse(student.StudentID, inclusionReasonId, promptAnswers, Contants.SCRUTINY_REASON_HOME_TUITION, 31902);
                }

                //31903
                else if (promptAnswerID == 31903)
                {
                    //Check for answer to Exceptional Circumstances prompt
                    return ProcessExceptionalCircumstancesResponse(student.StudentID, inclusionReasonId, promptAnswers, Contants.SCRUTINY_REASON_FUNDING_FOLLOWED, 31903);
                }

                //31905
                else if (promptAnswerID == 31905)
                {
                    throw new NotImplementedException();
                }

                //31906
                else if (promptAnswerID == 31906)
                {
                    return ProcessNonConditionalRequiredTextBoxPrompt(
                        student.StudentID,                                      //Student for whom request is generated
                        inclusionReasonId,                                      //Inclusion reason
                        promptAnswers,                                          //Prompt Answers provided
                        31906,                                                  //Text Box Prompt ID
                        Contants.SCRUTINY_REASON_ATTENDANCE,      //The scrutiny reason if answer is accepted
                        null,                                                   //No rejection code
                        Contants.SCRUTINY_STATUS_PENDINGFORVUS,   //Scrutiny status if message provided
                        null                                                    //Completion message
                        );
                }

                //31907
                else if (promptAnswerID == 31907)
                {
                    return ProcessKS3NCYearGroupAdjustment(student, inclusionReasonId, promptAnswers, 31907);
                }

                //31908
                else if (promptAnswerID == 31908)
                {
                    //Check for answer to Exceptional Circumstances prompt
                    return ProcessExceptionalCircumstancesResponse(student.StudentID, inclusionReasonId, promptAnswers, Contants.SCRUTINY_REASON_SPECIAL_NEEDS, 31908);
                }

                //31909
                else if (promptAnswerID == 31909)
                {
                    return ProcessNonConditionalRequiredTextBoxPrompt(
                        student.StudentID,                                      //Student for whom request is generated
                        inclusionReasonId,                                      //Inclusion reason
                        promptAnswers,                                          //Prompt Answers provided
                        31909,                                                  //Text Box Prompt ID
                        Contants.SCRUTINY_REASON_REMOVE_DUAL_REGISTERED,      //The scrutiny reason if answer is accepted
                        null,                                                   //No rejection code
                        Contants.SCRUTINY_STATUS_PENDINGFORVUS,   //Scrutiny status if message provided
                        null                                                    //Completion message
                        );
                }

                //31910
                else if (promptAnswerID == 31910)
                {
                    return ProcessNonConditionalRequiredTextBoxPrompt(
                        student.StudentID,                                      //Student for whom request is generated
                        inclusionReasonId,                                      //Inclusion reason
                        promptAnswers,                                          //Prompt Answers provided
                        31910,                                                  //Text Box Prompt ID
                        Contants.SCRUTINY_REASON_PRISON,          //The scrutiny reason if answer is accepted
                        null,                                                   //No rejection code
                        Contants.SCRUTINY_STATUS_PENDINGFORVUS,   //Scrutiny status if message provided
                        null                                                    //Completion message
                        );
                }

                //31911
                else if (promptAnswerID == 31911)
                {
                    //Check for answer to Exceptional Circumstances prompt
                    return ProcessExceptionalCircumstancesResponse(student.StudentID, inclusionReasonId, promptAnswers, Contants.SCRUTINY_REASON_PUPIL_NOT_KNOWN, 31911);
                }

                //31912
                else if (promptAnswerID == 31912)
                {
                    //Check for answer to Exceptional Circumstances prompt
                    return ProcessExceptionalCircumstancesResponse(student.StudentID, inclusionReasonId, promptAnswers, Contants.SCRUTINY_REASON_TRAVELLER, 31912);
                }

                //31913
                else if (promptAnswerID == 31913)
                {
                    return ProcessNonConditionalRequiredTextBoxPrompt(
                        student.StudentID,                                      //Student for whom request is generated
                        inclusionReasonId,                                      //Inclusion reason
                        promptAnswers,                                          //Prompt Answers provided
                        31913,                                                  //Text Box Prompt ID
                        Contants.SCRUTINY_REASON_CONTINGENCY,      //The scrutiny reason if answer is accepted
                        null,                                                   //No rejection code
                        Contants.SCRUTINY_STATUS_PENDINGFORVUS,   //Scrutiny status if message provided
                        null                                                    //Completion message
                        );
                    
                }

                //31914
                else if (promptAnswerID == 31914)
                {
                    return ProcessNonConditionalRequiredTextBoxPrompt(
                        student.StudentID,                                      //Student for whom request is generated
                        inclusionReasonId,                                      //Inclusion reason
                        promptAnswers,                                          //Prompt Answers provided
                        31914,                                                  //Text Box Prompt ID
                        Contants.SCRUTINY_REASON_OTHER,      //The scrutiny reason if answer is accepted
                        null,                                                   //No rejection code
                        Contants.SCRUTINY_STATUS_PENDINGFORVUS,   //Scrutiny status if message provided
                        null                                                    //Completion message
                        );
                    
                }
                else
                {
                    throw new ArgumentOutOfRangeException("promptAnswerID", "PromptAnswerID for prompt Other is out of range.");
                }

            }
            else
            {
                //Error with answer not found.
                throw Web09Exception.GetBusinessException(Web09MessageList.InsufficientStudentAdjustmentPromptAnswerProvided);
            }
        }

        /// <summary>
        /// A generic method to analyse a response to a text box prompt that has no conditions other than
        /// that a text answer has been provided.
        /// </summary>
        /// <returns>AdjustmentPromptAnalysis to indicate if prompt answer is complete or not</returns>
        private static AdjustmentPromptAnalysis ProcessNonConditionalRequiredTextBoxPrompt(int studentId, 
            int inclusionReasonId, 
            PromptAnswerList promptAnswers, 
            int textBoxPromptId,
            int requestReason,
            int? rejectionReason,
            string scrutinyStatus,
            string requestCompletionMessage)
        {
            if (promptAnswers.HasPromptAnswer(textBoxPromptId) && !String.IsNullOrEmpty(promptAnswers.GetPromptAnswerByPromptID(textBoxPromptId).PromptStringAnswer))
            {
                //The adjustment reason is complete, submit to database with a scrutiny status of pending.
                return new AdjustmentPromptAnalysis(new CompletedStudentAdjustment(
                    studentId,
                    inclusionReasonId,
                    promptAnswers,
                    requestReason,
                    rejectionReason,
                    scrutinyStatus,
                    requestCompletionMessage)
                    );
            }
            else if (promptAnswers.HasPromptAnswer(textBoxPromptId) && !String.IsNullOrEmpty(promptAnswers.GetPromptAnswerByPromptID(textBoxPromptId).PromptStringAnswer))
            {
                //Text Box Prompt exists but no answer provided => throw error.
                throw Web09Exception.GetBusinessException(Web09MessageList.InsufficientStudentAdjustmentPromptAnswerProvided);

            }
            else
            {
                List<Prompts> furtherPrompts = new List<Prompts>();
                furtherPrompts.Add(GetPromptByPromptID(textBoxPromptId));
                return new AdjustmentPromptAnalysis(furtherPrompts);
            }
        }

    }

    
}
