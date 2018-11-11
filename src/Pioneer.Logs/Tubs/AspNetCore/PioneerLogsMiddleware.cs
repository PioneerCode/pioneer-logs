using System;
using System.Diagnostics;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Pioneer.Logs.Models;

namespace Pioneer.Logs.Tubs.AspNetCore
{
    /// <summary>
    /// ASP.NET Core Middleware used to handle <see cref="Exception"/>'s at a global level.
    /// </summary>
    public class PioneerLogsMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly PioneerLogsConfiguration _configuration;

        public PioneerLogsMiddleware(RequestDelegate next, IOptions<PioneerLogsConfiguration> configuration)
        {
            _configuration = configuration.Value;
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
                await HandleExceptionAsync(httpContext, ex);
            }
        }

        private async Task HandleExceptionAsync(HttpContext context, Exception ex)
        {
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;

            PioneerLogsTub.LogError(ex, context);

            var errorId = Activity.Current?.Id ?? context.TraceIdentifier;
            var jsonResponse = JsonConvert.SerializeObject(new PioneerLogsErrorResponse
            {
                TraceId = errorId,
                Message = "Internal server error."
            });

           await context.Response.WriteAsync(jsonResponse, Encoding.UTF8);
        }
    }
}
