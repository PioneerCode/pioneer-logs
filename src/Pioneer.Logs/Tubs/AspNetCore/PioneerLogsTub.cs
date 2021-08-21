using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.WebUtilities;
using Pioneer.Logs.Models;
using UAParser;
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
                if (Configuration.MapToEcs)
                {
                    var details = GetTubEcsDetail(message, LevelEnum.Usage, context, additionalInfo);
                    PioneerLogger.WriteUsage(details);
                }
                else
                {
                    var details = GetTubDetail(message, context, additionalInfo);
                    PioneerLogger.WriteUsage(details);
                }
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
                if (Configuration.MapToEcs)
                {
                    var details = GetTubEcsDetail(message, LevelEnum.Diagnostic, context, additionalInfo);
                    PioneerLogger.WriteDiagnostic(details);
                }
                else
                {
                    var details = GetTubDetail(message, context, additionalInfo);
                    PioneerLogger.WriteDiagnostic(details);
                }
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
                if (Configuration.MapToEcs)
                {
                    var details = GetTubEcsDetail(null, LevelEnum.Error, context, additionalInfo);
                    details.Error = new PioneerLogError
                    {
                        Code = ex.HResult.ToString(),
                        Message = ex.Message,
                        StackTrace = ex.StackTrace,
                        Type = ex.ToString()
                    };
                    PioneerLogger.WriteError(details);
                }
                else
                {
                    var details = GetTubDetail(null, context, additionalInfo);
                    details.Exception = ex;
                    PioneerLogger.WriteError(details);
                }
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
                if (Configuration.MapToEcs)
                {
                    var details = GetTubEcsDetail(message, LevelEnum.Error, context, additionalInfo);
                    PioneerLogger.WriteError(details);
                }
                else
                {
                    var details = GetTubDetail(message, context, additionalInfo);
                    PioneerLogger.WriteError(details);
                }
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
        /// Get as <see cref="PioneerLog"/> object pre-populated with details parsed
        /// from the ASP.NET Core environment.
        /// </summary>
        public static PioneerLogEcs GetTubEcsDetail(string message,
            LevelEnum level,
            HttpContext context = null,
            Dictionary<string, object> additionalInfo = null)
        {
            var detail = new PioneerLogEcs
            {
                Timestamp = DateTime.UtcNow,
                Message = message,
                CustomInfo = additionalInfo,
                Labels = new PioneerLogLabels
                {
                    ApplicationName = Configuration.ApplicationName,
                    ApplicationLayer = Configuration.ApplicationLayer
                },
                Event = new PioneerLogEvent
                {
                    Dataset = level.ToString()
                },
                Host = new PioneerLogHost
                {
                    Hostname = Dns.GetHostName(),
                    Host = Environment.MachineName
                },
                Log = new PioneerLogLog
                {
                    File = new PioneerLogLogFile
                    {
                        Path = @"logs\pioneer-logs-" + level.ToString().ToLower() + "-timestamp-.log"
                    }
                },
                Tracing = new PioneerLogTracing
                {
                    Transaction = new PioneerLogTracingTransaction
                    {
                        Id = IsNullOrEmpty(CorrelationId) ? Guid.NewGuid().ToString() : CorrelationId
                    }
                }
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

        private static void GetRequestData(PioneerLogEcs detail, HttpContext context)
        {
            if (context.Request == null)
                return;

            detail.Labels.ApplicatoinLocation = context.Request.Path;

            var userAgent = context.Request.Headers["User-Agent"];
            var uaParser = Parser.GetDefault();
            var clientInfo = uaParser.Parse(userAgent);
            detail.UserAgent.Original = clientInfo.ToString();
            detail.UserAgent.Name = clientInfo.UA.Family;
            detail.UserAgent.Version = $"{clientInfo.UA.Major}.{clientInfo.UA.Minor}.{clientInfo.UA.Patch}";
            detail.UserAgent.Device.Name = clientInfo.Device.ToString();

            string requestBodyStr;
            using (var reader = new StreamReader(context.Request.Body, Encoding.UTF8, true, 1024, true))
            {
                requestBodyStr = reader.ReadToEndAsync().Result;
            }
            detail.Http.Request = new PioneerLogHttpRequest
            {
                Referrer = context.Request.Headers["Referer"],
                MimeType = context.Request.ContentType,
                Method = context.Request.Method,
                Body = new PioneerLogHttpRequestBody
                {
                    Bytes = context.Response.ContentLength,
                    Content = requestBodyStr
                }
            };

            string responseBodyStr = null;
            if (context.Response.Body.CanRead)
            {
                using var reader = new StreamReader(context.Response.Body, Encoding.UTF8, true, 1024, true);
                responseBodyStr = reader.ReadToEndAsync().Result;
            }
            detail.Http.Response = new PioneerLogHttpResponse
            {
                StatusCode = context.Response.StatusCode,
                MimeType = context.Response.ContentType,
                Body = new PioneerLogHttpResponseBody
                {
                    Bytes = context.Response.ContentLength,
                    Content = responseBodyStr
                }
            };
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

        private static void GetUserData(PioneerLogEcs detail, HttpContext context)
        {
            if (context == null)
                return;

            string userId = null;
            string userName = null;

            var user = context.User;

            if (user?.Claims == null)
                return;

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
                        detail.CustomInfo.Add($"UserClaim-{i++}-{claim.Type}", claim.Value);
                        break;
                }
            }

            detail.User.Id = userId;
            detail.User.Name = userName;
        }
    }
}
