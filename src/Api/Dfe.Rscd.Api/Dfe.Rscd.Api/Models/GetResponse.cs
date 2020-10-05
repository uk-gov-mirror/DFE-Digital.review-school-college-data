using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Dfe.Rscd.Api.Models
{
    public class GetResponse<T>
    {
        public T Result { get; set; }

        public Error Error { get; set; }
    }
}
