using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace Pioneer.Logs.Models 
{
    /// <summary>
    /// All fields defined directly at the root of the events
    /// </summary>
    public class PioneerLogEcsBase
    {
        /// <summary>
        /// Timestamp at moment log was created
        /// </summary>
        [JsonProperty(PropertyName = "@timestamp")]
        public DateTime Timestamp { get; set; }

        /// <summary>
        /// Optimized log message for viewing
        /// </summary>
        public string Message { get; set; }

        /// <summary>
        /// Meta information about event
        /// </summary>
        public PioneerLogLabels Labels { get; set; } = new PioneerLogLabels();

        /// <summary>
        /// List of keywords used to tag each event
        /// </summary>
        public string[] Tags { get; set; }
    }

    public class PioneerLogEcs : PioneerLogEcsBase
    {
        /// <summary>
        /// Fields about the client side of a network connection, used with server
        /// </summary>
        public PioneerLogCient Client { get; set; } = new PioneerLogCient();

        /// <summary>
        /// Fields about the client side of a network connection, used with server.
        /// </summary>
        public PioneerLogContainer Container { get; set; } = new PioneerLogContainer();

        /// <summary>
        /// Fields about error
        /// </summary>
        public PioneerLogError Error { get; set; } = new PioneerLogError();

        /// <summary>
        /// Fields breaking down the event details.
        /// </summary>
        public PioneerLogEvent Event { get; set; } = new PioneerLogEvent();

        /// <summary>
        /// A host is defined as a general computing instance.
        /// </summary>
        public PioneerLogHost Host { get; set; } = new PioneerLogHost();

        /// <summary>
        /// Non ecs standard
        /// </summary>
        public PioneerLogKubernetes Kubernetes { get; set; } = new PioneerLogKubernetes();

        /// <summary>
        /// Details about the event’s logging mechanism.
        /// </summary>
        public PioneerLogLog Log { get; set; } = new PioneerLogLog();

        /// <summary>
        /// Fields related to distributed tracing.
        /// </summary>
        public PioneerLogTracing Tracing { get; set; } = new PioneerLogTracing();

        /// <summary>
        /// Fields to describe the user relevant to the event. 
        /// </summary>
        public PioneerLogUser User { get; set; } = new PioneerLogUser();

        /// <summary>
        /// Non ecs standard
        /// </summary>
        [JsonProperty(PropertyName = "custom_info")]
        public Dictionary<string, object> CustomInfo { get; set; } = new Dictionary<string, object>();

        /// <summary>
        /// Fields to describe a browser user_agent string.
        /// </summary>
        [JsonProperty(PropertyName = "user_agent")]
        public PioneerLogUserAgent UserAgent { get; set; } = new PioneerLogUserAgent();
    }

    /// <summary>
    /// Structure meta information about event
    /// </summary>
    public class PioneerLogLabels
    {
        /// <summary>
        /// What application did this derive from?
        /// </summary>
        [JsonProperty(PropertyName = "application_name")]
        public string ApplicationName { get; set; }

        /// <summary>
        /// What layer of the Application did this derive from?
        /// </summary>
        [JsonProperty(PropertyName = "application_layer")]
        public string ApplicationLayer { get; set; }

        /// <summary>
        /// Where did this derive from in the Application Layer
        /// </summary>
        [JsonProperty(PropertyName = "application_location")]
        public string ApplicatoinLocation { get; set; }
    }

    /// <summary>
    /// Fields about the client side of a network connection, used with server.
    /// </summary>
    public class PioneerLogCient
    {
        /// <summary>
        /// IP address of the client
        /// </summary>
        public string Ip { get; set; }
    }

    /// <summary>
    /// Fields describing the container that generated this event.
    /// </summary>
    public class PioneerLogContainer
    {
        /// <summary>
        /// Unique container id.
        /// </summary>
        public string Id { get; set; }
    }

    /// <summary>
    /// Fields about errors of any kind.
    /// </summary>
    public class PioneerLogError
    {
        /// <summary>
        /// Error code describing the error
        /// </summary>
        public string Code { get; set; }

        /// <summary>
        /// Uid of error
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// Error message
        /// </summary>
        public string Message { get; set; }

        /// <summary>
        /// Stack trace
        /// </summary>
        [JsonProperty(PropertyName = "stack_trace")]
        public string StackTrace { get; set; }

        /// <summary>
        /// Type of error, for example the class name of the exception.
        /// </summary>
        public string Type { get; set; }
    }

    /// <summary>
    /// Fields breaking down the event details.
    /// </summary>
    public class PioneerLogEvent
    {
        /// <summary>
        /// Name of the dataset
        /// 
        /// If an event source publishes more than one type of log or events
        /// (e.g. access log, error log), the dataset is used to specify which
        /// one the event comes from.
        /// </summary>
        public string Dataset { get; set; }
    }

    /// <summary>
    /// Fields describing the relevant computing instance.
    /// </summary>
    public class PioneerLogHost
    {
        /// <summary>
        /// Hostname of the host.
        /// It normally contains what the hostname command returns on the host machine.
        /// </summary>
        public string Hostname { get; set; }

        /// <summary>
        /// Name of the host.
        /// </summary>
        public string Host { get; set; }
    }

    /// <summary>
    /// Fields breaking Kubernetes Pod if applicable.
    /// Non ecs standard
    /// </summary>
    public class PioneerLogKubernetes
    {
        public PioneerLogKubernetesPod Pod { get; set; }
    }

    /// <summary>
    /// Fields breaking Kubernetes Pod if applicable.
    /// Non ecs standard
    /// </summary>
    public class PioneerLogKubernetesPod
    {
        /// <summary>
        /// Kubernetes Pod UID.
        /// </summary>
        public string Uid { get; set; }
    }

    /// <summary>
    /// Details about the event’s logging mechanism.
    /// </summary>
    public class PioneerLogLog
    {
        public PioneerLogLogFile File { get; set; }
    }

    /// <summary>
    /// Details about the event’s logging file.
    /// </summary>
    public class PioneerLogLogFile
    {
        /// <summary>
        /// Full path to the log file this event came from
        /// </summary>
        public string Path { get; set; }
    }

    /// <summary>
    /// Fields related to distributed tracing.
    /// </summary>
    public class PioneerLogTracing
    {
        /// <summary>
        /// A transaction is the highest level of work measured within a service, such as a request to a server.
        /// </summary>
        public PioneerLogTracingTransaction Transaction { get; set; } = new PioneerLogTracingTransaction();
    }

    /// <summary>
    /// A transaction is the highest level of work measured within a service, such as a request to a server.
    /// </summary>
    public class PioneerLogTracingTransaction
    {
        /// <summary>
        /// Unique identifier of the transaction within the scope of its trace.
        /// </summary>
        public string Id { get; set;}
    }

    /// <summary>
    /// Fields to describe the user relevant to the event.
    /// </summary>
    public class PioneerLogUser
    {
        /// <summary>
        /// User email address.
        /// </summary>
        [JsonProperty(PropertyName = "email_address")]
        public string EmailAddress { get; set; }

        /// <summary>
        /// User full name.
        /// </summary>
        [JsonProperty(PropertyName = "full_name")]
        public string FullName { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// username
        /// </summary>
        public string[] Roles { get; set; }

        /// <summary>
        /// Array of user roles at the time of the event
        /// </summary>
        public string Id { get; set; }
    }

    /// <summary>
    /// Fields to describe a browser user_agent string
    /// </summary>
    public class PioneerLogUserAgent
    {
        /// <summary>
        /// Name of the device
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Unparsed user_agent string
        /// </summary>
        public string Original { get; set; }

        /// <summary>
        /// Version of the user agent
        /// </summary>
        public string Version { get; set; }

        public PionerLogUserAgentDevice Device { get; set; } = new PionerLogUserAgentDevice();
    }

    /// <summary>
    /// Fields to describe a browser user_agent device.
    /// </summary>
    public class PionerLogUserAgentDevice
    {
        /// <summary>
        /// Name of device
        /// </summary>
        public string Name { get; set; }
    }

}
