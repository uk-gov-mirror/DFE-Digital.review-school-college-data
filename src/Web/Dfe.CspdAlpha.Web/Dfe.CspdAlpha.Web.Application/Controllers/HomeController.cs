using System.Diagnostics;
using Dfe.CspdAlpha.Web.Application.Models.ViewModels;
using Dfe.CspdAlpha.Web.Domain.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Dfe.CspdAlpha.Web.Application.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IAmendmentService _amendmentService;

        public HomeController(ILogger<HomeController> logger,
            IAmendmentService amendmentService)
        {
            _logger = logger;
            _amendmentService = amendmentService;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Index(HomeViewModel homeViewModel)
        {
            if (ModelState.IsValid)
            {
                return RedirectToAction("Index", "Service");
            }

            return View(homeViewModel);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        public IActionResult DynamicsReadTest()
        {
            var result = _amendmentService.GetAddPupilAmendments(7654321);

            return Json(result);
        }

        public IActionResult DynamicsCreateTest()
        {
            var result = _amendmentService.CreateAddPupilAmendment(null);

            return Json(result);
        }
    }
}
