﻿using System;
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
        public static PioneerLogsConfiguration Configuration { get; set; } = new PioneerLogsConfiguration();
        private static PioneerLogsPerformanceTracker Tracker { get; set; }
        private static string TrackerStartingMethodName { get; set; }

        /// <summary>
        /// Log usage
        /// </summary>
        /// <param name="message">Accompanying message.</param>
        /// <param name="additionalInfo">Dictionary of additional values.</param>
        public static void LogUsage(string message,
            Dictionary<string, object> additionalInfo = null)
        {
            if (Configuration.Usage.WriteToFile)
            {
                var details = GetTubDetail(message, additionalInfo);
                PioneerLogger.WriteUsage(details);
            }

            if (Configuration.Usage.WriteToConsole)
            {
                PioneerLogger.ConsoleLogger.Information("USAGE: " + message);
            }
        }

        /// <summary>
        /// Log diagnostics
        /// </summary>
        /// <param name="message">Accompanying message.</param>
        /// <param name="additionalInfo">Dictionary of additional values.</param>
        public static void LogDiagnostic(string message,
            Dictionary<string, object> additionalInfo = null)
        {
            if (Configuration.Diagnostics.WriteToFile)
            {
                var details = GetTubDetail(message, additionalInfo);
                PioneerLogger.WriteDiagnostic(details);
            }

            if (Configuration.Diagnostics.WriteToConsole)
            {
                PioneerLogger.ConsoleLogger.Information("DIAGNOSTIC: " + message);
            }
        }

        /// <summary>
        /// Log Errors message with exception
        /// Empties CorrelationId
        /// </summary>
        /// <param name="ex">Exception to log</param>
        public static void LogError(Exception ex)
        {
            if (Configuration.Errors.WriteToFile)
            {
                var details = GetTubDetail(null);
                details.Exception = ex;
                PioneerLogger.WriteError(details);
            }
   
            if (Configuration.Errors.WriteToConsole)
            {
                PioneerLogger.ConsoleLogger.Error("ERROR: " + ex);
            }

            CorrelationId = Empty;
        }

        /// <summary>
        /// Log Errors message with message string
        /// </summary>
        /// <param name="message">Accompanying message.</param>
        public static void LogError(string message)
        {
            if (Configuration.Errors.WriteToFile)
            {
                var details = GetTubDetail(message);
                PioneerLogger.WriteError(details);
            }

            if (Configuration.Errors.WriteToConsole)
            {
                PioneerLogger.ConsoleLogger.Error("ERROR: " + message);
            }

            CorrelationId = Empty;
        }

        /// <summary>
        /// Log Errors message with <see cref="Exception"/> and write to console with message
        /// </summary>
        /// <param name="ex">Exception thrown</param>
        /// <param name="message">Accompanying message.</param>
        public static void LogError(Exception ex, string message)
        {
            if (Configuration.Errors.WriteToFile)
            {
                var details = GetTubDetail(message);
                details.Exception = ex;
                PioneerLogger.WriteError(details);
            }

            if (Configuration.Errors.WriteToConsole)
            {
                PioneerLogger.ConsoleLogger.Error("ERROR: " + message);
            }

            CorrelationId = Empty;
        }

        /// <summary>
        /// Get as <see cref="PioneerLog"/> object pre-populated with details parsed
        /// from the .NET Core environment.
        /// </summary>
        /// <param name="message">Accompanying message.</param>
        /// <param name="additionalInfo">Dictionary of additional values.</param>
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
        /// <param name="message">Accompanying message.</param>
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

            if (Configuration.Performance.WriteToConsole)
            {
                var st = new StackTrace();
                PioneerLogger.ConsoleLogger.Information(
                    $"PERF: Started at {TrackerStartingMethodName} and ended in {st.GetFrame(1).GetMethod().Name} - {log.PerformanceElapsedMilliseconds} ms");
            }

            TrackerStartingMethodName = Empty;
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

            Configuration.Diagnostics = builder.GetValue<Diagnostics>("Diagnostics");
            Configuration.Errors = builder.GetValue<Errors>("Errors");
            Configuration.Usage = builder.GetValue<Usage>("Usage");
            Configuration.Performance = builder.GetValue<Performance>("Performance");

            return Configuration;
        }

        private static readonly UnhandledExceptionEventHandler UnhandledExceptionHandler = (s, args) =>
        {
            var ex = args.ExceptionObject as Exception;
            LogError(ex);
        };
    }
}
