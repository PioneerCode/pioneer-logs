using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using Pioneer.Logs.Models;

namespace Pioneer.Logs
{
    /// <summary>
    /// Shared performance tracker
    /// </summary>
    public class PioneerLogsPerformanceTracker
    {
        private readonly Stopwatch _sw;
        private readonly PioneerLog _log;

        public PioneerLogsPerformanceTracker(PioneerLog details)
        {
            _sw = Stopwatch.StartNew();
            _log = details;

            var beginTime = DateTime.Now;
            if (_log.AdditionalInfo == null)
            {
                _log.AdditionalInfo = new Dictionary<string, object>
                {
                    { "Started", beginTime.ToString(CultureInfo.InvariantCulture) }
                };
            }
            else
            {
                _log.AdditionalInfo.Add("Started", beginTime.ToString(CultureInfo.InvariantCulture));
            }
        }

        public PioneerLog Stop()
        {
            _sw.Stop();
            _log.PerformanceElapsedMilliseconds = _sw.ElapsedMilliseconds;
            PioneerLogger.WritePerf(_log);
            return _log;
        }
    }
}
