using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Dfe.CspdAlpha.Web.Application.Models;
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

            switch (homeViewModel.SelectedService)
            {
                case ServiceOptions.CheckData:
                    return RedirectToAction("Index", "School");
                case ServiceOptions.RemovePupil:
                case ServiceOptions.ViewEarlyAccess:
                default:
                    return RedirectToAction("Index", "Service");

            }
        }

    }
}