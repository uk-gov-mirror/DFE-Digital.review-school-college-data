using System.Linq;
using Dfe.Rscd.Api.Infrastructure.SqlServer.DTOs;

namespace Dfe.Rscd.Api.Infrastructure.SqlServer.Repositories
{
    public interface IDataRepository
    {
        IQueryable<T> Get<T>() where T : class;
    }
}