using Microsoft.AspNetCore.Builder;

namespace Dfe.CspdAlpha.Api.Middleware.BasicAuth
{
    public static class IApplicationBuilderExtensions
    {
        public static IApplicationBuilder UseBasicAuth(
            this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<BasicAuthMiddleware>();
        }
    }
}
