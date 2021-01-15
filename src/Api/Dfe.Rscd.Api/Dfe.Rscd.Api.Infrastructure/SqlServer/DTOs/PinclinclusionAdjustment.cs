using System;
using System.Collections.Generic;

#nullable disable

namespace Dfe.Rscd.Api.Infrastructure.SqlServer.DTOs
{
    public partial class PinclinclusionAdjustment
    {
        public PinclinclusionAdjustment()
        {
            PinclinclusionAdjData = new HashSet<PinclinclusionAdjDatum>();
        }

        public string PIncl { get; set; }
        public short IncAdjReasonId { get; set; }

        public virtual InclusionAdjustmentReason IncAdjReason { get; set; }
        public virtual Pincl PInclNavigation { get; set; }
        public virtual ICollection<PinclinclusionAdjDatum> PinclinclusionAdjData { get; set; }
    }
}
