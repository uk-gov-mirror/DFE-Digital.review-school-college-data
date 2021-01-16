using System;
using System.Collections.Generic;

#nullable disable

namespace Dfe.Rscd.Api.Infrastructure.SqlServer.DTOs
{
    internal partial class PromptType
    {
        internal PromptType()
        {
            Prompts = new HashSet<Prompt>();
        }

        internal short PromptTypeId { get; set; }
        internal string PromptTypeName { get; set; }

        internal virtual ICollection<Prompt> Prompts { get; set; }
    }
}
