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
        private MasterJobCode InitilizeMasterJobCode(int itemid)
        {
            var item = new MasterJobCode();
            if (itemid == 0)
                return item;

            item = Service.CAT.Master.MasterJobCode.GetId(itemid);
            return item;
        }

        //[AuthorizeAcces(ActionType = AuthorizeAcces.IsRead)]
        public ActionResult MasterJobCode()
        {
            ViewBag.AllowRead = AuthorizeAcces.AllowRead;
            ViewBag.AllowCreate = AuthorizeAcces.AllowCreated;
            ViewBag.AllowUpdate = AuthorizeAcces.AllowUpdated;
            ViewBag.AllowDelete = AuthorizeAcces.AllowDeleted;
            PaginatorBoot.Remove("SessionTRN");
            return View();
        }
        public ActionResult MasterJobCodePage()
        {
            PaginatorBoot.Remove("SessionTRN");
            return MasterJobCodePageXt();
        }

        public ActionResult MasterJobCodePageXt()
        {
            Func<MasterSearchForm, IList<MasterJobCode>> func = delegate(MasterSearchForm crit)
            {
                List<MasterJobCode> list = Service.CAT.Master.MasterJobCode.GetList(crit);
                return list.OrderBy(o => o.ModifiedDate).ToList();
            };

            ActionResult paging = PaginatorBoot.Manage("SessionTRN", func).Pagination.ToJsonResult();
            return Json(paging, JsonRequestBehavior.AllowGet);
        }

        [AuthorizeAcces(ActionType = AuthorizeAcces.IsRead, UrlMenu = "MasterJobCode")]
        [HttpGet]
        public ActionResult MasterJobCodeCreate()
        {
            ViewBag.crudMode = "I";
            var item = InitilizeMasterJobCode(0);
            return PartialView("MasterJobCode.iud", item);
        }

        [AuthorizeAcces(ActionType = AuthorizeAcces.IsCreated, UrlMenu = "MasterJobCode")]
        [HttpPost]
        public ActionResult MasterJobCodeCreate(MasterJobCode item)
        {
            ViewBag.crudMode = "I";

            if (Service.CAT.Master.MasterJobCode.ExistMasterJobCode(item) != "")
                return JsonMessage("Master Job Code : " + item.JobCode + " already exists in the database.", 1, Service.CAT.Master.MasterJobCode.ExistMasterJobCode(item));

            if (ModelState.IsValid)
            {
                Service.CAT.Master.MasterJobCode.crud(item, ViewBag.crudMode);
                return JsonCRUDMessage(ViewBag.crudMode);
            }

            return Json(new { success = false });
        }

        [AuthorizeAcces(ActionType = AuthorizeAcces.IsRead, UrlMenu = "MasterJobCode")]
        [HttpGet]
        public ActionResult MasterJobCodeEdit(int id)
        {
            ViewBag.crudMode = "U";
            var item = InitilizeMasterJobCode(id);
            if (item == null)
            {
                return HttpNotFound();
            }

            return PartialView("MasterJobCode.iud", item);
        }

        [AuthorizeAcces(ActionType = AuthorizeAcces.IsUpdated, UrlMenu = "MasterJobCode")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult MasterJobCodeEdit(MasterJobCode item)
        {
            ViewBag.crudMode = "U";

            if (Service.CAT.Master.MasterJobCode.ExistMasterJobCode(item) != "")
            {
                if (item.ID.ToString() != Service.CAT.Master.MasterJobCode.ExistMasterJobCode(item))
                    return JsonMessage("Master Job Code : " + item.JobCode + " already exists in the database.", 1, Service.CAT.Master.MasterJobCode.ExistMasterJobCode(item));
            }

            if (ModelState.IsValid)
            {
                Service.CAT.Master.MasterJobCode.crud(item, ViewBag.crudMode);
                return JsonCRUDMessage(ViewBag.crudMode);
            }
            return Json(new { success = false });
        }

        [AuthorizeAcces(ActionType = AuthorizeAcces.IsRead, UrlMenu = "MasterJobCode")]
        [HttpGet]
        public ActionResult MasterJobCodeDelete(string id)
        {
            ViewBag.crudMode = "D";
            UserViewModel item = new UserViewModel();
            if (item.User == null)
            {
                return HttpNotFound();
            }

            return PartialView("MasterJobCode.iud", item);
        }

        [AuthorizeAcces(ActionType = AuthorizeAcces.IsDeleted, UrlMenu = "MasterJobCode")]
        [HttpPost]
        public ActionResult MasterJobCodeDelete(int id)
        {
            try
            {
                MasterJobCode item = Service.CAT.Master.MasterJobCode.GetId(id);
                item.IsActive = false;
                Service.CAT.Master.MasterJobCode.crud(item, "U");
                return JsonCRUDMessage("D");
            }
            catch (Exception ex)
            {
                return Json(new { result = "failed", message = ex.Message }, JsonRequestBehavior.DenyGet);
            }
        }
	}
}