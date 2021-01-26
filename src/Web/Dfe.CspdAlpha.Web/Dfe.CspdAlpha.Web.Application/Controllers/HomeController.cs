using Dfe.CspdAlpha.Web.Application.Application.Helpers;
using Dfe.CspdAlpha.Web.Application.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Diagnostics;
using Dfe.CspdAlpha.Web.Application.Application;

namespace Dfe.CspdAlpha.Web.Application.Controllers
{
    public class HomeController : SessionController
    {
        private readonly ILogger<HomeController> _logger;
        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            SetCurrentUrn();
            
            return View(new HomeViewModel());
        }

        [HttpPost]
        public IActionResult Index(HomeViewModel homeViewModel)
        {
            if (ModelState.IsValid)
            {

                return RedirectToAction("Index", "TaskList", new { phase = homeViewModel.SelectedKeyStage.ToLower(), urn = GetCurrentUrn() });
            }

            return View(homeViewModel);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
