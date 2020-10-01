using OCP_Import.Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace OCP_Import
{
    public class SessionExpireFilter : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {

            #region CheckSession
            
                string domain = Convert.ToString(HttpContext.Current.Session["SellerExist"]) != "" ? Convert.ToString(HttpContext.Current.Session["SellerExist"]) : Convert.ToString(HttpContext.Current.Session["SellerInstall"]);
                if (domain == "")
                {
                    domain = HttpContext.Current.Request.Cookies.Get(".App.Handshake.ShopUrl") != null ? HttpContext.Current.Request.Cookies.Get(".App.Handshake.ShopUrl").Value : "";
                    //domain = Request.Cookies.Get(".App.Handshake.ShopUrl").Value;
                    //if (domain == "" && shop != "" && ShopifySharp.AuthorizationService.IsAuthenticRequest(HttpContext.Current.Request.QueryString.ToKvps(), ApplicationEngine.ShopifySecretKeyPublicApp))
                    //{
                    //    domain = shop;

                    //    var cookie = new System.Web.HttpCookie(".App.Handshake.ShopUrl", shop)
                    //    {
                    //        Expires = DateTime.Now.AddDays(2),
                    //        Secure = true
                    //    };
                    //    cookie.Path += ";SameSite=None";
                    //   HttpContext.Current.Response.SetCookie(cookie);
                        
                    //}
                }

            #endregion




            HttpContext ctx = HttpContext.Current;
            if (HttpContext.Current.Session["SellerExist"] == null )
            {
               
                filterContext.Result = new RedirectResult("~/Account/LogIn?returnUrl=" + ctx.Request.Path);
                return;
            }



            // check if session is supported
            if (ctx.Session != null)
            {
                // check if a new session id was generated
                if (ctx.Session.IsNewSession)
                {
                    // If it says it is a new session, but an existing cookie exists, then it must
                    // have timed out
                    string sessionCookie = ctx.Request.Headers["Cookie"];
                    if ((null != sessionCookie) && (sessionCookie.IndexOf("ASP.NET_SessionId") >= 0))
                    {
                        ctx.Session["SessionExpMsg"] = "Session timed out, Login to proceed!";
                        ctx.Response.Redirect("~/Account/LogIn");
                    }
                }
            }

            //string encodingsAccepted = filterContext.HttpContext.Request.Headers["Accept-Encoding"];
            //if (string.IsNullOrEmpty(encodingsAccepted))
            //    return;

            //encodingsAccepted = encodingsAccepted.ToLowerInvariant();
            //HttpResponseBase response = filterContext.HttpContext.Response;

            //if (encodingsAccepted.Contains("gzip"))
            //{
            //    response.AppendHeader("Content-encoding", "gzip");
            //    response.Filter = new GZipStream(response.Filter, CompressionMode.Compress);
            //}
            //else if (encodingsAccepted.Contains("deflate"))
            //{
            //    response.AppendHeader("Content-encoding", "deflate");
            //    response.Filter = new DeflateStream(response.Filter, CompressionMode.Compress);
            //}
            base.OnActionExecuting(filterContext);
        }
    }

}