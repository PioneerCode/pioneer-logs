using System.Runtime.CompilerServices;
using Microsoft.AspNetCore.Builder;
using Pioneer.Logs.Models;

namespace Pioneer.Logs.Tubs.AspNetCore
{
    /// <summary>
    /// Extensions for Pioneer Logs
    /// </summary>
    public static class PioneerLogsBuilderExtensions
    {
        /// <summary>
        /// Add Middleware to HTTP pipeline that captures and logs exceptions
        /// with the <see cref="PioneerLogsTub"/>
        /// </summary>
        /// <param name="app"><inheritdoc cref="IApplicationBuilder"/></param>
        /// <param name="configuration"><inheritdoc cref="PioneerLogsTubConfiguration"/></param>
        public static void UsePioneerLogs(this IApplicationBuilder app, PioneerLogsTubConfiguration configuration)
        {
            // Set default by way of constructor before attempting overrides. 
            RuntimeHelpers.RunClassConstructor(typeof(PioneerLogger).TypeHandle);
            SetLoggers(configuration);

            app.UseMiddleware<PioneerLogsMiddleware>();
        }

        /// <summary>
        /// Set overrides for <see cref="Serilog.ILogger"/> types if provided.
        /// </summary>
        /// <param name="configuration"><see cref="PioneerLogsTubConfiguration"/></param>
        private static void SetLoggers(PioneerLogsTubConfiguration configuration)
        {
            PioneerLogger.DiagnosticLogger = configuration.DiagnosticLogger ?? PioneerLogger.DiagnosticLogger;
            PioneerLogger.PerforamnceLogger = configuration.PerforamnceLogger ?? PioneerLogger.PerforamnceLogger;
            PioneerLogger.UsageLogger = configuration.UsageLogger ?? PioneerLogger.UsageLogger;
            PioneerLogger.ErrorLogger = configuration.ErrorLogger ?? PioneerLogger.ErrorLogger;
        }
    }
}
