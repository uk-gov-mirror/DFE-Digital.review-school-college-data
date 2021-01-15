using System.Collections.Generic;

namespace Dfe.Rscd.Api.BusinessLogic.Contracts.Entities
{
    public class AdjustmentFurtherPrompts
    {
        public List<Prompts> FurtherPrompts;
        public bool IsNonAdjustment;
        
        public AdjustmentFurtherPrompts(List<Prompts> furtherPrompts)
        {
            FurtherPrompts = furtherPrompts;
            IsNonAdjustment = false;
        }

        public AdjustmentFurtherPrompts(List<Prompts> furtherPrompts, bool isNonAdjustment)
        {
            FurtherPrompts = furtherPrompts;
            IsNonAdjustment = isNonAdjustment;
        }

            
    }
}
