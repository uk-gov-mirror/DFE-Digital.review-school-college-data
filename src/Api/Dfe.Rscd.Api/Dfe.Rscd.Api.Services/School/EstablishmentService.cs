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
        private List<int> NonPlascNfTypes = new List<int>() { 29, 30, 31, 32, 33, 35, 36, 38, 42, 48, 60, 98, 99 };

        public EstablishmentService(IDocumentRepository documentRepository, IAllocationYearConfig year)
        {
            _documentRepository = documentRepository;
            _allocationYear = year.Value;
        }

        public School GetByURN(CheckingWindow checkingWindow, URN urn)
        {
            var collectionName = $"{checkingWindow.ToString().ToLower()}_establishments_{_allocationYear}";

            var establishmentDtos = _documentRepository
                .Get<EstablishmentDTO>(collectionName)
                .Where(x => x.id == urn.Value)
                .ToList();

            if (establishmentDtos.Any())
            {
                return GetEstablishment(establishmentDtos.First());
            }

            return null;
        }

        public School GetByDFESNumber(CheckingWindow checkingWindow, string dfesNumber)
        {
            var collectionName = $"{checkingWindow.ToString().ToLower()}_establishments_{_allocationYear}";

            var establishmentDtos = _documentRepository
                .Get<EstablishmentDTO>(collectionName)
                .Where(x => x.DFESNumber == dfesNumber)
                .ToList();

            if (establishmentDtos.Any())
            {
                return GetEstablishment(establishmentDtos.First());
            }

            return null;
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
        
        public bool IsNonPlascEstablishment(CheckingWindow checkingWindow, URN urn)
        {
            var collectionName = $"{checkingWindow.ToString().ToLower()}_establishments_{_allocationYear}";
            var result = _documentRepository
                .Get<EstablishmentNfTypeDTO>(collectionName)
                .Where(x=>x.id == urn.ToString())
                .ToList().First();
            var nfType = result.InstitutionTypeNumber;
            return NonPlascNfTypes.Contains(nfType.GetValueOrDefault());
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