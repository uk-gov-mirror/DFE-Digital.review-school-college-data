using System;
using System.Collections.Generic;

#nullable disable

namespace Dfe.Rscd.Api.Infrastructure.SqlServer.DTOs
{
    internal partial class InclusionAdjustmentReason
    {
        internal InclusionAdjustmentReason()
        {
            PinclinclusionAdjustments = new HashSet<PinclinclusionAdjustment>();
        }

        internal short IncAdjReasonId { get; set; }
        internal string IncAdjReasonDescription { get; set; }
        internal bool InJuneChecking { get; set; }
        internal bool CanCancel { get; set; }
        internal bool IsInclusion { get; set; }
        internal bool IsNewStudentReason { get; set; }
        internal int? ListOrder { get; set; }

        internal virtual ICollection<PinclinclusionAdjustment> PinclinclusionAdjustments { get; set; }
    }
}
