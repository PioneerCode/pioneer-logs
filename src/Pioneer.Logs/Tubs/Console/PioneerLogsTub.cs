using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Configuration;
using Pioneer.Logs.Models;

namespace Pioneer.Logs.Tubs.Console
{
    /// <summary>
    /// Windows Console app logging wrapper.
    /// .NET Proper
    /// </summary>
    public static class PioneerLogsTub
    {
        public static PioneerLogsConfiguration Configuration { get; set; }

        static PioneerLogsTub()
        {
            Configuration = new PioneerLogsConfiguration();
        }

        public static void LogUsage(string message,
            Dictionary<string, object> additionalInfo = null)
        {
            var details = GetTubDetail(message, additionalInfo);
            PioneerLogger.WriteUsage(details);
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
        }

        public static void LogError(Exception ex)
        {
            var details = GetTubDetail(null);
            details.Exception = ex;

            PioneerLogger.WriteError(details);
        }

        /// <summary>
        /// Get as <see cref="PioneerLog"/> object pre-populated with details parsed
        /// from the .NET Core environment.
        /// </summary>
        public static PioneerLog GetTubDetail(string message,
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
        /// Create a new <see cref="PioneerLogsPerformanceTracker"/> object and by way, start the performance timer
        /// </summary>
        /// <param name="message">Message to log</param>
        /// <returns>PioneerLogsPerformanceTracker object</returns>
        public static PioneerLogsPerformanceTracker StartLogsPerformanceTracker(string message)
        {
            return new PioneerLogsPerformanceTracker(GetTubDetail(message));
        }

        /// <summary>
        ///  Subscribes to <see cref="AppDomain.UnhandledException"/> and log global scope exceptions accordingly.
        /// </summary>
        /// <param name="configuration">Entry to override Sinks</param>
        public static void RegisterLogger(PioneerLogsTubConfiguration configuration)
        {
            PioneerLogger.SetLoggers(configuration);
            RegisterLogger();
        }

        /// <summary>
        /// Subscribes to <see cref="AppDomain.UnhandledException"/> and log global scope exceptions accordingly.
        /// </summary>
        public static void RegisterLogger()
        {
            AppDomain.CurrentDomain.UnhandledException -= UnhandledExceptionHandler;
            AppDomain.CurrentDomain.UnhandledException += UnhandledExceptionHandler;

            var settingCollection =
                (NameValueCollection)ConfigurationManager.GetSection("PioneerLogsConfiguration");
            Configuration.ApplicationName = settingCollection["ApplicationName"];
            Configuration.ApplicationLayer = settingCollection["ApplicationLayer"];
            Configuration.WriteDiagnostics = Convert.ToBoolean(settingCollection["WriteDiagnostics"]);
        }

        private static readonly UnhandledExceptionEventHandler UnhandledExceptionHandler = (s, args) =>
        {
            var ex = args.ExceptionObject as Exception;
            if (ex != null)
            {
                LogError((Exception)args.ExceptionObject);
            }
        };
    }
}
