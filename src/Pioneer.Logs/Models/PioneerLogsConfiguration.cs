﻿namespace Pioneer.Logs.Models
{
    public class PioneerLogsConfiguration
    {
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
    }
}
