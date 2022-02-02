using App.Data.Caching;
using App.Data.Domain;
using App.Domain;
using App.Web.App_Start;
using App.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
namespace App.Web.Controllers.CAT
{
    public partial class CATController
    {
        private MasterJobLocation InitilizeMasterJobLocation(int itemid)
        {
            var item = new MasterJobLocation();
            if (itemid == 0)
                return item;

            item = Service.CAT.Master.MasterJobLocation.GetId(itemid);
            return item;
        }

        //[AuthorizeAcces(ActionType = AuthorizeAcces.IsRead)]
        public ActionResult MasterJobLocation()
        {
            ViewBag.AllowRead = AuthorizeAcces.AllowRead;
            ViewBag.AllowCreate = AuthorizeAcces.AllowCreated;
            ViewBag.AllowUpdate = AuthorizeAcces.AllowUpdated;
            ViewBag.AllowDelete = AuthorizeAcces.AllowDeleted;
            PaginatorBoot.Remove("SessionTRN");
            return View();
        }
        public ActionResult MasterJobLocationPage()
        {
            PaginatorBoot.Remove("SessionTRN");
            return MasterJobLocationPageXt();
        }

        public ActionResult MasterJobLocationPageXt()
        {
            Func<MasterSearchForm, IList<MasterJobLocation>> func = delegate(MasterSearchForm crit)
            {
                List<MasterJobLocation> list = Service.CAT.Master.MasterJobLocation.GetList(crit);
                return list.OrderBy(o => o.ModifiedDate).ToList();
            };

            ActionResult paging = PaginatorBoot.Manage("SessionTRN", func).Pagination.ToJsonResult();
            return Json(paging, JsonRequestBehavior.AllowGet);
        }

        [AuthorizeAcces(ActionType = AuthorizeAcces.IsRead, UrlMenu = "MasterJobLocation")]
        [HttpGet]
        public ActionResult MasterJobLocationCreate()
        {
            ViewBag.crudMode = "I";
            var item = InitilizeMasterJobLocation(0);
            return PartialView("MasterJobLocation.iud", item);
        }

        [AuthorizeAcces(ActionType = AuthorizeAcces.IsCreated, UrlMenu = "MasterJobLocation")]
        [HttpPost]
        public ActionResult MasterJobLocationCreate(MasterJobLocation item)
        {
            ViewBag.crudMode = "I";

            if (Service.CAT.Master.MasterJobLocation.ExistMasterJobLocation(item) != "")
                return JsonMessage("Master Job Location : " + item.JobLocation + " already exists in the database.", 1, Service.CAT.Master.MasterJobLocation.ExistMasterJobLocation(item));

            if (ModelState.IsValid)
            {
                Service.CAT.Master.MasterJobLocation.crud(item, ViewBag.crudMode);
                return JsonCRUDMessage(ViewBag.crudMode);
            }

            return Json(new { success = false });
        }

        [AuthorizeAcces(ActionType = AuthorizeAcces.IsRead, UrlMenu = "MasterJobLocation")]
        [HttpGet]
        public ActionResult MasterJobLocationEdit(int id)
        {
            ViewBag.crudMode = "U";
            var item = InitilizeMasterJobLocation(id);
            if (item == null)
            {
                return HttpNotFound();
            }

            return PartialView("MasterJobLocation.iud", item);
        }

        [AuthorizeAcces(ActionType = AuthorizeAcces.IsUpdated, UrlMenu = "MasterJobLocation")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult MasterJobLocationEdit(MasterJobLocation item)
        {
            ViewBag.crudMode = "U";

            if (Service.CAT.Master.MasterJobLocation.ExistMasterJobLocation(item) != "")
            {
                if (item.ID.ToString() != Service.CAT.Master.MasterJobLocation.ExistMasterJobLocation(item))
                    return JsonMessage("Master Master JobLocation : " + item.JobLocation + " already exists in the database.", 1, Service.CAT.Master.MasterJobLocation.ExistMasterJobLocation(item));
            }

            if (ModelState.IsValid)
            {
                Service.CAT.Master.MasterJobLocation.crud(item, ViewBag.crudMode);
                return JsonCRUDMessage(ViewBag.crudMode);
            }
            return Json(new { success = false });
        }

        [AuthorizeAcces(ActionType = AuthorizeAcces.IsRead, UrlMenu = "MasterJobLocation")]
        [HttpGet]
        public ActionResult MasterJobLocationDelete(string id)
        {
            ViewBag.crudMode = "D";
            UserViewModel item = new UserViewModel(); //InitilizeData(id);
            if (item.User == null)
            {
                return HttpNotFound();
            }

            return PartialView("MasterJobLocation.iud", item);
        }

        [AuthorizeAcces(ActionType = AuthorizeAcces.IsDeleted, UrlMenu = "MasterJobLocation")]
        [HttpPost]
        public ActionResult MasterJobLocationDelete(int id)
        {
            try
            {
                MasterJobLocation item = Service.CAT.Master.MasterJobLocation.GetId(id);
                item.IsActive = false;
                Service.CAT.Master.MasterJobLocation.crud(item, "U");
                return JsonCRUDMessage("D");
            }
            catch (Exception ex)
            {
                return Json(new { result = "failed", message = ex.Message }, JsonRequestBehavior.DenyGet);
            }
        }
	}
}