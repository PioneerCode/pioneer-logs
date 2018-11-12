using System;
using Pioneer.Logs.Tubs.Console;

namespace Pioneer.Logs.Samples.Console
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            PioneerLogsTub.RegisterLogger();

            RunPerformanceLoggingTask();
            RunUsageLoggingTask();

            throw new Exception("Global exception test");
        }

        private static void RunPerformanceLoggingTask()
        {
            var tracker = PioneerLogsTub.StartLogsPerformanceTracker("RunPerformanceLoggingTask performance test");
            System.Threading.Thread.Sleep(1000);
            tracker.Stop();
        }

        private static void RunUsageLoggingTask()
        {
            PioneerLogsTub.LogUsage("RunUsageLoggingTask usage test.");
        }
    }
}
