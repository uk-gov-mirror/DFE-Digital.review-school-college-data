using Dfe.Rscd.Api.Domain.Core;
using System.Collections.Generic;

namespace Dfe.Rscd.Api.Domain.Entities
{
    public class Establishment
    {
        public string Name { get; set; }
        public URN Urn { get; set; }
        public string LaEstab { get; set; }
        public string SchoolType { get; set; }
        public List<PerformanceMeasure> CohortMeasures { get; set; }
        public List<PerformanceMeasure> PerformanceMeasures { get; set; }
    }
}
