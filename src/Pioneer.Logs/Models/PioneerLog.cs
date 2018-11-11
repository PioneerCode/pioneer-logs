using System;
using System.Collections.Generic;

namespace Pioneer.Logs.Models
{
    /// <summary>
    /// Base class for all logging objects
    /// </summary>
    public class PioneerLog
    {
        public PioneerLog()
        {
            AdditionalInfo = new Dictionary<string, object>();
        }

        /// <summary>
        /// Internal UID if needed
        /// </summary>
        public Guid? Id { get; set; }

        /// <summary>
        /// Timestamp at moment log was created
        /// </summary>
        public DateTime CreationTimestamp { get; set; }

        /// <summary>
        /// What application did this derive from?
        /// </summary>
        public string ApplicationName { get; set; }

        /// <summary>
        /// What layer of the Application did this derive from?
        ///
        /// - Web: Name of ASP.NET Core application
        /// - API: Name of ASP.NET Core WebApi application
        /// - Service: Name of service associated with application
        /// </summary>
        public string ApplicationLayer { get; set; }

        /// <summary>
        /// Where did this derive from in the Application Layer>
        /// - Web: What route did it derive from?
        /// - API: What endpoint did it derive from?
        /// - Service: What class.function did it derive from?
        /// </summary>
        public string ApplicationLocation { get; set; }

        /// <summary>
        /// User supplied message
        /// </summary>
        public string Message { get; set; }

        /// <summary>
        /// Hostname: machine name.
        /// Typically can be grabbed from Enironment.Hostname
        /// </summary>
        public string Hostname { get; set; }

        /// <summary>
        /// Associated user ID
        ///
        /// Examples
        /// - Web: Logged in user ID
        /// - Service: Instance ID
        /// </summary>
        public string UserId { get; set; }

        /// <summary>
        /// Associated user
        ///
        /// Examples
        /// - Web: User name associated with id
        /// - Service: Instance Name
        /// </summary>
        public string UserName { get; set; }

        /// <summary>
        /// Elapsed Milliseconds of performance tracking
        /// </summary>
        public long? PerformanceElapsedMilliseconds { get; set; }

        /// <summary>
        /// Optional category
        /// Typically something the is derived from the BL level.
        /// Could be a log-level if necessary.
        /// </summary>
        public string Category { get; set; }

        /// <summary>
        /// Source <see cref="Exception"/> thrown.
        /// </summary>
        public Exception Exception { get; set; }

        /// <summary>
        /// Lowest message in Inner <see cref="Exception"/> hierarchy.
        /// </summary>
        public string ExceptionMessage => GetMessageFromException(Exception);

        /// <summary>
        /// When dealing with a multi-layer system,...
        /// - Browser => API
        /// - Service => Service
        /// ...use this to trace a "story" through that system
        /// </summary>
        public string CorrelationId { get; set; }

        /// <summary>
        /// Key value for unique entries
        /// </summary>
        public Dictionary<string, object> AdditionalInfo { get; set; }

        /// <summary>
        /// Get lowest message in Inner <see cref="Exception"/> hierarchy.
        /// </summary>
        private static string GetMessageFromException(Exception ex)
        {
            if (ex != null)
            {
                return ex.InnerException != null ? GetMessageFromException(ex.InnerException) : ex.Message;
            }

            return string.Empty;
        }
    }
}
