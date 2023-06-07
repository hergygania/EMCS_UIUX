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
        private MasterSOS InitilizeMasterSOS(int itemid)
        {
            var item = new MasterSOS();
            if (itemid == 0)
                return item;

            item = Service.CAT.Master.MasterSOS.GetSOSbyID(itemid);
            return item;
        }

        //[AuthorizeAcces(ActionType = AuthorizeAcces.IsRead)]
        public ActionResult MasterSOS()
        {
            ViewBag.AllowRead = AuthorizeAcces.AllowRead;
            ViewBag.AllowCreate = AuthorizeAcces.AllowCreated;
            ViewBag.AllowUpdate = AuthorizeAcces.AllowUpdated;
            ViewBag.AllowDelete = AuthorizeAcces.AllowDeleted;
            PaginatorBoot.Remove("SessionTRN");
            return View();
        }
        public ActionResult MasterSOSPage()
        {
            PaginatorBoot.Remove("SessionTRN");
            return MasterSOSPageXt();
        }

        public ActionResult MasterSOSPageXt()
        {
            Func<MasterSearchForm, IList<MasterSOS>> func = delegate(MasterSearchForm crit)
            {
                List<MasterSOS> list = Service.CAT.Master.MasterSOS.GetList(crit);
                return list.OrderBy(o => o.ModifiedDate).ToList();
            };

            ActionResult paging = PaginatorBoot.Manage("SessionTRN", func).Pagination.ToJsonResult();
            return Json(paging, JsonRequestBehavior.AllowGet);
        }

        public ActionResult MasterSOSbyID(int sos)
        {
            MasterSOS list = Service.CAT.Master.MasterSOS.GetId(sos);

            //Func<MasterSearchForm, IList<MasterSOS>> func = delegate(MasterSearchForm crit)
            //{
               
            //    return list.OrderBy(o => o.ModifiedDate).ToList();
            //};

            //ActionResult paging = PaginatorBoot.Manage("SessionTRN", func).Pagination.ToJsonResult();
            return Json(list, JsonRequestBehavior.AllowGet);
        }

        [AuthorizeAcces(ActionType = AuthorizeAcces.IsRead, UrlMenu = "MasterSOS")]
        [HttpGet]
        public ActionResult MasterSOSCreate()
        {
            ViewBag.crudMode = "I";
            var item = InitilizeMasterSOS(0);
            return PartialView("MasterSOS.iud", item);
        }

        [AuthorizeAcces(ActionType = AuthorizeAcces.IsCreated, UrlMenu = "MasterSOS")]
        [HttpPost]
        public ActionResult MasterSOSCreate(MasterSOS item)
        {
            ViewBag.crudMode = "I";

            if (Service.CAT.Master.MasterSOS.ExistMasterSOS(item) != "")
                return JsonMessage("Master SOS : " + item.SOS + " already exists in the database.", 1, Service.CAT.Master.MasterSOS.ExistMasterSOS(item));

            if (ModelState.IsValid)
            {
                Service.CAT.Master.MasterSOS.crud(item, ViewBag.crudMode);
                return JsonCRUDMessage(ViewBag.crudMode);
            }

            return Json(new { success = false });
        }

        [AuthorizeAcces(ActionType = AuthorizeAcces.IsRead, UrlMenu = "MasterSOS")]
        [HttpGet]
        public ActionResult MasterSOSEdit(int id)
        {
            ViewBag.crudMode = "U";
            var item = InitilizeMasterSOS(id);
            if (item == null)
            {
                return HttpNotFound();
            }

            return PartialView("MasterSOS.iud", item);
        }

        [AuthorizeAcces(ActionType = AuthorizeAcces.IsUpdated, UrlMenu = "MasterSOS")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult MasterSOSEdit(MasterSOS item)
        {
            ViewBag.crudMode = "U";

            //if (Service.CAT.Master.MasterSOS.ExistMasterSOS(item) != "")
            //{
            //    if (item.ID.ToString() != Service.CAT.Master.MasterSOS.ExistMasterSOS(item))
            //        return JsonMessage("Master SOS : " + item.SOS + " already exists in the database.", 1, Service.CAT.Master.MasterSOS.ExistMasterSOS(item));
            //}

            if (ModelState.IsValid)
            {
                Service.CAT.Master.MasterSOS.crud(item, ViewBag.crudMode);
                return JsonCRUDMessage(ViewBag.crudMode);
            }
            return Json(new { success = false });
        }

        [AuthorizeAcces(ActionType = AuthorizeAcces.IsRead, UrlMenu = "MasterSOS")]
        [HttpGet]
        public ActionResult MasterSOSDelete(string id)
        {
            ViewBag.crudMode = "D";
            UserViewModel item = new UserViewModel();
            if (item.User == null)
            {
                return HttpNotFound();
            }

            return PartialView("MasterSOS.iud", item);
        }

        [AuthorizeAcces(ActionType = AuthorizeAcces.IsDeleted, UrlMenu = "MasterSOS")]
        [HttpPost]
        public ActionResult MasterSOSDelete(int id)
        {
            try
            {
                MasterSOS item = Service.CAT.Master.MasterSOS.GetId(id);
                item.IsActive = false;
                Service.CAT.Master.MasterSOS.crud(item, "U");
                return JsonCRUDMessage("D");
            }
            catch (Exception ex)
            {
                return Json(new { result = "failed", message = ex.Message }, JsonRequestBehavior.DenyGet);
            }
        }
	}
}