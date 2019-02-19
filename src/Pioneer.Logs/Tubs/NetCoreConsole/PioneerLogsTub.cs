using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using Microsoft.Extensions.Configuration;
using Pioneer.Logs.Models;
using static System.String;

namespace Pioneer.Logs.Tubs.NetCoreConsole
{
    /// <summary>
    /// .NET Core Windows Console app logging wrapper.
    /// </summary>
    public static class PioneerLogsTub
    {
        /// <summary>
        /// Manually overrides system level correlation generated IDs
        /// If used, it is the responsibility of the client to manage state of this value. 
        /// </summary>
        public static string CorrelationId { get; set; }
        public static PioneerLogsConfiguration Configuration { get; set; }
        private static PioneerLogsPerformanceTracker Tracker { get; set; }
        private static string TrackerStartingMethodName { get; set; }

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
                PioneerLogger.ConsoleLogger.Information("USAGE: " + message);
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
                PioneerLogger.ConsoleLogger.Information("DIAGNOSTIC: " + message);
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
            CorrelationId = Empty;
            if (Configuration.WriteToConsole)
            {
                PioneerLogger.ConsoleLogger.Error("ERROR: " + ex);
            }
        }

        /// <summary>
        /// Log Error message with message string
        /// </summary>
        public static void LogError(string message)
        {
            var details = GetTubDetail(message);
            PioneerLogger.WriteError(details);
            CorrelationId = Empty;
            if (Configuration.WriteToConsole)
            {
                PioneerLogger.ConsoleLogger.Error("ERROR: " + message);
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
            CorrelationId = Empty;
            if (Configuration.WriteToConsole)
            {
                PioneerLogger.ConsoleLogger.Error("ERROR: " + message);
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
                Id = Guid.NewGuid(),
                ApplicationName = Configuration.ApplicationName,
                ApplicationLayer = Configuration.ApplicationLayer,
                Message = message,
                Username = Environment.UserName,
                Hostname = Environment.MachineName,
                CorrelationId = IsNullOrEmpty(CorrelationId) ? Guid.NewGuid().ToString() : CorrelationId,
                SystemGenerateCorrelationId = IsNullOrEmpty(CorrelationId),
                AdditionalInfo = additionalInfo ?? new Dictionary<string, object>(),
                CreationTimestamp = DateTime.UtcNow
            };

            return detail;
        }

        /// <summary>
        /// Create a new <see cref="PioneerLogsPerformanceTracker"/> object and by way, start the performance timer.
        /// </summary>
        /// <param name="message">Add message to persisted log</param>
        public static void StartPerformanceTracker(string message = null)
        {
            Tracker = new PioneerLogsPerformanceTracker(GetTubDetail(message));
            var st = new StackTrace();
            TrackerStartingMethodName = st.GetFrame(1).GetMethod().Name;
        }

        /// <summary>
        /// Stop the performance timer
        /// </summary>
        public static void StopPerformanceTracker()
        {
            var log = Tracker.Stop();
            Tracker = null;

            if (!Configuration.WriteToConsole)
            {
                return;
            }

            var st = new StackTrace();
            PioneerLogger.ConsoleLogger.Information(
                $"PERF: Started at {TrackerStartingMethodName} and ended in {st.GetFrame(1).GetMethod().Name} - {log.PerformanceElapsedMilliseconds} ms");

            TrackerStartingMethodName = string.Empty;
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
            Configuration.ApplicationLayer = builder.GetValue<string>("ApplicationLayer");
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
