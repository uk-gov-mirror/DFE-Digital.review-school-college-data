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

        protected AdjustmentOutcome() { }

        public AdjustmentOutcome(List<Prompt> furtherPrompts)
        {
            FurtherPrompts = furtherPrompts;
            IsComplete = false;
            IsAdjustmentCreated = false;
        }

        public AdjustmentOutcome(CompletedStudentAdjustment completedRequest)
        {
            CompletedRequest = completedRequest;
            IsComplete = true;
            IsAdjustmentCreated = true;
        }

        public AdjustmentOutcome(CompleteSimpleOutcomeCheck completedRequest)
        {
            CompleteSimpleOutcome = completedRequest;
            IsComplete = true;
            IsAdjustmentCreated = false;
        }

        public AdjustmentOutcome(CompletedNonStudentAdjustment completedNonRequest)
        {
            CompletedNonRequest = completedNonRequest;
            IsComplete = true;
            IsAdjustmentCreated = false;
        }

    }
}
