using System.Linq;
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

        public IQueryable<T> Get<T>(string collection)
        {
            var container = _cosmosDb.GetContainer(collection);
            return container.GetItemLinqQueryable<T>(true);
        }
    }
}