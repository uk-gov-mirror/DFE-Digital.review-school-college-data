using System.Linq;
using Dfe.Rscd.Api.Infrastructure.SqlServer.DTOs;

namespace Dfe.Rscd.Api.Infrastructure.SqlServer.Repositories
{
    public class DataRepository : IDataRepository
    {
        private readonly SqlDataRepositoryContext _context;

        public DataRepository(SqlDataRepositoryContext context)
        {
            _context = context;
        }

        public IQueryable<T> Get<T>() where T : class
        {
            return _context.Get<T>();
        }
    }
}