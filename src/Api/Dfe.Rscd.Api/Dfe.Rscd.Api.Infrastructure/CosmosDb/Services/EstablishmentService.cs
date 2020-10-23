using Dfe.Rscd.Api.Domain.Core;
using Dfe.Rscd.Api.Domain.Core.Enums;
using Dfe.Rscd.Api.Domain.Entities;
using Dfe.Rscd.Api.Domain.Interfaces;
using Dfe.Rscd.Api.Infrastructure.CosmosDb.Config;
using Dfe.Rscd.Api.Infrastructure.CosmosDb.DTOs;
using Dfe.Rscd.Api.Infrastructure.CosmosDb.Repositories;
using Microsoft.Azure.Cosmos;
using Microsoft.Extensions.Options;
using System.Linq;

namespace Dfe.Rscd.Api.Infrastructure.CosmosDb.Services
{
    public class EstablishmentService : IEstablishmentService
    {
        private Database _cosmosDb;

        public EstablishmentService(IOptions<CosmosDbOptions> options)
        {
            _cosmosDb = new CosmosClient(options.Value.Account, options.Value.Key).GetDatabase(options.Value.Database);
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
            var container = _cosmosDb.GetContainer(checkingWindow.ToString().ToLower() + "_establishments_2019");
            return new EstablishmentRepository(container);
        }
    }
}
