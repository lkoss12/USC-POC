using Microsoft.AspNetCore.Builder;

namespace AuthRefreshApi.Middleware
{
    public static class AuthRefreshExtensions
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