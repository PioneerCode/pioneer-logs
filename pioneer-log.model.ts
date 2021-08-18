/**
 * All fields defined directly at the root of the events
 */
export interface IPioneerLogBase {
  /**
   * Timestamp at moment log was created
   */
  '@timestamp': string | Date

  /**
   * Meta information about event
   */
  labels: IPioneerLogLabels;

  /**
   * Optimized log message for viewing
   */
  message: string;

  /**
   * List of keywords used to tag each event
   */
  tags?: Array<string>;
}

export interface IPioneerLog extends IPioneerLogBase {
  /**
   * Fields about the client side of a network connection, used with server
   */
  client: IPioneerLogClient

  /**
   * Fields describing the container that generated this event.
   */
  container?: IPioneerLogContainer;

  /**
   * Non Ecs
   */
  custom_info?: Record<string, object>;

  /**
   * Fields about error
   */
  error?: IPioneerLogError;

  /**
   * Fields breaking down the event details.
   */
  event?: IPioneerLogEvent;

  /**
   * A host is defined as a general computing instance.
   */
  host?: IPioneerLogHost;

  /**
   * Fields describing an HTTP request.
   */
  http?: IPioneerLogHttp;

  /**
   * Non ecs standard
   */
  kubernetes?: IPioneerLogKubernetes;

  /**
   * Details about the event’s logging mechanism.
   */
  log?: IPioneerLogLog;

  /**
   * Non ecs standard
   */
  performance?: IPioneerLogPerformance;

  /**
   * Non ecs standard
   */
  tracing?: IPioneerLogTracing;

  /**
   * Fields to describe the user relevant to the event.
   */
  user: IPioneerLogUser;

  /**
   * Fields to describe a browser user_agent string.
   */
  user_agent: IPioneerLogUserAgent;
}

/**
 * Structure meta information about event
 */
export interface IPioneerLogLabels {
  /**
   * What application did this derive from?
   */
  applicationName: string;

  /**
   * What layer of the Application did this derive from?
   */
  applicationLayer: string;

  /**
   * Where did this derive from in the Application Layer?
   */
  applicationLocation: string;
}

/**
 * Fields about the client side of a network connection, used with server.
 */
export interface IPioneerLogClient {
  /**
   * IP address of the client
   */
  ip: string;
}

/**
 * Fields describing the container that generated this event.
 */
export interface IPioneerLogContainer {
  /**
   * Unique container id.
   */
  id: string;
}

/**
 * Fields about errors of any kind.
 */
export interface IPioneerLogError {
  /**
   * Error code describing the error
   */
  code: string;

  /**
   * Uid of error
   */
  id: string;

  /**
   * Error message
   */
  message: string;

  /**
   * Stack trace
   */
  stack_trace: string;

  /**
   * Type of error, for example the class name of the exception.
   */
  type: string;
}

/**
 * Fields breaking down the event details.
 */
export interface IPioneerLogEvent {
  /**
   * Name of the dataset.
   *
   * If an event source publishes more than one type of log or events
   * (e.g. access log, error log), the dataset is used to specify which
   * one the event comes from.
   */
  dataset: string;
}

/**
 * Fields describing the relevant computing instance.
 */
export interface IPioneerLogHost {
  /**
   * Hostname of the host.
   * It normally contains what the hostname command returns on the host machine.
   */
  hostname: string;

  /**
   * Name of the host.
   */
  name: string;
}

/**
 * Fields describing an HTTP request.
 */
export interface IPioneerLogHttp {
  request: IPioneerLogHttpRequest;
  response: IPioneerLogHttpResponse;
}

export interface IPioneerLogHttpRequest {
  /**
   * Referrer for this HTTP request.
   */
  referrer: string;

  /**
   * Mime type of the body of the request.
   */
  mime_type: string;

  /**
   * HTTP request method.
   */
  method: string;

  body: IPioneerLogHttpRequestBody;
}

export interface IPioneerLogHttpRequestBody {
  /**
   * Size in bytes of the request body.
   */
  bytes?: number;

  /**
   * The full HTTP request body.
   */
  content: string;
}

export interface IPioneerLogHttpResponse {
  /**
   * HTTP response status code.
   */
  status_code: number;

  /**
   * Mime type of the body of the response.
   */
  mime_type: string;

  body: IPioneerLogHttpResponseBody;
}

export interface IPioneerLogHttpResponseBody {
  /**
   * Size in bytes of the response body.
   */
   bytes?: number;

   /**
    * The full HTTP response body.
    */
   content: string;
}

/**
 * Fields breaking Kubernetes if applicable.
 * Non ecs standard
 */
export interface IPioneerLogKubernetes {
  pod: IPioneerLogKubernetesPod;
}

/**
 * Fields breaking Kubernetes Pod if applicable.
 * Non ecs standard
 */
export interface IPioneerLogKubernetesPod {
  /**
   * Kubernetes Pod UID.
   */
  uid: string;
}

/**
 * Details about the event’s logging mechanism.
 */
export interface IPioneerLogLog {
  file: IPioneerLogLogFile
}

/**
 * Details about the event’s logging file
 */
export interface IPioneerLogLogFile {
  /**
   * Full path to the log file this event came from
   */
  path: string;
}

/**
 * Fields to describe the user relevant to the event.
 */
export interface IPioneerLogUser {
  /**
   * User email address.
   */
  email_address: string;

  /**
   * User full name.
   */
  full_name: string;

  /**
   * username
   */
  name: string;

  /**
   * Array of user roles at the time of the event
   */
  roles: Array<string>

  /**
   * Unique identifier of the user
   */
  id: string;
}

/**
 * Details about performance
 * Non ecs
 */
export interface IPioneerLogPerformance {
  elapsed_milliseconds: number;
}

/**
 * Fields related to distributed tracing.
 */
export interface IPioneerLogTracing {
  /**
   * A transaction is the highest level of work measured
   * within a service, such as a request to a server.
   */
  transaction: IPioneerLogTracingTransaction
}

/**
 * A transaction is the highest level of work measured
 * within a service, such as a request to a server.
 */
export interface IPioneerLogTracingTransaction {
  /**
   * Unique identifier of the transaction within
   * the scope of its trace.
   */
  id: string;
}

/**
 * Fields to describe a browser user_agent string.
 */
export interface IPioneerLogUserAgent {
  /**
   * Name of the user agent
   */
  name: string;

  /**
   * Unparsed user_agent string
   */
  original: string;

  /**
   * Version of the user agent
   */
  version: string;

  device: IPioneerLogUserAgentDevice
}

/**
 * Fields to describe a browser user_agent device.
 */
export interface IPioneerLogUserAgentDevice {
  /**
   * Name of device
   */
  name: string;
}

