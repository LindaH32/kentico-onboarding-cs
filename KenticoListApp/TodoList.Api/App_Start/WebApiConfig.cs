using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;

namespace TodoList.Api
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            // Web API configuration and services

            // Web API routes
            config.MapHttpAttributeRoutes();

            config.Routes.MapHttpRoute(
                name: "DefaultApiV1",
                routeTemplate: "api/v1/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );

            config.Routes.MapHttpRoute(
               name: "DefaultApiV2",
               routeTemplate: "api/v2/{controller}/{id}",
               defaults: new { id = RouteParameter.Optional }
           );
        }
    }
}
