using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Dfe.CspdAlpha.Web.Services.Interfaces
{
    public interface IHttpService
    {
        T ExecuteGet<T>(string resource);
        Task<dynamic> ExecuteGetAsync(string resource);

        dynamic ExecutePost(string resource, object body);
    }
}
