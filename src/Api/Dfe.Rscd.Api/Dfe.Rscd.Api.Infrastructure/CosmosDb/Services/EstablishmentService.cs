using System.Linq;
using Dfe.Rscd.Api.Domain.Core;
using Dfe.Rscd.Api.Domain.Core.Enums;
using Dfe.Rscd.Api.Domain.Entities;
using Dfe.Rscd.Api.Domain.Interfaces;
using Dfe.Rscd.Api.Infrastructure.CosmosDb.Config;
using Dfe.Rscd.Api.Infrastructure.CosmosDb.DTOs;
using Dfe.Rscd.Api.Infrastructure.CosmosDb.Repositories;
using Microsoft.Azure.Cosmos;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;

namespace Dfe.Rscd.Api.Infrastructure.CosmosDb.Services
{
    public class EstablishmentService : IEstablishmentService
    {
        private readonly Database _cosmosDb;
        private readonly string ALLOCATION_YEAR;

        public EstablishmentService(IOptions<CosmosDbOptions> options, IConfiguration configuration)
        {
            _cosmosDb = new CosmosClient(options.Value.Account, options.Value.Key).GetDatabase(options.Value.Database);
            ALLOCATION_YEAR = configuration["AllocationYear"];
        }

        public Establishment GetByURN(CheckingWindow checkingWindow, URN urn)
        {
            return GetRepository(checkingWindow).GetById(urn.Value).Establishment;
        }

        public Establishment GetByLAId(CheckingWindow checkingWindow, string laId)
        {
            var results = GetRepository(checkingWindow).Query().Where(e => e.DFESNumber == laId).ToList();
            return results.Count > 0 ? results.First().Establishment : null;
        }

        private IReadRepository<EstablishmentsDTO> GetRepository(CheckingWindow checkingWindow)
        {
            var container =
                _cosmosDb.GetContainer($"{checkingWindow.ToString().ToLower()}_establishments_{ALLOCATION_YEAR}");
            return new EstablishmentRepository(container);
        }
    }
}