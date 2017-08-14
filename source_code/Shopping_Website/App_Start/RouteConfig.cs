using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace Shopping_Website
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional }
            );

            routes.MapRoute(
              name: "Products",
              url: "{controller}/{action}/{id}",
              defaults: new { controller = "Product", action = "Index", id = UrlParameter.Optional }
            );

            routes.MapRoute(
              name: "Login",
              url: "{controller}/{action}/{id}",
              defaults: new { controller = "Login", action = "Index", id = UrlParameter.Optional }
            );

            routes.MapRoute(
             name: "Admin",
             url: "{controller}/{action}/{id}",
             defaults: new { controller = "Admin", action = "Index", id = UrlParameter.Optional }
           );

            routes.MapRoute(
             name: "Customer",
             url: "{controller}/{action}/{id}",
             defaults: new { controller = "Customer", action = "Index", id = UrlParameter.Optional }
           );

        }
    }
}
