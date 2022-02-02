using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using App.Data.Domain;
using App.Domain;
using App.Web.Models;
using App.Web.App_Start;
using App.Web.Helper;

namespace App.Web.Controllers
{
    public partial class MasterController
    {

        #region Initilize
        private Area InitilizeArea(int areaId)
        {
            var area = new Area();
            if (areaId==0)
            {
                return area;
            }
            area = Service.Master.Area.GetId(areaId);
            return area;
        }
        #endregion
        // GET: Master Controller 
        //[AuthorizeAcces(ActionType = AuthorizeAcces.IsRead)]
        public ActionResult AreaManagement()
        {
            ViewBag.AllowRead = AuthorizeAcces.AllowRead;
            ViewBag.AllowCreate = AuthorizeAcces.AllowCreated;
            ViewBag.AllowUpdate = AuthorizeAcces.AllowUpdated;
            ViewBag.AllowDelete = AuthorizeAcces.AllowDeleted;
            PaginatorBoot.Remove("SessionTRN");
            return View();
        }
        [AuthorizeAcces(ActionType = AuthorizeAcces.IsRead, UrlMenu = "AreaManagement")]
        public ActionResult AreaManagementPage()
        {
            PaginatorBoot.Remove("SessionTRN");
            return AreaManagementPageXt();
        }
        [AuthorizeAcces(ActionType = AuthorizeAcces.IsRead, UrlMenu = "AreaManagement")]
        public ActionResult AreaManagementPageXt()
        {
            Func<MasterSearchForm, IList<Area>> func = delegate(MasterSearchForm crit)
            {
                List<Area> list = Service.Master.Area.GetList(crit);
                return list.OrderBy(o => o.ModifiedDate).ToList();
            };

            ActionResult paging = PaginatorBoot.Manage("SessionTRN", func).Pagination.ToJsonResult();
            return Json(paging, JsonRequestBehavior.AllowGet);
        }

        [AuthorizeAcces(ActionType = AuthorizeAcces.IsRead, UrlMenu = "AreaManagement")]
        [HttpGet]
        public ActionResult AreaManagementCreate()
        {
            ViewBag.crudMode = "I";
            var AreaData = InitilizeArea(0);
            return PartialView("AreaManagement.iud", AreaData);
        }

        [AuthorizeAcces(ActionType = AuthorizeAcces.IsCreated, UrlMenu = "AreaManagement")]
        [HttpPost, ValidateInput(false)]
        public ActionResult AreaManagementCreate(Area items)
        {
            ViewBag.crudMode = "I";
            var ResultName = Service.Master.EmailRecipients.ValidateInputHtmlInjection(items.Name, "`^<>");
            var ResultDesc = Service.Master.EmailRecipients.ValidateInputHtmlInjection(items.Description, "`^<>");

            if (!ResultName)
            {
                return JsonMessage("Please Enter a Valid Name", 1, "i");
            }
            if (!ResultDesc)
            {
                return JsonMessage("Please Enter a Valid Description", 1, "i");
            }

            if (ModelState.IsValid)
            {
                items.Name = Common.Sanitize(items.Name);
                items.Description = Common.Sanitize(items.Description);

                Service.Master.Area.Update(
                    items,
                    ViewBag.crudMode);
                return JsonCRUDMessage(ViewBag.crudMode); //return Json(new { success = true });
            }
            return Json(new { success = false });

            //return PartialView("AreaManagement.iud", items);
        }
        [AuthorizeAcces(ActionType = AuthorizeAcces.IsRead, UrlMenu = "AreaManagement")]
        [HttpGet]
        public ActionResult AreaManagementEdit(int id)
        {
            ViewBag.crudMode = "U";
            var area = InitilizeArea(id);
            if (area == null)
            {
                return HttpNotFound();
            }

            return PartialView("AreaManagement.iud", area);
        }

        [AuthorizeAcces(ActionType = AuthorizeAcces.IsUpdated, UrlMenu = "AreaManagement")]
        [HttpPost, ValidateInput(false)]
        [ValidateAntiForgeryToken]
        public ActionResult AreaManagementEdit(Area items)
        {
            ViewBag.crudMode = "U";
            var ResultName = Service.Master.EmailRecipients.ValidateInputHtmlInjection(items.Name, "`^<>");
            var ResultDesc = Service.Master.EmailRecipients.ValidateInputHtmlInjection(items.Description, "`^<>");

            if (!ResultName)
            {
                return JsonMessage("Please Enter a Valid Name", 1, "i");
            }
            if (!ResultDesc)
            {
                return JsonMessage("Please Enter a Valid Description", 1, "i");
            }

            if (ModelState.IsValid)
            {
                Service.Master.Area.Update(
                    items,
                    ViewBag.crudMode);

                return JsonCRUDMessage(ViewBag.crudMode);
            }
            return Json(new { success = false });
        }

        [HttpGet]
        public ActionResult AreaManagementDelete(string id)
        {
            ViewBag.crudMode = "D";
            UserViewModel AreaData = InitilizeData(id);
            if (AreaData.User == null)
            {
                return HttpNotFound();
            }

            return PartialView("AreaManagement.iud", AreaData);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AreaManagementDelete(Area items)
        {
            ViewBag.crudMode = "D";
            if (ModelState.IsValid)
            {
                Service.Master.Area.Update(
                    items,
                    ViewBag.crudMode);
                return JsonCRUDMessage(ViewBag.crudMode);
            }
            return PartialView("AreaManagement.iud", items);
        }

        [HttpPost]
        public ActionResult AreaManagementDeleteById(int AreaId)
        {
            Area item = Service.Master.Area.GetId(AreaId);
            Service.Master.Area.Update(
                item,
                "D");
            return JsonCRUDMessage("D");
        }
    }
}