using App.Data.Domain;
using App.Domain;
using App.Web.App_Start;
using App.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using App.Web.Models.EMCS;
using App.Data.Domain.EMCS;
using System.Web.Script.Serialization;

namespace App.Web.Controllers.EMCS
{
    public partial class EmcsController
    {
        #region Initilize
        private MasterAreaUserCkb InitilizeAreaUserCkb(long areaCkbId)
        {
            MasterAreaUserCkb areaCkb = new MasterAreaUserCkb();
            if (areaCkbId == 0)
            {
                return areaCkb;
            }

            areaCkb = Service.EMCS.MasterAreaUserCkb.GetDataById(areaCkbId);
            return areaCkb;
        }
        #endregion

        [AuthorizeAcces(ActionType = AuthorizeAcces.IsRead)]
        public ActionResult AreaUserCkb()
        {
            ViewBag.AllowRead = AuthorizeAcces.AllowRead;
            ViewBag.AllowCreate = AuthorizeAcces.AllowCreated;
            ViewBag.AllowUpdate = AuthorizeAcces.AllowUpdated;
            ViewBag.AllowDelete = AuthorizeAcces.AllowDeleted;
            PaginatorBoot.Remove("SessionTRN");

            return View();
        }

        [AuthorizeAcces(ActionType = AuthorizeAcces.IsRead, UrlMenu = "AreaUserCKB")]
        public ActionResult AreaUserCkbPage()
        {
            this.PaginatorBoot.Remove("SessionTRN");
            return AreaUserCkbPageXt();
        }

        [AuthorizeAcces(ActionType = AuthorizeAcces.IsRead, UrlMenu = "AreaUserCKB")]
        public ActionResult AreaUserCkbPageXt()
        {
            var data = new App.Data.Domain.EMCS.AreaUserCkbListFilter();
            data.BAreaName = Request["SearchName"];
            Func<AreaUserCkbListFilter, List<SpAreaUserCkb>> func = delegate (AreaUserCkbListFilter filter)
            {
                var param = Request["params"];
                if (!string.IsNullOrEmpty(param))
                {
                    JavaScriptSerializer ser = new JavaScriptSerializer();
                    filter = ser.Deserialize<AreaUserCkbListFilter>(param);
                }
                var list = Service.EMCS.MasterAreaUserCkb.GetAreaUserCkbList(data);
                return list.ToList();
            };
            ActionResult paging = PaginatorBoot.Manage("SessionTRN", func).Pagination.ToJsonResult();
            return Json(paging, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult AreaUserCkbCreate()
        {
            ViewBag.crudMode = "I";
            var userCkbData = new AreaUserCkbModel();
            userCkbData.UserCkb = InitilizeAreaUserCkb(0);
            var crit = new Domain.MasterSearchForm();
            userCkbData.AreaList = Service.EMCS.MasterArea.GetList(crit);
            userCkbData.UserList = Service.EMCS.MasterUserCkb.GetUserCkbList();
            return PartialView("Modal.FormAreaUserCKB", userCkbData);
        }


        [HttpPost]
        public ActionResult AreaUserCkbCreate(AreaUserCkbModel items)
        {
            if (ModelState.IsValid)
            {
                ViewBag.crudMode = "I";
                Service.EMCS.MasterAreaUserCkb.Crud(items.UserCkb, ViewBag.crudMode);
                return JsonCRUDMessage(ViewBag.crudMode);
            }
            return Json(new { success = false });
        }

        [HttpGet]
        public ActionResult AreaUserCkbEdit(long id)
        {
            ViewBag.crudMode = "U";
            AreaUserCkbModel data = new AreaUserCkbModel();
            data.UserCkb = InitilizeAreaUserCkb(id);
            var crit = new Domain.MasterSearchForm();
            data.AreaList = Service.EMCS.MasterArea.GetList(crit);
            data.UserList = Service.EMCS.MasterUserCkb.GetUserCkbList();

            if (data.UserCkb == null)
            {
                return HttpNotFound();
            }

            return PartialView("Modal.FormAreaUserCKB", data);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AreaUserCkbEdit(AreaUserCkbModel items)
        {
            ViewBag.crudMode = "U";
            if (ModelState.IsValid)
            {
                MasterAreaUserCkb userCkb = Service.EMCS.MasterAreaUserCkb.GetDataById(items.UserCkb.Id);
                userCkb.Id = items.UserCkb.Id;
                userCkb.BAreaCode = items.UserCkb.BAreaCode;
                userCkb.Username = items.UserCkb.Username;

                Service.EMCS.MasterAreaUserCkb.Crud(userCkb, ViewBag.crudMode);
                return JsonCRUDMessage(ViewBag.crudMode);
            }
            return Json(new { success = false });
        }

        [HttpGet]
        public ActionResult AreaUserCkbDelete(long id)
        {
            ViewBag.crudMode = "D";
            var userCkb = new AreaUserCkbModel();
            userCkb.UserCkb = InitilizeAreaUserCkb(id);
            var crit = new Domain.MasterSearchForm();
            userCkb.AreaList = Service.EMCS.MasterArea.GetList(crit);

            if (userCkb.UserCkb == null)
            {
                return HttpNotFound();
            }

            return PartialView("AreaUserCKB", userCkb);
        }

        [HttpPost]
        //[ValidateAntiForgeryToken]
        public ActionResult AreaUserCkbDelete(MasterAreaUserCkb items)
        {
            ViewBag.crudMode = "D";

            Service.EMCS.MasterAreaUserCkb.Crud(
                items,
                ViewBag.crudMode);
            return JsonCRUDMessage(ViewBag.crudMode);

        }

        [HttpPost]
        public ActionResult AreaUserCkbDeleteById(long userCkbId)
        {
            MasterAreaUserCkb item = Service.EMCS.MasterAreaUserCkb.GetDataById(userCkbId);
            Service.EMCS.MasterAreaUserCkb.Crud(
                item,
                "D");
            return JsonCRUDMessage("D");
        }
    }
}