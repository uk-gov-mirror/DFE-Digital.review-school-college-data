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
        private static AdjustmentPromptAnalysis ProcessInclusionPromptResponses_OtherKS5(int studentId, int inclusionReasonId, PromptAnswerList answers)
        {
            PromptAnswer answer = answers.GetPromptAnswerByPromptID(Contants.PROMPT_ID_OTHER_KS5);
            List<Prompts> furtherPrompts = new List<Prompts> { };

            int promptAnswerID;

            // Collect Dropdown answer from prompt
            if (int.TryParse(answer.PromptSelectedValueAnswer, out promptAnswerID))
            {

                //5610
                if (promptAnswerID == 5610)
                {
                    return new AdjustmentPromptAnalysis(new CompletedStudentAdjustment(studentId,
                        inclusionReasonId,
                        answers,
                        Contants.SCRUTINY_REASON_DEATH,
                        null,
                        Contants.SCRUTINY_STATUS_ACCEPTEDAUTOMATICALLY,
                        GetInfoPromptText(5610))
                        );
                }
                //5620
                else if (promptAnswerID == 5620)
                {
                    return new AdjustmentPromptAnalysis(new CompletedStudentAdjustment(studentId,
                        inclusionReasonId,
                        answers,
                        Contants.SCRUTINY_REASON_ONE_YEAR_COURSE,
                        null,
                        Contants.SCRUTINY_STATUS_ACCEPTEDAUTOMATICALLY,
                        GetInfoPromptText(5620))
                        );
                }
                //5630
                else if (promptAnswerID == 5630)
                {
                    return new AdjustmentPromptAnalysis(new CompletedStudentAdjustment(studentId,
                        inclusionReasonId,
                        answers,
                        Contants.SCRUTINY_REASON_WORK_BASED_LEARNER,
                        null,
                        Contants.SCRUTINY_STATUS_ACCEPTEDAUTOMATICALLY,
                        GetInfoPromptText(5630))
                        );
                }
                //5640
                else if (promptAnswerID == 5640)
                {
                    return new AdjustmentPromptAnalysis(new CompletedStudentAdjustment(studentId,
                        inclusionReasonId,
                        answers,
                        Contants.SCRUTINY_REASON_PART_TIME,
                        null,
                        Contants.SCRUTINY_STATUS_ACCEPTEDAUTOMATICALLY,
                        GetInfoPromptText(5640))
                        );
                }
                //5650 --> Contingency only, currently not required.
                //else if (promptAnswerID == 5650)
                //{

                //}
                //5660
                else if (promptAnswerID == 5660)
                {
                    return ProcessSingularFurtherPrompt(5660, studentId, inclusionReasonId, answers,
                        Contants.SCRUTINY_REASON_OTHER,
                        null,
                        Contants.SCRUTINY_STATUS_PENDINGFORVUS,
                        null);
                }
                else
                {
                    //Answer outside the range of options
                    throw new ArgumentOutOfRangeException("promptAnswerID", "PromptAnswerID for prompt 5600 - Other is out of range.");
                }

            }
            else
            {
                throw Web09Exception.GetBusinessException(Web09MessageList.InsufficientStudentAdjustmentPromptAnswerProvided);
            }
        }
    }
}
