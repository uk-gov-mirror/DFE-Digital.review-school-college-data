using System;
using Dfe.CspdAlpha.Web.Application.Application;
using Dfe.CspdAlpha.Web.Application.Application.Helpers;
using Dfe.CspdAlpha.Web.Application.Application.Interfaces;
using Dfe.CspdAlpha.Web.Application.Models.Common;
using Dfe.CspdAlpha.Web.Application.Models.ViewModels;
using Dfe.Rscd.Web.ApiClient;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

namespace Dfe.CspdAlpha.Web.Application.Controllers
{
    public class TaskListController : Controller
    {
        private const string TASK_LIST = "task-list-{0}";
        private readonly ISchoolService _schoolService;

        public TaskListController(ISchoolService schoolService, IConfiguration configuration)
        {
            _schoolService = schoolService;
        }

        private CheckingWindow CheckingWindow => CheckingWindowHelper.GetCheckingWindow(RouteData);
        private string UserID { get; set; }

        public IActionResult Index()
        {
            HttpContext.Session.Remove(Constants.PROMPT_QUESTIONS);
            HttpContext.Session.Remove(Constants.PROMPT_ANSWERS);
            UserID = ClaimsHelper.GetUserId(User) + CheckingWindow;
            var viewModel = HttpContext.Session.Get<TaskListViewModel>(string.Format(TASK_LIST, UserID));
            if (viewModel == null)
            {
                var urn = ClaimsHelper.GetURN(User);
                viewModel = _schoolService.GetConfirmationRecord(CheckingWindow, UserID, urn) ??
                            new TaskListViewModel();
                viewModel.CheckingWindow = CheckingWindow;
                HttpContext.Session.Set(string.Format(TASK_LIST, UserID), viewModel);
            }

            return View(viewModel);
        }

        [HttpPost]
        public IActionResult Review()
        {
            UserID = ClaimsHelper.GetUserId(User) + CheckingWindow;
            var viewModel = HttpContext.Session.Get<TaskListViewModel>(string.Format(TASK_LIST, UserID));
            viewModel.ReviewChecked = true;
            var urn = ClaimsHelper.GetURN(User);
            _schoolService.UpdateConfirmation(CheckingWindow, viewModel, UserID, urn);
            HttpContext.Session.Set(string.Format(TASK_LIST, UserID), viewModel);
            return RedirectToAction("Index");
        }

        [HttpPost]
        public IActionResult ConfrimData()
        {
            UserID = ClaimsHelper.GetUserId(User) + CheckingWindow;
            var viewModel = HttpContext.Session.Get<TaskListViewModel>(string.Format(TASK_LIST, UserID));
            if (viewModel == null || !viewModel.ReviewChecked) return RedirectToAction("Index");
            viewModel.DataConfirmed = true;
            var urn = ClaimsHelper.GetURN(User);
            _schoolService.UpdateConfirmation(CheckingWindow, viewModel, UserID, urn);
            viewModel.ConfirmationDate = DateTime.Now.Date;
            HttpContext.Session.Set(string.Format(TASK_LIST, UserID), viewModel);
            return RedirectToAction("Index");
        }
    }
}
