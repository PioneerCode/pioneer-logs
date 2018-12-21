using System.Configuration;
using System.Xml;

namespace Pioneer.Logs.Models
{
    /// <inheritdoc />
    /// <summary>
    /// Pioneer Log specific configuration
    /// </summary>
    public class PioneerLogsConfiguration : IConfigurationSectionHandler
    {
        /// <summary>
        /// What application did this derive from?
        /// </summary>
        [ConfigurationProperty("ApplicationName", IsRequired = true)]
        public string ApplicationName { get; set; }

        /// <summary>
        /// What layer of the Application did this derive from?
        ///
        /// - Web: Name of ASP.NET Core application
        /// - API: Name of ASP.NET Core WebApi application
        /// - Service: Name of service associated with application
        /// </summary>
        [ConfigurationProperty("ApplicationLayer", IsRequired = true)]
        public string ApplicationLayer { get; set; }

        /// <summary>
        /// Enable or disable diagnostic logging. 
        /// </summary>
        [ConfigurationProperty("WriteDiagnostics", IsRequired = true)]
        public bool WriteDiagnostics { get; set; }

        public PioneerLogsConfiguration()
        {
            WriteDiagnostics = false;
            ApplicationName = "Pioneer Logs";
            ApplicationLayer = "Pioneer Logs Console";
        }

        public object Create(object parent, object configContext, XmlNode section)
        {
            if (section.FirstChild.Attributes == null)
            {
                return this;
            }

            ApplicationName = section.FirstChild.Attributes["ApplicationName"].Value;
            ApplicationLayer = section.FirstChild.Attributes["ApplicationLayer"].Value;
            WriteDiagnostics = section.FirstChild.Attributes["WriteDiagnostics"].Value.ToLower() == "true";
            return this;
        }
    }
}
