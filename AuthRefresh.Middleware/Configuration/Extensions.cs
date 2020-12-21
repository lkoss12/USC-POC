using Microsoft.AspNetCore.Builder;

namespace AuthRefresh.Middleware.Configuration
{
    public static class Extensions
    {
        public static IApplicationBuilder UseTokenAuthorization(
            this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<RequestTokenMiddleware>();
        }
        public static IApplicationBuilder UseImpersonation(
            this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<ImpersonationMiddleware>();
        }
    }
}