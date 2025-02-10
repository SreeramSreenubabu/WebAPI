using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using WebAPI.Data;

namespace WebAPI
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            // Enforce strict JSON validation globally
            var jsonSettings = config.Formatters.JsonFormatter.SerializerSettings;
            jsonSettings.MissingMemberHandling = MissingMemberHandling.Error;
            jsonSettings.NullValueHandling = NullValueHandling.Ignore;

            // Web API configuration and services
            //config.Filters.Add(new StrictModelValidationFilter());
            // Web API routes
            config.MapHttpAttributeRoutes();

            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );
        }
    }
}
