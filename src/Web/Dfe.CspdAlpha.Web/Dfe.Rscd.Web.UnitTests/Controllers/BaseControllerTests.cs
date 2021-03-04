using System.Collections.Generic;
using System.Security.Claims;
using System.Text.Json;
using Dfe.Rscd.Web.Application.Application;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Routing;
using Moq;
using static Dfe.Rscd.Web.Application.Application.Helpers.ClaimsHelper;

namespace Dfe.Rscd.Web.UnitTests.Controllers
{
    public abstract class BaseControllerTests
    {
        protected string SchoolUrn => "10345";
        protected string UserId => "987198729";
        protected TestSession Session { get; } = new();

        protected ClaimsPrincipal GetClaimsPrincipal()
        {
            var org = new Organisation
            {
                urn = SchoolUrn
            };
            var claims = new List<Claim>
            {
                new("http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier", UserId),
                new("organisation", JsonSerializer.Serialize(org))
            };
            var identity = new ClaimsIdentity(claims, "TestAuthType");
            var claimsPrincipal = new ClaimsPrincipal(identity);

            return claimsPrincipal;
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