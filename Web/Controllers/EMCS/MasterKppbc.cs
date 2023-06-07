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
        private MasterKppbc InitilizeKppbc(long kppbcId)
        {
            MasterKppbc kppbc = new MasterKppbc();
            if (kppbcId == 0)
            {
                return kppbc;
            }

            kppbc = Service.EMCS.MasterKppbc.GetDataById(kppbcId);
            return kppbc;
        }
        #endregion

        [AuthorizeAcces(ActionType = AuthorizeAcces.IsDeleted, UrlMenu = "Kppbc")]
        public ActionResult Kppbc()
        {
            ViewBag.AllowRead = AuthorizeAcces.AllowRead;
            ViewBag.AllowCreate = AuthorizeAcces.AllowCreated;
            ViewBag.AllowUpdate = AuthorizeAcces.AllowUpdated;
            ViewBag.AllowDelete = AuthorizeAcces.AllowDeleted;
            PaginatorBoot.Remove("SessionTRN");

            return View();
        }

        [AuthorizeAcces(ActionType = AuthorizeAcces.IsRead, UrlMenu = "Kppbc")]
        public ActionResult KppbcPage()
        {
            this.PaginatorBoot.Remove("SessionTRN");
            return KppbcPageXt();
        }

        [AuthorizeAcces(ActionType = AuthorizeAcces.IsRead, UrlMenu = "Kppbc")]
        public ActionResult KppbcPageXt()
        {
            Func<KppbcListFilter, List<SpGetListAllKppbc>> func = delegate (KppbcListFilter filter)
            {
                var param = Request["params"];
                if (!string.IsNullOrEmpty(param))
                {
                    JavaScriptSerializer ser = new JavaScriptSerializer();
                    filter = ser.Deserialize<KppbcListFilter>(param);
                }
                var list = Service.EMCS.MasterKppbc.GetKppbcList(filter);
                return list.ToList();
            };

            ActionResult paging = PaginatorBoot.Manage("SessionTRN", func).Pagination.ToJsonResult();
            return Json(paging, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult KppbcList(KppbcListFilter crit)
        {
            var data = Service.EMCS.MasterKppbc.GetKppbcList(crit);
            return Json(new { data }, JsonRequestBehavior.AllowGet);
        }

        [AuthorizeAcces(ActionType = AuthorizeAcces.IsCreated, UrlMenu = "Kppbc")]
        [HttpGet]
        public ActionResult KppbcCreate()
        {
            ViewBag.crudMode = "I";
            var kppbcData = new KppbcModel();
            kppbcData.Kppbc = InitilizeKppbc(0);
            var crit = new Domain.MasterSearchForm();
            kppbcData.AreaList = Service.EMCS.MasterArea.GetList(crit);
            return PartialView("Modal.FormKppbc", kppbcData);
        }


        [HttpPost, ValidateInput(false)]
        public ActionResult KppbcCreate(KppbcModel items)
        {
            var ResultCode = Service.Master.EmailRecipients.ValidateInputHtmlInjection(items.Kppbc.Code, "`^<>");
            var ResultName = Service.Master.EmailRecipients.ValidateInputHtmlInjection(items.Kppbc.Name, "`^<>");
            var ResultPropinsi = Service.Master.EmailRecipients.ValidateInputHtmlInjection(items.Kppbc.Propinsi, "`^<>");
            var ResultAddress = Service.Master.EmailRecipients.ValidateInputHtmlInjection(items.Kppbc.Address, "`^<>");
            if (!ResultCode)
            {
                return JsonMessage("Please Enter a Valid Code", 1, "i");
            }
            if (!ResultName)
            {
                return JsonMessage("Please Enter a Valid Name", 1, "i");
            }
            if (!ResultPropinsi)
            {
                return JsonMessage("Please Enter a Valid Propinsi", 1, "i");
            }
            if (!ResultAddress)
            {
                return JsonMessage("Please Enter a Valid District", 1, "i");
            }

            if (ModelState.IsValid)
            {
                ViewBag.crudMode = "I";
                Service.EMCS.MasterKppbc.Crud(items.Kppbc, ViewBag.crudMode);
                return JsonCRUDMessage(ViewBag.crudMode);
            }
            return Json(new { success = false });
        }

        [AuthorizeAcces(ActionType = AuthorizeAcces.IsUpdated, UrlMenu = "Kppbc")]
        [HttpGet]
        public ActionResult KppbcEdit(long id)
        {
            ViewBag.crudMode = "U";
            KppbcModel data = new KppbcModel();
            data.Kppbc = InitilizeKppbc(id);
            var crit = new Domain.MasterSearchForm();
            data.AreaList = Service.EMCS.MasterArea.GetList(crit);

            if (data.Kppbc == null)
            {
                return HttpNotFound();
            }

            return PartialView("Modal.FormKppbc", data);
        }

        [HttpPost, ValidateInput(false)]
        [ValidateAntiForgeryToken]
        public ActionResult KppbcEdit(KppbcModel items)
        {
            var ResultCode = Service.Master.EmailRecipients.ValidateInputHtmlInjection(items.Kppbc.Code, "`^<>");
            var ResultName = Service.Master.EmailRecipients.ValidateInputHtmlInjection(items.Kppbc.Name, "`^<>");
            var ResultPropinsi = Service.Master.EmailRecipients.ValidateInputHtmlInjection(items.Kppbc.Propinsi, "`^<>");
            var ResultAddress = Service.Master.EmailRecipients.ValidateInputHtmlInjection(items.Kppbc.Address, "`^<>");
            if (!ResultCode)
            {
                return JsonMessage("Please Enter a Valid Code", 1, "i");
            }
            if (!ResultName)
            {
                return JsonMessage("Please Enter a Valid Name", 1, "i");
            }
            if (!ResultPropinsi)
            {
                return JsonMessage("Please Enter a Valid Propinsi", 1, "i");
            }
            if (!ResultAddress)
            {
                return JsonMessage("Please Enter a Valid District", 1, "i");
            }

            ViewBag.crudMode = "U";
            if (ModelState.IsValid)
            {
                MasterKppbc kppbc = Service.EMCS.MasterKppbc.GetDataById(items.Kppbc.Id);
                kppbc.Id = items.Kppbc.Id;
                kppbc.Code = items.Kppbc.Code;
                kppbc.Name = items.Kppbc.Name;
                kppbc.Propinsi = items.Kppbc.Propinsi;
                kppbc.Address = items.Kppbc.Address;

                Service.EMCS.MasterKppbc.Crud(kppbc, ViewBag.crudMode);
                return JsonCRUDMessage(ViewBag.crudMode);
            }
            return Json(new { success = false });
        }

        [AuthorizeAcces(ActionType = AuthorizeAcces.IsDeleted, UrlMenu = "Kppbc")]
        [HttpPost]
        //[ValidateAntiForgeryToken]
        public ActionResult KppbcDelete(MasterKppbc items)
        {
            ViewBag.crudMode = "D";

            Service.EMCS.MasterKppbc.Crud(
                items,
                ViewBag.crudMode);
            return JsonCRUDMessage(ViewBag.crudMode);

        }

        [HttpPost]
        public ActionResult KppbcDeleteById(long kppbcId)
        {
            MasterKppbc item = Service.EMCS.MasterKppbc.GetDataById(kppbcId);
            Service.EMCS.MasterKppbc.Crud(
                item,
                "D");
            return JsonCRUDMessage("D");
        }
    }
}