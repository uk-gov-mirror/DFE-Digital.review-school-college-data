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
        private const string TASK_LIST = "task-list-{0}";

        public TaskListController(ISchoolService schoolService)
        {
            _schoolService = schoolService;
        }

        public IActionResult Index()
        {
            var userId = ClaimsHelper.GetUserId(this.User);
            var viewModel = HttpContext.Session.Get<TaskListViewModel>(string.Format(TASK_LIST, userId));
            if (viewModel == null)
            {
                var urn = ClaimsHelper.GetURN(this.User);
                viewModel = _schoolService.GetConfirmationRecord(userId, urn);
                HttpContext.Session.Set(string.Format(TASK_LIST, userId), viewModel ?? new TaskListViewModel());
            }

            return View(viewModel);
        }

        [HttpPost]
        public IActionResult Review()
        {
            var userId = ClaimsHelper.GetUserId(this.User);
            var viewModel = HttpContext.Session.Get<TaskListViewModel>(string.Format(TASK_LIST, userId));
            viewModel.ReviewChecked = true;
            var urn = ClaimsHelper.GetURN(this.User);
            _schoolService.UpdateConfirmation(viewModel, userId, urn);
            HttpContext.Session.Set(string.Format(TASK_LIST, userId), viewModel);
            return RedirectToAction("Index");
        }

        [HttpPost]
        public IActionResult ConfrimData()
        {
            var userId = ClaimsHelper.GetUserId(this.User);
            var viewModel = HttpContext.Session.Get<TaskListViewModel>(string.Format(TASK_LIST, userId));
            if (viewModel == null || !viewModel.ReviewChecked)
            {
                return RedirectToAction("Index");
            }
            viewModel.DataConfirmed = true;
            var urn = ClaimsHelper.GetURN(this.User);
            _schoolService.UpdateConfirmation(viewModel, userId, urn);
            HttpContext.Session.Set(string.Format(TASK_LIST, userId), viewModel);
            return RedirectToAction("Index");
        }
    }
}
