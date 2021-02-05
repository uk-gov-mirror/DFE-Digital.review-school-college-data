using System;
using System.Collections.Generic;

#nullable disable

namespace Dfe.Rscd.Api.Infrastructure.SqlServer.DTOs
{
    internal partial class PinclinclusionAdjustment
    {
        internal PinclinclusionAdjustment()
        {
        }

        internal string PIncl { get; set; }
        internal short IncAdjReasonId { get; set; }

        internal virtual InclusionAdjustmentReason IncAdjReason { get; set; }
        internal virtual Pincl PInclNavigation { get; set; }
    }
}
