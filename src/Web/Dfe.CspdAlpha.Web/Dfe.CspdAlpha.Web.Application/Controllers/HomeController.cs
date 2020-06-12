using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Dfe.CspdAlpha.Web.Application.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Query;

namespace Dfe.CspdAlpha.Web.Application.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IOrganizationService _organizationService;

        public HomeController(ILogger<HomeController> logger,
            IOrganizationService organizationService)
        {
            _logger = logger;
            _organizationService = organizationService;
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

        public IActionResult DynamicsTest()
        {
            var query = new QueryExpression("new_addpupilamendment");
            query.ColumnSet = new ColumnSet(true);

            var link = query.AddLink("new_amendmentmetadata", "cr3d5_metadata", "new_amendmentmetadataid", JoinOperator.LeftOuter);
            link.Columns = new ColumnSet(true);
            link.EntityAlias = "new_addpupilamendment_metadata";

            var entities = _organizationService.RetrieveMultiple(query);

            return Json(entities);
        }
    }
}
