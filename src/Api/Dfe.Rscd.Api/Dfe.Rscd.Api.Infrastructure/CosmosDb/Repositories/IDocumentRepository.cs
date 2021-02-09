using System.Linq;

namespace Dfe.Rscd.Api.Infrastructure.CosmosDb.Repositories
{
    public interface IDocumentRepository
    {
        IQueryable<T> Get<T>(string collection);
    }
}