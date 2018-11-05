using System;
using System.Collections.Generic;

namespace Pioneer.Logs.Models
{
    public class PioneerLog
    {
        public PioneerLog()
        {
            AdditionalInfo = new Dictionary<string, object>();
        }

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
        /// Specific <see cref="Exception"/> message
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
        /// - Web: Logged in user id
        /// </summary>
        public string UserId { get; set; }

        /// <summary>
        /// Associated user
        ///
        /// Examples
        /// - Web: User name associated with id
        /// </summary>
        public string UserName { get; set; }

        /// <summary>
        /// Only for performance entries
        /// </summary>
        public long? PerformanceElapsedMilliseconds { get; set; }

        /// <summary>
        /// Optional category
        /// Typically something the is derived from the BL level
        /// </summary>
        public string Category { get; set; }

        /// <summary>
        /// Source <see cref="Exception"/> thrown
        /// </summary>
        public Exception Exception { get; set; }

        /// <summary>
        /// Exception tracing from server to client
        /// </summary>
        public string CorrelationId { get; set; }

        /// <summary>
        /// Key value for unique entries
        /// </summary>
        public Dictionary<string, object> AdditionalInfo { get; set; }
    }
}
