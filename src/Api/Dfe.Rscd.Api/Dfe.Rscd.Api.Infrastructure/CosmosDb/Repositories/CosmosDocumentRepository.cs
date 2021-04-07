using System.Collections.Generic;
using System.Linq;
using Dfe.Rscd.Api.Infrastructure.CosmosDb.Config;
using Microsoft.Azure.Cosmos;

namespace Dfe.Rscd.Api.Infrastructure.CosmosDb.Repositories
{
    public class CosmosDocumentRepository : IDocumentRepository
    {
        private readonly Database _cosmosDb;
        private readonly IDictionary<string, string> _collectionLookup;

        public CosmosDocumentRepository(CosmosDbOptions options)
        {
            _cosmosDb = new CosmosClient(options.Account, options.Key).GetDatabase(options.Database);
            _collectionLookup = options.CollectionLookup;
        }

        public IQueryable<T> Get<T>(string collection)
        {
            var collectionName = _collectionLookup[collection];

            var container = _cosmosDb.GetContainer(collectionName);

            return container.GetItemLinqQueryable<T>(true);
        }
    }
}