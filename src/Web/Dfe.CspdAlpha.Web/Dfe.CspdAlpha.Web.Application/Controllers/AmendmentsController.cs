using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using Dfe.Rscd.Web.ApiClient;
using Dfe.Rscd.Web.Application.Application.Interfaces;
using Dfe.Rscd.Web.Application.Models.ViewModels.Amendments;
using Dfe.Rscd.Web.Application.Models.ViewModels.Pupil;
using Dfe.Rscd.Web.Infrastructure.Models;
using Microsoft.AspNetCore.Mvc;
using ProblemDetails = Dfe.Rscd.Web.ApiClient.ProblemDetails;

namespace Dfe.Rscd.Web.Application.Controllers
{
    public class AmendmentsController : SessionController
    {
        private readonly IAmendmentService _amendmentService;

        public AmendmentsController(IAmendmentService amendmentService)
        {
            _amendmentService = amendmentService;
        }

        public IActionResult Index(string urn)
        {
            var amendments = _amendmentService.GetAmendmentsListViewModel(urn);

            return View(amendments);
        }

        public IActionResult Cancel(string id)
        {
            if (_amendmentService.CancelAmendment(id)) return RedirectToAction("Index");

            return RedirectToAction("Error", "Home");
        }

        public IActionResult Clear()
        {
            ClearAmendmentAndRelated();
            return RedirectToAction("Index", "TaskList");
        }

        [ActionName("View")]
        public IActionResult ViewAmendment(string id)
        {
            var amendment = _amendmentService.GetAmendment(id);
            if (amendment != null) return View(new AmendmentViewModel {Amendment = amendment});
            return RedirectToAction("Error", "Home");
        }

        public IActionResult Confirm()
        {
            var amendment = GetAmendment();

            if (amendment == null)
            {
                RedirectToAction("Index", "TaskList");
            }
            var viewModel = new ConfirmViewModel
            {
                AmendmentType = amendment.AmendmentType,
                PupilDetails = new PupilViewModel(amendment.Pupil)
            };

            viewModel.BackAction = "Index";
            viewModel.BackController = "Reason";

            return View(viewModel);
        }

        [HttpPost]
        public IActionResult Confirm(ConfirmViewModel viewModel)
        {
            var amendment = GetAmendment();

            // Ensure steps haven't been manually skipped
            if (amendment == null) return RedirectToAction("Index", "TaskList");

            // Cancel amendment
            if (!viewModel.ConfirmAmendment)
            {
                // Cancel amendment
                ClearAmendmentAndRelated();
                return RedirectToAction("Index", "TaskList");
            }

            try
            {
                amendment.IsUserConfirmed = true;
                var amendmentOutcome = _amendmentService.CreateAmendment(amendment);
                // Create amendment and redirect to amendment received page

                if (amendmentOutcome.IsComplete && amendmentOutcome.IsAmendmentCreated)
                {
                    ClearAmendmentAndRelated();

                    var receivedViewModel = new ReceivedViewModel
                    {
                        NewAmendmentId = amendmentOutcome.NewAmendmentId,
                        NewAmendmentRef = amendmentOutcome.NewAmendmentReferenceNumber
                    };

                    return RedirectToAction("Received", receivedViewModel);
                }

                return RedirectToAction("Prompt", "Questions");
            }
            catch (ApiException<ProblemDetails> apiException)
            {
                var properties = apiException.Result.AdditionalProperties;
                dynamic titleContent = properties.Values.Last();
                return View("CustomMessage", new CustomMessageViewModel
                {
                    Description = properties.Values.First().ToString(),
                    Title = titleContent.errorMessage.ToString(),
                    PupilDetails = new PupilViewModel(amendment.Pupil)
                });
            }
        }

        [HttpPost]
        public IActionResult CustomMessage()
        {
            return RedirectToAction("Index", "TaskList");
        }

        public IActionResult Received(ReceivedViewModel viewModel)
        {
            return View("Received", viewModel);
        }


    }
}
