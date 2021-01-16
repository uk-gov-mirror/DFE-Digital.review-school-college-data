using System.Linq;
using System.Net;
using Dfe.Rscd.Api.Infrastructure.CosmosDb.Config;
using Microsoft.Azure.Cosmos;

namespace Dfe.Rscd.Api.Infrastructure.CosmosDb.Repositories
{
    public class CosmosDocumentRepository : IDocumentRepository
    {
        private readonly Database _cosmosDb;

        public CosmosDocumentRepository(CosmosDbOptions options)
        {
            _cosmosDb = new CosmosClient(options.Account, options.Key).GetDatabase(options.Database);
        }

        public T GetById<T>(string collection, string id)
        {
            var container = _cosmosDb.GetContainer(collection);
            var result = container.ReadItemAsync<T>(id, PartitionKey.None).Result;
            if (result.StatusCode == HttpStatusCode.OK) 
                return result.Resource;

            return default(T);
        }

        public IQueryable<T> Get<T>(string collection)
        {
            var container = _cosmosDb.GetContainer(collection);
            return container.GetItemLinqQueryable<T>(true);
        }
    }
}