using System;
using System.Collections.Generic;

#nullable disable

namespace Dfe.Rscd.Api.Infrastructure.SqlServer.DTOs
{
    internal partial class PromptResponse
    {
        internal short PromptId { get; set; }
        internal short ListOrder { get; set; }
        internal string ListValue { get; set; }
        internal bool Rejected { get; set; }

        internal virtual Prompt Prompt { get; set; }
    }
}
