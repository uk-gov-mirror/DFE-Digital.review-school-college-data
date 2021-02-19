using Dfe.Rscd.Web.ApiClient;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Hosting;

namespace Dfe.Rscd.Web.Application.Controllers
{
    public class QaController : SessionController
    {
        private readonly IWebHostEnvironment _env;

        public QaController(IWebHostEnvironment env)
        {
            _env = env;
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

        public bool IsDevelopment()
        {
            return _env.IsDevelopment() || _env.EnvironmentName == "Local";
        }
    }
}
