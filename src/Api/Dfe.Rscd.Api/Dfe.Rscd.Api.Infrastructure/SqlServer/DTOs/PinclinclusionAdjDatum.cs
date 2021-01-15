using System;
using System.Collections.Generic;

#nullable disable

namespace Dfe.Rscd.Api.Infrastructure.SqlServer.DTOs
{
    public partial class PinclinclusionAdjDatum
    {
        public string PinclinclusionAdjustmentsPIncl { get; set; }
        public short PinclinclusionAdjustmentsIncAdjReasonId { get; set; }
        public short PromptsPromptId { get; set; }

        public virtual PinclinclusionAdjustment PinclinclusionAdjustments { get; set; }
        public virtual Prompt PromptsPrompt { get; set; }
    }
}
