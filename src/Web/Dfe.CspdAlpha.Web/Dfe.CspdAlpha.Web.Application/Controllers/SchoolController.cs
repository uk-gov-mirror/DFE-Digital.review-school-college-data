using Dfe.CspdAlpha.Web.Application.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

namespace Dfe.CspdAlpha.Web.Application.Controllers
{
    public class SchoolController : Controller
    {
        private readonly ISchoolService _schoolService;
        private bool LateChecking { get; }

        public SchoolController(ISchoolService schoolService, IConfiguration configuration)
        {
            _schoolService = schoolService;
            LateChecking = configuration["CheckingPhase"] == "Late";
        }

        public IActionResult Index(string urn)
        {
            var viewModel = _schoolService.GetSchoolViewModel(urn);
            viewModel.LateCheckingPhase = LateChecking;
            return View(viewModel);
        }
    }
}
