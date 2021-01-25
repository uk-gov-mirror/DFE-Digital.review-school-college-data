using Dfe.CspdAlpha.Web.Application.Application.Helpers;
using Dfe.CspdAlpha.Web.Application.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

namespace Dfe.CspdAlpha.Web.Application.Controllers
{
    public class SchoolController : SessionController
    {
        private readonly IEstablishmentService _establishmentService;

        public SchoolController(IEstablishmentService establishmentService1, IConfiguration configuration)
        {
            _establishmentService = establishmentService1;
        }

        public IActionResult Index(string urn)
        {
            var checkingWindow = CheckingWindowHelper.GetCheckingWindow(RouteData);
            var viewModel = _establishmentService.GetSchoolViewModel(checkingWindow, urn);
            viewModel.CheckingWindow = checkingWindow;
            viewModel.DataDescription = "This is provisional data for 2018/19.";
            return View(viewModel);
        }
    }
}
