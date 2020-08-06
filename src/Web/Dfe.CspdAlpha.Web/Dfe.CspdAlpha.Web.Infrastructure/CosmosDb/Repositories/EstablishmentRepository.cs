using Dfe.CspdAlpha.Web.Domain.Interfaces;
using Dfe.CspdAlpha.Web.Infrastructure.CosmosDb.DTOs;
using Microsoft.Azure.Cosmos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;

namespace Dfe.CspdAlpha.Web.Infrastructure.CosmosDb.Repositories
{
    public class EstablishmentRepository : IReadRepository<EstablishmentsDTO>
    {
        private Container _container { get; }

        public EstablishmentRepository(CosmosClient cosmosClient, string database, string collection)
        {
            _container = cosmosClient.GetContainer(database, collection);
        }

        public EstablishmentsDTO GetById(string id)
        {
            var result = _container.ReadItemAsync<EstablishmentsDTO>(id, PartitionKey.None).Result;
            if (result.StatusCode == HttpStatusCode.OK)
            {
                return result.Resource;
            }
            return null;
        }

        public List<EstablishmentsDTO> Get()
        {
            throw new NotImplementedException();
        }

        public IQueryable<EstablishmentsDTO> Query()
        {
            return _container.GetItemLinqQueryable<EstablishmentsDTO>(true);
        }
    }
}
