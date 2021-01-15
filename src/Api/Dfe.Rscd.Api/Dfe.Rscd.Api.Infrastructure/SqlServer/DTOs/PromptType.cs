using System;
using System.Collections.Generic;

#nullable disable

namespace Dfe.Rscd.Api.Infrastructure.SqlServer.DTOs
{
    public partial class PromptType
    {
        public PromptType()
        {
            Prompts = new HashSet<Prompt>();
        }

        public short PromptTypeId { get; set; }
        public string PromptTypeName { get; set; }

        public virtual ICollection<Prompt> Prompts { get; set; }
    }
}
