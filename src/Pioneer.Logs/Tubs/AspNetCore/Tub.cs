﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Security.Claims;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.WebUtilities;
using Pioneer.Logs.Models;

namespace Pioneer.Logs.Tubs.AspNetCore
{
    public static class Tub
    {
        public static void LogWebUsage(string application, 
            string layer, 
            string activityName,
            HttpContext context, 
            Dictionary<string, object> additionalInfo = null)
        {
            var details = GetWeblogDetail(application, layer, activityName, context, additionalInfo);
            Logger.WriteUsage(details);
        }

        public static void LogWebDiagnostic(string application, 
            string layer, 
            string message,
            HttpContext context, 
            Dictionary<string, object> diagnosticInfo = null)
        {
            var details = GetWeblogDetail(application, layer, message, context, diagnosticInfo);
            Logger.WriteDiagnostic(details);
        }

        public static void LogWebError(string application, 
            string layer, 
            Exception ex,
            HttpContext context)
        {
            var details = GetWeblogDetail(application, layer, null, context, null);
            details.Exception = ex;

            Logger.WriteError(details);
        }

        public static Log GetWeblogDetail(string application, 
            string layer,
            string activityName, 
            HttpContext context,
            Dictionary<string, object> additionalInfo = null)
        {
            var detail = new Log
            {
                ApplicationName = application,
                ApplicationLayer = layer,
                Message = activityName,
                Hostname = Environment.MachineName,
                CorrelationId = Activity.Current?.Id ?? context.TraceIdentifier,
                AdditionalInfo = additionalInfo ?? new Dictionary<string, object>()
            };

            GetUserData(detail, context);
            GetRequestData(detail, context);

            return detail;
        }

        private static void GetRequestData(Log detail, HttpContext context)
        {
            var request = context.Request;
            if (request == null) return;

            detail.Location = request.Path;
            detail.AdditionalInfo.Add("UserAgent", request.Headers["User-Agent"]);
            detail.AdditionalInfo.Add("Languages", request.Headers["Accept-Language"]);

            var qdict = QueryHelpers.ParseQuery(request.QueryString.ToString());
            foreach (var key in qdict.Keys)
            {
                detail.AdditionalInfo.Add($"QueryString-{key}", qdict[key]);
            }
        }

        private static void GetUserData(Log detail, HttpContext context)
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
