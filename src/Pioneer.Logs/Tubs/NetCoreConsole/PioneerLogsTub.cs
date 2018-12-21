using System;
using System.Collections.Generic;
using System.IO;
using Microsoft.Extensions.Configuration;
using Pioneer.Logs.Models;

namespace Pioneer.Logs.Tubs.NetCoreConsole
{
    /// <summary>
    /// .NET Core Windows Console app logging wrapper.
    /// </summary>
    public static class PioneerLogsTub
    {
        public static PioneerLogsConfiguration Configuration { get; set; }
        private static PioneerLogsPerformanceTracker Tracker { get; set; }

        static PioneerLogsTub()
        {
            Configuration = new PioneerLogsConfiguration();
        }

        public static void LogUsage(string message,
            Dictionary<string, object> additionalInfo = null)
        {
            var details = GetTubDetail(message, additionalInfo);
            PioneerLogger.WriteUsage(details);

            if (Configuration.WriteToConsole)
            {
                PioneerLogger.ConsoleLogger.Information(message);
            }
        }

        public static void LogDiagnostic(string message,
            Dictionary<string, object> additionalInfo = null)
        {
            if (!Configuration.WriteDiagnostics)
            {
                return;
            }

            var details = GetTubDetail(message, additionalInfo);
            PioneerLogger.WriteDiagnostic(details);

            if (Configuration.WriteToConsole)
            {
                PioneerLogger.ConsoleLogger.Debug(message);
            }
        }

        /// <summary>
        /// Log Error message with exception
        /// </summary>
        /// <param name="ex">Exception to log</param>
        public static void LogError(Exception ex)
        {
            var details = GetTubDetail(null);
            details.Exception = ex;
            PioneerLogger.WriteError(details);
            if (Configuration.WriteToConsole)
            {
                PioneerLogger.ConsoleLogger.Error(ex.ToString());
            }
        }

        /// <summary>
        /// Log Error message with message string
        /// </summary>
        public static void LogError(string message)
        {
            var details = GetTubDetail(message);
            PioneerLogger.WriteError(details);
            if (Configuration.WriteToConsole)
            {
                PioneerLogger.ConsoleLogger.Error(message);
            }
        }

        /// <summary>
        /// Log Error message with <see cref="Exception"/> and write to console with message
        /// </summary>
        public static void LogError(Exception ex, string message)
        {
            var details = GetTubDetail(message);
            details.Exception = ex;
            PioneerLogger.WriteError(details);
            if (Configuration.WriteToConsole)
            {
                PioneerLogger.ConsoleLogger.Error(message);
            }
        }

        /// <summary>
        /// Get as <see cref="PioneerLog"/> object pre-populated with details parsed
        /// from the .NET Core environment.
        /// </summary>
        private static PioneerLog GetTubDetail(string message,
            Dictionary<string, object> additionalInfo = null)
        {
            var detail = new PioneerLog
            {
                ApplicationName = Configuration.ApplicationName,
                ApplicationLayer = Configuration.ApplicationLayer,
                Message = message,
                Hostname = Environment.MachineName,
                //CorrelationId = Activity.Current?.Id ?? context.TraceIdentifier,
                AdditionalInfo = additionalInfo ?? new Dictionary<string, object>(),
                CreationTimestamp = DateTime.UtcNow
            };

            return detail;
        }

        /// <summary>
        /// Create a new <see cref="PioneerLogsPerformanceTracker"/> object and by way, start the performance timer.
        /// </summary>
        /// <param name="message">Message to log</param>
        public static void StartPerformanceTracker(string message)
        {
            Tracker = new PioneerLogsPerformanceTracker(GetTubDetail(message));
        }

        /// <summary>
        /// Stop the performance timer
        /// </summary>
        public static void StopPerformanceTracker()
        {
            Tracker.Stop();
            Tracker = null;
            if (Configuration.WriteToConsole)
            {
                PioneerLogger.ConsoleLogger.Error(message);
            }
        }

        /// <summary>
        /// Subscribes to <see cref="AppDomain.UnhandledException"/> and log global scope exceptions accordingly.
        /// </summary>
        /// <returns><see cref="PioneerLogsConfiguration"/> configuration object.</returns>
        public static PioneerLogsConfiguration RegisterLogger()
        {
            // Unregister and register
            AppDomain.CurrentDomain.UnhandledException -= UnhandledExceptionHandler;
            AppDomain.CurrentDomain.UnhandledException += UnhandledExceptionHandler;

            // build configuration
            var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", false)
                .Build()
                .GetSection("PioneerLogsConfiguration");

            Configuration.ApplicationName = builder.GetValue<string>("ApplicationName");
            Configuration.ApplicationLayer = builder.GetValue<string>("ApplicationName");
            Configuration.WriteDiagnostics = builder.GetValue<bool>("WriteDiagnostics");
            Configuration.WriteToConsole = builder.GetValue<bool>("WriteToConsole");

            return Configuration;
        }

        private static readonly UnhandledExceptionEventHandler UnhandledExceptionHandler = (s, args) =>
        {
            var ex = args.ExceptionObject as Exception;
            LogError(ex);
        };
    }
}
