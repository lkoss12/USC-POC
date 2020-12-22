using AuthRefresh.Services.Interfaces;
using AuthRefresh.Services.Services;
using AuthRefresh.Services.TransferObjects;
using Microsoft.Extensions.DependencyInjection;

namespace AuthRefresh.Services.Configuration
{
    public static class Extensions
    {
        public static IServiceCollection AddAuthRefreshServices(
            this IServiceCollection services)
        {
            services.AddScoped<ITokenService, TokenService>();
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IUser, User>();
            services.AddScoped<IImpersonatedUser, ImpersonatedUser>();

            services.AddTransient<IAuthService, AuthService>();
            services.AddTransient<IDataService, DataService>();
            return services;
        }
    }
}