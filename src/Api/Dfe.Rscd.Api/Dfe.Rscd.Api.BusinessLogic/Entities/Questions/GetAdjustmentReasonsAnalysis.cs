using System.Collections.Generic;

namespace Dfe.Rscd.Api.BusinessLogic.Entities
{
    public class GetAdjustmentReasonsResponse : AdjustmentPromptAnalysis
    {

        public IEnumerable<InclusionAdjustmentReason> AdjustmentReasonList;
        public string PriorMessage;

        public GetAdjustmentReasonsResponse(IEnumerable<InclusionAdjustmentReason> adjustmentReasonList, string priorMessage)
        {
            AdjustmentReasonList = adjustmentReasonList;
            PriorMessage = priorMessage;
            IsAdjustmentCreated = false;
            IsComplete = false;
        }

        public GetAdjustmentReasonsResponse(List<Prompts> furtherPrompts) : base(furtherPrompts){}

        public GetAdjustmentReasonsResponse(CompletedStudentAdjustment completedRequest) : base(completedRequest) {}

        public GetAdjustmentReasonsResponse(CompletedNonStudentAdjustment completedNonRequest) : base(completedNonRequest){}
    }
}
