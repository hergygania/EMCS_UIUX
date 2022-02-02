

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
        private StoreViewModel InitilizeStore(int storeId)
        {
            var store = new StoreViewModel();
            var storeData = new Store();
            store.Store = storeData;
            store.HubList = Service.Master.Hub.GetList();
            store.AreaList = Service.Master.Area.GetList();
            store.RegionList = Service.Master.Region.GetRegionList().OrderBy(o => o.Name).ToList();
            store.Store = Service.Master.Stores.GetId(storeId);
            return store;
        }
        #endregion
        // GET: Master
        //[AuthorizeAcces(ActionType = AuthorizeAcces.IsRead)]
        public ActionResult StoreManagement()
        {
            ViewBag.AllowRead = AuthorizeAcces.AllowRead;
            ViewBag.AllowCreate = AuthorizeAcces.AllowCreated;
            ViewBag.AllowUpdate = AuthorizeAcces.AllowUpdated;
            ViewBag.AllowDelete = AuthorizeAcces.AllowDeleted;
            PaginatorBoot.Remove("SessionTRN");
            return View();
        }
        [AuthorizeAcces(ActionType = AuthorizeAcces.IsRead, UrlMenu = "StoreManagement")]
        public ActionResult StoreManagementPage()
        {
            PaginatorBoot.Remove("SessionTRN");
            return StoreManagementPageXt();
        }
        [AuthorizeAcces(ActionType = AuthorizeAcces.IsRead, UrlMenu = "StoreManagement")]
        public ActionResult StoreManagementPageXt()
        {
            Func<MasterSearchForm, IList<Store>> func = delegate(MasterSearchForm crit)
            {
                List<Store> list = Service.Master.Stores.GetList(crit);
                return list.OrderByDescending(o => o.ModifiedDate).ToList();
            };

            ActionResult paging = PaginatorBoot.Manage("SessionTRN", func).Pagination.ToJsonResult();
            return Json(paging, JsonRequestBehavior.AllowGet);
        }

        [AuthorizeAcces(ActionType = AuthorizeAcces.IsRead, UrlMenu = "StoreManagement")]
        [HttpGet]
        public ActionResult StoreManagementCreate()
        {
            ViewBag.crudMode = "I";
            var StoreData = InitilizeStore(0);
            return PartialView("StoreManagement.iud", StoreData);
        }

        [AuthorizeAcces(ActionType = AuthorizeAcces.IsCreated, UrlMenu = "StoreManagement")]
        [HttpPost, ValidateInput(false)]
        public ActionResult StoreManagementCreate(StoreViewModel items)
        {
            var ResultPlant = Service.Master.EmailRecipients.ValidateInputHtmlInjection(items.Store.Plant, "`^<>");
            var ResultName = Service.Master.EmailRecipients.ValidateInputHtmlInjection(items.Store.Name, "`^<>");
            var ResultC3LC = Service.Master.EmailRecipients.ValidateInputHtmlInjection(items.Store.C3LC, "`^<>");
            var ResultPrevName = Service.Master.EmailRecipients.ValidateInputHtmlInjection(items.Store.PrevName, "`^<>");
            var ResultDescription = Service.Master.EmailRecipients.ValidateInputHtmlInjection(items.Store.Description, "`^<>");

            if (!ResultPlant)
            {
                return JsonMessage("Please Enter a Valid Plant", 1, "i");
            }
            if (!ResultName)
            {
                return JsonMessage("Please Enter a Valid Name", 1, "i");
            }
            if (!ResultC3LC)
            {
                return JsonMessage("Please Enter a Valid C3LC", 1, "i");
            }
            if (!ResultPrevName)
            {
                return JsonMessage("Please Enter a Valid Prev Name", 1, "i");
            }
            if (!ResultDescription)
            {
                return JsonMessage("Please Enter a Valid Description", 1, "i");
            }

            ViewBag.crudMode = "I";
            if (ModelState.IsValid)
            {
                items.Store.Plant = Common.Sanitize(items.Store.Plant);
                items.Store.Name = Common.Sanitize(items.Store.Name);
                items.Store.C3LC = Common.Sanitize(items.Store.C3LC);
                items.Store.PrevName = Common.Sanitize(items.Store.PrevName);
                items.Store.Description = Common.Sanitize(items.Store.Description);

                Service.Master.Stores.Update(
                    items.Store,
                    ViewBag.crudMode);
                return JsonCRUDMessage(ViewBag.crudMode); //return Json(new { success = true });
            }
            return Json(new { success = false });

            //return PartialView("StoreManagement.iud", items);
        }

        [AuthorizeAcces(ActionType = AuthorizeAcces.IsRead, UrlMenu = "StoreManagement")]
        [HttpGet]
        public ActionResult StoreManagementEdit(int id)
        {
            ViewBag.crudMode = "U";
            var Store = InitilizeStore(id);
            if (Store == null)
            {
                return HttpNotFound();
            }

            return PartialView("StoreManagement.iud", Store);
        }

        [AuthorizeAcces(ActionType = AuthorizeAcces.IsUpdated, UrlMenu = "StoreManagement")]
        [HttpPost, ValidateInput(false)]
        [ValidateAntiForgeryToken]
        public ActionResult StoreManagementEdit(StoreViewModel items)
        {
            var ResultPlant = Service.Master.EmailRecipients.ValidateInputHtmlInjection(items.Store.Plant, "`^<>");
            var ResultName = Service.Master.EmailRecipients.ValidateInputHtmlInjection(items.Store.Name, "`^<>");
            var ResultC3LC = Service.Master.EmailRecipients.ValidateInputHtmlInjection(items.Store.C3LC, "`^<>");
            var ResultPrevName = Service.Master.EmailRecipients.ValidateInputHtmlInjection(items.Store.PrevName, "`^<>");
            var ResultDescription = Service.Master.EmailRecipients.ValidateInputHtmlInjection(items.Store.Description, "`^<>");

            if (!ResultPlant)
            {
                return JsonMessage("Please Enter a Valid Plant", 1, "i");
            }
            if (!ResultName)
            {
                return JsonMessage("Please Enter a Valid Name", 1, "i");
            }
            if (!ResultC3LC)
            {
                return JsonMessage("Please Enter a Valid C3LC", 1, "i");
            }
            if (!ResultPrevName)
            {
                return JsonMessage("Please Enter a Valid Prev Name", 1, "i");
            }
            if (!ResultDescription)
            {
                return JsonMessage("Please Enter a Valid Description", 1, "i");
            }

            ViewBag.crudMode = "U";
            if (ModelState.IsValid)
            {
                Service.Master.Stores.Update(
                    items.Store,
                    ViewBag.crudMode);

                return JsonCRUDMessage(ViewBag.crudMode);
            }
            return Json(new { success = false });
            //items = InitilizeData(items.Store.StoreID);

            //return PartialView("StoreManagement.iud", items);
        }

        [HttpGet]
        public ActionResult StoreManagementDelete(string id)
        {
            ViewBag.crudMode = "D";
            UserViewModel StoreData = InitilizeData(id);
            if (StoreData.User == null)
            {
                return HttpNotFound();
            }

            return PartialView("StoreManagement.iud", StoreData);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult StoreManagementDelete(StoreViewModel items)
        {
            ViewBag.crudMode = "D";
            if (ModelState.IsValid)
            {
                Service.Master.Stores.Update(
                    items.Store,
                    ViewBag.crudMode);
                return JsonCRUDMessage(ViewBag.crudMode);
            }
            return PartialView("StoreManagement.iud", items);
        }

        [HttpPost]
        public ActionResult StoreManagementDeleteById(int StoreId)
        {
            Store item = Service.Master.Stores.GetId(StoreId);
            Service.Master.Stores.Update(
                item,
                "D");
            return JsonCRUDMessage("D");
        }
    }
}