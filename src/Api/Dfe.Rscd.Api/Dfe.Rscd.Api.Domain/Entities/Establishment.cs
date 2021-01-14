using System.Collections.Generic;
using Dfe.Rscd.Api.Domain.Core;

namespace Dfe.Rscd.Api.Domain.Entities
{
    public class Establishment
    {
        public string Name { get; set; }
        public URN Urn { get; set; }
        public string DfesNumber { get; set; }
        public string SchoolType { get; set; }
        public List<PerformanceMeasure> CohortMeasures { get; set; }
        public List<PerformanceMeasure> PerformanceMeasures { get; set; }

        public int InstitutionTypeNumber { get; set; }

        public string HeadTeacher { get; set; }
        public int? HighestAge { get; set; }

        public int? LowestAge { get; set; }
    }
}