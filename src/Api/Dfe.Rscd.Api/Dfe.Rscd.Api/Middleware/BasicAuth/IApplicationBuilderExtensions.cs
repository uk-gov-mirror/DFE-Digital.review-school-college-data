using Microsoft.AspNetCore.Builder;

namespace Dfe.Rscd.Api.Middleware.BasicAuth
{
    public static class ApplicationBuilderExtensions
    {
        public static IApplicationBuilder UseBasicAuth(
            this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<BasicAuthMiddleware>();
        }
    }
}