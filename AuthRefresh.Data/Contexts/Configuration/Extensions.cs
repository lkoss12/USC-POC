using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Console;

namespace AuthRefresh.Data.Configuration
{
    public static class Extensions
    {
        public static readonly ILoggerFactory loggerFactory = LoggerFactory.Create(builder => {
            builder.AddConsole();
        });

        public static IServiceCollection AddAuthRefreshData(
            this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<AuthRefreshContext>(options => {
                options.UseSqlServer(configuration.GetConnectionString("AuthRefreshConnection"));
                options.UseLoggerFactory(loggerFactory);
            }, ServiceLifetime.Transient);

            return services;
        }
    }
}