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
        [AuthorizeAcces(ActionType = AuthorizeAcces.IsRead)]
        public ActionResult ExportTransaction()
        {
            ViewBag.AllowRead = AuthorizeAcces.AllowRead;
            ViewBag.AllowCreate = AuthorizeAcces.AllowCreated;
            ViewBag.AllowUpdate = AuthorizeAcces.AllowUpdated;
            ViewBag.AllowDelete = AuthorizeAcces.AllowDeleted;
            PaginatorBoot.Remove("SessionTRN");
            return View();
        }

        [AuthorizeAcces(ActionType = AuthorizeAcces.IsRead, UrlMenu = "ExportTransaction")]
        public ActionResult TotalExportMonthly(Domain.MasterSearchForm crit)
        {
            var monthly = Service.EMCS.SvcRExportTransaction.TotalExportMonthly(crit);
            return Json(monthly, JsonRequestBehavior.AllowGet);
        }

        [AuthorizeAcces(ActionType = AuthorizeAcces.IsRead, UrlMenu = "ExportTransaction")]
        public ActionResult TotalExportPort(Domain.MasterSearchForm crit)
        {
            var port = Service.EMCS.SvcRExportTransaction.TotalExportPort(crit);
            return Json(port, JsonRequestBehavior.AllowGet);
        }

        [AuthorizeAcces(ActionType = AuthorizeAcces.IsRead, UrlMenu = "ExportTransaction")]
        public ActionResult DownloadExportTransactionMonthly(Domain.MasterSearchForm crit)
        {
            var data = Service.EMCS.SvcRExportTransaction.TotalExportMonthlyDownload(crit);
            string fileExcel = Server.MapPath("~\\Content\\EMCS\\Templates\\TemplateExportTransactionMonthly.xls");
            MemoryStream output = Service.EMCS.SvcRExportTransaction.GetTotalExportTransactionMonthlyStream(data, fileExcel);
            return File(output.ToArray(), "application/vnd.ms-excel", "TotalExportTransactionMonthly_" + DateTime.Now.ToString("ddMMyyyyhhhmmss") + ".xls");
        }

        [AuthorizeAcces(ActionType = AuthorizeAcces.IsRead, UrlMenu = "ExportTransaction")]
        public ActionResult DownloadExportTransactionPort(Domain.MasterSearchForm crit)
        {
            var data = Service.EMCS.SvcRExportTransaction.TotalExportPort(crit);
            string fileExcel = Server.MapPath("~\\Content\\EMCS\\Templates\\TemplateExportTransactionPort.xls");
            MemoryStream output = Service.EMCS.SvcRExportTransaction.GetTotalExportTransactionPortStream(data, fileExcel);
            return File(output.ToArray(), "application/vnd.ms-excel", "TotalExportTransactionMonthly_" + DateTime.Now.ToString("ddMMyyyyhhhmmss") + ".xls");
        }

    }
}