using System.Diagnostics;
using Pioneer.Logs.Models;
using Serilog;
using Serilog.Events;
using PioneerLog = Pioneer.Logs.Models.PioneerLog;

namespace Pioneer.Logs
{
    /// <summary>
    /// Base Logger that sites on top of <see cref="Serilog"/>
    /// </summary>
    internal static class PioneerLogger
    {
        public static ILogger PerformanceLogger;
        public static ILogger UsageLogger;
        public static ILogger ErrorLogger;
        public static ILogger DiagnosticLogger;
        public static ILogger ConsoleLogger;

        /// <summary>
        /// Configure loggers
        /// </summary>
        static PioneerLogger()
        {
            const string outputTemplate = "{Message:l" + "j}{NewLine}";

            PerformanceLogger = new LoggerConfiguration()
                  .WriteTo.Async(a => a.File(path: @"logs\pioneer-logs-performance-.log", rollingInterval: RollingInterval.Day, outputTemplate: outputTemplate))
                  .CreateLogger();

            UsageLogger = new LoggerConfiguration()
                .WriteTo.Async(a => a.File(path: @"logs\pioneer-logs-usage-.log", rollingInterval: RollingInterval.Day, outputTemplate: outputTemplate))
                .CreateLogger();

            ErrorLogger = new LoggerConfiguration()
                .WriteTo.Async(a => a.File(path: @"logs\pioneer-logs-error-.log", rollingInterval: RollingInterval.Day, outputTemplate: outputTemplate))
                .CreateLogger();

            DiagnosticLogger = new LoggerConfiguration()
                .WriteTo.Async(a => a.File(path: @"logs\pioneer-logs-diagnostic-.log", rollingInterval: RollingInterval.Day, outputTemplate: outputTemplate))
                .CreateLogger();

            ConsoleLogger = new LoggerConfiguration()
                .MinimumLevel.Debug()
                .WriteTo.Console()
                .CreateLogger();

            Serilog.Debugging.SelfLog.Enable(msg => Debug.WriteLine(msg));
        }

        /// <summary>
        /// Do we have any slow areas?
        /// </summary>
        public static void WritePerf(PioneerLog infoToLog)
        {
            PerformanceLogger.Write(LogEventLevel.Information, "{@PioneerLog}", infoToLog);
        }

        /// <summary>
        /// What features are use most?
        /// </summary>
        public static void WriteUsage(PioneerLog infoToLog)
        {
            UsageLogger.Write(LogEventLevel.Information, "{@PioneerLog}", infoToLog);
        }

        /// <summary>
        /// Which errors occur the most?
        /// </summary>
        public static void WriteError(PioneerLog infoToLog)
        {
            ErrorLogger.Write(LogEventLevel.Information, "{@PioneerLog}", infoToLog);
        }

        /// <summary>
        /// Ad-hoc trouble shooting
        /// </summary>
        public static void WriteDiagnostic(PioneerLog infoToLog)
        {
            DiagnosticLogger.Write(LogEventLevel.Information, "{@PioneerLog}", infoToLog);
        }

        /// <summary>
        /// Resets <see cref="Serilog.Log.Logger"/> Serilog.Log.Logger to the default and disposes the original if possible
        /// </summary>
        public static void ClearLogger()
        {
            Log.CloseAndFlush();
        }

        /// <summary>
        /// Set overrides for <see cref="Serilog.ILogger"/> types if provided.
        /// </summary>
        /// <param name="configuration"><see cref="PioneerLogsTubConfiguration"/></param>
        public static void SetLoggers(PioneerLogsTubConfiguration configuration)
        {
            DiagnosticLogger = configuration.DiagnosticLogger ?? DiagnosticLogger;
            PerformanceLogger = configuration.PerformanceLogger ?? PerformanceLogger;
            UsageLogger = configuration.UsageLogger ?? UsageLogger;
            ErrorLogger = configuration.ErrorLogger ?? ErrorLogger;
            ConsoleLogger = configuration.ConsoleLogger ?? ConsoleLogger;
        }
    }
}
