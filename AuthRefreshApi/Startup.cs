using System;
using System.IO;
using System.Linq;
using AuthRefresh.Services.Configuration;
using AuthRefreshApi.Configuration;
using AuthRefresh.Middleware.Configuration;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Logging;

namespace AuthRefreshApi
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; } = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
            .AddJsonFile($"appsettings.{Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT").ToLower()}.json", optional: true)
            .AddEnvironmentVariables()
            .Build();

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();
            services.AddSingleton<IConfiguration>((x) => Configuration);
            services.AddAuthRefreshServices();
            
            IdentityModelEventSource.ShowPII = true;

            var corsDomains = new CorsDomainsConfigSection();
            Configuration.GetSection("CorsDomains").Bind(corsDomains);
            services.AddCors(options =>
            {
                options.AddPolicy("_AllowSpecificOrigins",
                builder =>
                {
                    builder.WithOrigins(corsDomains.Values.ToArray());
                    builder.AllowCredentials();
                    builder.AllowAnyHeader();
                    builder.AllowAnyMethod();
                });
            });


        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseCors("_AllowSpecificOrigins");

            app.UseTokenAuthorization();
            app.UseImpersonation();
            
            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
