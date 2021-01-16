using System;
using System.Collections.Generic;

#nullable disable

namespace Dfe.Rscd.Api.Infrastructure.SqlServer.DTOs
{
    internal partial class PinclinclusionAdjDatum
    {
        internal string PinclinclusionAdjustmentsPIncl { get; set; }
        internal short PinclinclusionAdjustmentsIncAdjReasonId { get; set; }
        internal short PromptsPromptId { get; set; }

        internal virtual PinclinclusionAdjustment PinclinclusionAdjustments { get; set; }
        internal virtual Prompt PromptsPrompt { get; set; }
    }
}
