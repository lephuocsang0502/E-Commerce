
using Microsoft.AspNet.SignalR;
using Microsoft.AspNet.SignalR.Hubs;
using OnlineShop.Common;
using OnlineShop.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using System.Web.Security;
using System.Web.SessionState;

namespace OnlineShop
{
    public class MvcApplication : System.Web.HttpApplication
    {
        
        protected void Application_Start()
        {
     
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);


            Application["PageView"] = 0;
            Application["ClientOnlineNow"] = 0;
            
        }
        protected void Session_Start()
        {
            Application.Lock();//dong bo hoa
            Application["PageView"] = (int)Application["PageView"] + 1;
            Application["ClientOnlineNow"] = (int)Application["ClientOnlineNow"] + 1;
            Application.UnLock();
        }
        protected void Session_End()
        {
            Application.Lock();//dong bo hoa
        
            Application["ClientOnlineNow"] = (int)Application["ClientOnlineNow"] - 1;
            Application.UnLock();
        }

    }
}
