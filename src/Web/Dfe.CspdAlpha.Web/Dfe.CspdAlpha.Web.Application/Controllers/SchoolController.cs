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
  
        public IActionResult Index()
        {
            var urn = ClaimsHelper.GetURN(this.User);
            if (string.IsNullOrEmpty(urn))
            {
                return RedirectToAction("Index", "Home");
            }
            var viewModel = _schoolService.GetSchoolViewModel(urn);
            return View(viewModel);
        }
    }
}