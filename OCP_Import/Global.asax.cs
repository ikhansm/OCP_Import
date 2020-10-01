using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

namespace OCP_Import
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            System.Net.ServicePointManager.SecurityProtocol = System.Net.SecurityProtocolType.Tls11 | System.Net.SecurityProtocolType.Tls12;
            ShopifySharp.ShopifyService.SetGlobalExecutionPolicy(new ShopifySharp.RetryExecutionPolicy());
            AreaRegistration.RegisterAllAreas();
            GlobalConfiguration.Configure(WebApiConfig.Register);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
        }

        void Session_Start(Object sender, EventArgs e)
        {
            Response.Cookies["ASP.NET_SessionId"].SameSite = System.Web.SameSiteMode.None;
            Response.Cookies["ASP.NET_SessionId"].Path += ";SameSite=None";
            //while we're at it lets also make it secure
            if (Request.IsSecureConnection)

                Response.Cookies["ASP.NET_SessionId"].Secure = true;
            Response.Cookies["ASP.NET_SessionId"].Path += ";SameSite=None";

        }



    }
}
