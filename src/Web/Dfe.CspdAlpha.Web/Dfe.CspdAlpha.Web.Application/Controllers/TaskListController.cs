using Dfe.CspdAlpha.Web.Application.Application.Extensions;
using Dfe.CspdAlpha.Web.Application.Application.Helpers;
using Dfe.CspdAlpha.Web.Application.Application.Interfaces;
using Dfe.CspdAlpha.Web.Application.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace Dfe.CspdAlpha.Web.Application.Controllers
{
    public class TaskListController : Controller
    {
        private ISchoolService _schoolService;
        private const string TASK_LIST = "task-list";

        public TaskListController(ISchoolService schoolService)
        {
            _schoolService = schoolService;
        }

        public IActionResult Index()
        {
            var viewModel = HttpContext.Session.Get<TaskListViewModel>(TASK_LIST);
            if (viewModel == null)
            {
                var urn = ClaimsHelper.GetURN(this.User);
                var userId = ClaimsHelper.GetUserId(this.User);
                viewModel = _schoolService.GetConfirmationRecord(userId, urn);
                HttpContext.Session.Set("task-list", viewModel ?? new TaskListViewModel());
            }

            return View(viewModel);
        }

        [HttpPost]
        public IActionResult Review()
        {
            var viewModel = HttpContext.Session.Get<TaskListViewModel>(TASK_LIST);
            viewModel.ReviewChecked = true;
            var urn = ClaimsHelper.GetURN(this.User);
            var userId = ClaimsHelper.GetUserId(this.User);
            _schoolService.UpdateConfirmation(viewModel, userId, urn);
            HttpContext.Session.Set("task-list", viewModel);
            return RedirectToAction("Index");
        }

        [HttpPost]
        public IActionResult ConfrimData()
        {
            var viewModel = HttpContext.Session.Get<TaskListViewModel>(TASK_LIST);
            if (!viewModel.ReviewChecked)
            {
                return RedirectToAction("Index");
            }
            viewModel.DataConfirmed = true;
            var urn = ClaimsHelper.GetURN(this.User);
            var userId = ClaimsHelper.GetUserId(this.User);
            _schoolService.UpdateConfirmation(viewModel, userId, urn);
            HttpContext.Session.Set("task-list", viewModel);
            return RedirectToAction("Index");
        }
    }
}
