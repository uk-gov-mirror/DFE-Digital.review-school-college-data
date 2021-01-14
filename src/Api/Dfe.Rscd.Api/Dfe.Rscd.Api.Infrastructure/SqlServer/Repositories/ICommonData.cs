using System.Collections.Generic;

namespace Dfe.Rscd.Api.Infrastructure.SqlServer.Repositories
{
    public interface ICommonData
    {
        List<T> Get<T>(string table);
        T GetById<T>(string id, string idColumn, string table);
    }
}