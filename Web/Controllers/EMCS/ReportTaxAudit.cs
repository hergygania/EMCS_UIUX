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
        public ActionResult ReportTaxAudit()
        {
            ViewBag.AllowRead = AuthorizeAcces.AllowRead;
            ViewBag.AllowCreate = AuthorizeAcces.AllowCreated;
            ViewBag.AllowUpdate = AuthorizeAcces.AllowUpdated;
            ViewBag.AllowDelete = AuthorizeAcces.AllowDeleted;
            PaginatorBoot.Remove("SessionTRN");
            return View();
        }

        [AuthorizeAcces(ActionType = AuthorizeAcces.IsRead, UrlMenu = "ReportTaxAudit")]
        public ActionResult RTaxAuditListPage(DateTime startDate, DateTime endDate)
        {
            this.PaginatorBoot.Remove("SessionTRN");
            return RTaxAuditPageXt(startDate, endDate);
        }

        public ActionResult RTaxAuditPageXt(DateTime startDate, DateTime endDate)
        {
            Func<Data.Domain.EMCS.SpRTaxAudit, List<Data.Domain.EMCS.SpRTaxAudit>> func = delegate
            {
                var param = Request["params"];
                if (!string.IsNullOrEmpty(param))
                {
                    JavaScriptSerializer ser = new JavaScriptSerializer();
                    ser.Deserialize<Data.Domain.EMCS.SpRTaxAudit>(param);
                }
                var list = Service.EMCS.SvcRTaxAudit.TaxAuditList(startDate, endDate);
                return list.ToList();
            };

            var paging = PaginatorBoot.Manage("SessionTRN", func).Pagination.ToJsonResult();
            return Json(paging, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetTaxAuditList(DateTime startDate, DateTime endDate)
        {
            var data = Service.EMCS.SvcRTaxAudit.GetTaxAuditList(startDate, endDate);
            return Json(data, JsonRequestBehavior.AllowGet);
        }

        //[AuthorizeAcces(ActionType = AuthorizeAcces.IsRead, UrlMenu = "ReportTaxAudit")]
        //public ActionResult DownloadTaxAudit(DateTime startDate, DateTime endDate)
        //{
        //    var data = Service.EMCS.SvcRTaxAudit.GetTaxAuditList(startDate, endDate);
        //    string fileExcel = Server.MapPath("~\\Content\\EMCS\\Templates\\Template_TaxAudit.xls");
        //    MemoryStream output = Service.EMCS.SvcRTaxAudit.GetTaxAuditStream(data, fileExcel);
        //    return File(output.ToArray(), "application/vnd.ms-excel", "Template_TaxAudit_" + DateTime.Now.ToString("ddMMyyyyhhhmmss") + ".xls");
        //}

        [AuthorizeAcces(ActionType = AuthorizeAcces.IsRead, UrlMenu = "ReportTaxAudit")]
        public FileResult DownloadTaxAudit(DateTime startDate, DateTime endDate)
        {
            try
            {
                var data = Service.EMCS.SvcRTaxAudit.GetTaxAuditList(startDate, endDate);
                MemoryStream output = new MemoryStream();
                output = Service.EMCS.SvcRTaxAudit.GetTaxAuditStream(data);
                return File(output.ToArray(),   //The binary data of the XLS file
                    "application/vnd.ms-excel",//MIME type of Excel files
                    "TaxAudit.xlsx");
                //Return the result to the end user
                //Suggested file name in the "Save as" dialog which will be displayed to the end user
            }
            catch (Exception ex)
            {
                //Write the Workbook to a memory stream
                MemoryStream output = new MemoryStream();
                //Return the result to the end user
                return File(output.ToArray(),   //The binary data of the XLS file
                 "application/vnd.ms-excel",//MIME type of Excel files
                 "TaxAudit.xlsx");    //Suggested file name in the "Save as" dialog which will be displayed to the end user
            }
        }

        public FileResult DownloadPebDocument(string filename)
        {
            string fullPath = Request.MapPath("~/Upload/EMCS/NPEPEB/" + filename);
            var fileBytes = System.IO.File.ReadAllBytes(fullPath);
            return File(fileBytes, System.Net.Mime.MediaTypeNames.Application.Octet, filename);
          
        }

        public FileResult DownloadBlAwbDocument(string filename)
        {
            string fullPath = Request.MapPath("~/Upload/EMCS/BLAWB/" + filename);
            var fileBytes = System.IO.File.ReadAllBytes(fullPath);
            return File(fileBytes, System.Net.Mime.MediaTypeNames.Application.Octet, filename);
        }
    }
}