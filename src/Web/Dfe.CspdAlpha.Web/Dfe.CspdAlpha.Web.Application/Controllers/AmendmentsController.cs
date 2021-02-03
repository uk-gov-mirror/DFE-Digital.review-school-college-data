using System.Linq;
using Dfe.CspdAlpha.Web.Application.Application.Helpers;
using Dfe.CspdAlpha.Web.Application.Application.Interfaces;
using Dfe.CspdAlpha.Web.Application.Models.ViewModels.Amendments;
using Dfe.CspdAlpha.Web.Application.Models.ViewModels.Pupil;
using Dfe.Rscd.Web.ApiClient;
using Microsoft.AspNetCore.Mvc;
using ProblemDetails = Dfe.Rscd.Web.ApiClient.ProblemDetails;

namespace Dfe.CspdAlpha.Web.Application.Controllers
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
            var checkingWindow = CheckingWindowHelper.GetCheckingWindow(RouteData);
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
            return View(GetConfirmViewModel(amendment));
        }

        private ConfirmViewModel GetConfirmViewModel(Amendment amendment)
        {
            var viewModel = new ConfirmViewModel
            {
                AmendmentType = amendment.AmendmentType,
                PupilDetails = new PupilViewModel(amendment.Pupil)
            };

            viewModel.BackAction = "Index";
            viewModel.BackController = "Reason";

            return viewModel;
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

                return RedirectToAction("Prompt");
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

        [HttpPost]
        public IActionResult Prompt(PromptAnswerViewModel promptAnswerViewModel)
        {
            // Validate Asnwer

            var promptAnswer = promptAnswerViewModel.GetAnswerAsString(Request.Form);

            var amendment = GetAmendment();

            var currentQuestion = amendment.Questions.Single(x => x.Id == promptAnswerViewModel.QuestionId);
            currentQuestion.Answers.First().Value = promptAnswer;

            SaveAmendment(amendment);
            
            var promptViewModel = new QuestionViewModel(amendment.Questions, promptAnswerViewModel.CurrentIndex + 1)
            {
                PupilDetails = new PupilViewModel(amendment.Pupil)
            };

            if (promptViewModel.HasMoreQuestions) return View("Prompt", promptViewModel);

            return RedirectToAction("Confirm");
        }

        public IActionResult Prompt()
        {
            var amendment = GetAmendment();

            if (amendment == null) return RedirectToAction("Index", "TaskList");

            var amendmentOutcome = _amendmentService.CreateAmendment(amendment);

            amendment.IsNewAmendment = false;
            amendment.EvidenceStatus = amendmentOutcome.EvidenceStatus;

            amendment.Questions = amendmentOutcome.FurtherQuestions;

            SaveAmendment(amendment);

            if (amendmentOutcome.IsComplete || amendmentOutcome.FurtherQuestions == null)
                return RedirectToAction("Confirm");

            var promptViewModel = new QuestionViewModel(amendmentOutcome.FurtherQuestions.ToList())
            {
                PupilDetails = new PupilViewModel(amendment.Pupil)
            };

            return View("Prompt", promptViewModel);
        }
    }
}
