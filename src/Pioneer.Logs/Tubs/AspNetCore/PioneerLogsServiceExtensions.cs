using System;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Pioneer.Logs.Models;

namespace Pioneer.Logs.Tubs.AspNetCore
{
    /// <summary>
    /// Extension for <see cref="IServiceCollection"/> that binds configuration
    /// from appsettings.json to Pioneer Logs
    /// </summary>
    public static class PioneerLogsServiceExtensions
    {
        /// <summary>
        /// Adds Configuration to <see cref="IServiceCollection"/> that is used to drive <see cref="PioneerLogger"/>.
        /// </summary>
        /// <param name="services">The <see cref="IServiceCollection"/> collection to configure to append to.</param>
        /// <param name="configuration"><see cref="PioneerLogsConfiguration"/> to drive settings for <see cref="PioneerLogger"/>.</param>
        public static IServiceCollection AddPioneerLogs(this IServiceCollection services,
            IConfiguration configuration)
        {
            _ = configuration ?? throw new ArgumentNullException(nameof(configuration));

            services.Configure<PioneerLogsConfiguration>(configuration.Bind);

            PioneerLogsTub.Configuration.ApplicationName = configuration.GetValue<string>("ApplicationName");
            PioneerLogsTub.Configuration.ApplicationLayer = configuration.GetValue<string>("ApplicationLayer");

            configuration.GetSection("Usage").Bind(PioneerLogsTub.Configuration.Usage);
            configuration.GetSection("Errors").Bind(PioneerLogsTub.Configuration.Errors);
            configuration.GetSection("Diagnostics").Bind(PioneerLogsTub.Configuration.Diagnostics);
            configuration.GetSection("Performance").Bind(PioneerLogsTub.Configuration.Performance);

            return services;
        }
    }
}
