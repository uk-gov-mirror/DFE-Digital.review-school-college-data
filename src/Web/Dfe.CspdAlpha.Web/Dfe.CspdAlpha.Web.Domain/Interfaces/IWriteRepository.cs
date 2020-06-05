using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;

namespace Dfe.CspdAlpha.Web.Domain.Interfaces
{
    public interface IWriteRepository<T>
    {
        void Add(T value);
        void Update(T value);
        void Delete(string id);
    }
}
