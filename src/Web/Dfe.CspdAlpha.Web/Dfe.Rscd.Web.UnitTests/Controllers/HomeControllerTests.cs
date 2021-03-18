using Dfe.Rscd.Web.Application.Controllers;
using Dfe.Rscd.Web.Application.Models.ViewModels;
using Dfe.Rscd.Web.Application.Security;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging.Abstractions;
using Xunit;

namespace Dfe.Rscd.Web.UnitTests.Controllers
{
    public class HomeControllerTests : BaseControllerTests
    {
        [Fact]
        public void WhenIndexCalledShouldReturnIndexViewWithHomeViewModel()
        {
            var controller = new HomeController(new NullLogger<HomeController>(), new UserInfo());

            var result = controller.Index() as ViewResult;

            Assert.IsType<HomeViewModel>(result.Model);
        }

        [Fact]
        public void WhenIndexCalledAndModelStateIsValidShouldRedirectToIndexOfTaskList()
        {
            var controller = new HomeController(new NullLogger<HomeController>(), GetUserInfo());

            var result = controller.Index(new HomeViewModel {SelectedKeyStage = "Ks5"}) as RedirectToActionResult;

            Assert.True(result.ActionName == "Index");
            Assert.True(result.ControllerName == "TaskList");
            Assert.True(result.RouteValues["phase"].ToString() == "ks5");
            Assert.True(result.RouteValues["urn"].ToString() == SchoolUrn);
        }
    }
}