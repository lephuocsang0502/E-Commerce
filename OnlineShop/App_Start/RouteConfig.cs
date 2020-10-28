﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace OnlineShop
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
              name: "Product Category",
              url: "product/{metatitle}-{id}",
              defaults: new { controller = "Product", action = "Category", id = UrlParameter.Optional },
              namespaces: new[] { "OnlineShop.Controllers" }
          );
            routes.MapRoute(
              name: "About",
              url: "about",
              defaults: new { controller = "Home", action = "About", id = UrlParameter.Optional },
              namespaces: new[] { "OnlineShop.Controllers" }
          );
            routes.MapRoute(
              name: "Contact",
              url: "contact",
              defaults: new { controller = "Home", action = "Contact", id = UrlParameter.Optional },
              namespaces: new[] { "OnlineShop.Controllers" }
          );
            routes.MapRoute(
             name: "Search",
             url: "tim-kiem",
             defaults: new { controller = "Product", action = "Search", id = UrlParameter.Optional },
             namespaces: new[] { "OnlineShop.Controllers" }
         );

            routes.MapRoute(
             name: "Product Detail",
             url: "detail/{metatitle}-{id}",
             defaults: new { controller = "Product", action = "Detail", id = UrlParameter.Optional },
             namespaces: new[] { "OnlineShop.Controllers" }
         );
            routes.MapRoute(
            name: "Cart",
            url: "gio-hang",
            defaults: new { controller = "Cart", action = "Index", id = UrlParameter.Optional },
            namespaces: new[] { "OnlineShop.Controllers" }
        );
            routes.MapRoute(
            name: "Success",
            url: "hoan-thanh",
            defaults: new { controller = "Cart", action = "Success", id = UrlParameter.Optional },
            namespaces: new[] { "OnlineShop.Controllers" }
        );
            routes.MapRoute(
          name: "Add Cart",
          url: "them-gio-hang",
          defaults: new { controller = "Cart", action = "AddItem", id = UrlParameter.Optional },
          namespaces: new[] { "OnlineShop.Controllers" }
      );


            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional },
                namespaces: new[] {"OnlineShop.Controllers"}
            );

          
        }
    }
}