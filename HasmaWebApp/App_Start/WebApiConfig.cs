﻿using Microsoft.Owin.Security.OAuth;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;

namespace HasmaWebApp
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
          //  config.EnableCors();

            // Web API configuration and services
            // Configure Web API to use only bearer token authentication.  
            config.SuppressDefaultHostAuthentication();
            config.Filters.Add(new HostAuthenticationFilter(OAuthDefaults.AuthenticationType));

            // Web API routes
            config.MapHttpAttributeRoutes();

            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "HasmaAPI/{controller}/{action}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );

            // WebAPI when dealing with JSON & JavaScript!  
            // Setup json serialization to serialize classes to camel (std. Json format)  
            var formatter = GlobalConfiguration.Configuration.Formatters.JsonFormatter;
            formatter.SerializerSettings.ContractResolver = new Newtonsoft.Json.Serialization.CamelCasePropertyNamesContractResolver();

            // Adding JSON type web api formatting.  
            config.Formatters.Clear();
            config.Formatters.Add(formatter);
        }
    }
}
