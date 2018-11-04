using System.Diagnostics;
using System.Text;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;

namespace Pioneer.Logs.Tubs.AspNetCoreApi
{
    public static class Extensions
    {

        public static void AddGlobalExceptionHandler(this IApplicationBuilder app)
        {
            app.UseExceptionHandler(eApp =>
            {
                eApp.Run(async context =>
                {
                    context.Response.StatusCode = 500;
                    context.Response.ContentType = "application/json";

                    var errorCtx = context.Features.Get<IExceptionHandlerFeature>();
                    if (errorCtx != null)
                    {
                        var ex = errorCtx.Error;
                        Tub.LogWebError("DriveSage", "Ds.Lpr.Api", ex, context);

                        var errorId = Activity.Current?.Id ?? context.TraceIdentifier;
                        var jsonResponse = JsonConvert.SerializeObject(new ErrorResponse
                        {
                            ErrorId = errorId,
                            Message = "Internal server error."
                        });
                        await context.Response.WriteAsync(jsonResponse, Encoding.UTF8);
                    }
                });
            });
        }
    }
}
