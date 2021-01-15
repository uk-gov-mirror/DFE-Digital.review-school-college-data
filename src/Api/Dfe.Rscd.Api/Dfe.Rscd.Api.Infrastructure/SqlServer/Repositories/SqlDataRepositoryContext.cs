using System.Linq;

namespace Dfe.Rscd.Api.Infrastructure.SqlServer.DTOs
{
    public partial class SqlDataRepositoryContext
    {
        public IQueryable<T> Get<T>() where T : class
        {
            return Set<T>();
        }
    }
}
