using App.Data.Domain;
using App.Data.Domain.Extensions;
using App.Web.App_Start;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using App.Web.Models;

namespace App.Web.Controllers
{
    public partial class MasterController
    {
        
        //[AuthorizeAcces(ActionType = AuthorizeAcces.IsRead)]
        public ActionResult RoleAccessMenu()
        {
            ViewBag.AllowRead = AuthorizeAcces.AllowRead;
            ViewBag.AllowCreate = AuthorizeAcces.AllowCreated;
            ViewBag.AllowUpdate = AuthorizeAcces.AllowUpdated;
            ViewBag.AllowDelete = AuthorizeAcces.AllowDeleted;           
            PaginatorBoot.Remove("SessionTRN");
            return View();
        }

        public JsonResult getMasterRole()
        {
            var data1 = Service.Master.RoleAccessMenu.GetSelectRoleList();
            return Json(data1.ToList(), JsonRequestBehavior.AllowGet);
        }

        [AuthorizeAcces(ActionType = AuthorizeAcces.IsRead, UrlMenu = "RoleAccessMenu")]
        public ActionResult GetRoleMenuList(int RoleID = 1)
        {
            var menu = Service.Master.RoleAccessMenu.GetRoleMenuList(RoleID);
            return Json(menu, JsonRequestBehavior.AllowGet);
        }

         [HttpPost]
        public JsonResult UpdateRoleAccessMenuDetail(List<RoleAccessDetailsMenu> json)
        {
            try
            {
                var data = (from p in json select p).FirstOrDefault();
                RoleAccessMenu item = Service.Master.RoleAccessMenu.GetRoleAccessMenuByRoleID(Convert.ToInt32(data.RoleID));

                if (item != null)
                    Service.Master.RoleAccessMenu.DeleteRole(Convert.ToInt32(data.RoleID));

                Service.Master.RoleAccessMenu.InsertRoleAccessMenu(json);

                var menu = Service.Master.RoleAccessMenu.GetRoleMenuList(Convert.ToInt32(data.RoleID));
                return Json(menu, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                return Json(e.Message, JsonRequestBehavior.AllowGet);
            }
        }

    }
}