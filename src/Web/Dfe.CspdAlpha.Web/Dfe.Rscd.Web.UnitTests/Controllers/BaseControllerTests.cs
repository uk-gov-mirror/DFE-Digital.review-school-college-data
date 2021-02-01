using System.Collections.Generic;
using System.Security.Claims;
using Dfe.CspdAlpha.Web.Application.Application;
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
        protected TestSession Session { get; } = new TestSession();

        protected ClaimsPrincipal GetClaimsPrincipal()
        {
            var claims = new List<Claim>() 
            { 
                new Claim("urn:oid:0.9.2342.19200300.100.1.1", UserId),
                new Claim("https://sa.education.gov.uk/idp/org/establishment/uRN", SchoolUrn),
                new Claim("name", "John Doe"),
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
