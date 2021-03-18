using System.Collections.Generic;
using System.Security.Claims;
using System.Text.Json;
using Dfe.Rscd.Web.Application.Application;
using Dfe.Rscd.Web.Application.Security;
using Dfe.Rscd.Web.Application.Security.DTO;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Routing;
using Moq;

namespace Dfe.Rscd.Web.UnitTests.Controllers
{
    public abstract class BaseControllerTests
    {
        protected string SchoolUrn => "10345";
        protected string UserId => "987198729";
        protected TestSession Session { get; } = new();

        protected UserInfo GetUserInfo()
        {
            return new UserInfo
            {
                UserId = UserId,
                Urn = SchoolUrn
            };
        }

        protected ControllerContext GetControllerContext(string phase, Mock<HttpContext> context)
        {
            var routes = new RouteValueDictionary {{"phase", phase}};

            Context.Configure(context.Object);

            return new ControllerContext(new ActionContext(context.Object, new RouteData(routes),
                new ControllerActionDescriptor()));
        }
    }
}