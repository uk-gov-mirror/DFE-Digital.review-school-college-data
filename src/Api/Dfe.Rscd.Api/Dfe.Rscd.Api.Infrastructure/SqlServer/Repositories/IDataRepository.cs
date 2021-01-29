using System.Linq;
using System.Threading.Tasks;

namespace Dfe.Rscd.Api.Infrastructure.SqlServer.Repositories
{
    public interface IDataRepository
    {
        IQueryable<T> Get<T>() where T : class;
    }
}