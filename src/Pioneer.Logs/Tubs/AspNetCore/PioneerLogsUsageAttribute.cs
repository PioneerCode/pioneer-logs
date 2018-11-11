using Microsoft.AspNetCore.Mvc.Filters;
using System.Collections.Generic;
using Microsoft.Extensions.Options;
using Pioneer.Logs.Models;

namespace Pioneer.Logs.Tubs.AspNetCore
{
    /// <inheritdoc />
    /// <summary>
    /// Usage logging for MVC Projects
    /// </summary>
    public class PioneerLogsTrackUsageAttribute : ActionFilterAttribute
    {
        private readonly string _activityName;
        private readonly PioneerLogsConfiguration _configuration;

        /// <summary>
        /// Create usage tracking log. 
        /// </summary>
        /// <param name="activityName">Identifying name of activity we are tracking.</param>
        /// <param name="configuration">appsettings.json => PioneerLogsConfiguration</param>
        public PioneerLogsTrackUsageAttribute(string activityName, IOptions<PioneerLogsConfiguration> configuration)
        {
            _configuration = configuration.Value;
            _activityName = activityName;
        }

        public override void OnActionExecuted(ActionExecutedContext context)
        {
            var dict = new Dictionary<string, object>();
            if (context.RouteData.Values?.Keys != null)
            {
                foreach (var key in context.RouteData.Values?.Keys)
                {
                    dict.Add($"RouteData-{key}", (string)context.RouteData.Values[key]);
                }
            }

            PioneerLogsTub.LogUsage(_configuration.ApplicationName, _configuration.ApplicationLayer, _activityName, context.HttpContext, dict);
        }
    }
}
