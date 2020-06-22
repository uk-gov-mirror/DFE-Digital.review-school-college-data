using Dfe.CspdAlpha.Web.Application.Application.Helpers;
using Dfe.CspdAlpha.Web.Application.Application.Interfaces;
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
            return View(viewModel);
        }

        public IActionResult ConfirmData()
        {
            return View();
        }

    }
}
