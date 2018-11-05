using Newtonsoft.Json;

namespace Pioneer.Logs.Tubs.AspNetCore
{
    /// <summary>
    /// Custom error response used to shield internal errors from client.
    /// </summary>
    public class PioneerLogsErrorResponse
    {
        /// <summary>
        /// Client should be able to supply this id as a
        /// tracer to other logs with deeper details
        /// - Browser: API error
        /// </summary>
        public string TraceId { get; set; }

        /// <summary>
        /// Custom message
        /// </summary>
        public string Message { get; set; }

        /// <summary>
        /// Serialize this to json string
        /// </summary>
        /// <returns>JSON string of "this".</returns>
        public override string ToString()
        {
            return JsonConvert.SerializeObject(this);
        }
    }
}
