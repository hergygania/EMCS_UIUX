using App.Data.Caching;
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
    public class MvcMenuHelpers
    {
        public static string menuHTML = string.Empty;
        private static StringBuilder sb = new StringBuilder();
        public static string UserName = string.Empty;
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
                        UserName = Domain.SiteConfiguration.UserName.ToString();
                    }

                    var UserRole = (from p in db.UserAccess_Role
                                    where p.UserID == UserName
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


                    menuHTML = sb.ToString();

                    return new MvcHtmlString(menuHTML.Replace("'", "\""));
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
<<<<<<< HEAD
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
=======

<<<<<<< HEAD
            baseUrl = string.Format("{0}://{1}{2}", request.Url.Scheme, request.Url.Authority, appUrl);
<<<<<<< HEAD
>>>>>>> 639d8d0 (Intial commit)
=======
=======
            if (!request.Url.Authority.Contains("localhost"))
            {

                baseUrl = string.Format("{0}://{1}{2}", request.Url.Scheme, "staging.mkindo.com:5181", appUrl);
            }
            else
            {
                baseUrl = string.Format("{0}://{1}{2}", request.Url.Scheme, request.Url.Authority, appUrl);
            }
>>>>>>> d3e2e7a (Tasks from P1-CIPL , P1-CL , P!-SS , P!-SI , P1-BL/AWB & P1-PEB_NPE)
            //baseUrl = string.Format("{0}://{1}:{2}{3}", request.Url.Scheme, request.Url.Host, request.Url.Port, appUrl);
>>>>>>> b773f28 (intial commit for changes from himanshu and vijendra)

            foreach (var menu in Menus)
            {
                if (menu.Name == "Home")
                    sb.AppendLine("<li class=\"treeview active\">");

                if (process == 0 && menu.Name != "Home")
                    sb.AppendLine("<li class='" + sClass + "'>");
                else if (process == 1 && menu.Name != "Home")
                    sb.AppendLine("<li>");

                sb.AppendLine("<a " + Convert.ToString((string.IsNullOrEmpty(menu.URL)) ? ">"
                    : (menu.URL.Trim().Substring(0, 1) != "/" && menu.URL.Trim().Substring(0, 1) != @"\")
                    ? "href='" + baseUrl + "/" + menu.URL.Replace(@"\", "/") + "'>"
                    : "href='" + baseUrl + "/" + menu.URL.Replace(@"\", "/") + "'>"));

                if (menu.children.Count() <= 0)
                {
                    sb.AppendLine("<i class=\"" + menu.icon + "\"></i>" + menu.Name + "");
                    sb.AppendLine("</a>");
                }
                else
                {
                    sb.AppendLine("<i class='" + menu.icon + "'></i><span>" + menu.Name + "</span><i class=\"fa fa-angle-left pull-right\"></i>");
                    sb.AppendLine("</a>");
                }

                if (menu.children.Count() > 0)
                {
                    sb.AppendLine("<ul class=\"treeview-menu\">");
                    CreateHTMLMenu(menu.children, sClass, 1);
                    sb.AppendLine("</ul>");
                }
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

<<<<<<< HEAD
<<<<<<< HEAD
        public List<RoleAccessDetailsMenu> CreateDataMenu(int RoleID, int ViewHome)
=======
        public List<RoleAccessDetailsMenu> CreateDataMenu(int RoleID,int ViewHome)
>>>>>>> 639d8d0 (Intial commit)
=======
        public List<RoleAccessDetailsMenu> CreateDataMenu(int RoleID, int ViewHome)
>>>>>>> d3e2e7a (Tasks from P1-CIPL , P1-CL , P!-SS , P!-SI , P1-BL/AWB & P1-PEB_NPE)
        {
            if (RoleID == 24)
                ViewHome = 1;

<<<<<<< HEAD
<<<<<<< HEAD
            var menus = GetListMenuAccess(RoleID, ViewHome).OrderBy(p => p.OrderNo).ToList();
=======
            var menus = GetListMenuAccess(RoleID,ViewHome).OrderBy(p => p.OrderNo).ToList();
>>>>>>> 639d8d0 (Intial commit)
=======
            var menus = GetListMenuAccess(RoleID, ViewHome).OrderBy(p => p.OrderNo).ToList();
>>>>>>> d3e2e7a (Tasks from P1-CIPL , P1-CL , P!-SS , P!-SI , P1-BL/AWB & P1-PEB_NPE)

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