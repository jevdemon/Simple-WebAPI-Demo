using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;

namespace ProductsApp
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            // Web API configuration and services

            // Web API routes

            // HTTP verbs are mapped to Controllers here.
            // When the Web API framework receives an HTTP request, it tries to match the URI against 
            // one of the route templates in the web.config. If no route matches, the client receives a 
            // 404 error.
            config.MapHttpAttributeRoutes();

            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );
            // This means URLs must use this format: api/<controller-class-name>/<parameter>
            // We can remove the 'api' from the URL and replace it with anything we like such as:
            //   routeTemplate: "myServices/{controller}/{id}"  OR
            //   routeTemplate: "{controller}/{id}"
        }
    }
}
