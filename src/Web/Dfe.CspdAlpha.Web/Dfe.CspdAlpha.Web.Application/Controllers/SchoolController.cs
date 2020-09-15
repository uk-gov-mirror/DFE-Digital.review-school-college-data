using Dfe.CspdAlpha.Web.Application.Application.Helpers;
using Dfe.CspdAlpha.Web.Application.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

namespace Dfe.CspdAlpha.Web.Application.Controllers
{
    public class SchoolController : Controller
    {
        private readonly ISchoolService _schoolService;

        public SchoolController(ISchoolService schoolService, IConfiguration configuration)
        {
            _schoolService = schoolService;
        }

        public IActionResult Index(string urn)
        {
            var viewModel = _schoolService.GetSchoolViewModel(urn);
            viewModel.CheckingWindow = CheckingWindowHelper.GetCheckingWindow(RouteData);
            viewModel.DataDescription = "This is provisional data for 2018/19.";
            return View(viewModel);
        }
    }
}
