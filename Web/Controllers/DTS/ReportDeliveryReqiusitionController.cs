using App.Data.Domain;
using App.Domain;
using App.Web.App_Start;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;

namespace App.Web.Controllers.DTS
{
    public partial class DtsController
    {
        [AuthorizeAcces(ActionType = AuthorizeAcces.IsRead, UrlMenu = "DailyReport")]
        public ActionResult ReportDeliveryRequisition()
        {
            ViewBag.AllowRead = AuthorizeAcces.AllowRead;
            ViewBag.AllowCreate = AuthorizeAcces.AllowCreated;
            ViewBag.AllowUpdate = AuthorizeAcces.AllowUpdated;
            ViewBag.AllowDelete = AuthorizeAcces.AllowDeleted;
            PaginatorBoot.Remove("SessionTRN");

            return View("~/Views/DTS/v2/ReportDeliveryRequisition.cshtml");
        }
        [AuthorizeAcces(ActionType = AuthorizeAcces.IsRead,UrlMenu = "DailyReport")]
        public ActionResult ReportDeliveryRequisitionPage()
        {
            PaginatorBoot.Remove("SessionTRN");
            return ReportDeliveryRequisitionPageXt();
        }
        [AuthorizeAcces(ActionType = AuthorizeAcces.IsRead, UrlMenu = "DailyReport")]
        public ActionResult ReportDeliveryRequisitionPageXt()
        {
            Func<App.Data.Domain.DTS.ReportDeliveryRequisitionFilter, List<Data.Domain.ReportDeliveryRequisition>> func = delegate (App.Data.Domain.DTS.ReportDeliveryRequisitionFilter filter)
            {
                var param = Request["params"];
                if (!string.IsNullOrEmpty(param))
                {
                    JavaScriptSerializer ser = new JavaScriptSerializer();
                    filter = ser.Deserialize<App.Data.Domain.DTS.ReportDeliveryRequisitionFilter>(param);
                }

                var list = Service.DTS.DeliveryRequisition.GetReportListFilter(filter);
                return list.ToList();
            };

            var paging = PaginatorBoot.Manage("SessionTRN", func).Pagination.ToJsonResult();
            return Json(paging, JsonRequestBehavior.AllowGet);
        }
        [AuthorizeAcces(ActionType = AuthorizeAcces.IsRead, UrlMenu = "DeliveryRequisitionListAcc")]
        public FileResult DownloadToExcelReportDR(string guid)
        {
            return Session[guid] as FileResult;
        }
        [AuthorizeAcces(ActionType = AuthorizeAcces.IsRead,UrlMenu = "DeliveryRequisitionListAcc")]
        public ActionResult DownloadReportDR(App.Data.Domain.DTS.ReportDeliveryRequisitionFilter filter)
        {
            Guid guid = Guid.NewGuid();
            Helper.Service.DTS.DownloadDRController data = new Helper.Service.DTS.DownloadDRController();

            Session[guid.ToString()] = data.DownloadReportToExcel(filter);

            return Json(guid.ToString(), JsonRequestBehavior.AllowGet);
        }
    }
}