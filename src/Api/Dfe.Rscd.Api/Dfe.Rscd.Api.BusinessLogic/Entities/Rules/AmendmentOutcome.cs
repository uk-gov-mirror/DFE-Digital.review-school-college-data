using System;
using System.Collections.Generic;

namespace Dfe.Rscd.Api.Domain.Entities
{
    public class AmendmentOutcome
    {
        public List<Question> FurtherQuestions { get; set; }
        public bool IsComplete { get; set; }
        public bool IsAmendmentCreated { get; set; }
        public Guid NewAmendmentId { get; set; }
        public string NewAmendmentReferenceNumber { get; set; }

        public string ScrutinyDetail { get; set; }
        public string ScrutinyStatusCode { get; set; }

        public int ReasonId { get; set; }

        public OutcomeStatus OutcomeStatus { get; set; }

        public EvidenceStatus EvidenceStatus { get; set; }
        public string Message { get; set; }

        public AmendmentOutcome(OutcomeStatus status, string message)
        {
            OutcomeStatus = status;
            IsComplete = true;
            FurtherQuestions = null;
            Message = message;
        }

        public AmendmentOutcome(OutcomeStatus status)
        {
            OutcomeStatus = status;
            IsComplete = true;
            FurtherQuestions = null;
        }

        public AmendmentOutcome(List<Question> furtherQuestion)
        {
            if (furtherQuestion == null)
            {
                FurtherQuestions = null;
                IsComplete = true;
                IsAmendmentCreated = false;
            }
            else
            {
                FurtherQuestions = furtherQuestion;
                IsComplete = false;
                IsAmendmentCreated = false;
            }
            
        }
    }
}
