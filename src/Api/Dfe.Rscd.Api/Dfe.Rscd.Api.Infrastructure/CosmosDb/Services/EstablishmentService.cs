using Dfe.Rscd.Api.Domain.Core;
using Dfe.Rscd.Api.Domain.Entities;
using Dfe.Rscd.Api.Domain.Interfaces;
using Dfe.Rscd.Api.Infrastructure.CosmosDb.DTOs;
using System.Linq;
using Dfe.Rscd.Api.Domain.Core.Enums;
using Dfe.Rscd.Api.Infrastructure.CosmosDb.Repositories;
using Microsoft.Azure.Cosmos;

namespace Dfe.Rscd.Api.Infrastructure.CosmosDb.Services
{
    public class EstablishmentService : IEstablishmentService
    {
        private Database _cosmosDb;

        public EstablishmentService(Database cosmosDB)
        {
            _cosmosDb = cosmosDB;
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
