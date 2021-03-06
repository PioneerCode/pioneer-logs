﻿using System;
using System.Collections.Generic;
using System.Security.Claims;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.WebUtilities;
using Pioneer.Logs.Models;
using static System.String;

namespace Pioneer.Logs.Tubs.AspNetCore
{
    /// <summary>
    /// ASP.NET Core logging wrapper.
    /// </summary>
    public static class PioneerLogsTub
    {
        /// <summary>
        /// Manually overrides system level correlation generated IDs
        /// If used, it is the responsibility of the client to manage state of this value. 
        /// </summary>
        public static string CorrelationId { get; set; }
        public static PioneerLogsConfiguration Configuration { get; set; }

        static PioneerLogsTub()
        {
            Configuration = new PioneerLogsConfiguration();
        }

        /// <summary>
        /// Log Usage
        /// </summary>
        /// <param name="message">User defined message</param>
        /// <param name="context">Optional <see cref="HttpContext"/> used to gather context based info.</param>
        /// <param name="additionalInfo">Dictionary of user define key value info.</param>
        /// <param name="forceWriteToFile">Override file write from configuration.</param>
        public static void LogUsage(string message,
            HttpContext context = null,
            Dictionary<string, object> additionalInfo = null, bool forceWriteToFile = false)
        {
            if (Configuration.Usage.WriteToFile || forceWriteToFile)
            {
                var details = GetTubDetail(message, context, additionalInfo);
                PioneerLogger.WriteUsage(details);
            }

            if (Configuration.Usage.WriteToConsole)
            {
                PioneerLogger.ConsoleLogger.Information("USAGE: " + message);
            }
        }

        /// <summary>
        /// Log Diagnostics
        /// </summary>
        /// <param name="message">User defined message</param>
        /// <param name="context">Optional <see cref="HttpContext"/> used to gather context based info.</param>
        /// <param name="additionalInfo">Dictionary of user define key value info.</param>
        /// <param name="forceWriteToFile">Override file write from configuration.</param>
        public static void LogDiagnostic(string message,
            HttpContext context = null,
            Dictionary<string, object> additionalInfo = null, bool forceWriteToFile = false)
        {
            if (Configuration.Diagnostics.WriteToFile || forceWriteToFile)
            {
                var details = GetTubDetail(message, context, additionalInfo);
                PioneerLogger.WriteDiagnostic(details);
            }

            if (Configuration.Diagnostics.WriteToConsole)
            {
                PioneerLogger.ConsoleLogger.Information("DIAGNOSTIC: " + message);
            }
        }

        /// <summary>
        /// Log Error
        /// </summary>
        /// <param name="ex">Exception thrown</param>
        /// <param name="context">Optional <see cref="HttpContext"/> used to gather context based info.</param>
        /// <param name="additionalInfo">Dictionary of user define key value info.</param>
        /// <param name="forceWriteToFile">Override file write from configuration.</param>
        public static void LogError(Exception ex,
            HttpContext context = null,
            Dictionary<string, object> additionalInfo = null,
            bool forceWriteToFile = false)
        {
            if (Configuration.Errors.WriteToFile || forceWriteToFile)
            {
                var details = GetTubDetail(null, context, additionalInfo);
                details.Exception = ex;
                PioneerLogger.WriteError(details);
            }

            if (Configuration.Errors.WriteToConsole)
            {
                PioneerLogger.ConsoleLogger.Error("ERROR: " + ex);
            }

            CorrelationId = Empty;
        }

        /// <summary>
        /// Log Error
        /// </summary>
        /// <param name="message">User defined message</param>
        /// <param name="context">Optional <see cref="HttpContext"/> used to gather context based info.</param>
        /// <param name="additionalInfo">Dictionary of user define key value info.</param>
        /// <param name="forceWriteToFile">Override file write from configuration.</param>
        public static void LogError(string message,
            HttpContext context = null,
            Dictionary<string, object> additionalInfo = null,
            bool forceWriteToFile = false)
        {
            if (Configuration.Errors.WriteToFile || forceWriteToFile)
            {
                var details = GetTubDetail(message, context, additionalInfo);
                PioneerLogger.WriteError(details);
            }

            if (Configuration.Errors.WriteToConsole)
            {
                PioneerLogger.ConsoleLogger.Error("ERROR: " + message);
            }

            CorrelationId = Empty;
        }

        /// <summary>
        /// Get as <see cref="PioneerLog"/> object pre-populated with details parsed
        /// from the ASP.NET Core environment.
        /// </summary>
        public static PioneerLog GetTubDetail(string message,
            HttpContext context = null,
            Dictionary<string, object> additionalInfo = null)
        {
            var detail = new PioneerLog
            {
                Id = Guid.NewGuid(),
                ApplicationName = Configuration.ApplicationName,
                ApplicationLayer = Configuration.ApplicationLayer,
                Message = message,
                Hostname = Environment.MachineName,
                CorrelationId = IsNullOrEmpty(CorrelationId) ? Guid.NewGuid().ToString() : CorrelationId,
                SystemGenerateCorrelationId = IsNullOrEmpty(CorrelationId),
                AdditionalInfo = additionalInfo ?? new Dictionary<string, object>(),
                CreationTimestamp = DateTime.UtcNow
            };

            if (context == null) return detail;

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
            detail.Username = userName;
        }
    }
}
