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
        private readonly PioneerLogEcs _logEcs;

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

        public PioneerLogsPerformanceTracker(PioneerLogEcs details)
        {
            _sw = Stopwatch.StartNew();
            _logEcs = details;

            var beginTime = DateTime.Now;
            if (_logEcs.CustomInfo == null)
            {
                _logEcs.CustomInfo = new Dictionary<string, object>
                {
                    { "Started", beginTime.ToString(CultureInfo.InvariantCulture) }
                };
            }
            else
            {
                _logEcs.CustomInfo.Add("Started", beginTime.ToString(CultureInfo.InvariantCulture));
            }
        }

        public PioneerLog Stop(bool logToFile = false)
        {
            _sw.Stop();
            _log.PerformanceElapsedMilliseconds = _sw.ElapsedMilliseconds;
            if (logToFile)
            {
                PioneerLogger.WritePerf(_log);
            }
            return _log;
        }

        public PioneerLogEcs StopEcs(bool logToFile = false)
        {
            _sw.Stop();
            _logEcs.Performance.ElapsedMilliseconds = _sw.ElapsedMilliseconds;
            if (logToFile)
            {
                PioneerLogger.WritePerf(_logEcs);
            }
            return _logEcs;
        }
    }
}
