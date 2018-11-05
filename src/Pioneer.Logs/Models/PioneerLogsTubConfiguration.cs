using System;
using System.Collections.Generic;
using System.Text;
using Serilog;

namespace Pioneer.Logs.Models
{
    public class PioneerLogsTubConfiguration
    {
        public ILogger PerforamnceLogger { get; set; }
        public ILogger UsageLogger { get; set; }
        public ILogger ErrorLogger { get; set; }
        public ILogger DiagnosticLogger { get; set; }
    }
}
