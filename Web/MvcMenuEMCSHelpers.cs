using App.Data.Caching;
using App.Data.Domain.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;

namespace App.Web
{
    public class MvcMenuEMCSHelpers : MvcMenuHelpers
    {
        private static StringBuilder sb = new StringBuilder();
        private const string cacheName = "MvcMenuHelpers";
        private readonly static ICacheManager _cacheManager = new MemoryCacheManager();

        private static void CreateHTMLMenu(ICollection<RoleAccessDetailsMenu> Menus, string sClass, int process)
        {
            string baseUrl = string.Empty;
            var request = HttpContext.Current.Request;
            var appUrl = HttpRuntime.AppDomainAppVirtualPath;

            if (appUrl == "/")
                appUrl = "";

            if (!request.Url.Authority.Contains("localhost"))
            {

                baseUrl = string.Format("{0}://{1}{2}", request.Url.Scheme, "staging.mkindo.com:5183", appUrl);
            }
            else
            {
                baseUrl = string.Format("{0}://{1}{2}", request.Url.Scheme, request.Url.Authority, appUrl);
            }
            //baseUrl = string.Format("{0}://{1}:{2}{3}", request.Url.Scheme, request.Url.Host, request.Url.Port, appUrl);

            foreach (var menu in Menus)
            {
                if (menu.Name == "Home")
                    sb.AppendLine("<li class=\"active\">");
                else
                    sb.AppendLine("<li>");

                sb.AppendLine("<a " + Convert.ToString((string.IsNullOrEmpty(menu.URL)) ? ">"
                    : (menu.URL.Trim().Substring(0, 1) != "/" && menu.URL.Trim().Substring(0, 1) != @"\")
                    ? "href='" + baseUrl + "/" + menu.URL.Replace(@"\", "/") + "'>"
                    : "href='" + baseUrl + "/" + menu.URL.Replace(@"\", "/") + "'>"));

                if (menu.children.Count() <= 0)
                {
                    sb.AppendLine("<i class=\"" + menu.icon + "\"></i><p>" + menu.Name + "</p>");
                    sb.AppendLine("</a>");
                }
                else
                {
                    sb.AppendLine("<a data-toggle=\"collapse\" href=\"#menu" + menu.ID + "\" class=\"collapsed\" aria-expanded=\"false\">");
                    sb.AppendLine("<i class=\"" + menu.icon + "\"></i><p>" + menu.Name + "<b class=\"caret\"></b></p>");
                    sb.AppendLine("</a>");
                    sb.AppendLine("<div class=\"collapse\" id=\"menu" + menu.ID + "\">");
                    sb.AppendLine("<ul class=\"nav\">");
                    CreateHTMLMenu(menu.children, sClass, 1);
                    sb.AppendLine("</ul>");
                }
                sb.AppendLine("</li>");
            }
        }
    
    }
}