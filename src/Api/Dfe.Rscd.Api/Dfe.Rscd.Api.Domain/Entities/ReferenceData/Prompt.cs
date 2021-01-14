using System;

namespace Dfe.Rscd.Api.Domain.Entities.ReferenceData
{
    public class Prompt
    {
        public Int16 PromptID { get; set; }
        public string PromptText { get; set; }
        public bool IsConditional { get; set; }
        public bool AllowNulls { get; set; }
        public int PromptTypes_PromptTypeID { get; set; }
        public string ColumnName { get; set; }
        public string PromptShortText { get; set; }
    }
}