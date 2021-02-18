using System.Collections.Generic;
using System.Linq;
using Dfe.Rscd.Web.ApiClient;
using Dfe.Rscd.Web.Application.Application.Interfaces;
using Dfe.Rscd.Web.Application.Models.ViewModels.Amendments;
using Dfe.Rscd.Web.Application.Models.ViewModels.Pupil;
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

        private bool ThisQuestionIsTheConditionalQuestion(Question thisQuestion, string promptAnswer)
        {
            if (thisQuestion.Answer.AnswerPotentials == null || thisQuestion.Answer.AnswerPotentials.Count == 0 || promptAnswer == string.Empty)
            {
                return thisQuestion.Answer.ConditionalValue == promptAnswer;
            }

            var answerText = thisQuestion.Answer.AnswerPotentials.Single(x => x.Value == promptAnswer).Description;

            return thisQuestion.Answer.ConditionalValue == promptAnswer || thisQuestion.Answer.ConditionalValue == answerText;
        }

        private Question FindQuestion(IList<Question> questions, string questionId)
        {
            var thisQuestion = questions.SingleOrDefault(x => x.Id == questionId);
            if (thisQuestion == null)
            {
                return questions.Select(x => x.Answer.ConditionalQuestion).Single(x => x != null && x.Id == questionId);
            }

            return thisQuestion;
        }

        [HttpPost]
        public IActionResult Prompt(PromptAnswerViewModel promptAnswerViewModel)
        {
            var questions = GetQuestions();
            var promptAnswer = promptAnswerViewModel.GetAnswerAsString(Request.Form);

            var thisQuestion = FindQuestion(questions, promptAnswerViewModel.QuestionId);
            
            
            SaveAnswer(new UserAnswer{ QuestionId = promptAnswerViewModel.QuestionId, Value = promptAnswer });
            var amendment = GetAmendment();

            if (thisQuestion.Answer.HasConditional && ThisQuestionIsTheConditionalQuestion(thisQuestion, promptAnswer))
            {
                var conditionalViewModel = new QuestionViewModel(questions, promptAnswerViewModel.CurrentIndex)
                {
                    PupilDetails = new PupilViewModel(amendment.Pupil),
                    ShowConditional = true
                };

                return View("Prompt", conditionalViewModel);
            }
            
            var outcome = _amendmentService.CreateAmendment(amendment);

            if (thisQuestion.QuestionType == QuestionType.NullableDate && string.IsNullOrEmpty(Request.Form[thisQuestion.Id]))
            {
                ViewData.ModelState.AddModelError(promptAnswerViewModel.QuestionId, "Select one");
                ViewData["errorMessage1"] = "Select one";
                ViewData["errorType"] = "NoneSelected";

                List<string> errorCollection = new List<string> {"Select one"};
                var validationErrors = new Dictionary<string, ICollection<string>>
                {
                    {promptAnswerViewModel.QuestionId, errorCollection}
                };

                var errorsPromptViewModel = new QuestionViewModel(questions, promptAnswerViewModel.CurrentIndex, validationErrors)
                {
                    PupilDetails = new PupilViewModel(amendment.Pupil),
                    ShowConditional = thisQuestion.Answer.IsConditional
                };

                return View("Prompt", errorsPromptViewModel);
            }

            if (thisQuestion.QuestionType == QuestionType.NullableDate &&
                Request.Form[thisQuestion.Id] == "1" &&
                string.IsNullOrEmpty(Request.Form["date-day"]) &&
                string.IsNullOrEmpty(Request.Form["date-month"]) &&
                string.IsNullOrEmpty(Request.Form["date-year"]))
            {
                // TODO: Add required field labels to API. This should not be here in Web
                ViewData.ModelState.AddModelError(promptAnswerViewModel.QuestionId, "Enter a date of arrival to UK");
                ViewData["errorMessage"] = "Enter a date of arrival to UK";

                List<string> errorCollection = new List<string> {"Enter a date of arrival to UK"};
                var validationErrors = new Dictionary<string, ICollection<string>>
                {
                    {promptAnswerViewModel.QuestionId, errorCollection}
                };

                var errorsPromptViewModel = new QuestionViewModel(questions, promptAnswerViewModel.CurrentIndex, validationErrors)
                {
                    PupilDetails = new PupilViewModel(amendment.Pupil),
                    ShowConditional = thisQuestion.Answer.IsConditional
                };

                return View("Prompt", errorsPromptViewModel);
            }

            if (outcome.ValidationErrors != null && outcome.ValidationErrors.Count > 0)
            {
                var errorsPromptViewModel = new QuestionViewModel(questions, promptAnswerViewModel.CurrentIndex, outcome.ValidationErrors)
                {
                    PupilDetails = new PupilViewModel(amendment.Pupil),
                    ShowConditional = thisQuestion.Answer.IsConditional
                };

                string actualMessage = string.Empty;
                foreach(var errorMessage in outcome.ValidationErrors)
                {
                    foreach(var promptError in errorMessage.Value)
                    {
                        ViewData.ModelState.AddModelError(errorMessage.Key, promptError);
                        actualMessage = promptError;
                    }
                }
                if (!string.IsNullOrEmpty(actualMessage))
                {
                    ViewData["errorMessage"] = actualMessage;
                }

                return View("Prompt", errorsPromptViewModel);
            }
            
            var nextQuestionViewModel = new QuestionViewModel(questions, promptAnswerViewModel.CurrentIndex + 1)
            {
                PupilDetails = new PupilViewModel(amendment.Pupil)
            };

            if (nextQuestionViewModel.HasMoreQuestions)
            {
                return View("Prompt", nextQuestionViewModel);
            }

            return RedirectToAction("Confirm");
        }

        public IActionResult Prompt(int currentIndex=0)
        {
            var amendment = GetAmendment();

            if (amendment == null) return RedirectToAction("Index", "TaskList");

            var amendmentOutcome = _amendmentService.CreateAmendment(amendment);

            amendment.EvidenceStatus = amendmentOutcome.EvidenceStatus;

            SaveAmendment(amendment);

            if (amendmentOutcome.IsComplete || amendmentOutcome.FurtherQuestions == null)
            {
                SaveAmendment(amendment);

                return RedirectToAction("Confirm");
            }

            var questions = amendmentOutcome.FurtherQuestions.ToList();
            SaveQuestions(questions);

            var promptViewModel = new QuestionViewModel(questions, currentIndex)
            {
                PupilDetails = new PupilViewModel(amendment.Pupil)
            };

            return View("Prompt", promptViewModel);
        }
    }
}
