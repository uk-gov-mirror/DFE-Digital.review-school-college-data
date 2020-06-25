using Dfe.CspdAlpha.Web.Application.Application.Extensions;
using Dfe.CspdAlpha.Web.Application.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace Dfe.CspdAlpha.Web.Application.Controllers
{
    public class TaskListController : Controller
    {
        private const string TASK_LIST = "task-list";
        public IActionResult Index()
        {
            var viewModel = HttpContext.Session.Get<TaskListViewModel>(TASK_LIST) ?? new TaskListViewModel();
            return View(viewModel);
        }

        [HttpPost]
        public IActionResult Review()
        {
            var viewModel = HttpContext.Session.Get<TaskListViewModel>(TASK_LIST) ?? new TaskListViewModel();
            viewModel.ReviewChecked = true;
            HttpContext.Session.Set("task-list", viewModel);
            return RedirectToAction("Index");
        }

        [HttpPost]
        public IActionResult ConfrimData()
        {
            var viewModel = HttpContext.Session.Get<TaskListViewModel>(TASK_LIST) ?? new TaskListViewModel();
            viewModel.DataConfirmed = viewModel.ReviewChecked;
            HttpContext.Session.Set("task-list", viewModel);
            return RedirectToAction("Index");
        }
    }
}
