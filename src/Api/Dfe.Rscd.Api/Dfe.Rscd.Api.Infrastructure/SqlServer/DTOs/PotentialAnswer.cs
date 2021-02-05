using System;
using System.Collections.Generic;

#nullable disable

namespace Dfe.Rscd.Api.Infrastructure.SqlServer.DTOs
{
    internal partial class PotentialAnswer
    {
        internal int Id { get; set; }
        internal string QuestionId { get; set; }
        internal string AnswerValue { get; set; }
        internal bool Rejected { get; set; }
    }
}
