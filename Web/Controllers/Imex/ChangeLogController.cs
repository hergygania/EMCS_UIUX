using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using App.Data.Domain.ChangeLog;
using App.Service.Imex;

namespace App.Web.Controllers.Imex
{
    public class ChangeLogController : Controller
    {
        ChangeLogBLL ChangeLogBLL = new ChangeLogBLL();
        // GET: ChangeLog
        public ActionResult Index()
        {
            return View();
        }

        public JsonResult GetChangeLogNewest()
        {
            List<ChangeLogModel> data = ChangeLogBLL.GetChangeLogNewest();
            return Json(data, JsonRequestBehavior.AllowGet);
        }
        public JsonResult GetChangeLogDaily()
        {
            List<ChangeLogModel> data = ChangeLogBLL.GetChangeLogDaily();
            return Json(data, JsonRequestBehavior.AllowGet);
        }
        public JsonResult GetChangeLogWeekly()
        {
            List<ChangeLogModel> data = ChangeLogBLL.GetChangeLogWeekly();
            return Json(data, JsonRequestBehavior.AllowGet);
        }
        public JsonResult GetChangeLogMonthly()
        {
            List<ChangeLogModel> data = ChangeLogBLL.GetChangeLogMonthly();
            return Json(data, JsonRequestBehavior.AllowGet);
        }
        public JsonResult GetChangeLogByDate(string dateFromW, string dateToW)
        {
            ChangeLogFilter filter = new ChangeLogFilter();

            #region Initialize Filter
            DateTime dateFrom = new DateTime();
            if (dateFromW != "" && !String.IsNullOrEmpty(dateFromW) && dateFromW != "null")
            {
                dateFrom = DateTime.ParseExact(dateFromW, "dd MMM yyyy", System.Globalization.CultureInfo.InvariantCulture);
                filter.DateFrom = dateFrom;
            }
            else
                filter.DateFrom = null;

            DateTime dateTo = new DateTime();
            if (dateToW != "" && !String.IsNullOrEmpty(dateToW) && dateToW != "null")
            {
                dateTo = DateTime.ParseExact(dateToW, "dd MMM yyyy", System.Globalization.CultureInfo.InvariantCulture);
                filter.DateTo = dateTo;
            }
            else
                filter.DateTo = null;
            #endregion

            List<ChangeLogModel> data = ChangeLogBLL.GetChangeLogByDate(filter);
            return Json(data, JsonRequestBehavior.AllowGet);
        }
        public JsonResult DownloadChangeLog(string SPName)
        {
            Helper.Service.DownloadChangelog DownloadChangelog = new Helper.Service.DownloadChangelog();
            Guid guid = Guid.NewGuid();
            Session[guid.ToString()] = DownloadChangelog.DownloadToExcel(SPName);
            return Json(guid.ToString(), JsonRequestBehavior.AllowGet);
        }
        public JsonResult DownloadChangeLogByDate(string dateFromW, string dateToW)
        {
            #region Initialize Filter
            DateTime dateFrom = new DateTime();
            ChangeLogFilter filter = new ChangeLogFilter();
            if (dateFromW != "" && !String.IsNullOrEmpty(dateFromW) && dateFromW != "null")
            {
                dateFrom = DateTime.ParseExact(dateFromW, "dd MMM yyyy", System.Globalization.CultureInfo.InvariantCulture);
                filter.DateFrom = dateFrom;
            }
            else
                filter.DateFrom = null;

            DateTime dateTo = new DateTime();
            if (dateToW != "" && !String.IsNullOrEmpty(dateToW) && dateToW != "null")
            {
                dateTo = DateTime.ParseExact(dateToW, "dd MMM yyyy", System.Globalization.CultureInfo.InvariantCulture);
                filter.DateTo = dateTo;
            }
            else
                filter.DateTo = null;
            #endregion

            Helper.Service.DownloadChangelog DownloadChangelog = new Helper.Service.DownloadChangelog();
            Guid guid = Guid.NewGuid();
            Session[guid.ToString()] = DownloadChangelog.DownloadToExcelByDate(filter);
            return Json(guid.ToString(), JsonRequestBehavior.AllowGet);
        }
            public FileResult DownloadToExcel(string guid)
        {
            return Session[guid] as FileResult;
        }

    }
}