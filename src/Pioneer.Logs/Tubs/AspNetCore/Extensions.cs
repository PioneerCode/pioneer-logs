using Microsoft.AspNetCore.Builder;

namespace Pioneer.Logs.Tubs.AspNetCore
{
    /// <summary>
    /// Extensions for Pioneer Logs
    /// </summary>
    public static class Extensions
    {
        /// <summary>
        /// Add Middleware to HTTP pipeline that captures and logs exceptions
        /// with the <see cref="Pioneer.Logs.Tubs.AspNetCore.Tub"/>
        /// </summary>
        /// <param name="app"><inheritdoc cref="IApplicationBuilder"/></param>
        /// <param name="applicationName">
        /// What application did this derive from?
        /// Sets the ApplicationName in <see cref="Models.Log"/> 
        /// </param>
        /// <param name="applicationLayer">
        /// What layer of the Application did this derive from?
        /// Sets the ApplicationLayer in <see cref="Models.Log"/> 
        /// </param>
        public static void UsePioneerLogs(this IApplicationBuilder app, 
            string applicationName, 
            string applicationLayer)
        {
            app.UseMiddleware<PioneerLogsMiddleware>(applicationName, applicationLayer);
        }
    }
}
