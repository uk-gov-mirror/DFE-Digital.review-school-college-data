using System;
using Dfe.Rscd.Web.Application.Application.Interfaces;
using Dfe.Rscd.Web.Application.Models.ViewModels;
using Dfe.Rscd.Web.Application.Security;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

namespace Dfe.Rscd.Web.Application.Controllers
{
    public class TaskListController : SessionController
    {
        private const string TASK_LIST = "task-list-{0}";
        private readonly ISchoolService _schoolService;

        public TaskListController(ISchoolService schoolService, IConfiguration configuration, UserInfo userInfo)
            : base(userInfo)
        {
            _schoolService = schoolService;
        }

        public IActionResult Index()
        {
            ClearAmendmentAndRelated();

            var viewModel = GetTaskList();
            if (viewModel == null)
            {
                viewModel = _schoolService.GetConfirmationRecord(UserId, _userInfo.Urn) ??
                            new TaskListViewModel();

                SaveTaskList(viewModel);
            }

            return View(viewModel);
        }

        [HttpPost]
        public IActionResult Review()
        {
            var viewModel = GetTaskList();

            viewModel.ReviewChecked = true;

            _schoolService.UpdateConfirmation(viewModel, UserId, _userInfo.Urn);

            SaveTaskList(viewModel);

            return RedirectToAction("Index");
        }

        [HttpPost]
        public IActionResult ConfrimData()
        {
            var viewModel = GetTaskList();

            if (viewModel == null || !viewModel.ReviewChecked) return RedirectToAction("Index");
            viewModel.DataConfirmed = true;

            _schoolService.UpdateConfirmation(viewModel, UserId, _userInfo.Urn);

            viewModel.ConfirmationDate = DateTime.Now;

            SaveTaskList(viewModel);

            return RedirectToAction("Index");
        }
    }
}
