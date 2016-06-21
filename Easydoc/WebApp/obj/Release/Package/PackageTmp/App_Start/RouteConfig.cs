using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;


namespace MK.Easydoc.WebApp
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
              name: "Principal",
              url: "Principal",
              defaults: new { controller = "Home", action = "Inicio" }
            );

            routes.MapRoute(
             name: "Logout",
             url: "Logout",
             defaults: new { controller = "Login", action = "EncerrarAcesso" }
           );

            routes.MapRoute(
                "Default", // Route name
                "{controller}/{action}/{id}", // URL with parameters
                new { controller = "Login", action = "Index", area = "", id = UrlParameter.Optional },
                new[] { "MK.Easydoc.WebApp.Controllers" }
                // Parameter defaults
            );
        }
    }
}