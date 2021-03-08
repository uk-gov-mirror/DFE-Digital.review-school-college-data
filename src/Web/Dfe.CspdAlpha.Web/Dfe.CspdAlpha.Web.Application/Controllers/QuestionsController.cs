using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using Dfe.Rscd.Web.ApiClient;
using Dfe.Rscd.Web.Application.Application.Interfaces;
using Dfe.Rscd.Web.Application.Models.ViewModels.Amendments;
using Dfe.Rscd.Web.Application.Models.ViewModels.Pupil;
using Dfe.Rscd.Web.Infrastructure.Models;
using Microsoft.AspNetCore.Mvc;

namespace Dfe.Rscd.Web.Application.Controllers
{
    public class QuestionsController : SessionController
    {
        private readonly IAmendmentService _amendmentService;
        private readonly IEvidenceService _evidenceService;
        private long ThreeMegabytes => 3000000;

        public QuestionsController(IAmendmentService amendmentService, IEvidenceService evidenceService)
        {
            _amendmentService = amendmentService;
            _evidenceService = evidenceService;
        }

        public IActionResult Prompt(int currentIndex=0)
        {
            var amendment = GetAmendment();

            if (amendment == null) return RedirectToAction("Index", "TaskList");

            var amendmentOutcome = _amendmentService.CreateAmendment(amendment);

            SaveAmendment(amendment);

            if (amendmentOutcome.IsComplete || amendmentOutcome.FurtherQuestions == null)
            {
                SaveAmendment(amendment);

                return RedirectToAction("Confirm", "Amendments");
            }

            var questions = amendmentOutcome.FurtherQuestions.ToList();
            SaveQuestions(questions);

            var promptViewModel = new QuestionViewModel(questions, currentIndex)
            {
                PupilDetails = new PupilViewModel(amendment.Pupil)
            };

            return View("Prompt", promptViewModel);
        }

        [HttpPost]
        public IActionResult Prompt(PromptAnswerViewModel promptAnswerViewModel, bool Continue)
        {
            var questions = GetQuestions();
            var thisQuestion = FindQuestion(questions, promptAnswerViewModel.QuestionId);

            var promptAnswer = promptAnswerViewModel.GetAnswerAsString(Request.Form);
            SaveAnswer(new UserAnswer{ QuestionId = promptAnswerViewModel.QuestionId, Value = promptAnswer });

            var amendment = GetAmendment();

            if (thisQuestion.QuestionType == QuestionType.Evidence)
            {
                if (Request.Form.Files.Count > 0)
                {
                    var result = UploadEvidence(amendment);

                    if (result != null)
                    {
                        var errorUploadViewModel = CreateValidationErrorUploadModel(promptAnswerViewModel, result,
                            questions, amendment);

                        return View("Prompt", errorUploadViewModel);
                    }
                }
                else if (!Continue && Request.Form.ContainsKey("remove"))
                {
                    var fileId = Request.Form["remove"];
                    var file = GetFiles().Files.Single(x => x.Id == fileId);
                    _evidenceService.DeleteEvidenceFile(Guid.Parse(file.Id));

                    RemoveFile(fileId);
                    var uploadEvidenceViewModel = CreateUploadMoreViewModel(promptAnswerViewModel, questions,
                        amendment);

                    ViewBag.HasAdded = !Request.Form.ContainsKey("skipValidation");
                    return View("Prompt", uploadEvidenceViewModel);
                }
                else if(!Continue && !Request.Form.ContainsKey("skipValidation"))
                {
                    var errorUploadViewModel = CreateErrorUploadViewModel(promptAnswerViewModel, thisQuestion,
                        questions, amendment);

                    return View("Prompt", errorUploadViewModel);
                }

                if (!Continue)
                {
                    var uploadEvidenceViewModel = CreateUploadMoreViewModel(promptAnswerViewModel, questions,
                        amendment);

                    ViewBag.HasAdded = !Request.Form.ContainsKey("skipValidation");
                    return View("Prompt", uploadEvidenceViewModel);
                }

                SaveAnswer(new UserAnswer{ QuestionId = promptAnswerViewModel.QuestionId, Value = amendment.EvidenceFolderName });
            }

            amendment = GetAmendment();

            if (ConditionalQuestion(thisQuestion, promptAnswer))
            {
                var conditionalViewModel = CreateConditionalPrompt(promptAnswerViewModel, questions, amendment);

                return View("Prompt", conditionalViewModel);
            }

            var outcome = _amendmentService.CreateAmendment(amendment);

            if (outcome.FurtherQuestions != null)
            {
                SaveQuestions(outcome.FurtherQuestions);
                questions = GetQuestions();
            }

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

            return RedirectToAction("Confirm", "Amendments");
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
            ViewBag.HasAdded = !Request.Form.ContainsKey("skipValidation");
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

        internal QuestionViewModel CreateValidationErrorUploadModel(PromptAnswerViewModel promptAnswerViewModel, FileValidationError error, List<Question> questions, Amendment amendment)
        {
            ViewData.ModelState.AddModelError(promptAnswerViewModel.QuestionId, error.Title);
            ViewData["errorMessage"] = error.Detail;

            var errorUploadViewModel = new QuestionViewModel(questions, promptAnswerViewModel.CurrentIndex)
            {
                PupilDetails = new PupilViewModel(amendment.Pupil),
                ShowConditional = true
            };

            ViewBag.Upload = GetFiles();
            return errorUploadViewModel;
        }

        private FileValidationError UploadEvidence(Amendment amendment)
        {
            var file = Request.Form.Files.First();
            var whitelist = ".jpg,.doc,.docx,.pdf,.html".Split(',');

            if (!whitelist.Any(x => file.FileName.EndsWith(x, true, CultureInfo.InvariantCulture)))
            {
                return new FileValidationError("The file is not an acceptable file format","The file is not an acceptable file format - acceptable file format is .JPG, .DOC, .DOCX, PDF, .HTML");
            }

            if (file.Length > ThreeMegabytes)
            {
                return new FileValidationError("The file size can not be more than 3MB", "The file size can not be more than 3MB");
            }

            var files = GetFiles();

            if(files?.Files != null && files.Files.Count >= 12)
            {
                return new FileValidationError("Only 12 files allowed", "There is only a maximum of 12 files allowable. Please remove file(s) before uploading more evidence.");
            }

            if (string.IsNullOrEmpty(amendment.EvidenceFolderName))
            {
                var fileUploadResult = _evidenceService.UploadEvidence(file);

                amendment.EvidenceFolderName = fileUploadResult.FolderName;
                AddFile(fileUploadResult);
                SaveAmendment(amendment);
            }
            else
            {
                var fileUploadResult = _evidenceService.UploadEvidence(amendment.EvidenceFolderName, file);

                AddFile(fileUploadResult);
            }

            return null;
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

        private bool ThisQuestionIsTheConditionalQuestion(Question thisQuestion, string promptAnswer)
        {
            if (thisQuestion.Answer.AnswerPotentials == null || thisQuestion.Answer.AnswerPotentials.Count == 0 || string.IsNullOrEmpty(promptAnswer))
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
    }
}
