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
        public ActionResult PttuBranch()
        {
            ViewBag.AllowRead = AuthorizeAcces.AllowRead;
            ViewBag.AllowCreate = AuthorizeAcces.AllowCreated;
            ViewBag.AllowUpdate = AuthorizeAcces.AllowUpdated;
            ViewBag.AllowDelete = AuthorizeAcces.AllowDeleted;
            PaginatorBoot.Remove("SessionTRN");
            return View();
        }

        public JsonResult GetPttuBranchList(string startDate, string endDate, string type)
        {
            var data = Service.EMCS.SvcRpttuBranch.PttuBranchList(startDate, endDate, type);
            return Json(new { rows = data.ToList(), total = data.Count }, JsonRequestBehavior.AllowGet);
        }

        [AuthorizeAcces(ActionType = AuthorizeAcces.IsRead, UrlMenu = "PTTUBranch")]
        public JsonResult GetPttuBranchAverage(string startDate, string endDate)
        {
            var average = Service.EMCS.SvcRpttuBranch.PttuBranchAverage(startDate, endDate);
            var ytdAverage = Service.EMCS.SvcRpttuBranch.PttuBranchAverageYtd(startDate, endDate);
            return Json(new { avg = average.ToList(), ytd_avg = ytdAverage }, JsonRequestBehavior.AllowGet);
        }

        [AuthorizeAcces(ActionType = AuthorizeAcces.IsRead, UrlMenu = "PTTUBranch")]
        public ActionResult DownloadPttuBranch(string startDate, string endDate, string type)
        {
            string fileExcel = Server.MapPath("~\\Content\\EMCS\\Templates\\Template_PTTUBranch.xls");
            MemoryStream output = Service.EMCS.SvcRpttuBranch.GetPttuBranchStream(startDate, endDate, type, fileExcel);
            return File(output.ToArray(), "application/vnd.ms-excel", "PTTUBranch_" + DateTime.Now.ToString("ddMMyyyyhhhmmss") + ".xls");
        }
    }
}