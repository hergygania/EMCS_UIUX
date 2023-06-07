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
        public ActionResult DestinationSailingTime()
        {
            ViewBag.AppTitle = "Export Monitoring & Controlling System";
            ViewBag.AllowRead = AuthorizeAcces.AllowRead;
            ViewBag.AllowCreate = AuthorizeAcces.AllowCreated;
            ViewBag.AllowUpdate = AuthorizeAcces.AllowUpdated;
            ViewBag.AllowDelete = AuthorizeAcces.AllowDeleted;
            PaginatorBoot.Remove("SessionTRN");
            return View();
        }

        [AuthorizeAcces(ActionType = AuthorizeAcces.IsRead, UrlMenu = "DestinationSailingTime")]
        public ActionResult RSailingEstimateListPage()
        {
            this.PaginatorBoot.Remove("SessionTRN");
            return RSailingEstimatePageXt();
        }

        public ActionResult RSailingEstimatePageXt()
        {
            Func<App.Data.Domain.EMCS.SpRSailingEstimation, List<Data.Domain.EMCS.SpRSailingEstimation>> func = delegate (App.Data.Domain.EMCS.SpRSailingEstimation filter)
            {
                var param = Request["params"];
                if (!string.IsNullOrEmpty(param))
                {
                    JavaScriptSerializer ser = new JavaScriptSerializer();
                    filter = ser.Deserialize<App.Data.Domain.EMCS.SpRSailingEstimation>(param);
                }
                var list = Service.EMCS.SvcRSailingEstimation.SailingEstimationList(filter);
                return list.ToList();
            };

            var paging = PaginatorBoot.Manage("SessionTRN", func).Pagination.ToJsonResult();
            return Json(paging, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetEstimationList(string origin, string destination)
        {
            var data = Service.EMCS.SvcRSailingEstimation.GetEstimationListExport(origin, destination);
            
            return Json(data, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult GetCountryList(MasterSearchForm crit)
        {
            var destination = Service.EMCS.SvcCipl.GetCountryList();
           
            return Json(destination, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult GetAreaList(MasterSearchForm crit)
        {
            var destination = Service.EMCS.SvcCipl.GetAreaList();

            return Json(destination, JsonRequestBehavior.AllowGet);
        }

        [AuthorizeAcces(ActionType = AuthorizeAcces.IsRead, UrlMenu = "DestinationSailingTime")]
        public ActionResult DownloadEstimationList(string origin, string destination)
        {

            var data = Service.EMCS.SvcRSailingEstimation.GetEstimationListExport(origin, destination);
            string fileExcel = Server.MapPath("~\\Content\\EMCS\\Templates\\Template_EstimationTime.xls");
            MemoryStream output = Service.EMCS.SvcRSailingEstimation.GetSailingEstimationStream(data, fileExcel);
            return File(output.ToArray(), "application/vnd.ms-excel", "DestinationAndSailingEstimation_" + DateTime.Now.ToString("ddMMyyyyhhhmmss") + ".xls");
        }
    }
}