using Dfe.Rscd.Web.ApiClient;
using Dfe.Rscd.Web.Application.Application.Cache.Redis;
using Dfe.Rscd.Web.Application.Security;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Hosting;

namespace Dfe.Rscd.Web.Application.Controllers
{
    public class QaController : SessionController
    {
        private readonly IWebHostEnvironment _env;
        private readonly IRedisCache _cache;

        public QaController(IWebHostEnvironment env, UserInfo userInfo, IRedisCache cache)
            : base(userInfo)
        {
            _env = env;
            _cache = cache;
        }

        [HttpPost]
        public ActionResult SetSession([FromBody] Amendment amendment)
        {
            if (IsDevelopment() || _env.IsStaging())
            {
                SaveAmendment(amendment);
                return Json(amendment);
            }

            return Json("Not Allowed");
        }

        public ActionResult GetSession()
        {
            if (IsDevelopment() || _env.IsStaging())
            {
                return Json(GetAmendment());
            }

            return Json("Not Allowed");
        }

        public void ClearSession()
        {
            if (IsDevelopment() || _env.IsStaging())
            {
                ClearAmendmentAndRelated();
            }
        }

        public void ClearCache()
        {
            if (IsDevelopment() || _env.IsStaging())
            {
                _cache.Clear();
            }
        }

        public bool IsDevelopment()
        {
            return _env.IsDevelopment() || _env.EnvironmentName == "Local";
        }
    }
}
