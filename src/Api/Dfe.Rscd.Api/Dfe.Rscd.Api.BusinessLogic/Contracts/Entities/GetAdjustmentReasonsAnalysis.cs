using System.Collections.Generic;
using Dfe.Rscd.Api.Infrastructure.SqlServer.DTOs;

namespace Dfe.Rscd.Api.BusinessLogic.Contracts.Entities
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
