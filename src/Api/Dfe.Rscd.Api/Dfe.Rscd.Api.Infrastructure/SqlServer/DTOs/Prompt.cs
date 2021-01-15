using System;
using System.Collections.Generic;

#nullable disable

namespace Dfe.Rscd.Api.Infrastructure.SqlServer.DTOs
{
    public partial class Prompt
    {
        public Prompt()
        {
            PinclinclusionAdjData = new HashSet<PinclinclusionAdjDatum>();
            PromptResponses = new HashSet<PromptResponse>();
        }

        public short PromptId { get; set; }
        public string PromptText { get; set; }
        public bool IsConditional { get; set; }
        public bool AllowNulls { get; set; }
        public short PromptTypesPromptTypeId { get; set; }
        public string ColumnName { get; set; }
        public string PromptShortText { get; set; }

        public virtual PromptType PromptTypesPromptType { get; set; }
        public virtual ICollection<PinclinclusionAdjDatum> PinclinclusionAdjData { get; set; }
        public virtual ICollection<PromptResponse> PromptResponses { get; set; }
    }
}
