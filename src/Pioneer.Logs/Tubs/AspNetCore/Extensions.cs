using System.Diagnostics;
using System.Text;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;

namespace Pioneer.Logs.Tubs.AspNetCore
{
    /// <summary>
    /// Extensions for Pioneer Logs
    /// </summary>
    public static class Extensions
    {
        public static void UsePioneerLogs(this IApplicationBuilder app)
        {
            app.UseMiddleware<PioneerLogsMiddleware>();
        }
    }
}
