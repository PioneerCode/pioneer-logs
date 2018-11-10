﻿using System.Diagnostics;
using Serilog;
using Serilog.Events;
using PioneerLog = Pioneer.Logs.Models.PioneerLog;

namespace Pioneer.Logs
{
    /// <summary>
    /// Base Logger that sites on top of <see cref="Serilog"/>
    /// </summary>
    public static class PioneerLogger
    {
        public static ILogger PerformanceLogger;
        public static ILogger UsageLogger;
        public static ILogger ErrorLogger;
        public static ILogger DiagnosticLogger;

        static PioneerLogger()
        {
            PerformanceLogger = new LoggerConfiguration()
                  .WriteTo.File(path: @"logs\performance.txt", rollingInterval: RollingInterval.Day)
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
            Serilog.Log.CloseAndFlush();
        }
    }
}
