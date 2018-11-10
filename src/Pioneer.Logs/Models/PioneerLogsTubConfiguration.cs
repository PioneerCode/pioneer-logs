using Pioneer.Logs.Tubs.AspNetCore;
using Serilog;

namespace Pioneer.Logs.Models
{
    /// <summary>
    /// Configuration for <see cref="PioneerLogsTub"/> <see cref="ILogger"/> sinks.
    /// https://github.com/serilog/serilog/wiki/Provided-Sinks
    /// </summary>
    public class PioneerLogsTubConfiguration
    {
        public ILogger PerformanceLogger { get; set; }
        public ILogger UsageLogger { get; set; }
        public ILogger ErrorLogger { get; set; }
        public ILogger DiagnosticLogger { get; set; }
    }
}
