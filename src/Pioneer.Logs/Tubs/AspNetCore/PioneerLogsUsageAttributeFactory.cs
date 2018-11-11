using System;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.DependencyInjection;

namespace Pioneer.Logs.Tubs.AspNetCore
{
    /// <inheritdoc cref="ActionFilterAttribute" />
    /// <summary>
    /// Usage logging for ASP.NET Core projects
    /// </summary>
    public class PioneerLogsTrackUsageFactoryAttribute : ActionFilterAttribute, IFilterFactory
    {
        public string ActivityName { get; set; }

        public bool IsReusable => false;

        public IFilterMetadata CreateInstance(IServiceProvider serviceProvider)
        {
            var filter = serviceProvider.GetService<PioneerLogsTrackUsageAttribute>();
            filter.ActivityName = ActivityName;
            return filter;
        }
    }
}
