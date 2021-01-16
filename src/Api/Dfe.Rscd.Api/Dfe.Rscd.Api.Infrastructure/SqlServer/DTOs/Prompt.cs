using System;
using System.Collections.Generic;

#nullable disable

namespace Dfe.Rscd.Api.Infrastructure.SqlServer.DTOs
{
    internal partial class Prompt
    {
        internal Prompt()
        {
            PinclinclusionAdjData = new HashSet<PinclinclusionAdjDatum>();
            PromptResponses = new HashSet<PromptResponse>();
        }

        internal short PromptId { get; set; }
        internal string PromptText { get; set; }
        internal bool IsConditional { get; set; }
        internal bool AllowNulls { get; set; }
        internal short PromptTypesPromptTypeId { get; set; }
        internal string ColumnName { get; set; }
        internal string PromptShortText { get; set; }

        internal virtual PromptType PromptTypesPromptType { get; set; }
        internal virtual ICollection<PinclinclusionAdjDatum> PinclinclusionAdjData { get; set; }
        internal virtual ICollection<PromptResponse> PromptResponses { get; set; }
    }
}
