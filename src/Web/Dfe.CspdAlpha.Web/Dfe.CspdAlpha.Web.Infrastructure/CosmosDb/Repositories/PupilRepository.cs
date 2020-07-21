using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using Dfe.CspdAlpha.Web.Domain.Interfaces;
using Microsoft.Azure.Cosmos;

namespace Dfe.CspdAlpha.Web.Infrastructure.CosmosDb.Repositories
{
    public class PupilRepository : IReadRepository<PupilDTO>
    {
        private Container _container { get; }
        public PupilRepository(CosmosClient cosmosClient, string database, string collection)
        {
            _container = cosmosClient.GetContainer(database, collection);
        }

        public List<PupilDTO> Get()
        {
            throw new NotImplementedException();
        }

        public PupilDTO GetById(string id)
        {
            var result = _container.ReadItemAsync<PupilDTO>(id, PartitionKey.None).Result;
            if (result.StatusCode == HttpStatusCode.OK)
            {
                return result.Resource;
            }

            return null;
        }

        public IQueryable<PupilDTO> Query()
        {
            return _container.GetItemLinqQueryable<PupilDTO>(true);
        }
    }
}
