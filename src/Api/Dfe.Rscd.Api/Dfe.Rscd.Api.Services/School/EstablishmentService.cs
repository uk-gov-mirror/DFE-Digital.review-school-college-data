using System;
using System.Collections.Generic;
using System.Linq;
using Dfe.Rscd.Api.Domain.Entities;
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
            return Get(x => x.id == urn.Value, checkingWindow);
        }

        public School GetByDFESNumber(CheckingWindow checkingWindow, string dfesNumber)
        {
            return Get(x => x.DFESNumber == dfesNumber, checkingWindow);
        }

        public bool DoesSchoolExist(string dfesNumber)
        {
            var collectionName = $"{CheckingWindow.KS4June.ToString().ToLower()}_establishments_{_allocationYear}";

            var establishmentDtos = _documentRepository
                .Get<EstablishmentProxyDTO>(collectionName)
                .Where(x=>x.DFESNumber == dfesNumber)
                .ToList();

            if(establishmentDtos.Any()) return true;
            
            collectionName = $"{CheckingWindow.KS4Late.ToString().ToLower()}_establishments_{_allocationYear}";

            establishmentDtos = _documentRepository
                .Get<EstablishmentProxyDTO>(collectionName)
                .Where(x=>x.DFESNumber == dfesNumber)
                .ToList();

            if (establishmentDtos.Any()) return true;
            
            collectionName = $"{CheckingWindow.KS5.ToString().ToLower()}_establishments_{_allocationYear}";

            establishmentDtos = _documentRepository
                .Get<EstablishmentProxyDTO>(collectionName)
                .Where(x=>x.DFESNumber == dfesNumber)
                .ToList();

            return establishmentDtos.Any();
        }

        private School Get(Func<EstablishmentDTO, bool> predicate, CheckingWindow checkingWindow)
        {
            var collectionName = $"{checkingWindow.ToString().ToLower()}_establishments_{_allocationYear}";

            var establishmentDtos = _documentRepository
                .Get<EstablishmentDTO>(collectionName)
                .Where(predicate)
                .ToList();

            if (establishmentDtos.Any())
            {
                return GetEstablishment(establishmentDtos.First());
            }

            return null;
        }

        private School GetEstablishment(EstablishmentDTO dto)
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