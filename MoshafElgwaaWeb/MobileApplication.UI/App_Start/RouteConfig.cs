using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace MobileApplication.UI
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
             "Default", "{controller}/{action}/{id}", new { area = "ControlPanel", controller = "account", action = "Default", id = UrlParameter.Optional }
            , new[] { "MobileApplication.UI.Areas.ControlPanel" }

        ).DataTokens.Add("area", "ControlPanel");
        }
    }
}
