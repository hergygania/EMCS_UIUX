using App.Data.Domain;
using App.Domain;
using App.Web.App_Start;
using App.Web.Helper;
using App.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace App.Web.Controllers
{
    public partial class MasterController
    {
        #region Initilize
        private MenuModel InitilizeMenu(int menuId)
        {
            var menu = new MenuModel();
            var menuData = new Menu();
            menu.Menu = menuData;
            menu.MenuList = Service.Master.Menus.GetSelectMenuList();
            if (menuId == 0)
                return menu;

            menu.Menu = Service.Master.Menus.GetId(menuId);
            return menu;
        }
        #endregion
        // GET: Menu

        //[AuthorizeAcces(ActionType = AuthorizeAcces.IsRead)]
        public ActionResult Menu()
        {
            ViewBag.AllowRead = AuthorizeAcces.AllowRead;
            ViewBag.AllowCreate = AuthorizeAcces.AllowCreated;
            ViewBag.AllowUpdate = AuthorizeAcces.AllowUpdated;
            ViewBag.AllowDelete = AuthorizeAcces.AllowDeleted;
            PaginatorBoot.Remove("SessionTRN");
            return View();
        }

        [AuthorizeAcces(ActionType = AuthorizeAcces.IsRead, UrlMenu = "Menu")]
        public ActionResult MenuPage()
        {
            PaginatorBoot.Remove("SessionTRN");
            return MenuPageXt();
        }

        [AuthorizeAcces(ActionType = AuthorizeAcces.IsRead, UrlMenu = "Menu")]
        public ActionResult MenuPageXt()
        {
            Func<MasterSearchForm, IList<Menu>> func = delegate(MasterSearchForm crit)
            {
                List<Menu> list = Service.Master.Menus.GetMenuList(crit);
                return list.OrderBy(p => p.SelectedParent).ToList();
            };

            ActionResult paging = PaginatorBoot.Manage("SessionTRN", func).Pagination.ToJsonResult();
            return Json(paging, JsonRequestBehavior.AllowGet);
        }

        private string CheckParentMenu(MenuModel items)
        {
            if (items.Menu != null)
            {
                var NameExistDB = Service.Master.Menus.GetMenu(Convert.ToInt32(items.Menu.ParentID), items.Menu.Name);
                if (NameExistDB != null)
                {
                    return NameExistDB.ID.ToString();
                }
            }
            return string.Empty;
        }

        #region Region Create
        [AuthorizeAcces(ActionType = AuthorizeAcces.IsRead, UrlMenu = "Menu")]
        [HttpGet]
        public ActionResult MenuCreate()
        {
            ViewBag.crudMode = "I";
            var MenuData = InitilizeMenu(0);
            return PartialView("Menu.iud", MenuData);
        }

        [AuthorizeAcces(ActionType = AuthorizeAcces.IsCreated, UrlMenu = "Menu")]
        [HttpPost, ValidateInput(false)]
        public ActionResult MenuCreate(MenuModel items)
        {
            var ResultName = Service.Master.EmailRecipients.ValidateInputHtmlInjection(items.Menu.Name, "`^<>");
            var ResultURL = Service.Master.EmailRecipients.ValidateInputHtmlInjection(items.Menu.URL, "`^<>");
            var ResultIcon = Service.Master.EmailRecipients.ValidateInputHtmlInjection(items.Menu.Icon, "`^<>");
            var ResultOrderNo = Service.Master.EmailRecipients.ValidateInputHtmlInjection(items.Menu.OrderNo.ToString(), "`^<>");
            if (!ResultName)
            {
                return JsonMessage("Please Enter a Valid Name", 1, "i");
            }
            if (!ResultURL)
            {
                return JsonMessage("Please Enter a Valid URL", 1, "i");
            }
            if (!ResultIcon)
            {
                return JsonMessage("Please Enter a Valid Icon", 1, "i");
            }
            if (!ResultOrderNo)
            {
                return JsonMessage("Please Enter a Valid Order No", 1, "i");
            }

            ViewBag.crudMode = "I";
            var errors = ModelState.Values.SelectMany(v => v.Errors);
            if (ModelState.IsValid)
            {
                items.Menu.Name = Common.Sanitize(items.Menu.Name);
                items.Menu.URL = Common.Sanitize(items.Menu.URL);
                items.Menu.Icon = Common.Sanitize(items.Menu.Icon);

                var NameExistDB = Service.Master.Menus.GetMenu(Convert.ToInt32(items.Menu.ParentID), items.Menu.Name);
                if (NameExistDB != null)
                {
                    return JsonMessage("Menu : " + NameExistDB.Name + " with Parent : " + NameExistDB.SelectedParent + " already exists in the database", 1, NameExistDB);
                }

                Service.Master.Menus.crud(items.Menu, ViewBag.crudMode);
                return JsonCRUDMessage(ViewBag.crudMode);
            }


            return Json(new { success = false });
        }

        #endregion

        #region Region Edit
        [AuthorizeAcces(ActionType = AuthorizeAcces.IsRead, UrlMenu = "Menu")]
        [HttpGet]
        public ActionResult MenuEdit(int id)
        {
            ViewBag.crudMode = "U";
            var menu = InitilizeMenu(id);
            if (menu == null)
            {
                return HttpNotFound();
            }

            return PartialView("Menu.iud", menu);
        }

        [AuthorizeAcces(ActionType = AuthorizeAcces.IsUpdated, UrlMenu = "Menu")]
        [HttpPost, ValidateInput(false)]
        [ValidateAntiForgeryToken]
        public ActionResult MenuEdit(MenuModel items)
        {
            var ResultName = Service.Master.EmailRecipients.ValidateInputHtmlInjection(items.Menu.Name, "`^<>");
            var ResultURL = Service.Master.EmailRecipients.ValidateInputHtmlInjection(items.Menu.URL, "`^<>");
            var ResultIcon = Service.Master.EmailRecipients.ValidateInputHtmlInjection(items.Menu.Icon, "`^<>");
            var ResultOrderNo = Service.Master.EmailRecipients.ValidateInputHtmlInjection(items.Menu.OrderNo.ToString(), "`^<>");
            if (!ResultName)
            {
                return JsonMessage("Please Enter a Valid Name", 1, "i");
            }
            if (!ResultURL)
            {
                return JsonMessage("Please Enter a Valid URL", 1, "i");
            }
            if (!ResultIcon)
            {
                return JsonMessage("Please Enter a Valid Icon", 1, "i");
            }
            if (!ResultOrderNo)
            {
                return JsonMessage("Please Enter a Valid Order No", 1, "i");
            }

            ViewBag.crudMode = "U";

            if (CheckParentMenu(items) != "")
            {
                if (items.Menu.ID.ToString() != CheckParentMenu(items))
                    return JsonMessage("Menu : " + items.Menu.Name + " with Parent : " + items.Menu.SelectedParent + " already exists in the database", 1, items.Menu);
            }

            if (ModelState.IsValid)
            {
                Service.Master.Menus.crud(items.Menu, ViewBag.crudMode);
                return JsonCRUDMessage(ViewBag.crudMode);
            }
            return Json(new { success = false });
        }

        #endregion

        #region Region Delete
        [AuthorizeAcces(ActionType = AuthorizeAcces.IsRead, UrlMenu = "Menu")]
        [HttpGet]
        public ActionResult MenuDelete(string id)
        {
            ViewBag.crudMode = "D";
            UserViewModel MenuData = InitilizeData(id);
            if (MenuData.User == null)
            {
                return HttpNotFound();
            }

            return PartialView("Menu.iud", MenuData);
        }

        [AuthorizeAcces(ActionType = AuthorizeAcces.IsDeleted, UrlMenu = "Menu")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult MenuDelete(MenuModel items)
        {
            ViewBag.crudMode = "D";
            if (ModelState.IsValid)
            {
                Service.Master.Menus.crud(
                    items.Menu,
                    ViewBag.crudMode);
                return JsonCRUDMessage(ViewBag.crudMode);
            }
            return PartialView("Menu.iud", items);
        }

        [AuthorizeAcces(ActionType = AuthorizeAcces.IsDeleted, UrlMenu = "Menu")]
        [HttpPost]
        public ActionResult MenuDeleteById(int RegionId)
        {
            Menu item = Service.Master.Menus.GetId(RegionId);
            Service.Master.Menus.crud(
                item,
                "D");
            return JsonCRUDMessage("D");
        }

        [HttpPost]
        public ActionResult MenuIsActiveById(int ID)
        {
            var ModifiedBy = Domain.SiteConfiguration.UserName;
            try
            {
                var db = new Data.RepositoryFactory(new Data.EfDbContext());
                string query = "";
                query = @"Update Master_Menu set isActive = 0, ModifiedDate = GETDATE(), 
                            ModifiedBy = '" + ModifiedBy.ToString() + "' where ID= " + ID + "";

                db.DbContext.Database.ExecuteSqlCommand(query);
                return JsonCRUDMessage("D");
            }
            catch (Exception ex)
            {
                return Json(new { result = "failed", message = ex.Message }, JsonRequestBehavior.DenyGet);
            }
        }

        #endregion
    }
}