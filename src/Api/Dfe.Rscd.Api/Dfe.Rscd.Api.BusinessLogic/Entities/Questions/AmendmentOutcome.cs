using System;
using System.Collections.Generic;

namespace Dfe.Rscd.Api.BusinessLogic.Entities
{
    public class AmendmentOutcome
    {
        public List<Prompt> FurtherPrompts { get; set; }
        public CompletedStudentAdjustment CompletedRequest;
        public CompletedNonStudentAdjustment CompletedNonRequest;
        public CompleteSimpleOutcomeCheck CompleteSimpleOutcome;
        public bool IsComplete { get; set; }
        public bool IsAmendmentCreated { get; set; }
        public Guid NewAmendmentId { get; set; }
        public string NewAmendmentReferenceNumber { get; set; }

        public OutcomeStatus OutcomeStatus { get; set; }

        public EvidenceStatus EvidenceStatus { get; set; }

        public AmendmentOutcome(List<Prompt> furtherPrompts)
        {
            if (furtherPrompts == null)
            {
                FurtherPrompts = null;
                IsComplete = true;
                IsAmendmentCreated = false;
            }
            else
            {
                FurtherPrompts = furtherPrompts;
                IsComplete = false;
                IsAmendmentCreated = false;
            }
            
        }

        public AmendmentOutcome(CompletedStudentAdjustment completedRequest)
        {
            CompletedRequest = completedRequest;
            OutcomeStatus = completedRequest.OutcomeStatus;
            IsComplete = true;
            IsAmendmentCreated = false;
        }

        public AmendmentOutcome(CompleteSimpleOutcomeCheck completedRequest)
        {
            CompleteSimpleOutcome = completedRequest;
            OutcomeStatus = completedRequest.OutcomeStatus;
            IsComplete = true;
            IsAmendmentCreated = false;
        }

        public AmendmentOutcome(CompletedNonStudentAdjustment completedNonRequest)
        {
            CompletedNonRequest = completedNonRequest;
            OutcomeStatus = completedNonRequest.OutcomeStatus;
            IsComplete = true;
            IsAmendmentCreated = false;
        }

    }
}
