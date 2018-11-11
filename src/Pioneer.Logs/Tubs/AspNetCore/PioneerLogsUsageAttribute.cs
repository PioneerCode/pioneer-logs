using Microsoft.AspNetCore.Mvc.Filters;
using System.Collections.Generic;

namespace Pioneer.Logs.Tubs.AspNetCore
{
    /// <inheritdoc />
    /// <summary>
    /// Usage logging for MVC Projects
    /// </summary>
    public class PioneerLogsTrackUsageAttribute : ActionFilterAttribute
    {
        public string ActivityName { get; set; }

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

            PioneerLogsTub.LogUsage(ActivityName, context.HttpContext, dict);
        }
    }
}
