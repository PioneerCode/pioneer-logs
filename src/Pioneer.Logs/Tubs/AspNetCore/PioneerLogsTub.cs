using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Security.Claims;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.WebUtilities;
using Pioneer.Logs.Models;

namespace Pioneer.Logs.Tubs.AspNetCore
{
    /// <summary>
    /// ASP.NET Core logging wrapper.
    /// </summary>
    public static class PioneerLogsTub
    {
        public static void LogUsage(string application, 
            string layer, 
            string activityName,
            HttpContext context, 
            Dictionary<string, object> additionalInfo = null)
        {
            var details = GetTubDetail(application, layer, activityName, context, additionalInfo);
            PioneerLogger.WriteUsage(details);
        }

        public static void LogDiagnostic(string application, 
            string layer, 
            string message,
            HttpContext context, 
            Dictionary<string, object> diagnosticInfo = null)
        {
            var details = GetTubDetail(application, layer, message, context, diagnosticInfo);
            PioneerLogger.WriteDiagnostic(details);
        }

        public static void LogError(string application, 
            string layer, 
            Exception ex,
            HttpContext context)
        {
            var details = GetTubDetail(application, layer, null, context);
            details.Exception = ex;

            PioneerLogger.WriteError(details);
        }

        /// <summary>
        /// Get as <see cref="PioneerLog"/> object pre-populated with details parsed
        /// from the ASP.NET Core environment.
        /// </summary>
        public static PioneerLog GetTubDetail(string application, 
            string layer,
            string activityName, 
            HttpContext context,
            Dictionary<string, object> additionalInfo = null)
        {
            var detail = new PioneerLog
            {
                ApplicationName = application,
                ApplicationLayer = layer,
                Message = activityName,
                Hostname = Environment.MachineName,
                TraceId = Activity.Current?.Id ?? context.TraceIdentifier,
                AdditionalInfo = additionalInfo ?? new Dictionary<string, object>()
            };

            GetUserData(detail, context);
            GetRequestData(detail, context);

            return detail;
        }

        /// <summary>
        /// Gather details about the request made to this HTTP pipeline request.
        /// </summary>
        private static void GetRequestData(PioneerLog detail, HttpContext context)
        {
            if (context.Request == null) return;

            detail.ApplicationLocation = context.Request.Path;
            detail.AdditionalInfo.Add("UserAgent", context.Request.Headers["User-Agent"]);
            detail.AdditionalInfo.Add("Languages", context.Request.Headers["Accept-Language"]);

            ExtractQueryStringIntoAdditionalInfo(detail, context);
        }

        private static void ExtractQueryStringIntoAdditionalInfo(PioneerLog detail, HttpContext context)
        {
            var qdict = QueryHelpers.ParseQuery(context.Request.QueryString.ToString());
            foreach (var key in qdict.Keys)
            {
                detail.AdditionalInfo.Add($"QueryString-{key}", qdict[key]);
            }
        }

        /// <summary>
        /// Gather details about current user associate with this HTTP pipeline request.
        /// </summary>
        private static void GetUserData(PioneerLog detail, HttpContext context)
        {
            var userId = "";
            var userName = "";
            var user = context.User;

            if (user != null)
            {
                var i = 1; // i included in dictionary key to ensure uniqueness
                foreach (var claim in user.Claims)
                {
                    switch (claim.Type)
                    {
                        case ClaimTypes.NameIdentifier:
                            userId = claim.Value;
                            break;
                        case "name":
                            userName = claim.Value;
                            break;
                        default:
                            detail.AdditionalInfo.Add($"UserClaim-{i++}-{claim.Type}", claim.Value);
                            break;
                    }
                }
            }

            detail.UserId = userId;
            detail.UserName = userName;
        }
    }
}
