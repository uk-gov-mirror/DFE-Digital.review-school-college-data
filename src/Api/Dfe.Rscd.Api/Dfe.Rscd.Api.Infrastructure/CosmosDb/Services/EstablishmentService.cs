using System.Linq;
using Dfe.Rscd.Api.Domain.Core;
using Dfe.Rscd.Api.Domain.Core.Enums;
using Dfe.Rscd.Api.Domain.Entities;
using Dfe.Rscd.Api.Domain.Interfaces;
using Dfe.Rscd.Api.Infrastructure.CosmosDb.DTOs;
using Dfe.Rscd.Api.Infrastructure.CosmosDb.Repositories;
using Microsoft.Extensions.Configuration;

namespace Dfe.Rscd.Api.Infrastructure.CosmosDb.Services
{
    public class EstablishmentService : IEstablishmentService
    {
        private readonly IRepository _repository;
        private readonly string _allocationYear;

        public EstablishmentService(IRepository repository, IConfiguration configuration)
        {
            _repository = repository;
            _allocationYear = configuration["AllocationYear"];
        }

        public Establishment GetByURN(CheckingWindow checkingWindow, URN urn)
        {
            var establishmentDTO = _repository.GetById<EstablishmentDTO>(GetCollection(checkingWindow), urn.Value);
            return establishmentDTO.GetEstablishment();
        }

        public Establishment GetByDFESNumber(CheckingWindow checkingWindow, string dfesNumber)
        {
            var collectionUri = GetCollection(checkingWindow);

            var establishmentDtos = _repository
                .Get<EstablishmentDTO>(collectionUri)
                .Where(x => x.DFESNumber == dfesNumber)
                .ToList();

            if (establishmentDtos.Any())
            {
                return establishmentDtos.First().GetEstablishment();
            }

            return null;
        }

        private string GetCollection(CheckingWindow checkingWindow)
        {
            return $"{checkingWindow.ToString().ToLower()}_establishments_{_allocationYear}";
        }
    }
}