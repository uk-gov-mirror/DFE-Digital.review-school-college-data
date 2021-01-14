using System.Collections.Generic;
using System.Linq;
using Dfe.Rscd.Api.Domain.Core;
using Dfe.Rscd.Api.Domain.Entities;

namespace Dfe.Rscd.Api.Infrastructure.CosmosDb.DTOs
{
    public class EstablishmentDTO
    {
        public string id { get; set; }
        public string DFESNumber { get; set; }
        public string SchoolName { get; set; }
        public string SchoolType { get; set; }
        public string HeadTitleCode { get; set; }
        public string HeadFirstName { get; set; }
        public string HeadLastName { get; set; }
        public short? HighestAge { get; set; }
        public int? InstitutionTypeNumber { get; set; }
        public int SchoolStatusCode { get; set; }
        public int? LowestAge { get; set; }
        public string WebsiteAddress { get; set; }

        public List<PerformanceDto> performance { get; set; }

        public Establishment GetEstablishment()
        {
            return new Establishment
            {
                Urn = new URN(id),
                DfesNumber = DFESNumber,
                Name = SchoolName,
                SchoolType = SchoolType,
                CohortMeasures = new List<PerformanceMeasure>(),
                PerformanceMeasures = performance
                    .Select(p => new PerformanceMeasure {Name = p.Code, Value = p.CodeValue})
                    .ToList(),
                HeadTeacher = HeadTitleCode + " " + HeadFirstName + " " + HeadLastName,
                HighestAge = HighestAge,
                InstitutionTypeNumber = InstitutionTypeNumber ?? 0,
                LowestAge = LowestAge
            };
        }

        public class PerformanceDto
        {
            public string Code { get; set; }
            public string CodeValue { get; set; }
        }
    }
}