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
        private Region InitilizeRegion(int regionId)
        {
            var region = new Region();
            if (regionId == 0)
            {
                return region;
            }
            region = Service.Master.Region.GetId(regionId);
            return region;
        }
        #endregion
        // GET: Region
        //[AuthorizeAcces(ActionType = AuthorizeAcces.IsRead)]
        public ActionResult Region()
        {
            ViewBag.AllowRead = AuthorizeAcces.AllowRead;
            ViewBag.AllowCreate = AuthorizeAcces.AllowCreated;
            ViewBag.AllowUpdate = AuthorizeAcces.AllowUpdated;
            ViewBag.AllowDelete = AuthorizeAcces.AllowDeleted;
            PaginatorBoot.Remove("SessionTRN");
            return View();
        }

        [AuthorizeAcces(ActionType = AuthorizeAcces.IsRead, UrlMenu = "Region")]
        public ActionResult RegionPage()
        {
            PaginatorBoot.Remove("SessionTRN");
            return RegionPageXt();
        }

        [AuthorizeAcces(ActionType = AuthorizeAcces.IsRead, UrlMenu = "Region")]
        public ActionResult RegionPageXt()
        {
            Func<MasterSearchForm, IList<Region>> func = delegate(MasterSearchForm crit)
            {
                List<Region> list = Service.Master.Region.GetRegionList(crit);
                return list.OrderBy(o => o.ModifiedDate).ToList();
            };

            ActionResult paging = PaginatorBoot.Manage("SessionTRN", func).Pagination.ToJsonResult();
            return Json(paging, JsonRequestBehavior.AllowGet);
        }


        private string CheckCode(Region items)
        {
            if (items.Code != null)
            {
                var CodeExistDB = Service.Master.Region.GetCode(items.Code.Trim().ToLower());
                if (CodeExistDB != null)
                {
                    return CodeExistDB.ID.ToString();
                }
            }
            return string.Empty;
        }

        public string CheckName(Region items)
        {
            if (items.Name != null)
            {
                var NameExistDB = Service.Master.Region.GetName(items.Name.Trim().ToLower());
                if (NameExistDB != null)
                {
                    return NameExistDB.ID.ToString();
                }
            }
            return string.Empty;
        }

        #region Region Create
        [AuthorizeAcces(ActionType = AuthorizeAcces.IsRead, UrlMenu = "Region")]
        [HttpGet]
        public ActionResult RegionCreate()
        {
            ViewBag.crudMode = "I";
            var RegionData = InitilizeRegion(0);
            return PartialView("Region.iud", RegionData);
        }

        [AuthorizeAcces(ActionType = AuthorizeAcces.IsCreated, UrlMenu = "Region")]
        [HttpPost, ValidateInput(false)]
        public ActionResult RegionCreate(Region items)
        {
            var ResultCode = Service.Master.EmailRecipients.ValidateInputHtmlInjection(items.Code, "`^<>");
            var ResultName = Service.Master.EmailRecipients.ValidateInputHtmlInjection(items.Name, "`^<>");
            if (!ResultCode)
            {
                return JsonMessage("Please Enter a Valid Code", 1, "i");
            }
            if (!ResultName)
            {
                return JsonMessage("Please Enter a Valid Name", 1, "i");
            }

            ViewBag.crudMode = "I";

            if (CheckCode(items) != "")
                return JsonMessage("Region Code : " + items.Code + " already exists in the database.", 1, CheckCode(items));

            if (CheckName(items) != "")
                return JsonMessage("Region Name : " + items.Name + " already exists in the database.", 1, CheckName(items));


            if (ModelState.IsValid)
            {
                items.Code = Common.Sanitize(items.Code);
                items.Name = Common.Sanitize(items.Name);

                Service.Master.Region.crud(items, ViewBag.crudMode);
                return JsonCRUDMessage(ViewBag.crudMode);
            }


            return Json(new { success = false });
        }

        #endregion

        #region Region Edit
        [AuthorizeAcces(ActionType = AuthorizeAcces.IsRead, UrlMenu = "Region")]
        [HttpGet]
        public ActionResult RegionEdit(int id)
        {
            ViewBag.crudMode = "U";
            var region = InitilizeRegion(id);
            if (region == null)
            {
                return HttpNotFound();
            }

            return PartialView("Region.iud", region);
        }

        [AuthorizeAcces(ActionType = AuthorizeAcces.IsUpdated, UrlMenu = "Region")]
        [HttpPost, ValidateInput(false)]
        [ValidateAntiForgeryToken]
        public ActionResult RegionEdit(Region items)
        {
            var ResultCode = Service.Master.EmailRecipients.ValidateInputHtmlInjection(items.Code, "`^<>");
            var ResultName = Service.Master.EmailRecipients.ValidateInputHtmlInjection(items.Name, "`^<>");
            if (!ResultCode)
            {
                return JsonMessage("Please Enter a Valid Code", 1, "i");
            }
            if (!ResultName)
            {
                return JsonMessage("Please Enter a Valid Name", 1, "i");
            }

            ViewBag.crudMode = "U";

            if (CheckCode(items) != "")
            {
                if (items.ID.ToString() != CheckCode(items))
                    return JsonMessage("Region Code : " + items.Code + " already exists in the database.", 1, CheckCode(items));
            }

            if (CheckName(items) != "")
            {
                if(items.ID.ToString() != CheckName(items))
                return JsonMessage("Region Name : " + items.Name + " already exists in the database.", 1, CheckName(items));
            }

            if (ModelState.IsValid)
            {
                Service.Master.Region.crud(items, ViewBag.crudMode);
                return JsonCRUDMessage(ViewBag.crudMode);
            }
            return Json(new { success = false });
        }

        #endregion

        #region Region Delete
        [AuthorizeAcces(ActionType = AuthorizeAcces.IsRead, UrlMenu = "Region")]
        [HttpGet]
        public ActionResult RegionDelete(string id)
        {
            ViewBag.crudMode = "D";
            UserViewModel RegionData = InitilizeData(id);
            if (RegionData.User == null)
            {
                return HttpNotFound();
            }

            return PartialView("Region.iud", RegionData);
        }

        [AuthorizeAcces(ActionType = AuthorizeAcces.IsDeleted, UrlMenu = "Region")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult RegionDelete(Region items)
        {
            ViewBag.crudMode = "D";
            if (ModelState.IsValid)
            {
                Service.Master.Region.crud(
                    items,
                    ViewBag.crudMode);
                return JsonCRUDMessage(ViewBag.crudMode);
            }
            return PartialView("Region.iud", items);
        }

        [AuthorizeAcces(ActionType = AuthorizeAcces.IsDeleted, UrlMenu = "Region")]
        [HttpPost]
        public ActionResult RegionDeleteById(int RegionId)
        {
            Region item = Service.Master.Region.GetId(RegionId);
            Service.Master.Region.crud(
                item,
                "D");
            return JsonCRUDMessage("D");
        }

        [HttpPost]
        public ActionResult RegionIsActiveById(int ID)
        {
            try
            {
                Region item = Service.Master.Region.GetId(ID);
                item.IsActive = false;
                Service.Master.Region.crud(
                    item,
                    "U");

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