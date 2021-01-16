using System;
using System.Collections.Generic;

#nullable disable

namespace Dfe.Rscd.Api.Infrastructure.SqlServer.DTOs
{
    internal partial class AwardingBody
    {
        internal int AwardingBodyId { get; set; }
        internal string AwardingBodyNumber { get; set; }
        internal string AwardingBodyCode { get; set; }
        internal string AwardingBodyName { get; set; }
        internal string WasOther { get; set; }
        internal bool DoesGradedExams { get; set; }
    }
}
