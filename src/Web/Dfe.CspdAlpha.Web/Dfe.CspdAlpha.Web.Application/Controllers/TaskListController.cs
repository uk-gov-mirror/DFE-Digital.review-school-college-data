using Dfe.CspdAlpha.Web.Application.Application.Extensions;
using Dfe.CspdAlpha.Web.Application.Application.Helpers;
using Dfe.CspdAlpha.Web.Application.Application.Interfaces;
using Dfe.CspdAlpha.Web.Application.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

namespace Dfe.CspdAlpha.Web.Application.Controllers
{
    public class TaskListController : Controller
    {
        private ISchoolService _schoolService;
        private const string TASK_LIST = "task-list-{0}";
        private bool LateChecking { get; }
        private string UserID { get; set; }

        public TaskListController(ISchoolService schoolService, IConfiguration configuration)
        {
            _schoolService = schoolService;
            LateChecking = configuration["CheckingPhase"] == "Late";
        }

        public IActionResult Index()
        {
            UserID = ClaimsHelper.GetUserId(this.User) + (LateChecking ? "-late" : string.Empty);
            var viewModel = HttpContext.Session.Get<TaskListViewModel>(string.Format(TASK_LIST, UserID));
            if (viewModel == null)
            {
                var urn = ClaimsHelper.GetURN(this.User);
                viewModel = _schoolService.GetConfirmationRecord(UserID, urn) ?? new TaskListViewModel();
                viewModel.LateCheckingPhase = LateChecking;
                HttpContext.Session.Set(string.Format(TASK_LIST, UserID), viewModel);
            }
            return View(viewModel);
        }

        [HttpPost]
        public IActionResult Review()
        {
            UserID = ClaimsHelper.GetUserId(this.User) + (LateChecking ? "-late" : string.Empty);
            var viewModel = HttpContext.Session.Get<TaskListViewModel>(string.Format(TASK_LIST, UserID));
            viewModel.ReviewChecked = true;
            var urn = ClaimsHelper.GetURN(this.User);
            _schoolService.UpdateConfirmation(viewModel, UserID, urn);
            HttpContext.Session.Set(string.Format(TASK_LIST, UserID), viewModel);
            return RedirectToAction("Index");
        }

        [HttpPost]
        public IActionResult ConfrimData()
        {
            UserID = ClaimsHelper.GetUserId(this.User) + (LateChecking ? "-late" : string.Empty);
            var viewModel = HttpContext.Session.Get<TaskListViewModel>(string.Format(TASK_LIST, UserID));
            if (viewModel == null || !viewModel.ReviewChecked)
            {
                return RedirectToAction("Index");
            }
            viewModel.DataConfirmed = true;
            var urn = ClaimsHelper.GetURN(this.User);
            _schoolService.UpdateConfirmation(viewModel, UserID, urn);
            HttpContext.Session.Set(string.Format(TASK_LIST, UserID), viewModel);
            return RedirectToAction("Index");
        }
    }
}
