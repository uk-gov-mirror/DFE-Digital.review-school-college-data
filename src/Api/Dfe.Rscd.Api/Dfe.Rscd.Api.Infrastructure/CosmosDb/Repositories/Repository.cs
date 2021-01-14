using System.Linq;
using System.Net;
using Dfe.Rscd.Api.Infrastructure.CosmosDb.Config;
using Microsoft.Azure.Cosmos;
using Microsoft.Extensions.Options;

namespace Dfe.Rscd.Api.Infrastructure.CosmosDb.Repositories
{
    public interface IRepository
    {
        IQueryable<T> Get<T>(string collection);
        T GetById<T>(string collection, string id);
    }

    public class Repository : IRepository
    {
        private readonly Database _cosmosDb;

        public Repository(IOptions<CosmosDbOptions> options)
        {
            _cosmosDb = new CosmosClient(options.Value.Account, options.Value.Key).GetDatabase(options.Value.Database);
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