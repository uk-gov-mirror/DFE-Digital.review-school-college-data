using System;
using System.Collections.Generic;
using System.Text;

namespace Dfe.CspdAlpha.Web.Domain.Interfaces
{
    public interface IReadRepository<T>
    {
        T GetById(string id);
        List<T> Get();
    }
}
