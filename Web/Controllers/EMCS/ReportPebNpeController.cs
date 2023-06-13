using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using App.Web.App_Start;
using System.Web.Script.Serialization;
using System.IO;
using System.Net;
using NPOI.SS.Formula.Functions;
using System.Net.Http;
using System.Threading.Tasks;
using RestSharp;
using Newtonsoft.Json;
using App.Data.Domain.EMCS;

namespace App.Web.Controllers.EMCS
{
    public partial class EmcsController
    {

        [AuthorizeAcces(ActionType = AuthorizeAcces.IsRead)]
        public ActionResult PebReport()
        {
            ViewBag.AllowRead = AuthorizeAcces.AllowRead;
            ViewBag.AllowCreate = AuthorizeAcces.AllowCreated;
            ViewBag.AllowUpdate = AuthorizeAcces.AllowUpdated;
            ViewBag.AllowDelete = AuthorizeAcces.AllowDeleted;
            PaginatorBoot.Remove("SessionTRN");
            return View();
        }

        public ActionResult RPebNpePage(bool isclick = false)
        {
            this.PaginatorBoot.Remove("SessionTRN");
            return RPebNpePageXt(isclick);
        }

        public ActionResult RPebNpePageXt(bool isclick = false)
        {
            try
            {
                Func<App.Data.Domain.EMCS.DetailTrackingListFilter, List<Data.Domain.EMCS.SpRPebReport>> func = delegate (App.Data.Domain.EMCS.DetailTrackingListFilter filter)
                {
                    var param = Request["params"];
                    if (!string.IsNullOrEmpty(param))
                    {
                        JavaScriptSerializer ser = new JavaScriptSerializer();
                        filter = ser.Deserialize<App.Data.Domain.EMCS.DetailTrackingListFilter>(param);
                    }
                    if (isclick == true)
                    {
                       
                        var list = Service.EMCS.SvcRPebReport.RPebReports_old(filter);
                        return list.ToList();

                    }
                    else
                    {
                        filter.startMonth = "";
                        filter.endMonth = "";
                        var list = Service.EMCS.SvcRPebReport.RPebReports_old(filter);
                        return list.ToList();

                    }
                    //GetConvert(list);
                };
                var paging = PaginatorBoot.Manage("SessionTRN", func).Pagination.ToJsonResult();

                if (((App.Framework.Mvc.UI.LazyPagination<App.Data.Domain.EMCS.SpRPebReport>)((App.Framework.Mvc.JsonObject)((System.Web.Mvc.JsonResult)paging).Data).result).PageSize == 15)
                {
                    ((App.Framework.Mvc.UI.LazyPagination<App.Data.Domain.EMCS.SpRPebReport>)((App.Framework.Mvc.JsonObject)((System.Web.Mvc.JsonResult)paging).Data).result).PageSize = 10;
                }
                ((App.Framework.Mvc.JsonPageData)((System.Web.Mvc.JsonResult)paging).Data).sortorder = "Descending";
                return Json(paging, JsonRequestBehavior.AllowGet);

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public ActionResult GetConvert(List<SpRPebReport> list)
        {
            try
            {
                var Value = "";
                foreach (var item in list)
                {
                    if ("-" == item.Balanced.Substring(0, 1))
                    {
                        item.Balanced = item.Balanced.Trim(new Char[] { '-' });
                    }
                    string result = "";
                    WebRequest WReq = WebRequest.Create("https://api.exchangerate.host/convert?from=USD&to=IDR&amount=" + item.Balanced);
                    WebResponse wResp = WReq.GetResponse();
                    StreamReader sr = new StreamReader(wResp.GetResponseStream());
                    result = sr.ReadToEnd();
                    sr.Close();
                    dynamic obj = JsonConvert.DeserializeObject(result);
                    Value = ((Newtonsoft.Json.Linq.JProperty)((Newtonsoft.Json.Linq.JContainer)obj).Last).Value.ToString();
                    item.Balanced = Value;
                }
                return Json(list);


            }
            catch (Exception ex)
            {

                throw ex;
            }

        }
        [AuthorizeAcces(ActionType = AuthorizeAcces.IsRead, UrlMenu = "PebReport")]
        public JsonResult GetPebNpeList(string startDate, string endDate, string paramName, string paramValue, string keynum)
        {
            var data = Service.EMCS.SvcRPebReport.RPebReportsList(startDate, endDate, paramName, paramValue, keynum);
            //var total = Service.EMCS.SvcRPebReport.RPebReportsListCount(startDate, endDate, paramName, paramValue, keynum,1);
            //var paging = PaginatorBoot.Manage("SessionTRN", func).Pagination.ToJsonResult();
            return Json(new { rows = data.ToList(), total = data.Count }, JsonRequestBehavior.AllowGet);
        }

        //public JsonResult GetDetailsTrackingFilter()
        //{
        //    var category = Service.EMCS.MasterParameter.GetSelectList("Category");
        //    var categoryunit = Service.EMCS.MasterParameter.GetSelectList("CategoryUnit");
        //    var categoryspareparts = Service.EMCS.MasterParameter.GetSelectList("CategorySpareparts");

        //    return Json(new { category, categoryunit, categoryspareparts }, JsonRequestBehavior.AllowGet);
        //}

        [AuthorizeAcces(ActionType = AuthorizeAcces.IsRead, UrlMenu = "PebReport")]
        public FileResult DownloadPebReport(string startDate, string endDate, string paramName, string paramValue, string keyNum)
        {
            try
            {
                var data = Service.EMCS.SvcRPebReport.RPebReportsList(startDate, endDate, paramName, paramValue, keyNum);
                MemoryStream output = new MemoryStream();
                output = Service.EMCS.SvcRPebReport.GetPebReportStream(data);
                return File(output.ToArray(),   //The binary data of the XLS file
                    "application/vnd.ms-excel",//MIME type of Excel files
                    "PebReport.xlsx");
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
                 "DetailsTracking.xlsx");    //Suggested file name in the "Save as" dialog which will be displayed to the end user
            }
        }
    }
}