using System;
using System.Collections.Generic;

namespace Dfe.Rscd.Api.BusinessLogic.Entities
{
    public class AdjustmentOutcome
    {
        public List<Prompt> FurtherPrompts;
        public CompletedStudentAdjustment CompletedRequest;
        public CompletedNonStudentAdjustment CompletedNonRequest;
        public CompleteSimpleOutcomeCheck CompleteSimpleOutcome;
        public bool IsComplete;
        public bool IsAdjustmentCreated;
        public Guid NewAmendmentId { get;set; }
        public string NewAmendmentReferenceNumber { get; set; }

        public OutcomeStatus OutcomeStatus { get; set; }


        public AdjustmentOutcome(List<Prompt> furtherPrompts)
        {
            FurtherPrompts = furtherPrompts;
            IsComplete = false;
            IsAdjustmentCreated = false;
        }

        public AdjustmentOutcome(CompletedStudentAdjustment completedRequest)
        {
            CompletedRequest = completedRequest;
            OutcomeStatus = completedRequest.OutcomeStatus;
            IsComplete = true;
            IsAdjustmentCreated = true;
        }

        public AdjustmentOutcome(CompleteSimpleOutcomeCheck completedRequest)
        {
            CompleteSimpleOutcome = completedRequest;
            OutcomeStatus = completedRequest.OutcomeStatus;
            IsComplete = true;
            IsAdjustmentCreated = false;
        }

        public AdjustmentOutcome(CompletedNonStudentAdjustment completedNonRequest)
        {
            CompletedNonRequest = completedNonRequest;
            OutcomeStatus = completedNonRequest.OutcomeStatus;
            IsComplete = true;
            IsAdjustmentCreated = false;
        }

    }
}
