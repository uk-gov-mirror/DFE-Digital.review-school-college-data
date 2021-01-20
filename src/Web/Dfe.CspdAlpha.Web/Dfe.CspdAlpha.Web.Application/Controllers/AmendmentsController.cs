using System.Linq;
using Dfe.CspdAlpha.Web.Application.Application;
using Dfe.CspdAlpha.Web.Application.Application.Helpers;
using Dfe.CspdAlpha.Web.Application.Application.Interfaces;
using Dfe.CspdAlpha.Web.Application.Models.Common;
using Dfe.CspdAlpha.Web.Application.Models.ViewModels.Amendments;
using Dfe.CspdAlpha.Web.Application.Models.ViewModels.Pupil;
using Microsoft.AspNetCore.Mvc;
using Dfe.Rscd.Web.ApiClient;
using ProblemDetails = Dfe.Rscd.Web.ApiClient.ProblemDetails;

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
                AmendmentType = amendment.AmendmentType,
                PupilDetails = new PupilViewModel(amendment.Pupil, CheckingWindow)
            };

            viewModel.BackAction = "Index";
            viewModel.BackController = "Reason";
            
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

            try
            {
                amendment.IsUserConfirmed = true;
                var amendmentOutcome = _amendmentService.CreateAmendment(amendment);
                // Create amendment and redirect to amendment received page
                
                if (amendmentOutcome.IsComplete && amendmentOutcome.IsAmendmentCreated)
                {
                    HttpContext.Session.Remove(Constants.AMENDMENT_SESSION_KEY);
                    HttpContext.Session.Set(Constants.NEW_AMENDMENT_ID, amendmentOutcome.NewAmendmentId);
                    HttpContext.Session.Set(Constants.NEW_REFERENCE_ID, amendmentOutcome.NewAmendmentReferenceNumber);

                    return RedirectToAction("Received");
                }

                return RedirectToAction("Prompt");
            }
            catch (ApiException<ProblemDetails> apiException)
            {
                var properties = apiException.Result.AdditionalProperties;
                dynamic titleContent = properties.Values.Last();
                return View("CustomMessage", new CustomMessageViewModel{Description = properties.Values.First().ToString(),
                    Title=titleContent.errorMessage.ToString(), PupilDetails = new PupilViewModel(amendment.Pupil, CheckingWindow)});
            }
            
        }

        [HttpPost]
        public IActionResult CustomMessage()
        {
            return RedirectToAction("Index", "TaskList");
        }

        public IActionResult Received()
        {
            var reference = HttpContext.Session.Get<string>(Constants.NEW_REFERENCE_ID);
            return View("Received", reference);
        }

        public IActionResult Prompt()
        {
            // Ensure steps haven't been manually skipped
            var amendment = HttpContext.Session.Get<Amendment>(Constants.AMENDMENT_SESSION_KEY);
            if (amendment == null)
            {
                return RedirectToAction("Index", "TaskList");
            }

            var amendmentOutcome = _amendmentService.CreateAmendment(amendment);

            amendment.IsNewAmendment = false;
            amendment.EvidenceStatus = amendmentOutcome.EvidenceStatus;
            HttpContext.Session.Set(Constants.AMENDMENT_SESSION_KEY, amendment);

            if (amendmentOutcome.IsComplete && amendmentOutcome.FurtherPrompts == null)
            {
                return RedirectToAction("Confirm");
            }

            return View("Prompt", new PromptViewModel(amendmentOutcome){AmendmentType = amendment.AmendmentType,
                PupilDetails = new PupilViewModel(amendment.Pupil, CheckingWindow)});
        }
    }
}
