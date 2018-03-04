using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

namespace CJChat
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
        }

        protected void Session_Start()
        {
            HttpCookie UserCookie = Request.Cookies.Get("NickerBoxUser");

            if(UserCookie != null && !String.IsNullOrWhiteSpace(UserCookie.Value))
            {
                Session["UserName"] = UserCookie.Value;
            }
        }
    }
}
