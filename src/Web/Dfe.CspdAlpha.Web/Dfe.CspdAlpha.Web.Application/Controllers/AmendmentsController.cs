using Dfe.CspdAlpha.Web.Application.Application.Extensions;
using Dfe.CspdAlpha.Web.Application.Application.Helpers;
using Dfe.CspdAlpha.Web.Application.Application.Interfaces;
using Dfe.CspdAlpha.Web.Application.Models.Amendments;
using Dfe.CspdAlpha.Web.Application.Models.Common;
using Dfe.CspdAlpha.Web.Application.Models.ViewModels.Amendments;
using Microsoft.AspNetCore.Mvc;
using System;
using Dfe.CspdAlpha.Web.Application.Models.Amendments.AmendmentTypes;

namespace Dfe.CspdAlpha.Web.Application.Controllers
{
    public class AmendmentsController : Controller
    {
        private readonly IAmendmentService _amendmentService;
        private CheckingWindow CheckingWindow => CheckingWindowHelper.GetCheckingWindow(RouteData);

        public AmendmentsController(IAmendmentService amendmentService)
        {
            _amendmentService = amendmentService;
        }

        public IActionResult Index(string urn)
        {
            var checkingWindow = CheckingWindowHelper.GetCheckingWindow(RouteData);
            var amendments = _amendmentService.GetAmendmentsListViewModel(urn, checkingWindow);
            amendments.CheckingWindow = checkingWindow;
            return View(amendments);
        }

        public IActionResult Cancel(string id)
        {
            if (_amendmentService.CancelAmendment(CheckingWindow, id))
            {
                return RedirectToAction("Index");
            }

            return RedirectToAction("Error", "Home");
        }

        public IActionResult Clear()
        {
            HttpContext.Session.Remove(Constants.AMENDMENT_SESSION_KEY);
            return RedirectToAction("Index", "TaskList");
        }

        [ActionName("View")]
        public IActionResult ViewAmendment(string id)
        {
            var amendment = _amendmentService.GetAmendment(CheckingWindow, id);
            if (amendment != null)
            {
                return View(new AmendmentViewModel{ Amendment = amendment});
            }
            return RedirectToAction("Error", "Home");
        }

        public IActionResult Confirm()
        {
            var amendment = HttpContext.Session.Get<Amendment>(Constants.AMENDMENT_SESSION_KEY);
            return View(GetConfirmViewModel(amendment));
        }

        private ConfirmViewModel GetConfirmViewModel(Amendment amendment)
        {
            var viewModel = new ConfirmViewModel
            {
                AmendmentType = amendment.AmendmentDetail.AmendmentType,
                PupilDetails = amendment.PupilDetails
            };
            if (viewModel.AmendmentType == AmendmentType.RemovePupil)
            {
                viewModel.BackController = "RemovePupil";
                var reason = ((RemovePupil) amendment.AmendmentDetail).Reason;
                switch (reason)
                {
                    case Constants.NOT_AT_END_OF_16_TO_18_STUDY:
                    case Constants.INTERNATIONAL_STUDENT:
                    case Constants.DECEASED:
                        viewModel.BackAction = "Reason";
                        break;
                    case Constants.NOT_ON_ROLL:
                    case Constants.OTHER_EVIDENCE_NOT_REQUIRED:
                        viewModel.BackAction = "AllocationYear";
                        break;
                    default:
                        viewModel.BackAction = "Details";
                        break;
                }
            }
            else if (viewModel.AmendmentType == AmendmentType.AddPupil)
            {
                viewModel.BackController = "Evidence";
                viewModel.BackAction = amendment.EvidenceOption == EvidenceOption.UploadNow ? "Upload" : "Index";
            }

            return viewModel;
        }

        [HttpPost]
        public IActionResult Confirm(ConfirmViewModel viewModel)
        {
            // Ensure steps haven't been manually skipped
            var amendment = HttpContext.Session.Get<Amendment>(Constants.AMENDMENT_SESSION_KEY);
            if (amendment == null)
            {
                return RedirectToAction("Index", "TaskList");
            }

            // Cancel amendment
            if (!viewModel.ConfirmAmendment)
            {
                // Cancel amendment
                HttpContext.Session.Remove(Constants.AMENDMENT_SESSION_KEY);
                return RedirectToAction("Index", "TaskList");
            }

            var id = _amendmentService.CreateAmendment(amendment);
            // Create amendment and redirect to amendment received page
            if (!string.IsNullOrWhiteSpace(id))
            {
                HttpContext.Session.Remove(Constants.AMENDMENT_SESSION_KEY);
                HttpContext.Session.Set(Constants.NEW_AMENDMENT_ID, id);
                return RedirectToAction("Received");
            }

            return View(GetConfirmViewModel(amendment));
        }

        public IActionResult Received()
        {
            var amendmentId = HttpContext.Session.Get<string>(Constants.NEW_AMENDMENT_ID);
            return View("Received", amendmentId);
        }
    }
}
