using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Pioneer.Logs.Tubs.AspNetCore
{
    public class PioneerLogsPerformanceFilter : IActionFilter
    {
        private PioneerLogsPerformanceTracker _tracker;

        public void OnActionExecuting(ActionExecutingContext context)
        {
            var request = context.HttpContext.Request;
            var message = $"{request.Path}-{request.Method}";

            var dict = new Dictionary<string, object>();
            if (context.RouteData.Values?.Keys != null)
            {
                foreach (var key in context.RouteData.Values?.Keys)
                {
                    dict.Add($"RouteData-{key}", (string) context.RouteData.Values[key]);
                }
            }

            var details = PioneerLogsTub.GetTubDetail(message, context.HttpContext, dict);

            _tracker = new PioneerLogsPerformanceTracker(details);
        }

        public void OnActionExecuted(ActionExecutedContext context)
        {
            _tracker?.Stop();
        }
    }
}
