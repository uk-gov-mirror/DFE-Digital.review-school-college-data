using System;
using System.Collections.Generic;
using System.Text;
using Dfe.CspdAlpha.Web.Application.Application.Interfaces;
using Dfe.CspdAlpha.Web.Application.Controllers;
using Dfe.CspdAlpha.Web.Application.Models.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging.Abstractions;
using Moq;
using Xunit;

namespace Dfe.Rscd.Web.UnitTests.Controllers
{
    public class TaskListControllerTests : BaseControllerTests
    {
        [Fact]
        public void WhenIndexCalledShouldReturnIndexViewWithHomeViewModel()
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
    }
}
