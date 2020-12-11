using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using Dfe.Rscd.Api.Domain.Interfaces;
using Dfe.Rscd.Api.Infrastructure.CosmosDb.DTOs;
using Microsoft.Azure.Cosmos;

namespace Dfe.Rscd.Api.Infrastructure.CosmosDb.Repositories
{
    public class PupilRepository : IReadRepository<PupilDTO>
    {
        public PupilRepository(Container container)
        {
            _container = container;
        }

        private Container _container { get; }

        public List<PupilDTO> Get()
        {
            throw new NotImplementedException();
        }

        public PupilDTO GetById(string id)
        {
            try
            {
                var result = _container.ReadItemAsync<PupilDTO>(id, PartitionKey.None).Result;
                if (result.StatusCode == HttpStatusCode.OK) return result.Resource;
            }
            catch
            {
            }

            return null;
        }

        public IQueryable<PupilDTO> Query()
        {
            return _container.GetItemLinqQueryable<PupilDTO>(true);
        }
    }
}