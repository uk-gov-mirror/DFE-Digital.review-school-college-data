﻿using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using Dfe.Rscd.Api.Domain.Entities.Questions;

namespace Dfe.Rscd.Api.Domain.Entities
{
    public class AmendmentOutcome
    {
        [AllowNull]
        public List<Question> FurtherQuestions { get; set; }
        [AllowNull]
        public Dictionary<string, List<string>> ValidationErrors { get; set; }
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

        public AmendmentOutcome(List<Question> questions, Dictionary<string, List<string>> errors)
        {
            IsComplete = false;
            FurtherQuestions = questions;
            ValidationErrors = errors;
            OutcomeStatus = OutcomeStatus.AwaitingValidationPass;
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
                OutcomeStatus = OutcomeStatus.AwaitingValidationPass;
                IsComplete = false;
                IsAmendmentCreated = false;
            }
            
        }
    }
}