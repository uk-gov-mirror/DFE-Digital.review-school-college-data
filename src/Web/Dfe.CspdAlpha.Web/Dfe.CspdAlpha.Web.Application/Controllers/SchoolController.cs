using Dfe.CspdAlpha.Web.Application.Application.Extensions;
using Dfe.CspdAlpha.Web.Application.Application.Interfaces;
using Dfe.CspdAlpha.Web.Application.Models.Common;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Dfe.CspdAlpha.Web.Application.Controllers
{
    public class SchoolController : Controller
    {
        private readonly ISchoolService _schoolService;

        public SchoolController(ISchoolService schoolService)
        {
            _schoolService = schoolService;
        }

        public IActionResult Index(string urn)
        {
            var viewModel = _schoolService.GetSchoolViewModel(urn);
            viewModel.ConfirmDataBanner = HttpContext.Session.Get<ConfirmDataBanner>("data-confirmed") ?? new ConfirmDataBanner();
            
            return View(viewModel);
        }

        public IActionResult ConfirmData()
        {
            return View();
        }
        [HttpPost]
        public IActionResult DataConfirmed()
        {
            var banner = new ConfirmDataBanner{DataConfirmed = true};
            HttpContext.Session.Set("data-confirmed", banner);
            return View();
        }

    }
}
