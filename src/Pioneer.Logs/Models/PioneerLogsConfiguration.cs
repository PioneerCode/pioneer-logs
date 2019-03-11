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
        public string ApplicationName { get; set; } = "Pioneer Logs";

        /// <summary>
        /// What layer of the Application did this derive from?
        ///
        /// - Web: Name of ASP.NET Core application
        /// - API: Name of ASP.NET Core WebApi application
        /// - Service: Name of service associated with application
        /// </summary>
        public string ApplicationLayer { get; set; } = "Pioneer Logs Layer";

        public Diagnostics Diagnostics { get; set; } = new Diagnostics();
        public Usage Usage { get; set; } = new Usage();
        public Errors Errors { get; set; } = new Errors();
        public Performance Performance { get; set; } = new Performance();

        public object Create(object parent, object configContext, XmlNode section)
        {
            if (section.FirstChild.Attributes == null)
            {
                return this;
            }

            ApplicationName = section.FirstChild.Attributes["ApplicationName"].Value;
            ApplicationLayer = section.FirstChild.Attributes["ApplicationLayer"].Value;
            //Diagnostics = section.FirstChild.Attributes["Diagnostics"].Value;
            //Usage = section.FirstChild.Attributes["Usage"].Value;
            //Errors = section.FirstChild.Attributes["Errors"].Value;
            //Performance = section.FirstChild.Attributes["Performance"].Value;
            return this;
        }
    }

    public class Diagnostics
    {
        public bool WriteToConsole { get; set; } = true;
        public bool WriteToFile { get; set; } = false;
    }

    public class Usage
    {
        public bool WriteToConsole { get; set; } = true;
        public bool WriteToFile { get; set; } = false;
    }

    public class Performance
    {
        public bool WriteToConsole { get; set; } = true;
        public bool WriteToFile { get; set; } = false;
    }

    public class Errors
    {
        public bool WriteToConsole { get; set; } = true;
        public bool WriteToFile { get; set; } = true;
    }
}
