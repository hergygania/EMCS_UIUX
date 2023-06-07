using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using App.Data.Caching;
using App.Data.Domain;
using App.Domain;
using App.Web.Models;
using App.Web.App_Start;
using System.Globalization;
using Newtonsoft.Json;
using System.Web.Script.Serialization;
using System.Configuration;
using System.Net;
using System.IO;

namespace App.Web.Controllers.EMCS
{
    public partial class EmcsController
    {
        // GET: ReportTaxAudit
        [AuthorizeAcces(ActionType = AuthorizeAcces.IsRead)]
        public ActionResult ReportOutstandingCipl()
        {
            ViewBag.AllowRead = AuthorizeAcces.AllowRead;
            ViewBag.AllowCreate = AuthorizeAcces.AllowCreated;
            ViewBag.AllowUpdate = AuthorizeAcces.AllowUpdated;
            ViewBag.AllowDelete = AuthorizeAcces.AllowDeleted;
            PaginatorBoot.Remove("SessionTRN");
            return View();
        }

        [AuthorizeAcces(ActionType = AuthorizeAcces.IsRead, UrlMenu = "ReportOutstandingCipl")]
        public ActionResult ROutstandingCiplListPage(DateTime startDate, DateTime endDate)
        {
            this.PaginatorBoot.Remove("SessionTRN");
            return ROutstandingCiplPageXt(startDate, endDate);
        }

        public ActionResult ROutstandingCiplPageXt(DateTime startDate, DateTime endDate)
        {
            Func<Data.Domain.EMCS.SpROutstandingCipl, List<Data.Domain.EMCS.SpROutstandingCipl>> func = delegate (Data.Domain.EMCS.SpROutstandingCipl filter)
            {
                var param = Request["params"];
                if (!string.IsNullOrEmpty(param))
                {
                    JavaScriptSerializer ser = new JavaScriptSerializer();
                    ser.Deserialize<Data.Domain.EMCS.SpROutstandingCipl>(param);
                }
                var list = Service.EMCS.SvcROutstandingCipl.GetOutstandingCiplList(startDate, endDate);
                return list.ToList();
            };

            var paging = PaginatorBoot.Manage("SessionTRN", func).Pagination.ToJsonResult();
            return Json(paging, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetOutstandingCiplList(DateTime startDate, DateTime endDate)
        {
            var data = Service.EMCS.SvcROutstandingCipl.GetOutstandingCiplList(startDate, endDate);
            return Json(data, JsonRequestBehavior.AllowGet);
        }

        [AuthorizeAcces(ActionType = AuthorizeAcces.IsRead, UrlMenu = "ReportOutstandingCipl")]
        public ActionResult DownloadOutstandingCipl(DateTime startDate, DateTime endDate)
        {
            var data = Service.EMCS.SvcROutstandingCipl.GetOutstandingCiplList(startDate, endDate);
            string fileExcel = Server.MapPath("~\\Content\\EMCS\\Templates\\Template_OutstandingCipl.xls");
            MemoryStream output = Service.EMCS.SvcROutstandingCipl.GetROutstandingCiplStream(data, fileExcel);
            return File(output.ToArray(), "application/vnd.ms-excel", "Report_OutstandingCipl_" + DateTime.Now.ToString("ddMMyyyyhhhmmss") + ".xls");
        }
    }
}