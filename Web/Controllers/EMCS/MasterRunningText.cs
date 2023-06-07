using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using App.Data.Caching;
using App.Data.Domain;
using App.Domain;
using App.Web.App_Start;
using System.Globalization;
using Newtonsoft.Json;
using System.Web.Script.Serialization;
using System.Configuration;
using System.Net;
using App.Data.Domain.EMCS;
using App.Web.Models.EMCS;

namespace App.Web.Controllers.EMCS
{
    public partial class EmcsController
    {
        // GET: DailyReport
        [AuthorizeAcces(ActionType = AuthorizeAcces.IsRead)]
        public ActionResult MasterRunningText()
        {
            ViewBag.IsAdminDTS = AuthorizeAcces.AllowCreated;
            ViewBag.AllowRead = AuthorizeAcces.AllowRead;
            ViewBag.AllowCreate = AuthorizeAcces.AllowCreated;
            ViewBag.AllowUpdate = AuthorizeAcces.AllowUpdated;
            ViewBag.AllowDelete = AuthorizeAcces.AllowDeleted;

            var userId = User.Identity.GetUserId();
            var detail = Service.DTS.DeliveryRequisition.GetDetailUser(userId);
            ViewBag.userFullName = detail.FullName;
            ViewBag.userPhone = detail.Phone;
            ViewBag.userID = userId;

            ViewBag.RssSources = Service.EMCS.MasterParameter.GetParamByGroup("rssSource");

            return View();
        }

        public JsonResult GetRunningTextData(string key)
        {
            var item = Service.DTS.MasterUsers.GetListFilter(key);
            return Json(item, JsonRequestBehavior.AllowGet);
        }

        [AuthorizeAcces(ActionType = AuthorizeAcces.IsRead, UrlMenu = "MasterRunningText")]
        public ActionResult RunningTextPage()
        {
            this.PaginatorBoot.Remove("SessionTRN");
            return RunningTextPageXt();
        }

        [AuthorizeAcces(ActionType = AuthorizeAcces.IsRead, UrlMenu = "MasterRunningText")]
        public ActionResult RunningTextPageXt()
        {
            Func<MasterSearchForm, IList<MasterRunningText>> func = delegate (MasterSearchForm crit)
            {
                List<MasterRunningText> list = Service.EMCS.MasterRunningText.GetList(crit);
                return list.ToList();
            };

            ActionResult paging = PaginatorBoot.Manage("SessionTRN", func).Pagination.ToJsonResult();
            return Json(paging, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult RunningTextDeleteById(long id)
        {
            MasterRunningText item = Service.EMCS.MasterRunningText.GetById(id);
            Service.EMCS.MasterRunningText.Crud(item, "D");
            return JsonCRUDMessage("D");
        }

        #region Region Edit
        [AuthorizeAcces(ActionType = AuthorizeAcces.IsUpdated, UrlMenu = "MasterRunningText")]
        [HttpGet]
        public ActionResult RunningTextEdit(long id)
        {
            ViewBag.crudMode = "U";
            RunningTextModel data = new RunningTextModel();
            data.Running = Service.EMCS.MasterRunningText.GetById(id);
            data.StatusList = Service.EMCS.MasterIncoterms.StatusList();

            if (data.Running == null)
            {
                return HttpNotFound();
            }

            return PartialView("Modal.FormRunningText", data);
        }

        [HttpPost, ValidateInput(false)]
        public JsonResult RunningTextEdit(RunningTextModel form)
        {
            var ResultContent = Service.Master.EmailRecipients.ValidateInputHtmlInjection(form.Running.Content, "<>");
            if (!ResultContent)
            {
                return JsonMessage("Please Enter a Valid Title", 1, "i");
            }

            ViewBag.crudMode = "U";

            MasterRunningText data = Service.EMCS.MasterRunningText.GetById(form.Running.Id);
            data.Content = form.Running.Content;
            data.StartDate = form.Running.StartDate;
            data.EndDate = form.Running.EndDate;
            data.IsDelete = form.Running.IsDelete;

            Service.EMCS.MasterRunningText.Crud(data, ViewBag.crudMode);
            return JsonCRUDMessage("U");
        }
        #endregion

        #region Region Create
        [AuthorizeAcces(ActionType = AuthorizeAcces.IsCreated, UrlMenu = "MasterRunningText")]
        [HttpGet]
        public ActionResult RunningTextCreate()
        {
            ViewBag.crudMode = "I";
            RunningTextModel data = new RunningTextModel();
            data.Running = Service.EMCS.MasterRunningText.GetById(0);
            data.StatusList = Service.EMCS.MasterIncoterms.StatusList();

            return PartialView("Modal.FormRunningText", data);
        }

        [HttpPost, ValidateInput(false)]
        public JsonResult RunningTextCreate(RunningTextModel form)
        {
            var ResultContent = Service.Master.EmailRecipients.ValidateInputHtmlInjection(form.Running.Content, "<>");
            if (!ResultContent)
            {
                return JsonMessage("Please Enter a Valid Title", 1, "i");
            }

            Service.EMCS.MasterRunningText.Crud(form.Running, "I");
            return JsonCRUDMessage("I");
        }
        #endregion

        [HttpPost]
        public JsonResult SetRunningTextSource(long id, int status)
        {
            try
            {
                var item = Service.EMCS.MasterParameter.GetParamById(id);
                item.IsDeleted = ((status == 0) ? false : true);
                var crud = Service.EMCS.MasterParameter.Crud(item, "U");
                return Json(new { status = crud, msg = "Update item success" });
            }
            catch (Exception)
            {
                return Json(new { status = 0, msg = "Failed to change status" });
            }
        }
    }
}