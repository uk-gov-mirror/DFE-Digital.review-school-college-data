using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Dfe.CspdAlpha.Web.Application.Application.Helpers;
using Dfe.CspdAlpha.Web.Application.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace Dfe.CspdAlpha.Web.Application.Controllers
{
    public class ServiceController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Index(ServiceViewModel homeViewModel)
        {
            if (!ModelState.IsValid)
            {
                return View(homeViewModel);
            }

            var urn = ClaimsHelper.GetURN(this.User);

            switch (homeViewModel.SelectedService)
            {
                case ServiceOptions.CheckData:
                    return RedirectToAction("Index", "School", new {urn = urn });
                case ServiceOptions.RemovePupil:
                case ServiceOptions.ViewEarlyAccess:
                default:
                    return RedirectToAction("Index", "Service");

            }
        }

    }
}
