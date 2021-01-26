using Microsoft.AspNetCore.Http;

namespace Dfe.CspdAlpha.Web.Application.Application
{
    public static class Context
    {
        private static IHttpContextAccessor _httpContextAccessor;

        public static HttpContext Current => _httpContextAccessor.HttpContext;

        public static void Configure(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }
    }
}
