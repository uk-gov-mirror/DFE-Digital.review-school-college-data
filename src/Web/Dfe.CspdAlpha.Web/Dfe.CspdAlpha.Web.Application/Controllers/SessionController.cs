using System.Collections.Generic;
using System.Linq;
using Dfe.CspdAlpha.Web.Application.Application;
using Dfe.CspdAlpha.Web.Application.Application.Helpers;
using Dfe.CspdAlpha.Web.Application.Models.ViewModels;
using Dfe.Rscd.Web.ApiClient;
using Microsoft.AspNetCore.Mvc;

namespace Dfe.CspdAlpha.Web.Application.Controllers
{
    public class SessionController : Controller
    {
        private const string AMENDMENT_SESSION_KEY = "current-amendment";
        private const string PROMPT_QUESTIONS = "new-promptquestions-ref";
        private const string TASK_LIST = "task-list-{0}";

        protected CheckingWindow CheckingWindow => CheckingWindowHelper.GetCheckingWindow(RouteData);

        protected Amendment GetAmendment()
        {
            return HttpContext.Session.Get<Amendment>(AMENDMENT_SESSION_KEY);
        }

        protected List<PromptAnswer> GetAnswers()
        {
            var amendment = GetAmendment();
            if (amendment.Answers == null)
            {
                return new List<PromptAnswer>();
            }
            return amendment.Answers.ToList();
        }

        protected List<Prompt> GetQuestions()
        {
            return HttpContext.Session.Get<List<Prompt>>(PROMPT_QUESTIONS);
        }

        protected string UserId => ClaimsHelper.GetUserId(User) + CheckingWindow;

        protected TaskListViewModel GetTaskList()
        {
            return HttpContext.Session.Get<TaskListViewModel>(string.Format(TASK_LIST, UserId));
        }

        protected void SaveTaskList(TaskListViewModel model)
        {
            HttpContext.Session.Set<TaskListViewModel>(string.Format(TASK_LIST, UserId), model);
        }

        protected void ClearAll()
        {
            ClearTaskList();
            ClearAmendment();
            ClearQuestions();
        }

        protected void ClearAmendmentAndRelated()
        {
            ClearAmendment();
            ClearQuestions();
        }
        protected void ClearAmendment()
        {
            HttpContext.Session.Remove(AMENDMENT_SESSION_KEY);
        }

        protected void ClearQuestions()
        {
            HttpContext.Session.Remove(PROMPT_QUESTIONS);
        }

        protected void ClearAnswers()
        {
            var amendment = GetAmendment();
            if (amendment != null)
            {
                amendment.Answers = new List<PromptAnswer>();
            }
        }

        protected void ClearTaskList()
        {
            HttpContext.Session.Remove(string.Format(TASK_LIST, UserId));
        }

        protected void SaveAmendment(Amendment amendment)
        {
            HttpContext.Session.Set(AMENDMENT_SESSION_KEY, amendment);
        }

        protected void SaveQuestions(List<Prompt> prompts)
        {
            var existingQuestions = GetQuestions();
            if (existingQuestions == null)
            {
                existingQuestions = new List<Prompt>();
            }
            existingQuestions.AddRange(prompts);
            HttpContext.Session.Set(PROMPT_QUESTIONS, existingQuestions);
        }

        protected void SaveAnswers(List<PromptAnswer> answers)
        {
            var amendment = GetAmendment();
            amendment.Answers = answers ?? new List<PromptAnswer>();
            SaveAmendment(amendment);
        }

        protected void AddAnswer(PromptAnswer promptAnswer)
        {
            var answers = GetAnswers();
            answers.Add(promptAnswer);
            SaveAnswers(answers);
        }
    }
}
