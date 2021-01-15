using System;
using System.Collections.Generic;

#nullable disable

namespace Dfe.Rscd.Api.Infrastructure.SqlServer.DTOs
{
    public partial class AwardingBody
    {
        public int AwardingBodyId { get; set; }
        public string AwardingBodyNumber { get; set; }
        public string AwardingBodyCode { get; set; }
        public string AwardingBodyName { get; set; }
        public string WasOther { get; set; }
        public bool DoesGradedExams { get; set; }
    }
}
