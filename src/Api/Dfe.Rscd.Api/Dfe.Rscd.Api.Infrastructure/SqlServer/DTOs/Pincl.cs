using System;
using System.Collections.Generic;

#nullable disable

namespace Dfe.Rscd.Api.Infrastructure.SqlServer.DTOs
{
    internal partial class Pincl
    {
        internal Pincl()
        {
            PinclinclusionAdjustments = new HashSet<PinclinclusionAdjustment>();
        }

        internal string PIncl1 { get; set; }
        internal string PIncldescription { get; set; }
        internal string DisplayFlag { get; set; }

        internal virtual ICollection<PinclinclusionAdjustment> PinclinclusionAdjustments { get; set; }
    }
}
