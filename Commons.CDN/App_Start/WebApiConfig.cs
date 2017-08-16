using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;

namespace bOS.Services.CDN
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            // Web API configuration and services

            // Web API routes
            config.MapHttpAttributeRoutes();

            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{paziente}/{visita}/{folderType}/{fileName}",
                defaults: new { paziente = RouteParameter.Optional, 
                                visita = RouteParameter.Optional,
                                folderType = RouteParameter.Optional,
                                fileName = RouteParameter.Optional
                }
            );
            
        }
    }
}
