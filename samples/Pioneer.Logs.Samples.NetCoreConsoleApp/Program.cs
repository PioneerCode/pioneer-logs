﻿using System;
using Pioneer.Logs.Models;
using Pioneer.Logs.Tubs.NetCoreConsole;

namespace Pioneer.Logs.Samples.NetCoreConsoleApp
{
    internal class Program
    {
        private static PioneerLogsConfiguration _config;

        private static void Main(string[] args)
        {
            _config = PioneerLogsTub.RegisterLogger();

            BusinessLogicLoop( Guid.NewGuid());

            Console.ReadLine();
            throw new Exception("Global exception test");
        }

        private static void BusinessLogicLoop(Guid id)
        {
            PioneerLogsTub.CorrelationId = id.ToString();
            RunPerformanceLoggingTask();
            RunUsageLoggingTask();
            RunDiagnosticLoggingTask();
            PioneerLogsTub.LogError("Oh NO!!!");
        }

        private static void RunPerformanceLoggingTask()
        {
            PioneerLogsTub.StartPerformanceTracker();
            System.Threading.Thread.Sleep(1000);
            PioneerLogsTub.StopPerformanceTracker();
        }

        private static void RunUsageLoggingTask()
        {
            PioneerLogsTub.LogUsage("RunUsageLoggingTask");
        }

        private static void RunDiagnosticLoggingTask()
        {
            PioneerLogsTub.LogDiagnostic("Some Random Message.");
        }
    }
}
