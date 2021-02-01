using System;
using System.Net.Cache;
using Dfe.CspdAlpha.Web.Application.Application;
using Dfe.CspdAlpha.Web.Application.Application.Interfaces;
using Dfe.CspdAlpha.Web.Application.Controllers;
using Dfe.CspdAlpha.Web.Application.Models.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Configuration;
using Moq;
using Xunit;

namespace Dfe.Rscd.Web.UnitTests.Controllers
{
    public class TaskListControllerTests : BaseControllerTests
    {
        [Fact]
        public void WhenIndexCalledShouldReturnTaskListViewModelWithDataUnconfirmed()
        {
            var context = new Mock<HttpContext>();
            var schoolService = new Mock<ISchoolService>();
            var config = new Mock<IConfiguration>();

            schoolService.Setup(x => x.GetConfirmationRecord(It.IsAny<string>(), It.IsAny<string>()))
                .Returns(new TaskListViewModel("ks5"));

            context.Setup(x => x.Session).Returns(Session);
            context.Setup(x => x.User).Returns(GetClaimsPrincipal());

            var controller = new TaskListController(schoolService.Object, config.Object)
            {
                ControllerContext = GetControllerContext("ks5", context)
            };

            var result = controller.Index() as ViewResult;
            var model = result.Model as TaskListViewModel;

            Assert.True(model != null);
            Assert.True(model.DataConfirmed == false);
        }

        [Fact]
        public void WhenReviewDataCalledAfterIndexShouldReturnTaskListViewModelWithReviewConfirmed()
        {
            var context = new Mock<HttpContext>();
            var schoolService = new Mock<ISchoolService>();
            var config = new Mock<IConfiguration>();

            var httpRequest = new Mock<HttpRequest>();
            httpRequest.Setup(x=> x.RouteValues).Returns(new RouteValueDictionary {{"phase", "ks5"}});

            schoolService.Setup(x => x.UpdateConfirmation(It.IsAny<TaskListViewModel>(), It.IsAny<string>(), It.IsAny<string>()));

            context.Setup(x => x.Session).Returns(Session);
            context.Setup(x => x.User).Returns(GetClaimsPrincipal());
            context.Setup(x => x.Request).Returns(httpRequest.Object);

            var controller = new TaskListController(schoolService.Object, config.Object)
            {
                ControllerContext = GetControllerContext("ks5", context)
            };

            controller.Index();
            var result = controller.Review() as RedirectToActionResult;

            var viewModel = Session.Get<TaskListViewModel>($"task-list-{UserId}KS5");

            Assert.True(viewModel != null);
            Assert.True(viewModel.ReviewChecked);
        }

        [Fact]
        public void WhenConfirmDataCalledAfterIndexShouldReturnTaskListViewModelWithDataNotConfirmed()
        {
            var context = new Mock<HttpContext>();
            var schoolService = new Mock<ISchoolService>();
            var config = new Mock<IConfiguration>();

            var httpRequest = new Mock<HttpRequest>();
            httpRequest.Setup(x=> x.RouteValues).Returns(new RouteValueDictionary {{"phase", "ks5"}});

            schoolService.Setup(x => x.UpdateConfirmation(It.IsAny<TaskListViewModel>(), It.IsAny<string>(), It.IsAny<string>()));

            context.Setup(x => x.Session).Returns(Session);
            context.Setup(x => x.User).Returns(GetClaimsPrincipal());
            context.Setup(x => x.Request).Returns(httpRequest.Object);

            var controller = new TaskListController(schoolService.Object, config.Object)
            {
                ControllerContext = GetControllerContext("ks5", context)
            };

            controller.Index();
            var result = controller.ConfrimData() as RedirectToActionResult;

            var viewModel = Session.Get<TaskListViewModel>($"task-list-{UserId}KS5");

            Assert.True(viewModel != null);
            Assert.True(viewModel.DataConfirmed == false);
        }

        [Fact]
        public void WhenConfirmDataCalledAfterReviewIndexShouldReturnTaskListViewModelWithDataConfirmed()
        {
            var context = new Mock<HttpContext>();
            var schoolService = new Mock<ISchoolService>();
            var config = new Mock<IConfiguration>();

            var httpRequest = new Mock<HttpRequest>();
            httpRequest.Setup(x=> x.RouteValues).Returns(new RouteValueDictionary {{"phase", "ks5"}});

            schoolService.Setup(x => x.UpdateConfirmation(It.IsAny<TaskListViewModel>(), It.IsAny<string>(), It.IsAny<string>()));

            context.Setup(x => x.Session).Returns(Session);
            context.Setup(x => x.User).Returns(GetClaimsPrincipal());
            context.Setup(x => x.Request).Returns(httpRequest.Object);

            var controller = new TaskListController(schoolService.Object, config.Object)
            {
                ControllerContext = GetControllerContext("ks5", context)
            };

            controller.Index();
            controller.Review();
            var result = controller.ConfrimData() as RedirectToActionResult;

            var viewModel = Session.Get<TaskListViewModel>($"task-list-{UserId}KS5");

            Assert.True(viewModel != null);
            Assert.True(viewModel.DataConfirmed);
        }
    }
}