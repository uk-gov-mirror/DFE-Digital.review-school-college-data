using System.Collections.Generic;

namespace Dfe.Rscd.Api.BusinessLogic.Contracts.Entities
{
    public class AdjustmentPromptAnalysis
    {
        
        public List<Prompts> FurtherPrompts;
        public CompletedStudentAdjustment CompletedRequest;
        public CompletedNonStudentAdjustment CompletedNonRequest;
        public bool IsComplete;
        public bool IsAdjustmentCreated;

        protected AdjustmentPromptAnalysis() { }

        public AdjustmentPromptAnalysis(List<Prompts> furtherPrompts)
        {
            FurtherPrompts = furtherPrompts;
            IsComplete = false;
            IsAdjustmentCreated = false;
        }

        public AdjustmentPromptAnalysis(CompletedStudentAdjustment completedRequest)
        {
            CompletedRequest = completedRequest;
            IsComplete = true;
            IsAdjustmentCreated = true;
        }

        public AdjustmentPromptAnalysis(CompletedNonStudentAdjustment completedNonRequest)
        {
            CompletedNonRequest = completedNonRequest;
            IsComplete = true;
            IsAdjustmentCreated = false;
        }

    }
}
