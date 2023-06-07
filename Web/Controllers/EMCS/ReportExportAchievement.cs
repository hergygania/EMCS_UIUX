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
        public ActionResult Achievement()
        {
            ViewBag.AllowRead = AuthorizeAcces.AllowRead;
            ViewBag.AllowCreate = AuthorizeAcces.AllowCreated;
            ViewBag.AllowUpdate = AuthorizeAcces.AllowUpdated;
            ViewBag.AllowDelete = AuthorizeAcces.AllowDeleted;
            PaginatorBoot.Remove("SessionTRN");
            return View();
        }

        public ActionResult RAchievementListPage(string startDate,string endDate)
        {
            this.PaginatorBoot.Remove("SessionTRN");
            return RAchievementPageXt(startDate, endDate);
        }

        public ActionResult RAchievementPageXt(string startDate, string endDate)
        {
            Func<Data.Domain.EMCS.SpRAchievement, IList<Data.Domain.EMCS.SpRAchievement>> func = delegate
            {
                var list = Service.EMCS.SvcRAchievement.GetAchievementListExport(startDate, endDate);
                return list.ToList();
            };

            var paging = PaginatorBoot.Manage("SessionTRN", func).Pagination.ToJsonResult();
            return Json(paging, JsonRequestBehavior.AllowGet);
        }

        [AuthorizeAcces(ActionType = AuthorizeAcces.IsRead, UrlMenu = "Achievement")]
        public JsonResult GetPercentageAchievement(String startDate, String endDate, String cycle)
        {
            var data = Service.EMCS.SvcRAchievement.AchievementChart(startDate, endDate, cycle);
            return Json(data, JsonRequestBehavior.AllowGet);
        }

        [AuthorizeAcces(ActionType = AuthorizeAcces.IsRead, UrlMenu = "Achievement")]
        public ActionResult DownloadExportAchievement(string startDate, string endDate)
        {
            var data = Service.EMCS.SvcRAchievement.GetAchievementListExport(startDate, endDate);
            string fileExcel = Server.MapPath("~\\Content\\EMCS\\Templates\\Template_ExportAchievement.xls");
            MemoryStream output = Service.EMCS.SvcRAchievement.GetAchievementStream(data, fileExcel);
            return File(output.ToArray(), "application/vnd.ms-excel", "ExportAchievement_" + DateTime.Now.ToString("ddMMyyyyhhhmmss") + ".xls");
        }
    }
}