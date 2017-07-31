using Aggregator.Core.Monitoring;
using Aggregator.WebHooks.Utils;
using BasicAuthentication.Filters;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web.Http;

namespace Aggregator.WebHooks
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            // Web API configuration and services

            // apply the filter to all Web API controllers
            var logger = new AspNetEventLogger("pre-request-parsing", GetDefaultLoggingLevel());
            config.Filters.Add(new IdentityBasicAuthenticationAttribute(logger));

            // Web API routes
            config.MapHttpAttributeRoutes();

            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );
        }

        private static LogLevel GetDefaultLoggingLevel()
        {
            var defaultLoggingLevelAsString = ConfigurationManager.AppSettings["DefaultLoggingLevel"];
            var defaultLoggingLevel = LogLevel.Normal;
            Enum.TryParse<LogLevel>(defaultLoggingLevelAsString, true, out defaultLoggingLevel);
            return defaultLoggingLevel;
        }
    }
}
