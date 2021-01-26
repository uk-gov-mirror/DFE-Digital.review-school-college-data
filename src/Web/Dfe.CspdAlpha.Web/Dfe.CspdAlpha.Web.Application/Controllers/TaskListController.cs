using System;
using Dfe.CspdAlpha.Web.Application.Application.Interfaces;
using Dfe.CspdAlpha.Web.Application.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

namespace Dfe.CspdAlpha.Web.Application.Controllers
{
    public class TaskListController : SessionController
    {
        private const string TASK_LIST = "task-list-{0}";
        private readonly ISchoolService _schoolService;

        public TaskListController(ISchoolService schoolService, IConfiguration configuration)
        {
            _schoolService = schoolService;
        }

        public IActionResult Index()
        {
            ClearAmendmentAndRelated();

            var viewModel = GetTaskList();
            if (viewModel == null)
            {
                viewModel = _schoolService.GetConfirmationRecord(UserId, GetCurrentUrn()) ??
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

            _schoolService.UpdateConfirmation(viewModel, UserId, GetCurrentUrn());
            SaveTaskList(viewModel);

            return RedirectToAction("Index");
        }

        [HttpPost]
        public IActionResult ConfrimData()
        {
            var viewModel = GetTaskList();

            if (viewModel == null || !viewModel.ReviewChecked) return RedirectToAction("Index");
            viewModel.DataConfirmed = true;

            _schoolService.UpdateConfirmation(viewModel, UserId, GetCurrentUrn());

            viewModel.ConfirmationDate = DateTime.Now.Date;

            SaveTaskList(viewModel);

            return RedirectToAction("Index");
        }
    }
}
