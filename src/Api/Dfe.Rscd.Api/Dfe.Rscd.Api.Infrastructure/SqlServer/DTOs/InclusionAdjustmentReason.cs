using System;
using System.Collections.Generic;

#nullable disable

namespace Dfe.Rscd.Api.Infrastructure.SqlServer.DTOs
{
    public partial class InclusionAdjustmentReason
    {
        public InclusionAdjustmentReason()
        {
            PinclinclusionAdjustments = new HashSet<PinclinclusionAdjustment>();
        }

        public short IncAdjReasonId { get; set; }
        public string IncAdjReasonDescription { get; set; }
        public bool InJuneChecking { get; set; }
        public bool CanCancel { get; set; }
        public bool IsInclusion { get; set; }
        public bool IsNewStudentReason { get; set; }
        public int? ListOrder { get; set; }

        public virtual ICollection<PinclinclusionAdjustment> PinclinclusionAdjustments { get; set; }
    }
}
