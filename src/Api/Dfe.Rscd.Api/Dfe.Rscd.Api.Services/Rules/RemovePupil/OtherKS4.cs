﻿using System.Collections.Generic;
using Dfe.Rscd.Api.BusinessLogic.Entities;

namespace Dfe.Rscd.Api.Services.Rules
{
    public partial class RemovePupilRulesV2
    {

        public AdjustmentOutcome ProcessInclusionPromptResponses_OtherKS4(Pupil student, int inclusionReasonId, List<PromptAnswer> promptAnswers)
        {

            //// Collect descriptive text answer from prompt
            //if (promptAnswers.HasPromptAnswer(1900) && IsPromptAnswerComplete(promptAnswers, 1900))
            //{
            //    return new AdjustmentOutcome(new CompletedStudentAdjustment(student.StudentID,
            //        inclusionReasonId,
            //        promptAnswers,
            //        Contants.SCRUTINY_REASON_OTHER,
            //        null,
            //        Contants.SCRUTINY_STATUS_PENDINGFORVUS,
            //        null)
            //        );

            //}
            //else
            //{
            //    //Error with answer not found.
            //    throw Web09Exception.GetBusinessException(Web09MessageList.InsufficientStudentAdjustmentPromptAnswerProvided);
            //}

            return new AdjustmentOutcome(new CompletedNonStudentAdjustment("TODO"));
        }
        
    }
}
