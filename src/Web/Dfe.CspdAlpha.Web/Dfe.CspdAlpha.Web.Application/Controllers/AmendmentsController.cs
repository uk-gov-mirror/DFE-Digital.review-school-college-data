using System.Collections.Generic;
using System.Linq;
using Dfe.Rscd.Web.ApiClient;
using Dfe.Rscd.Web.Application.Application.Interfaces;
using Dfe.Rscd.Web.Application.Models.ViewModels.Amendments;
using Dfe.Rscd.Web.Application.Models.ViewModels.Pupil;
using Dfe.Rscd.Web.Infrastructure.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Office.SharePoint.Tools;
using ProblemDetails = Dfe.Rscd.Web.ApiClient.ProblemDetails;

namespace Dfe.Rscd.Web.Application.Controllers
{
    public class AmendmentsController : SessionController
    {
        private readonly IAmendmentService _amendmentService;
        private readonly IEvidenceService _evidenceService;

        public AmendmentsController(IAmendmentService amendmentService, IEvidenceService evidenceService)
        {
            _amendmentService = amendmentService;
            _evidenceService = evidenceService;
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
        public IActionResult Prompt(PromptAnswerViewModel promptAnswerViewModel, bool Continue)
        {
            var questions = GetQuestions();
            var thisQuestion = FindQuestion(questions, promptAnswerViewModel.QuestionId);
            var promptAnswer = promptAnswerViewModel.GetAnswerAsString(Request.Form);
            var amendment = GetAmendment();

            if (thisQuestion.QuestionType == QuestionType.Evidence)
            {
                if (Request.Form.Files.Count > 0)
                {
                    promptAnswer = UploadEvidence(amendment);
                }
                else
                {
                    var errorUploadViewModel = CreateErrorUploadViewModel(promptAnswerViewModel, thisQuestion,
                        questions, amendment);

                    return View("Prompt", errorUploadViewModel);
                }

                if (!Continue)
                {
                    var uploadEvidenceViewModel = CreateUploadMoreViewModel(promptAnswerViewModel, questions,
                        amendment);

                    return View("Prompt", uploadEvidenceViewModel);
                }
            }

            SaveAnswer(new UserAnswer{ QuestionId = promptAnswerViewModel.QuestionId, Value = promptAnswer });
            amendment = GetAmendment();

            if (ConditionalQuestion(thisQuestion, promptAnswer))
            {
                var conditionalViewModel = CreateConditionalPrompt(promptAnswerViewModel, questions, amendment);

                return View("Prompt", conditionalViewModel);
            }

            var outcome = _amendmentService.CreateAmendment(amendment);

            if (NotNullableDateIsSelected(thisQuestion))
            {
                var errorsPromptViewModel = CreateNullableDateErrorPrompt(promptAnswerViewModel,
                    thisQuestion, questions, amendment);

                return View("Prompt", errorsPromptViewModel);
            }

            if (NullableDateQuestionHasErrors(thisQuestion))
            {
                var errorsPromptViewModel = CreateNullableInvalidDateErrorPrompt(promptAnswerViewModel,
                    thisQuestion, questions, amendment);

                return View("Prompt", errorsPromptViewModel);
            }

            if (GenericQuestionHasErrors(outcome))
            {
                var errorsPromptViewModel = CreateGenericQuestionErrorPrompt(promptAnswerViewModel, questions,
                    outcome, amendment, thisQuestion);

                return View("Prompt", errorsPromptViewModel);
            }

            var nextQuestionViewModel = GoToTheNextQuestion(promptAnswerViewModel, questions, amendment);

            if (nextQuestionViewModel.HasMoreQuestions)
            {
                return View("Prompt", nextQuestionViewModel);
            }

            return RedirectToAction("Confirm");
        }

        private QuestionViewModel CreateUploadMoreViewModel(PromptAnswerViewModel promptAnswerViewModel, List<Question> questions,
            Amendment amendment)
        {
            var uploadEvidenceViewModel = new QuestionViewModel(questions, promptAnswerViewModel.CurrentIndex)
            {
                PupilDetails = new PupilViewModel(amendment.Pupil),
                ShowConditional = true
            };

            ViewBag.Upload = GetFiles();
            return uploadEvidenceViewModel;
        }

        private QuestionViewModel CreateErrorUploadViewModel(PromptAnswerViewModel promptAnswerViewModel, Question thisQuestion,
            List<Question> questions, Amendment amendment)
        {
            ViewData.ModelState.AddModelError(promptAnswerViewModel.QuestionId, thisQuestion.Validator.NullErrorMessage);
            ViewData["errorMessage"] = thisQuestion.Validator.NullErrorMessage;

            var errorUploadViewModel = new QuestionViewModel(questions, promptAnswerViewModel.CurrentIndex)
            {
                PupilDetails = new PupilViewModel(amendment.Pupil),
                ShowConditional = true
            };

            ViewBag.Upload = GetFiles();
            return errorUploadViewModel;
        }

        private string UploadEvidence(Amendment amendment)
        {
            string promptAnswer;
            if (string.IsNullOrEmpty(amendment.EvidenceFolderName))
            {
                var fileUploadResult = _evidenceService.UploadEvidence(Request.Form.Files.First());
                promptAnswer = amendment.EvidenceFolderName = fileUploadResult.FolderName;
                AddFile(fileUploadResult);
                SaveAmendment(amendment);
            }
            else
            {
                var fileUploadResult =
                    _evidenceService.UploadEvidence(amendment.EvidenceFolderName, Request.Form.Files.First());
                promptAnswer = fileUploadResult.FolderName;
                AddFile(fileUploadResult);
            }

            return promptAnswer;
        }

        private static QuestionViewModel GoToTheNextQuestion(PromptAnswerViewModel promptAnswerViewModel, List<Question> questions,
            Amendment amendment)
        {
            var nextQuestionViewModel = new QuestionViewModel(questions, promptAnswerViewModel.CurrentIndex + 1)
            {
                PupilDetails = new PupilViewModel(amendment.Pupil)
            };
            return nextQuestionViewModel;
        }

        private bool ConditionalQuestion(Question thisQuestion, string promptAnswer)
        {
            return thisQuestion.Answer.HasConditional && ThisQuestionIsTheConditionalQuestion(thisQuestion, promptAnswer);
        }

        private bool NotNullableDateIsSelected(Question thisQuestion)
        {
            return thisQuestion.QuestionType == QuestionType.NullableDate && string.IsNullOrEmpty(Request.Form[thisQuestion.Id]);
        }

        private bool NullableDateQuestionHasErrors(Question thisQuestion)
        {
            return thisQuestion.QuestionType == QuestionType.NullableDate &&
                   Request.Form[thisQuestion.Id] == "1" &&
                   string.IsNullOrEmpty(Request.Form["date-day"]) &&
                   string.IsNullOrEmpty(Request.Form["date-month"]) &&
                   string.IsNullOrEmpty(Request.Form["date-year"]);
        }

        private static bool GenericQuestionHasErrors(AmendmentOutcome outcome)
        {
            return outcome.ValidationErrors != null && outcome.ValidationErrors.Count > 0;
        }

        private static QuestionViewModel CreateConditionalPrompt(PromptAnswerViewModel promptAnswerViewModel, List<Question> questions,
            Amendment amendment)
        {
            var conditionalViewModel = new QuestionViewModel(questions, promptAnswerViewModel.CurrentIndex)
            {
                PupilDetails = new PupilViewModel(amendment.Pupil),
                ShowConditional = true
            };
            return conditionalViewModel;
        }

        private QuestionViewModel CreateGenericQuestionErrorPrompt(PromptAnswerViewModel promptAnswerViewModel, List<Question> questions,
            AmendmentOutcome outcome, Amendment amendment, Question thisQuestion)
        {
            var errorsPromptViewModel =
                new QuestionViewModel(questions, promptAnswerViewModel.CurrentIndex, outcome.ValidationErrors)
                {
                    PupilDetails = new PupilViewModel(amendment.Pupil),
                    ShowConditional = thisQuestion.Answer.IsConditional
                };

            string actualMessage = string.Empty;
            foreach (var errorMessage in outcome.ValidationErrors)
            {
                foreach (var promptError in errorMessage.Value)
                {
                    ViewData.ModelState.AddModelError(errorMessage.Key, promptError);
                    actualMessage = promptError;
                }
            }

            if (!string.IsNullOrEmpty(actualMessage))
            {
                ViewData["errorMessage"] = actualMessage;
            }

            return errorsPromptViewModel;
        }

        private QuestionViewModel CreateNullableInvalidDateErrorPrompt(PromptAnswerViewModel promptAnswerViewModel,
            Question thisQuestion, List<Question> questions, Amendment amendment)
        {
            ViewData.ModelState.AddModelError(promptAnswerViewModel.QuestionId, thisQuestion.Validator.NullErrorMessage);
            ViewData["errorMessage"] = thisQuestion.Validator.NullErrorMessage;

            List<string> errorCollection = new List<string> {thisQuestion.Validator.NullErrorMessage};
            var validationErrors = new Dictionary<string, ICollection<string>>
            {
                {promptAnswerViewModel.QuestionId, errorCollection}
            };

            var errorsPromptViewModel = new QuestionViewModel(questions, promptAnswerViewModel.CurrentIndex, validationErrors)
            {
                PupilDetails = new PupilViewModel(amendment.Pupil),
                ShowConditional = thisQuestion.Answer.IsConditional
            };
            return errorsPromptViewModel;
        }

        private QuestionViewModel CreateNullableDateErrorPrompt(PromptAnswerViewModel promptAnswerViewModel,
            Question thisQuestion, List<Question> questions, Amendment amendment)
        {
            ViewData.ModelState.AddModelError(promptAnswerViewModel.QuestionId, thisQuestion.Answer.Label);
            ViewData["errorMessage1"] = thisQuestion.Answer.Label;
            ViewData["errorType"] = "NoneSelected";

            List<string> errorCollection = new List<string> {thisQuestion.Answer.Label};
            var validationErrors = new Dictionary<string, ICollection<string>>
            {
                {promptAnswerViewModel.QuestionId, errorCollection}
            };

            var errorsPromptViewModel = new QuestionViewModel(questions, promptAnswerViewModel.CurrentIndex, validationErrors)
            {
                PupilDetails = new PupilViewModel(amendment.Pupil),
                ShowConditional = thisQuestion.Answer.IsConditional
            };
            return errorsPromptViewModel;
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
