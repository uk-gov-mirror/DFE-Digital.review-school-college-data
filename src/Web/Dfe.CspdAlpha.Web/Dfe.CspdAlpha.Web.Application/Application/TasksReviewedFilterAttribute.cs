using System;
using System.Collections.Generic;
using System.Linq;
using Dfe.Rscd.Web.Application.Application.Helpers;
using Dfe.Rscd.Web.Application.Models.ViewModels;
using Dfe.Rscd.Web.Application.Security;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Dfe.Rscd.Web.Application.Application
{
    public class TasksReviewedFilterAttribute : TypeFilterAttribute
    {
        public TasksReviewedFilterAttribute(string allowedActions) : base(typeof(TasksReviewedFilterAttributeImpl))
        {
            Arguments = new object[] { allowedActions };
        }

        private class TasksReviewedFilterAttributeImpl : IActionFilter
        {
            private const string TASK_LIST = "task-list-{0}";

            private readonly List<string> _allowedActions;
            private readonly UserInfo _userInfo;

            public TasksReviewedFilterAttributeImpl(string allowedActions, UserInfo userInfo)
            {
                _allowedActions = allowedActions
                    .Split(',', StringSplitOptions.RemoveEmptyEntries)
                    .Select(a => a.Trim())
                    .ToList();
                _userInfo = userInfo;
            }

            public void OnActionExecuting(ActionExecutingContext context)
            {
                if (_allowedActions.All(a => !a.Equals(context.RouteData.Values["action"].ToString(), StringComparison.InvariantCultureIgnoreCase)))
                {
                    var checkingWindow = CheckingWindowHelper.GetCheckingWindow(context.RouteData);
                    var userId = _userInfo.UserId + checkingWindow.ToString();
                    var viewModel = context.HttpContext.Session.Get<TaskListViewModel>(string.Format(TASK_LIST, userId));

                    if (viewModel == null || !viewModel.ReviewChecked)
                    {
                        context.Result = new RedirectToActionResult("Index", "TaskList", null);
                    }
                }
            }

            public void OnActionExecuted(ActionExecutedContext context)
            {
            }
        }
    }
}
