using Web09.Checking.Business.Logic.Common;
using Web09.Checking.Business.Logic.Entities;
using Web09.Checking.DataAccess;
using Web09.Common.Exceptions;

namespace Web09.Checking.Business.Logic.TransactionScripts
{
    public partial class TSIncludeRemovePupil
    {

        public static AdjustmentPromptAnalysis ProcessInclusionPromptResponses_OtherKS4(Students student, int inclusionReasonId, PromptAnswerList promptAnswers)
        {

            // Collect descriptive text answer from prompt
            if (promptAnswers.HasPromptAnswer(1900) && IsPromptAnswerComplete(promptAnswers, 1900))
            {
                return new AdjustmentPromptAnalysis(new CompletedStudentAdjustment(student.StudentID,
                    inclusionReasonId,
                    promptAnswers,
                    Contants.SCRUTINY_REASON_OTHER,
                    null,
                    Contants.SCRUTINY_STATUS_PENDINGFORVUS,
                    null)
                    );

            }
            else
            {
                //Error with answer not found.
                throw Web09Exception.GetBusinessException(Web09MessageList.InsufficientStudentAdjustmentPromptAnswerProvided);
            }


            //******************************************************************************************
            //This was changed from having a subset of reasons under other to just a simple text box.
            //
            //Retain the original code until decision confirmed by DA's and DCSF. At the time of
            //this comment, this change request came from DCSF, but was not sighted by DA's
            //
            //******************************************************************************************
            //
            //PromptAnswer answer = promptAnswers.GetPromptAnswerByPromptID(Contants.PROMPT_ID_OTHER_KS4);
            //List<Prompts> furtherPrompts = new List<Prompts> { };

            //int promptAnswerID;

            //// Collect Dropdown answer from prompt
            //if (int.TryParse(answer.PromptSelectedValueAnswer, out promptAnswerID))
            //{

            //    //1901
            //    if (promptAnswerID == 1901)
            //    {

            //        return ProcessNonConditionalRequiredTextBoxPrompt(
            //            student.StudentID,                                      //Student for whom request is generated
            //            inclusionReasonId,                                      //Inclusion reason
            //            promptAnswers,                                          //Prompt Answers provided
            //            1901,                                                   //Text Box Prompt ID
            //            Contants.SCRUTINY_REASON_ILLNESS,         //The scrutiny reason if answer is accepted
            //            null,                                                   //No rejection code
            //            Contants.SCRUTINY_STATUS_PENDINGFORVUS,   //Scrutiny status if message provided
            //            null                                                    //Completion message
            //            );

            //    }
            //    //1902
            //    else if (promptAnswerID == 1902)
            //    {
            //        return ProcessExceptionalCircumstancesResponse(student.StudentID,
            //            inclusionReasonId,
            //            promptAnswers,
            //            Contants.SCRUTINY_REASON_HOME_TUITION,
            //            1902);
            //    }

            //    //1903
            //    else if (promptAnswerID == 1903)
            //    {
            //        return ProcessExceptionalCircumstancesResponse(student.StudentID,
            //            inclusionReasonId,
            //            promptAnswers,
            //            Contants.SCRUTINY_REASON_FUNDING_FOLLOWED,
            //            1903);
            //    }

            //    //1904
            //    else if (promptAnswerID == 1904)
            //    {
            //        return ProcessExceptionalCircumstancesResponse(student.StudentID,
            //            inclusionReasonId,
            //            promptAnswers,
            //            Contants.SCRUTINY_REASON_LEFT_SCHOOL_AFTER_ASC,
            //            1904);
            //    }

            //    //1905
            //    else if (promptAnswerID == 1905)
            //    {
            //        //Link to 18
            //        return ProcessInclusionPromptResponses_LeftSchoolRollBeforeTests(student, inclusionReasonId, promptAnswers);
            //    }

            //    //1906
            //    else if (promptAnswerID == 1906)
            //    {
            //        return ProcessNonConditionalRequiredTextBoxPrompt(
            //           student.StudentID,                                      //Student for whom request is generated
            //           inclusionReasonId,                                      //Inclusion reason
            //           promptAnswers,                                          //Prompt Answers provided
            //           1906,                                                   //Text Box Prompt ID
            //           Contants.SCRUTINY_REASON_ATTENDANCE,         //The scrutiny reason if answer is accepted
            //           null,                                                   //No rejection code
            //           Contants.SCRUTINY_STATUS_PENDINGFORVUS,   //Scrutiny status if message provided
            //           null                                                    //Completion message
            //           );
            //    }

            //    //1907
            //    else if (promptAnswerID == 1907)
            //    {
            //        //Link to reason 14
            //        return ProcessKS4NCYearGroupAdjustment(student, inclusionReasonId, promptAnswers, 1907);
            //    }

            //    //1908
            //    else if (promptAnswerID == 1908)
            //    {
            //        return ProcessExceptionalCircumstancesResponse(student.StudentID,
            //           inclusionReasonId,
            //           promptAnswers,
            //           Contants.SCRUTINY_REASON_SPECIAL_NEEDS,
            //           1908);
            //    }

            //    //1909
            //    else if (promptAnswerID == 1909)
            //    {
            //        return ProcessNonConditionalRequiredTextBoxPrompt(
            //           student.StudentID,                                      //Student for whom request is generated
            //           inclusionReasonId,                                      //Inclusion reason
            //           promptAnswers,                                          //Prompt Answers provided
            //           1909,                                                   //Text Box Prompt ID
            //           Contants.SCRUTINY_REASON_REMOVE_DUAL_REGISTERED,         //The scrutiny reason if answer is accepted
            //           null,                                                   //No rejection code
            //           Contants.SCRUTINY_STATUS_PENDINGFORVUS,   //Scrutiny status if message provided
            //           null                                                    //Completion message
            //           );
            //    }

            //    //1910
            //    else if (promptAnswerID == 1910)
            //    {
            //        return ProcessNonConditionalRequiredTextBoxPrompt(
            //           student.StudentID,                                      //Student for whom request is generated
            //           inclusionReasonId,                                      //Inclusion reason
            //           promptAnswers,                                          //Prompt Answers provided
            //           1910,                                                   //Text Box Prompt ID
            //           Contants.SCRUTINY_REASON_PRISON,          //The scrutiny reason if answer is accepted
            //           null,                                                   //No rejection code
            //           Contants.SCRUTINY_STATUS_PENDINGFORVUS,   //Scrutiny status if message provided
            //           null                                                    //Completion message
            //           );
            //    }

            //    //1911
            //    else if (promptAnswerID == 1911)
            //    {
            //        return ProcessExceptionalCircumstancesResponse(student.StudentID,
            //           inclusionReasonId,
            //           promptAnswers,
            //           Contants.SCRUTINY_REASON_PUPIL_NOT_KNOWN,
            //           1911);
            //    }

            //    //1912
            //    else if (promptAnswerID == 1912)
            //    {
            //        return ProcessExceptionalCircumstancesResponse(student.StudentID,
            //           inclusionReasonId,
            //           promptAnswers,
            //           Contants.SCRUTINY_REASON_TRAVELLER,
            //           1912);
            //    }

            //    //1913
            //    else if (promptAnswerID == 1913)
            //    {
            //        return ProcessNonConditionalRequiredTextBoxPrompt(
            //           student.StudentID,                                      //Student for whom request is generated
            //           inclusionReasonId,                                      //Inclusion reason
            //           promptAnswers,                                          //Prompt Answers provided
            //           1913,                                                   //Text Box Prompt ID
            //           Contants.SCRUTINY_REASON_CONTINGENCY,     //The scrutiny reason if answer is accepted
            //           null,                                                   //No rejection code
            //           Contants.SCRUTINY_STATUS_PENDINGFORVUS,   //Scrutiny status if message provided
            //           null                                                    //Completion message
            //           );
            //    }

            //    //1914
            //    else if (promptAnswerID == 1914)
            //    {
            //        return ProcessNonConditionalRequiredTextBoxPrompt(
            //           student.StudentID,                                      //Student for whom request is generated
            //           inclusionReasonId,                                      //Inclusion reason
            //           promptAnswers,                                          //Prompt Answers provided
            //           1914,                                                   //Text Box Prompt ID
            //           Contants.SCRUTINY_REASON_OTHER,           //The scrutiny reason if answer is accepted
            //           null,                                                   //No rejection code
            //           Contants.SCRUTINY_STATUS_PENDINGFORVUS,   //Scrutiny status if message provided
            //           null                                                    //Completion message
            //           );
            //    }
            //    else
            //    {
            //        throw new ArgumentOutOfRangeException("promptAnswerID", "PromptAnswerID for prompt Other is out of range.");
            //    }

            //}
            //else
            //{
            //    //Error with answer not found.
            //    throw Web09Exception.GetBusinessException(Web09MessageList.InsufficientStudentAdjustmentPromptAnswerProvided);
            //}
        }
        
    }
}
