using System.Collections.Generic;
using System.Linq;
using Dfe.Rscd.Api.BusinessLogic.Entities;
using Dfe.Rscd.Api.Infrastructure.CosmosDb.Config;
using Dfe.Rscd.Api.Infrastructure.CosmosDb.DTOs;
using Dfe.Rscd.Api.Infrastructure.CosmosDb.Repositories;

namespace Dfe.Rscd.Api.Services
{
    public class EstablishmentService : IEstablishmentService
    {
        private readonly IDocumentRepository _documentRepository;
        private readonly string _allocationYear;

        public EstablishmentService(IDocumentRepository documentRepository, IAllocationYearConfig year)
        {
            _documentRepository = documentRepository;
            _allocationYear = year.Value;
        }

        public School GetByURN(CheckingWindow checkingWindow, URN urn)
        {
            var establishmentDTO = _documentRepository.GetById<EstablishmentDTO>(GetCollection(checkingWindow), urn.Value);
            return GetEstablishment(establishmentDTO);
        }

        public School GetByDFESNumber(CheckingWindow checkingWindow, string dfesNumber)
        {
            var collectionUri = GetCollection(checkingWindow);

            var establishmentDtos = _documentRepository
                .Get<EstablishmentDTO>(collectionUri)
                .Where(x => x.DFESNumber == dfesNumber)
                .ToList();

            if (establishmentDtos.Any())
            {
                return GetEstablishment(establishmentDtos.First());
            }

            return null;
        }

        private string GetCollection(CheckingWindow checkingWindow)
        {
            return $"{checkingWindow.ToString().ToLower()}_establishments_{_allocationYear}";
        }

        public School GetEstablishment(EstablishmentDTO dto)
        {
            return new School
            {
                Urn = new URN(dto.id),
                DfesNumber = ConvertId(dto.DFESNumber),
                SchoolName = dto.SchoolName,
                SchoolType = dto.SchoolType,
                CohortMeasures = new List<PerformanceMeasure>(),
                PerformanceMeasures = dto.performance
                    .Select(p => new PerformanceMeasure { Name = p.Code, Value = p.CodeValue })
                    .ToList(),
                HeadTeacher = dto.HeadTitleCode + " " + dto.HeadFirstName + " " + dto.HeadLastName,
                HighestAge = dto.HighestAge,
                InstitutionTypeNumber = dto.InstitutionTypeNumber ?? 0,
                LowestAge = dto.LowestAge
            };
        }

        private int ConvertId(string id)
        {
            if (int.TryParse(id, out var idParsed))
            {
                return idParsed;
            }

            return 0;
        }
    }
}