using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Pioneer.Logs.Models;
using Pioneer.Logs.Tubs.AspNetCore;
using Serilog;

namespace Pioneer.Logs.Samples.AspNetCore
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddPioneerLogs(Configuration.GetSection("PioneerLogsConfiguration"));
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseHsts();
            }

            app.UsePioneerLogs(new PioneerLogsTubConfiguration {
                PerformanceLogger = new LoggerConfiguration()
                    .WriteTo.File(path: @"logs\2performance.txt", rollingInterval: RollingInterval.Day)
                    .CreateLogger(),
                UsageLogger = new LoggerConfiguration()
                    .WriteTo.File(path: @"logs\2usage.txt", rollingInterval: RollingInterval.Day)
                    .CreateLogger(),
                ErrorLogger = new LoggerConfiguration()
                    .WriteTo.File(path: @"logs\2error.txt", rollingInterval: RollingInterval.Day)
                    .CreateLogger(),
                DiagnosticLogger = new LoggerConfiguration()
                    .WriteTo.File(path: @"logs\2diagnostic.txt", rollingInterval: RollingInterval.Day)
                    .CreateLogger()
            });

            app.UseHttpsRedirection();
            app.UseMvc();
        }
    }
}
