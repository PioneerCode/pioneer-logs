using System;
using System.Text.Json.Serialization;
using System.Collections.Generic;

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
        [JsonPropertyName("@timestamp")]
        public DateTime Timestamp { get; set; }

        /// <summary>
        /// Optimized log message for viewing
        /// </summary>
        [JsonPropertyName("message")]
        public string Message { get; set; }

        /// <summary>
        /// Meta information about event
        /// </summary>
        [JsonPropertyName("labels")]
        public PioneerLogLabels Labels { get; set; } = new PioneerLogLabels();

        /// <summary>
        /// List of keywords used to tag each event
        /// </summary>
        [JsonPropertyName("tags")]
        public string[] Tags { get; set; }
    }

    public class PioneerLogEcs : PioneerLogEcsBase
    {
        /// <summary>
        /// Fields about the client side of a network connection, used with server
        /// </summary>
        [JsonPropertyName("client")]
        public PioneerLogClient Client { get; set; } = new PioneerLogClient();

        /// <summary>
        /// Fields about the client side of a network connection, used with server.
        /// </summary>
        [JsonPropertyName("container")]
        public PioneerLogContainer Container { get; set; } = new PioneerLogContainer();

        /// <summary>
        /// Non ecs standard
        /// </summary>
        [JsonPropertyName("custom_info")]
        public Dictionary<string, object> CustomInfo { get; set; } = new Dictionary<string, object>();

        /// <summary>
        /// Fields about error
        /// </summary>
        [JsonPropertyName("error")]
        public PioneerLogError Error { get; set; } = new PioneerLogError();

        /// <summary>
        /// Fields breaking down the event details.
        /// </summary>
        [JsonPropertyName("event")]
        public PioneerLogEvent Event { get; set; } = new PioneerLogEvent();

        /// <summary>
        /// A host is defined as a general computing instance.
        /// </summary>
        [JsonPropertyName("host")]
        public PioneerLogHost Host { get; set; } = new PioneerLogHost();

        /// <summary>
        /// Non ecs standard
        /// </summary>
        [JsonPropertyName("kubernetes")]
        public PioneerLogKubernetes Kubernetes { get; set; } = new PioneerLogKubernetes();

        /// <summary>
        /// Details about the event’s logging mechanism.
        /// </summary>
        [JsonPropertyName("log")]
        public PioneerLogLog Log { get; set; } = new PioneerLogLog();

        /// <summary>
        /// Non ecs standard
        /// </summary>
        [JsonPropertyName("performance")]
        public PioneerLogPerformance Performance { get; set; } = new PioneerLogPerformance();

        /// <summary>
        /// Fields related to distributed tracing.
        /// </summary>
        [JsonPropertyName("tracing")]
        public PioneerLogTracing Tracing { get; set; } = new PioneerLogTracing();

        /// <summary>
        /// Fields to describe the user relevant to the event. 
        /// </summary>
        [JsonPropertyName("user")]
        public PioneerLogUser User { get; set; } = new PioneerLogUser();

        /// <summary>
        /// Fields to describe a browser user_agent string.
        /// </summary>
        [JsonPropertyName("user_agent")]
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
        [JsonPropertyName("application_name")]
        public string ApplicationName { get; set; }

        /// <summary>
        /// What layer of the Application did this derive from?
        /// </summary>
        [JsonPropertyName("application_layer")]
        public string ApplicationLayer { get; set; }

        /// <summary>
        /// Where did this derive from in the Application Layer
        /// </summary>
        [JsonPropertyName("application_location")]
        public string ApplicatoinLocation { get; set; }
    }

    /// <summary>
    /// Fields about the client side of a network connection, used with server.
    /// </summary>
    public class PioneerLogClient
    {
        /// <summary>
        /// IP address of the client
        /// </summary>
        [JsonPropertyName("ip")]
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
        [JsonPropertyName("id")]
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
        [JsonPropertyName("code")]
        public string Code { get; set; }

        /// <summary>
        /// Uid of error
        /// </summary>
        [JsonPropertyName("id")]
        public string Id { get; set; }

        /// <summary>
        /// Error message
        /// </summary>
        [JsonPropertyName("message")]
        public string Message { get; set; }

        /// <summary>
        /// Stack trace
        /// </summary>
        [JsonPropertyName("stack_trace")]
        public string StackTrace { get; set; }

        /// <summary>
        /// Type of error, for example the class name of the exception.
        /// </summary>
        [JsonPropertyName("type")]
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
        [JsonPropertyName("dataset")]
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
        [JsonPropertyName("hostname")]
        public string Hostname { get; set; }

        /// <summary>
        /// Name of the host.
        /// </summary>
        [JsonPropertyName("host")]
        public string Host { get; set; }
    }

    /// <summary>
    /// Fields breaking Kubernetes Pod if applicable.
    /// Non ecs standard
    /// </summary>
    public class PioneerLogKubernetes
    {
        [JsonPropertyName("pod")]
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
        [JsonPropertyName("uid")]
        public string Uid { get; set; }
    }

    /// <summary>
    /// Details about the event’s logging mechanism.
    /// </summary>
    public class PioneerLogLog
    {
        [JsonPropertyName("file")]
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
        [JsonPropertyName("path")]
        public string Path { get; set; }
    }

    /// <summary>
    /// Details about performance
    /// </summary>
    public class PioneerLogPerformance
    {
        /// <summary>
        /// Elapsed Milliseconds of performance tracking
        /// </summary>
        [JsonPropertyName("elapsed_milliseconds")]
        public long ElapsedMilliseconds { get; set; }
    }

    /// <summary>
    /// Fields related to distributed tracing.
    /// </summary>
    public class PioneerLogTracing
    {
        /// <summary>
        /// A transaction is the highest level of work measured within a service, such as a request to a server.
        /// </summary>
        [JsonPropertyName("transaction")]
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
        [JsonPropertyName("id")]
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
        [JsonPropertyName("email_address")]
        public string EmailAddress { get; set; }

        /// <summary>
        /// User full name.
        /// </summary>
        [JsonPropertyName("full_name")]
        public string FullName { get; set; }

        /// <summary>
        /// username
        /// </summary>
        [JsonPropertyName("name")]
        public string Name { get; set; }

        /// <summary>
        /// List of user roles
        /// </summary>
        [JsonPropertyName("roles")]
        public string[] Roles { get; set; }

        /// <summary>
        /// User id
        /// </summary>
        [JsonPropertyName("id")]
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
        [JsonPropertyName("name")]
        public string Name { get; set; }

        /// <summary>
        /// Unparsed user_agent string
        /// </summary>
        [JsonPropertyName("original")]
        public string Original { get; set; }

        /// <summary>
        /// Version of the user agent
        /// </summary>
        [JsonPropertyName("version")]
        public string Version { get; set; }

        [JsonPropertyName("device")]
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
        [JsonPropertyName("name")]
        public string Name { get; set; }
    }

}
