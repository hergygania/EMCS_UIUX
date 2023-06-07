using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using App.Web.App_Start;
using System.Web.Script.Serialization;
using System.IO;

namespace App.Web.Controllers.EMCS
{
    public partial class EmcsController
    {

        [AuthorizeAcces(ActionType = AuthorizeAcces.IsRead)]
        public ActionResult DetailsTracking()
        {
            ViewBag.AllowRead = AuthorizeAcces.AllowRead;
            ViewBag.AllowCreate = AuthorizeAcces.AllowCreated;
            ViewBag.AllowUpdate = AuthorizeAcces.AllowUpdated;
            ViewBag.AllowDelete = AuthorizeAcces.AllowDeleted;
            PaginatorBoot.Remove("SessionTRN");
            return View();
        }

        public ActionResult RDetailsTrackingListPage()
        {
            this.PaginatorBoot.Remove("SessionTRN");
            return RDetailsTrackingPageXt();
        }

        public ActionResult RDetailsTrackingPageXt()
        {
            Func<App.Data.Domain.EMCS.DetailTrackingListFilter, List<Data.Domain.EMCS.SpRDetailsTracking>> func = delegate (App.Data.Domain.EMCS.DetailTrackingListFilter filter)
            {
                var param = Request["params"];
                if (!string.IsNullOrEmpty(param))
                {
                    JavaScriptSerializer ser = new JavaScriptSerializer();
                    filter = ser.Deserialize<App.Data.Domain.EMCS.DetailTrackingListFilter>(param);
                }
                var list = Service.EMCS.SvcRDetailsTracking.DetailsTrackingList_old(filter);
                return list.ToList();
            };

            var paging = PaginatorBoot.Manage("SessionTRN", func).Pagination.ToJsonResult();
            return Json(paging, JsonRequestBehavior.AllowGet);
        }

        [AuthorizeAcces(ActionType = AuthorizeAcces.IsRead, UrlMenu = "DetailsTracking")]
        public JsonResult GetDetailsTrackingList(string startDate, string endDate, string paramName, string paramValue, string keynum)
        {
            var data = Service.EMCS.SvcRDetailsTracking.DetailsTrackingList(startDate, endDate, paramName, paramValue, keynum);

            //var paging = PaginatorBoot.Manage("SessionTRN", func).Pagination.ToJsonResult();
            return Json(new { rows = data.ToList(), total = data.Count }, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetDetailsTrackingFilter()
        {
            var category = Service.EMCS.MasterParameter.GetSelectList("Category");
            var categoryunit = Service.EMCS.MasterParameter.GetSelectList("CategoryUnit");
            var categoryspareparts = Service.EMCS.MasterParameter.GetSelectList("CategorySpareparts");

            return Json(new { category, categoryunit, categoryspareparts }, JsonRequestBehavior.AllowGet);
        }

        [AuthorizeAcces(ActionType = AuthorizeAcces.IsRead, UrlMenu = "DetailsTracking")]
        public FileResult DownloadDetailsTracking(string startDate, string endDate, string paramName, string paramValue, string keyNum)
        {
            try
            {
                var data = Service.EMCS.SvcRDetailsTracking.DetailsTrackingList(startDate, endDate, paramName, paramValue, keyNum);
                MemoryStream output = new MemoryStream();
                output = Service.EMCS.SvcRDetailsTracking.GetDetailsTrackingStream(data);
                return File(output.ToArray(),   //The binary data of the XLS file
                    "application/vnd.ms-excel",//MIME type of Excel files
                    "DetailsTracking.xlsx");
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