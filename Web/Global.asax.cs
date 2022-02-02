using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using System.Web.Security;

namespace App.Web
{
    public class MvcApplication : System.Web.HttpApplication
    {

        protected void Application_Error(object sender, EventArgs e)
        {
            Exception ex = Server.GetLastError();
            if (ex is ThreadAbortException)
                return;
            //Server.Transfer("/Shared/BadRequest", true);
            Server.ClearError();

            // Possible that a partially rendered page has already been written to response buffer before encountering error, so clear it.
            Response.Clear();

            // Finally redirect, transfer, or render a error view
            Response.Redirect("~/Shared/BadRequest", true);
        }
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);

            //THIS IS ENTITY FRAMEWORK'S STUFF, 
            //TO DISABLE EXCEPTION WHEN A DOMAIN CLASS AND ITS ASSOCIATED TABLE IS FOUND DIFFERENT
            try
            {
                Database.SetInitializer<App.Data.EfDbContext>(null);
            }
            catch (Exception ex)
            {
                var exx = ex.Message;
            }


            //create cache			
            //#if (DEBUG)
            //#else
            //			var partList = Service.Master.PartsLists.GetList();
            //			var partMaping = Service.Imex.PartsMapping.GetList();
            //#endif

        }

        private static Regex ExcludedStringRegex = new Regex(@"/bundles/|/Content/|/reports/|/picker/|/sign-in|/sign-out|/sign-inVendor|account/|/initsession|/clearsession/hardclearsession", RegexOptions.IgnoreCase);
        private static Regex ExcludedFileExtensions = new Regex(@"\.(htm|html|axd|cab|css|cur|js|ico|gif|jpg|jpeg|png|rpt|xml)$", RegexOptions.IgnoreCase);

        protected void Application_AuthenticateRequest(Object sender, EventArgs e)
        {
            var reqUrl = Request.Url == null ? "" : Request.Url.AbsoluteUri.ToLower();

            int iPostQn = reqUrl.IndexOf("?");

            if (iPostQn != -1)
            {
                reqUrl = reqUrl.Substring(0, iPostQn);
            }

            if (ExcludedStringRegex.IsMatch(reqUrl))
            {
                return;
            }

            int iPostDot = reqUrl.LastIndexOf(".");

            if (iPostDot != -1)
            {
                reqUrl = reqUrl.Substring(iPostDot);
            }

            if (ExcludedFileExtensions.IsMatch(reqUrl))
            {
                return;
            }

            Web.Helper.Authentication.GetCookie(HttpContext.Current);

            string employeeId = "";
            try { employeeId = User.Identity.GetEmployeeName(); }
            catch { employeeId = ""; }

            ///access-denied|

            if (string.IsNullOrEmpty(employeeId))
            {
                string loginUrl = FormsAuthentication.LoginUrl;
                string path = Request.Url.AbsolutePath.ToLower();
                //if(!path.Contains("default") && !path.Contains("/index"))
                //	loginUrl = loginUrl + "?returnUrl=" + Request.Url.AbsolutePath;
                path = path.ToLower().Contains("/index-portal") ? "/" : path;

                var appPath = Request.ApplicationPath;
                bool isAuthenticated = false;
                try { isAuthenticated = User.Identity.IsAuthenticated; } catch { }

                if (path == "/" && appPath == path && !isAuthenticated)
                    //Response.Redirect("~/redirect?url=~" + loginUrl);
                    return;
                else
                    Response.Redirect(loginUrl);
            }

        }

        protected void Session_Start(object src, EventArgs e)
        {
            string loginUrl = FormsAuthentication.LoginUrl;

            if (Context.Session != null)
            {
                if (Context.Session.IsNewSession)//|| Context.Session.Count==0)
                {
                    string sCookieHeader = Request.Headers["Cookie"];
                    if ((null != sCookieHeader) && (sCookieHeader.IndexOf("ASP.NET_SessionId") >= 0))
                    {
                        //if (Request.IsAuthenticated)
                        FormsAuthentication.SignOut();
                        Response.Redirect(loginUrl);
                    }
                }
            }

        }

        protected void Session_End(object sender, EventArgs e)
        {
            //Code that runs when a session ends. 
            //Note: The Session_End event is raised only when the sessionstate mode 
            //is set to InProc in the Web.config file. If session mode is set to StateServer
            //or SQLServer, the event is not raised. 
            Session.Clear();
        }

        protected void Application_PreSendRequestHeaders()
        {
            Response.Headers.Remove("Server");
            Response.Headers.Remove("X-Powered-By");
            Response.Headers.Remove("X-AspNet-Version");
            Response.Headers.Remove("X-AspNetMvc-Version");
        }
    }
}
