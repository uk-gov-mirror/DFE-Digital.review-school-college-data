using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Dfe.CspdAlpha.Web.Application.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Dfe.CspdAlpha.Web.Application.Controllers
{
    public class AmendmentsController : Controller
    {
        private readonly ISchoolService _schoolService;

        public AmendmentsController(ISchoolService schoolService)
        {
            _schoolService = schoolService;
        }

        public IActionResult Index(string urn)
        {

            var amendments = _schoolService.GetAmendmentsListViewModel(urn);
            return View(amendments);
        }

        public IActionResult Cancel(string id)
        {
            if (_schoolService.CancelAmendment(id))
            {
                return RedirectToAction("Index");
            }

            return RedirectToAction("Error", "Home");
        }
    }
}
