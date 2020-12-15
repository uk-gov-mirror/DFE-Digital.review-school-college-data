using System.Collections.Generic;
using System.Linq;

namespace Dfe.Rscd.Api.Domain.Interfaces
{
    public interface IReadRepository<T>
    {
        T GetById(string id);
        List<T> Get();
        IQueryable<T> Query();
    }
}