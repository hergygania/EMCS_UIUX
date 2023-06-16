﻿﻿﻿using App.Data.Caching;
using App.Data.Domain.Extensions;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace App.Web
{
    public class MvcMenuEMCSHelpers : MvcMenuHelpers
    {
        public static string menuEMCSHTML = string.Empty;
        private static StringBuilder sb = new StringBuilder();
        public static string UserNameEMCS = string.Empty;
        private const string cacheName = "MvcMenuHelpers";
        private readonly static ICacheManager _cacheManager = new MemoryCacheManager();

        public MvcHtmlString Menu(string sClass)
        {
            try
            {
                string key = string.Format(cacheName);
                //var list = _cacheManager.Get(key, () =>
                //{
                using (var db = new Data.EfDbContext())
                {
                    int RoleID = 0;
                    if (Domain.SiteConfiguration.UserName != null)
                    {
                        UserNameEMCS = Domain.SiteConfiguration.UserName.ToString();
                    }

                    var UserRole = (from p in db.UserAccess_Role
                                    where p.UserID == UserNameEMCS
                                    select p).ToList();

                    bool IsAdmin = false;

                    if (UserRole.Count == 1)
                    {
                        IsAdmin = true;
                    }
                    int row = 0;
                    sb.Clear();
                    sb = new StringBuilder();
                    foreach (var q in UserRole)
                    {

                        if (UserRole != null)
                            RoleID = Convert.ToInt32(q.RoleID);
                        if (IsAdmin == false)
                        {
                            if (RoleID != 1)
                            {
                                if (row == 0)
                                {
                                    CreateHTMLMenu(CreateDataMenu(RoleID, 1).OrderBy(p => p.OrderNo).ToList(), sClass, 0);
                                }
                                else
                                {
                                    CreateHTMLMenu(CreateDataMenu(RoleID, 0).OrderBy(p => p.OrderNo).ToList(), sClass, 0);
                                }

                            }
                        }
                        else
                        {
                            CreateHTMLMenu(CreateDataMenu(RoleID, 1).OrderBy(p => p.OrderNo).ToList(), sClass, 0);
                        }
                        row++;
                    }


                    menuEMCSHTML = sb.ToString();

                    return new MvcHtmlString(menuEMCSHTML.Replace("'", "\""));
                }
                //});
                //return list;
            }
            catch (Exception e)
            {
                return new MvcHtmlString(e.Message);
            }
        }

        private static void CreateHTMLMenu(ICollection<RoleAccessDetailsMenu> Menus, string sClass, int process)
        {
            string baseUrl = string.Empty;
            var request = HttpContext.Current.Request;
            var appUrl = HttpRuntime.AppDomainAppVirtualPath;

            if (appUrl == "/")
                appUrl = "";
            string url = HttpContext.Current.Request.Url.AbsoluteUri;
            Uri url1 = new Uri(url);
            string host = url1.GetLeftPart(UriPartial.Authority);

            if (!request.Url.Authority.Contains("localhost"))
            {

                baseUrl = string.Format("{0}://{1}{2}", request.Url.Scheme, host, appUrl);
            }
            else
            {
                baseUrl = string.Format("{0}://{1}{2}", request.Url.Scheme, request.Url.Authority, appUrl);
            }
            //baseUrl = string.Format("{0}://{1}:{2}{3}", request.Url.Scheme, request.Url.Host, request.Url.Port, appUrl);

            foreach (var menu in Menus)
            {

                //prev
                //if (menu.Name == "Home")
                //    sb.AppendLine("<li class=\"\">");
                //else
                //    sb.AppendLine("<li>");

                //sb.AppendLine("<a class=\"\" " + Convert.ToString((string.IsNullOrEmpty(menu.URL)) ? ">"
                //    : (menu.URL.Trim().Substring(0, 1) != "/" && menu.URL.Trim().Substring(0, 1) != @"\")
                //    ? "href='" + baseUrl + "/" + menu.URL.Replace(@"\", "/") + "'>"
                //    : "href='" + baseUrl + "/" + menu.URL.Replace(@"\", "/") + "'>"));

                //if (menu.children.Count() <= 0)
                //{
                //    sb.AppendLine("<i style=\"color: #666d7d;\" class=\"" + menu.icon + "\"></i><p class=\"\" style=\"color: #666d7d;\">" + menu.Name + "</p>");
                //    sb.AppendLine("</a>");
                //}
                //else
                //{
                //    sb.AppendLine("<a data-toggle=\"collapse\" href=\"#menu" + menu.ID + "\" class=\"\" aria-expanded=\"false\">");
                //    sb.AppendLine("<i style=\"color: #666d7d;\" class=\" " + menu.icon + "\"></i><p style=\"color: #666d7d;\">" + menu.Name + "</p>");
                //    sb.AppendLine("</a>");
                //    sb.AppendLine("<div class=\"\" id=\"menu" + menu.ID + "\">");
                //    sb.AppendLine("<ul class=\"y-2 flex flex-col space-y-2\">");
                //    CreateHTMLMenu(menu.children, sClass, 1);
                //    sb.AppendLine("</ul>");
                //}
                //sb.AppendLine("</li>");
                //sb.AppendLine("</li>");

                //new for UI/UX
                if (menu.Name == "Home")
                    sb.AppendLine("<li class=\"hidden\">");
                else
                    sb.AppendLine("<li>");

                if (menu.URL != "#")
                {
                    if (process == 1)
                    {
                        sb.AppendLine("<a class=\"nav-link--level-1\" " + Convert.ToString((string.IsNullOrEmpty(menu.URL)) ? ">"
                        : (menu.URL.Trim().Substring(0, 1) != "/" && menu.URL.Trim().Substring(0, 1) != @"\")
                        ? "href='" + baseUrl + "/" + menu.URL.Replace(@"\", "/") + "'>"
                        : "href='" + baseUrl + "/" + menu.URL.Replace(@"\", "/") + "'>"));
                    } else
                    {
                        sb.AppendLine("<a class=\"nav-link--level-0\" " + Convert.ToString((string.IsNullOrEmpty(menu.URL)) ? ">"
                        : (menu.URL.Trim().Substring(0, 1) != "/" && menu.URL.Trim().Substring(0, 1) != @"\")
                        ? "href='" + baseUrl + "/" + menu.URL.Replace(@"\", "/") + "'>"
                        : "href='" + baseUrl + "/" + menu.URL.Replace(@"\", "/") + "'>"));
                    }
                    
                }

                if (menu.children.Count() <= 0)
                {
                    sb.AppendLine("<div class=\"nav-link-wrap\">");
                    sb.AppendLine("<i style=\"color: #666d7d; padding-right: 15px;\" class=\"uil " + menu.icon + "\"></i><span class=\"nav-label\">" + menu.Name + "</span>");
<<<<<<< HEAD
                    sb.AppendLine("</div>");
=======
>>>>>>> cfd35badf37a2208f5855c22042cd90d017c16cc
                    sb.AppendLine("</a>");
                }
                else
                {
                    sb.AppendLine("<button class=\"nav-collapse nav-collapse--active\" type=\"button\">");
                    sb.AppendLine("<div class=\"nav-link-wrap\">");
                    sb.AppendLine("<i style=\"color: #666d7d;\" class=\"uil " + menu.icon + "\"></i><span class=\"nav-label\">" + menu.Name + "</span>");
                    sb.AppendLine("</div>");
                    sb.AppendLine("</button>");
                    sb.AppendLine("<div class=\"nav-collapse--child\" id=\"menu" + menu.ID + "\">");
                    sb.AppendLine("<ul class=\"py-2 flex flex-col space-y-1\">");
                    CreateHTMLMenu(menu.children, sClass, 1);
                    sb.AppendLine("</ul>");
                }
                sb.AppendLine("</li>");
                sb.AppendLine("</li>");
            }
        }

        public static IList<GetMenuRoleAccessDetail_Result> GetListMenuAccess(int RoleID, int ViewHome)
        {
            using (var db = new Data.RepositoryFactory(new Data.EfDbContext()))
            {
                List<SqlParameter> parameterList = new List<SqlParameter>();
                parameterList.Add(new SqlParameter("@RoleID", RoleID));
                parameterList.Add(new SqlParameter("@ViewHome", ViewHome));
                SqlParameter[] parameters = parameterList.ToArray();

                var data = db.DbContext.Database.SqlQuery<GetMenuRoleAccessDetail_Result>("dbo.GetMenuRoleAccess @RoleID,@ViewHome", parameters).ToList();
                return data;
            }
        }

        public List<RoleAccessDetailsMenu> CreateDataMenu(int RoleID, int ViewHome)
        {
            if (RoleID == 24)
                ViewHome = 1;

            var menus = GetListMenuAccess(RoleID, ViewHome).OrderBy(p => p.OrderNo).ToList();

            List<RoleAccessDetailsMenu> hierarchy = new List<RoleAccessDetailsMenu>();

            hierarchy.AddRange(menus
                            .Where(m => m.ParentID == 1)
                            .Select(m => new RoleAccessDetailsMenu
                            {
                                ID = m.ID,
                                ParentID = m.ParentID,
                                Name = m.Name,
                                URL = m.URL,
                                icon = m.icon,
                                OrderNo = m.OrderNo,
                                children = GetSubMenuGroupDetail(menus, m.ID).ToList()
                            })
                            .ToList());
            return hierarchy.OrderBy(p => p.OrderNo).ToList();
        }

        public static List<RoleAccessDetailsMenu> GetSubMenuGroupDetail(List<GetMenuRoleAccessDetail_Result> menus, int parentID)
        {
            return menus
                    .Where(m => m.ParentID == parentID)
                    .Select(m => new RoleAccessDetailsMenu
                    {
                        ID = m.ID,
                        ParentID = m.ParentID,
                        Name = m.Name,
                        URL = m.URL,
                        icon = m.icon,
                        children = GetSubMenuGroupDetail(menus, m.ID)
                    })
                    .ToList();
        }


    }
}