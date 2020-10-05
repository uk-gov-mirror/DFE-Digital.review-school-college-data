using Microsoft.AspNetCore.Builder;

namespace Dfe.Rscd.Api.Middleware.BasicAuth
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
