using System.Diagnostics;
using Pioneer.Logs.Models;
using Serilog;
using Serilog.Events;

namespace Pioneer.Logs
{
    public static class Logger
    {
        private static readonly ILogger PerfLogger;
        private static readonly ILogger UsageLogger;
        private static readonly ILogger ErrorLogger;
        private static readonly ILogger DiagnosticLogger;

        static Logger()
        {
            PerfLogger = new LoggerConfiguration()
                  .WriteTo.File(path: @"logs\perf.txt", rollingInterval: RollingInterval.Day)
                  .CreateLogger();

            UsageLogger = new LoggerConfiguration()
                .WriteTo.File(path: @"logs\usage.txt", rollingInterval: RollingInterval.Day)
                .CreateLogger();

            ErrorLogger = new LoggerConfiguration()
                .WriteTo.File(path: @"logs\error.txt", rollingInterval: RollingInterval.Day)
                .CreateLogger();

            DiagnosticLogger = new LoggerConfiguration()
                .WriteTo.File(path: @"logs\diagnostic.txt", rollingInterval: RollingInterval.Day)
                .CreateLogger();

            Serilog.Debugging.SelfLog.Enable(msg => Debug.WriteLine(msg));
        }

        /// <summary>
        /// Do we have any slow areas?
        /// </summary>
        public static void WritePerf(LoggerDetail infoToLog)
        {
            PerfLogger.Write(LogEventLevel.Information, "{@LoggerDetial}", infoToLog);
        }

        /// <summary>
        /// What features are use most?
        /// </summary>
        public static void WriteUsage(LoggerDetail infoToLog)
        {
            UsageLogger.Write(LogEventLevel.Information, "{@LoggerDetial}", infoToLog);
        }

        /// <summary>
        /// Which errors occur the most?
        /// </summary>
        public static void WriteError(LoggerDetail infoToLog)
        {
            ErrorLogger.Write(LogEventLevel.Information, "{@LoggerDetial}", infoToLog);
        }

        /// <summary>
        /// Ad-hoc trouble shooting
        /// </summary>
        public static void WriteDiagnostic(LoggerDetail infoToLog)
        {
            DiagnosticLogger.Write(LogEventLevel.Information, "{@LoggerDetial}", infoToLog);
        }

        /// <summary>
        /// Resets <see cref="Serilog.Log.Logger"/> Serilog.Log.Logger to the default and disposes the original if possible
        /// </summary>
        public static void ClearLogger()
        {
            Log.CloseAndFlush();
        }
    }
}
