using Dfe.Rscd.Web.Application.Controllers;
using Dfe.Rscd.Web.Application.Models.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Logging.Abstractions;
using Moq;
using Xunit;

namespace Dfe.Rscd.Web.UnitTests.Controllers
{
    public class HomeControllerTests : BaseControllerTests
    {
        [Fact]
        public void WhenIndexCalledShouldReturnIndexViewWithHomeViewModel()
        {
            var context = new Mock<HttpContext>();
            var controller = new HomeController(new NullLogger<HomeController>())
            {
                ControllerContext =
                    new ControllerContext(new ActionContext(context.Object, new RouteData(),
                        new ControllerActionDescriptor()))
            };

            var result = controller.Index() as ViewResult;

            Assert.IsType<HomeViewModel>(result.Model);
        }

        [Fact]
        public void WhenIndexCalledAndModelStateIsValidShouldRedirectToIndexOfTaskList()
        {
            var context = new Mock<HttpContext>();
            context.SetupGet(x => x.User).Returns(GetClaimsPrincipal());

            var controller = new HomeController(new NullLogger<HomeController>())
            {
                ControllerContext =
                    new ControllerContext(new ActionContext(context.Object, new RouteData(),
                        new ControllerActionDescriptor()))
            };

            var result = controller.Index(new HomeViewModel {SelectedKeyStage = "Ks5"}) as RedirectToActionResult;

            Assert.True(result.ActionName == "Index");
            Assert.True(result.ControllerName == "TaskList");
            Assert.True(result.RouteValues["phase"].ToString() == "ks5");
            Assert.True(result.RouteValues["urn"].ToString() == SchoolUrn);
        }
    }
}