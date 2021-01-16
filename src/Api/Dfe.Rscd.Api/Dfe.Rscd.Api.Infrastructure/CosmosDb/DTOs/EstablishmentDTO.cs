using System.Collections.Generic;
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

        public class PerformanceDto
        {
            public string Code { get; set; }
            public string CodeValue { get; set; }
        }
    }
}