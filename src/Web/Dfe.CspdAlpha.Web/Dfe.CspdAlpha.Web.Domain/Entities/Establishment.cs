
using System;
using System.Collections.Generic;
using System.Text;
using Dfe.CspdAlpha.Web.Domain.Core;

namespace Dfe.CspdAlpha.Web.Domain.Entities
{
    public class Establishment
    {
        public string Name { get; set; }
        public URN Urn { get; set; }
        public string LaEstab { get; set; }
        public List<PerformanceMeasure> PerformanceMeasures { get; set; }
    }
}
