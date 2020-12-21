using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace AuthRefresh.Data.Configuration
{
    public static class Extensions
    {
        public static IServiceCollection AddAuthRefreshData(
            this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<AuthRefreshContext>(options => {
                options.UseSqlServer(configuration.GetConnectionString("AuthRefreshConnection"));
            });
            return services;
        }
    }
}