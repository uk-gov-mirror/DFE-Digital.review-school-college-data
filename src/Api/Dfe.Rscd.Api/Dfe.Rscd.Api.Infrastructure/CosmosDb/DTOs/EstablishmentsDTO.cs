using Dfe.Rscd.Api.Domain.Core;
using Dfe.Rscd.Api.Domain.Entities;
using System.Collections.Generic;
using System.Linq;

namespace Dfe.Rscd.Api.Infrastructure.CosmosDb.DTOs
{
    public class EstablishmentsDTO
    {
        public string id { get; set; }
        public string DFESNumber { get; set; }
        public string SchoolName { get; set; }
        public string SchoolType { get; set; }
        public List<PerformanceDTO> performance { get; set; }

        public Establishment Establishment => new Establishment
        {
            Urn = new URN(id),
            LaEstab = DFESNumber,
            Name = SchoolName,
            SchoolType = SchoolType,
            CohortMeasures = new List<PerformanceMeasure>(),
            PerformanceMeasures = performance.Select(p => new PerformanceMeasure { Name = p.Code, Value = p.CodeValue }).ToList()
        };
    }

    public class PerformanceDTO
    {
        public string Code { get; set; }
        public string CodeValue { get; set; }
    }

}
