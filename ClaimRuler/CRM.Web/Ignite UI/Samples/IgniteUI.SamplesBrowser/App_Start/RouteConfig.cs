using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace IgniteUI.SamplesBrowser
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");
            routes.IgnoreRoute("IGUploadStatusHandler.ashx");
           
            routes.MapRoute(
                name: "Default",
                url: "",
                defaults: new { controller = "Home", action = "Index" }
            );
           
            routes.MapRoute(
                name: "ApplicationSamples",
                url: "application-samples",
                defaults: new { controller = "Home", action = "ApplicationSamples" }
            );

            routes.MapRoute(
                name: "GettingStarted",
                url: "getting-started",
                defaults: new { controller = "Home", action = "GettingStarted", filePath = "getting-started" }
            );

            routes.MapRoute(
                name: "GettingStartedMobile",
                url: "getting-started-mobile",
                defaults: new { controller = "Home", action = "GettingStarted", filePath = "getting-started-mobile" }
            );

            routes.MapRoute(
                name: "AllJs",
                url: "js/all.js",
                defaults: new { controller = "Home", action = "AllJs" }
            );

            routes.MapRoute(
                name: "DataFiles",
                url: "js-data/{fileName}",
                defaults: new { controller = "Home", action = "Data" }
            );

            routes.MapRoute(
                name: "JsFiles",
                url: "js/{fileName}",
                defaults: new { controller = "Home", action = "Js" }
            );

            routes.MapRoute(
                name: "AllCss",
                url: "css/all.css",
                defaults: new { controller = "Home", action = "AllCss" }
            );

            if (ConfigurationManager.AppSettings["EnableLocalInstallGen"] != null && bool.Parse(ConfigurationManager.AppSettings["EnableLocalInstallGen"]))
            {
                routes.MapRoute(
                    name: "LocalInstallPageGeneration",
                    url: "localinstallgen",
                    defaults: new { controller = "LocalInstall", action = "LocalInstallPageGeneration" }
                );
            }
            routes.MapRoute(
              name: "404Error",
              url: "Error/{action}",
              defaults: new { controller = "Error", action = "HttpError404" }
          );
            routes.MapRoute(
                name: "Control",
                url: "{controlPath}",
                defaults: new { controller = "Home", action = "Control" }
            );

            routes.MapRoute(
                name: "Sample",
                url: "{controlPath}/{samplePath}",
                defaults: new { controller = "Home", action = "Sample" }
            );
           
            routes.MapRoute(
                name: "SampleJson",
                url: "{controlPath}/{samplePath}/json",
                defaults: new { controller = "Home", action = "SampleJson" }
            );

            routes.MapRoute(
                name: "ASPNET",
                url: "aspnet/{controller}/{action}",
                defaults: new { controller = "Home", action = "Index" }
            );

             
                      
        }
    }
}