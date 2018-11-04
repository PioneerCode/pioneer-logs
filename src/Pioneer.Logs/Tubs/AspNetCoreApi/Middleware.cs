using System;
using Microsoft.AspNetCore.Builder;

namespace Pioneer.Logs.Tubs.AspNetCoreApi
{
    public static class Middleware
    {
        public static IApplicationBuilder UsePioneerLogs(this IApplicationBuilder builder)
        {
            return builder;
        }
    }
}
