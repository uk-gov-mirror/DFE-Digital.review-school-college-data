using System;
using System.Collections.Generic;

#nullable disable

namespace Dfe.Rscd.Api.Infrastructure.SqlServer.DTOs
{
    public partial class Pincl
    {
        public Pincl()
        {
            PinclinclusionAdjustments = new HashSet<PinclinclusionAdjustment>();
        }

        public string PIncl1 { get; set; }
        public string PIncldescription { get; set; }
        public string DisplayFlag { get; set; }

        public virtual ICollection<PinclinclusionAdjustment> PinclinclusionAdjustments { get; set; }
    }
}
