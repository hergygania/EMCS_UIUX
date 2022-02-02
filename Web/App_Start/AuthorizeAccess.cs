using App.Data.Caching;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace App.Web.App_Start
{
    public class AuthorizeAcces : AuthorizeAttribute
    {
        public const string IsRead = "Read";
        public const string IsCreated = "Created";
        public const string IsUpdated = "Updated";
        public const string IsDeleted = "Deleted";
        //private const string cacheName = "AuthorizeAcces";
        //private readonly static ICacheManager _cacheManager = new MemoryCacheManager();
        public static bool AllowRead { get; private set; }
        public static bool AllowCreated { get; private set; }
        public static bool AllowUpdated { get; private set; }
        public static bool AllowDeleted { get; private set; }
        public string ActionType { get; set; }
        public string UrlMenu { get; set; }
        public int ID { get; set; }

        protected override void HandleUnauthorizedRequest(AuthorizationContext filterContext)
        {
            filterContext.Result = new RedirectToRouteResult(
                 new RouteValueDictionary(
                     new
                     {
                         controller = "Shared",
                         action = "Unauthorised"
                     })
                 );
        }

        protected override bool AuthorizeCore(HttpContextBase httpContext)
        {
            using (var db = new Data.EfDbContext())
            {
                bool ret = false;
                string currentAction = string.Empty;
                string currentController = string.Empty;
                //string key = string.Format(cacheName);
                //var listUserRole = _cacheManager.Get(key, () =>
                //{

                string UserID = HttpContext.Current.User.Identity.Name;
                var UserRole = (from p in db.UserAccesses where p.UserID == UserID select p).FirstOrDefault();

                var userAccessRole = (from p in db.UserAccess_Role where p.UserID == UserID && p.RoleID != 1 select p.RoleID.ToString()).ToList();


                if (UserRole == null)
                    return false;

                if (UserRole.RoleID == 1)
                {
                    AllowRead = true;
                    AllowCreated = true;
                    AllowUpdated = true;
                    AllowDeleted = true;
                    return true;
                }

                var rd = httpContext.Request.RequestContext.RouteData;
                if (rd.GetRequiredString("Action").Contains("Edit"))
                    currentAction = rd.GetRequiredString("Action").Replace("Edit", "");
                else if (rd.GetRequiredString("Action").Contains("Delete"))
                    currentAction = rd.GetRequiredString("Action").Replace("Delete", "");
                else if (rd.GetRequiredString("Action").Contains("Create"))
                    currentAction = rd.GetRequiredString("Action").Replace("Create", "");
                else
                    currentAction = rd.GetRequiredString("Action");


                if (rd.GetRequiredString("controller").Contains("Vetting"))
                {
                    if (!currentAction.Contains("Manual"))
                    {
                        if (rd.GetRequiredString("freight").Contains("sea"))
                            currentAction = "/sea-freight";
                        else if (rd.GetRequiredString("freight").Contains("air"))
                            currentAction = "/air-freight";
                    }
                    else
                        currentAction = "/manual-vetting";

                    currentController = "vetting-process" + currentAction;
                }
                else
                    currentController = rd.GetRequiredString("controller") + "/" + currentAction;


                var stringMenu = "";

                if (UrlMenu != null)
                {
                    stringMenu = rd.GetRequiredString("controller") + "/" + UrlMenu;
                } else
                {
                    stringMenu = currentController;
                }

                var menu = (from p in db.Menus
                        where p.URL
                            == stringMenu && p.IsActive == true
                        select p).FirstOrDefault();

                if (menu == null)
                    return false;

                var data = (from p in db.RoleAccessMenus
                            where p.MenuID == menu.ID
                            && p.RoleID == UserRole.RoleID
                            //&& userAccessRole.Contains(p.RoleID.ToString())
                            select p).FirstOrDefault();

                if (data == null) return false;

                switch (ActionType)
                {
                    case IsRead:
                        ret = Convert.ToBoolean(data.IsRead);
                        AllowRead = Convert.ToBoolean(data.IsRead);
                        AllowCreated = Convert.ToBoolean(data.IsCreate);
                        AllowUpdated = Convert.ToBoolean(data.IsUpdated);
                        AllowDeleted = Convert.ToBoolean(data.IsDeleted);
                        break;
                    case IsCreated:
                        ret = Convert.ToBoolean(data.IsCreate);
                        AllowRead = Convert.ToBoolean(data.IsRead);
                        AllowCreated = Convert.ToBoolean(data.IsCreate);
                        AllowUpdated = Convert.ToBoolean(data.IsUpdated);
                        AllowDeleted = Convert.ToBoolean(data.IsDeleted);
                        break;
                    case IsUpdated:
                        ret = Convert.ToBoolean(data.IsUpdated);
                        AllowRead = Convert.ToBoolean(data.IsRead);
                        AllowCreated = Convert.ToBoolean(data.IsUpdated);
                        AllowUpdated = Convert.ToBoolean(data.IsUpdated);
                        AllowDeleted = Convert.ToBoolean(data.IsDeleted);
                        break;
                    case IsDeleted:
                        ret = Convert.ToBoolean(data.IsDeleted);
                        AllowRead = Convert.ToBoolean(data.IsRead);
                        AllowCreated = Convert.ToBoolean(data.IsCreate);
                        AllowUpdated = Convert.ToBoolean(data.IsUpdated);
                        AllowDeleted = Convert.ToBoolean(data.IsDeleted);
                        break;
                }

                return ret;
            }
        }
    }
}