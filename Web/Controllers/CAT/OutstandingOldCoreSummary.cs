using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using App.Web.Models.CAT;
using System.Web.Script.Serialization;

namespace App.Web.Controllers.CAT
{
    public partial class CATController
    {
        public ActionResult OutstandingOldCoreSummary()
        {
            var outstandingoldcoresummaryfilter = new RptOutstandingOldCoreSummaryFilter { 
                storelist = Service.CAT.Master.MasterStore.GetList(),
                locationlist = Service.CAT.Master.MasterStore.GetList(),
                RangeWeek = Service.CAT.Report.RptOutstandingOldCoreDetailList.GetRangeWeek()
            };
            return View(outstandingoldcoresummaryfilter);
        }
        public ActionResult OutstandingOldCoreSummaryPage()
        {
            this.PaginatorBoot.Remove("SessionTRN");
            return OutstandingOldCoreSummaryPageXt();
        }

        public ActionResult OutstandingOldCoreSummaryPageXt()
        {
            Func<RptOutstandingOldCoreSummaryFilter, List<Data.Domain.RptOutstandingOldCoreSummaryList>> func = delegate(RptOutstandingOldCoreSummaryFilter filter)
            {
                var param = Request["params"];
                if (!string.IsNullOrEmpty(param))
                {
                    JavaScriptSerializer ser = new JavaScriptSerializer();
                    filter = ser.Deserialize<RptOutstandingOldCoreSummaryFilter>(param);
                }

                var list = Service.CAT.Report.RptOutstandingOldCoreSummaryList.GetList(filter.storeid, filter.DateFilter);
                return list.ToList();
            };

            var paging = PaginatorBoot.Manage("SessionTRN", func).Pagination.ToJsonResult();
            return Json(paging, JsonRequestBehavior.AllowGet);
        }

        public ActionResult DownloadOutstandingOldCoreSummary(RptOutstandingOldCoreSummaryFilter filter)
        {
            Guid guid = Guid.NewGuid();
            Helper.Service.CAT.DownloadReportOutstandingOldCoreSummary data = new Helper.Service.CAT.DownloadReportOutstandingOldCoreSummary();

            Session[guid.ToString()] = data.DownloadToExcel(filter);

            return Json(guid.ToString(), JsonRequestBehavior.AllowGet);
        }
    }
}