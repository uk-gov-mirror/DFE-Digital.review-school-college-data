using System;
using System.Collections.Generic;

#nullable disable

namespace Dfe.Rscd.Api.Infrastructure.SqlServer.DTOs
{
    public partial class PromptResponse
    {
        public short PromptId { get; set; }
        public short ListOrder { get; set; }
        public string ListValue { get; set; }
        public bool Rejected { get; set; }

        public virtual Prompt Prompt { get; set; }
    }
}
