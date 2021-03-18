using Dfe.Rscd.Web.Application.Application.Helpers;
using Dfe.Rscd.Web.Application.Application.Interfaces;
using Dfe.Rscd.Web.Application.Security;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

namespace Dfe.Rscd.Web.Application.Controllers
{
    public class SchoolController : SessionController
    {
        private readonly IEstablishmentService _establishmentService;

        public SchoolController(
            IEstablishmentService establishmentService, IConfiguration configuration, UserInfo userInfo)
            : base(userInfo)
        {
            _establishmentService = establishmentService;
        }

        public IActionResult Index(string urn)
        {
            var checkingWindow = CheckingWindowHelper.GetCheckingWindow(RouteData);
            var viewModel = _establishmentService.GetSchoolViewModel(urn);

            viewModel.DataDescription = "This is provisional data for 2018/19.";
            return View(viewModel);
        }
    }
}
