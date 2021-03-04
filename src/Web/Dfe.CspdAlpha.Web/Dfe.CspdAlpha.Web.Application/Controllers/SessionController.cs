using System.Collections.Generic;
using System.Linq;
using Dfe.Rscd.Web.ApiClient;
using Dfe.Rscd.Web.Application.Application;
using Dfe.Rscd.Web.Application.Application.Helpers;
using Dfe.Rscd.Web.Application.Models.ViewModels;
using Dfe.Rscd.Web.Infrastructure.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Dfe.Rscd.Web.Application.Controllers
{
    public class SessionController : Controller
    {
        private const string AMENDMENT_SESSION_KEY = "current-amendment";
        private const string PROMPT_QUESTIONS = "new-promptquestions-ref";
        private const string TASK_LIST = "task-list-{0}";
        private const string FILE_LIST = "files-list";

        protected CheckingWindow CheckingWindow => CheckingWindowHelper.GetCheckingWindow(RouteData);

        protected Amendment GetAmendment()
        {
            return HttpContext.Session.Get<Amendment>(AMENDMENT_SESSION_KEY);
        }

        protected FilesViewModel GetFiles()
        {
            return HttpContext.Session.Get<FilesViewModel>(FILE_LIST);
        }

        protected void AddFile(FileUploadResult file)
        {
            var files = GetFiles() ?? new FilesViewModel();

            if (files.Files.All(x => x.Id != file.FileId.ToString()))
            {
                files.Files.Add(new FileViewModel
                {
                    Id = file.FileId.ToString(),
                    FileName = file.FileName,
                    FolderName = file.FolderName
                });
            }

             HttpContext.Session.Set(FILE_LIST, files);
        }

        protected List<Question> GetQuestions()
        {
            return HttpContext.Session.Get<List<Question>>(PROMPT_QUESTIONS);
        }

        protected string UserId => ClaimsHelper.GetUserId(User) + CheckingWindow;

        protected TaskListViewModel GetTaskList()
        {
            return HttpContext.Session.Get<TaskListViewModel>(string.Format(TASK_LIST, UserId));
        }

        protected void SaveTaskList(TaskListViewModel model)
        {
            HttpContext.Session.Set(string.Format(TASK_LIST, UserId), model);
        }

        protected string GetCurrentUrn()
        {
            return ClaimsHelper.GetURN(User);
        }

        protected void ClearAll()
        {
            ClearTaskList();
            ClearAmendment();
        }

        protected void ClearAmendmentAndRelated()
        {
            ClearAmendment();
            ClearQuestions();
        }
        protected void ClearQuestions()
        {
            HttpContext.Session.Remove(PROMPT_QUESTIONS);
        }

        protected void ClearAmendment()
        {
            HttpContext.Session.Remove(AMENDMENT_SESSION_KEY);
        }

        protected void ClearTaskList()
        {
            HttpContext.Session.Remove(string.Format(TASK_LIST, UserId));
        }

        protected void SaveQuestions(List<Question> questions)
        {
            HttpContext.Session.Set(PROMPT_QUESTIONS, questions);
        }

        protected void SaveAnswer(UserAnswer userAnswer)
        {
            var amendment = GetAmendment();
            amendment.Answers ??= new List<UserAnswer>();

            var answer = amendment.Answers.SingleOrDefault(x => x.QuestionId == userAnswer.QuestionId);
            if (answer == null)
                amendment.Answers.Add(userAnswer);
            else
            {
                amendment.Answers.Remove(answer);
                amendment.Answers.Add(userAnswer);
            }

            SaveAmendment(amendment);
        }

        protected void SaveAmendment(Amendment amendment)
        {
            HttpContext.Session.Set(AMENDMENT_SESSION_KEY, amendment);
        }
    }
}
