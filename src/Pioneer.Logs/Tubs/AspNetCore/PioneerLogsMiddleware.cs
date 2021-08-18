using System;
using System.Diagnostics;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;

namespace Pioneer.Logs.Tubs.AspNetCore
{
    /// <summary>
    /// ASP.NET Core Middleware used to handle <see cref="Exception"/>'s at a global level.
    /// </summary>
    public class PioneerLogsMiddleware
    {
        private readonly RequestDelegate _next;

        public PioneerLogsMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext httpContext)
        {
            try
            {
                await _next(httpContext);
            }
            catch (Exception ex)
            {
                await Invoke(httpContext, ex);
            }
        }

        private static async Task Invoke(HttpContext context, Exception ex)
        {
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;

            PioneerLogsTub.LogError(ex, context);

            var errorId = Activity.Current?.Id ?? context.TraceIdentifier;
            var jsonResponse = JsonConvert.SerializeObject(new PioneerLogsErrorResponse
            {
                CorrelationId = errorId,
                Message = "Internal server error."
            });

           await context.Response.WriteAsync(jsonResponse, Encoding.UTF8);
        }
    }
}
