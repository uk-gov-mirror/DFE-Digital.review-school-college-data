using System;
using Dfe.CspdAlpha.Web.Application.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

namespace Dfe.CspdAlpha.Web.Application.Controllers
{
    public class AmendmentsController : Controller
    {
        private readonly IAmendmentService _amendmentService;
        private bool LateChecking { get; }

        public AmendmentsController(IAmendmentService amendmentService, IConfiguration configuration)
        {
            _amendmentService = amendmentService;
            LateChecking = configuration["CheckingPhase"] == "Late";
        }

        public IActionResult Index(string urn)
        {

            var amendments = _amendmentService.GetAmendmentsListViewModel(urn, LateChecking);
            amendments.LateCheckingPhase = LateChecking;
            return View(amendments);
        }

        public IActionResult Cancel(string id)
        {
            if (_amendmentService.CancelAmendment(id))
            {
                return RedirectToAction("Index");
            }

            return RedirectToAction("Error", "Home");
        }

        [ActionName("View")]
        public IActionResult ViewAmendment(string id)
        {
            var amendment = _amendmentService.GetAddPupilAmendmentViewModel(new Guid(id));
            if (amendment != null)
            {
                return View(amendment);
            }
            return RedirectToAction("Error", "Home");
        }
    }
}
