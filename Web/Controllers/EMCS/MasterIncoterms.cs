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

namespace App.Web.Controllers.EMCS
{
    public partial class EmcsController
    {
        // GET: DailyReport
        [AuthorizeAcces(ActionType = AuthorizeAcces.IsRead)]
        public ActionResult IncotermsList()
        {
            ViewBag.IsAdminDTS = AuthorizeAcces.AllowCreated;
            ViewBag.AllowRead = AuthorizeAcces.AllowRead;
            ViewBag.AllowCreate = AuthorizeAcces.AllowCreated;
            ViewBag.AllowUpdate = AuthorizeAcces.AllowUpdated;
            ViewBag.AllowDelete = AuthorizeAcces.AllowDeleted;

            Session["AllowRead_DeliveryRequisition"] = AuthorizeAcces.AllowRead;
            Session["AllowCreated_DeliveryRequisition"] = AuthorizeAcces.AllowCreated;
            Session["AllowUpdated_DeliveryRequisition"] = AuthorizeAcces.AllowUpdated;
            Session["AllowDeleted_DeliveryRequisition"] = AuthorizeAcces.AllowDeleted;

            var userId = User.Identity.GetUserId();
            var detail = Service.DTS.DeliveryRequisition.GetDetailUser(userId);
            ViewBag.userFullName = detail.FullName;
            ViewBag.userPhone = detail.Phone;
            ViewBag.userID = userId;

            return View();
        }

        public JsonResult IncotermsOptionSelect(MasterSearchForm crit)
        {
            var data = Service.EMCS.MasterIncoterms.GetSelectOption(crit);
            return Json(new { data }, JsonRequestBehavior.AllowGet);
        }

        [AuthorizeAcces(ActionType = AuthorizeAcces.IsRead, UrlMenu = "IncotermsList")]
        public ActionResult IncotermsPage()
        {
            this.PaginatorBoot.Remove("SessionTRN");
            return IncotermsPageXt();
        }

        [AuthorizeAcces(ActionType = AuthorizeAcces.IsRead, UrlMenu = "IncotermsList")]
        public ActionResult IncotermsPageXt()
        {
            Func<MasterSearchForm, IList<MasterIncoterms>> func = delegate (MasterSearchForm crit)
            {
                List<MasterIncoterms> list = Service.EMCS.MasterIncoterms.GetList(crit);
                return list.ToList();
            };

            ActionResult paging = PaginatorBoot.Manage("SessionTRN", func).Pagination.ToJsonResult();
            return Json(paging, JsonRequestBehavior.AllowGet);
        }

        private MasterIncoterms GetDataByID(long id)
        {
            return Service.EMCS.MasterIncoterms.GetDataById(id);
        }

        [HttpPost]
        public ActionResult MenuDeleteById(long id)
        {
            MasterIncoterms item = Service.EMCS.MasterIncoterms.GetDataById(id);
            Service.EMCS.MasterIncoterms.Crud(
                item,
                "D");
            return JsonCRUDMessage("D");
        }

        #region Region Edit
        [HttpGet]
        public ActionResult IncotermsEdit(long id)
        {
            ViewBag.crudMode = "U";
            IncotermsModel data = new IncotermsModel();
            data.Incoterms = GetDataByID(id);
            data.StatusList = Service.EMCS.MasterIncoterms.StatusList();

            if (data.Incoterms == null)
            {
                return HttpNotFound();
            }

            return PartialView("Modal.FormIncoterms", data);
        }

        [HttpPost, ValidateInput(false)]
        public JsonResult IncotermsEdit(IncotermsModel form)
        {
            var ResultName = Service.Master.EmailRecipients.ValidateInputHtmlInjection(form.Incoterms.Name, "`^<>");
            var ResultDescription = Service.Master.EmailRecipients.ValidateInputHtmlInjection(form.Incoterms.Description, "`^<>");
            if (!ResultName)
            {
                return JsonMessage("Please Enter a Valid Name", 1, "i");
            }
            if (!ResultDescription)
            {
                return JsonMessage("Please Enter a Valid Description", 1, "i");
            }

            ViewBag.crudMode = "U";
            var number = Service.EMCS.MasterIncoterms.GetDataByNumber(form.Incoterms.Number, form.Incoterms.Id);
            if (number != null)
            {
                string msg = "Sorry this " + form.Incoterms.Number + " is Already Use by Another Incoterms";
                return Json(new { Status = 1, Msg = msg });
            }

            MasterIncoterms data = GetDataByID(form.Incoterms.Id);
            data.Description = form.Incoterms.Description;
            data.IsDeleted = form.Incoterms.IsDeleted;
            data.Number = form.Incoterms.Number;

            Service.EMCS.MasterIncoterms.Crud(data, ViewBag.crudMode);
            return JsonCRUDMessage("U");
        }
        #endregion

        #region Region Create
        [AuthorizeAcces(ActionType = AuthorizeAcces.IsCreated, UrlMenu = "IncotermsList")]
        [HttpGet]
        public ActionResult IncotermsCreate()
        {
            ViewBag.crudMode = "I";
            IncotermsModel data = new IncotermsModel();
            data.Incoterms = GetDataByID(0);
            data.StatusList = Service.EMCS.MasterIncoterms.StatusList();

            return PartialView("Modal.FormIncoterms", data);
        }

        [HttpPost, ValidateInput(false)]
        public JsonResult IncotermsCreate(IncotermsModel form)
        {
            var ResultName = Service.Master.EmailRecipients.ValidateInputHtmlInjection(form.Incoterms.Name, "`^<>");
            var ResultDescription = Service.Master.EmailRecipients.ValidateInputHtmlInjection(form.Incoterms.Description, "`^<>");
            if (!ResultName)
            {
                return JsonMessage("Please Enter a Valid Name", 1, "i");
            }
            if (!ResultDescription)
            {
                return JsonMessage("Please Enter a Valid Description", 1, "i");
            }

            ViewBag.crudMode = (form.Incoterms.Id == 0) ? "I" : "U";
            string msg;
            var number = Service.EMCS.MasterIncoterms.GetDataByNumber(form.Incoterms.Number, form.Incoterms.Id);
            if (number != null)
            {
                msg = "Sorry this " + form.Incoterms.Number + " is Already Use by Another Incoterms";
                return Json(new { Status = false, Msg = msg });
            }
            Service.EMCS.MasterIncoterms.Crud(form.Incoterms, ViewBag.crudMode);
            return JsonCRUDMessage(ViewBag.crudMode);
        }
        #endregion
    }
}