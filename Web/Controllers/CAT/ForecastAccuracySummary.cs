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
        public ActionResult ForecastAccuracySummary()
        {
            ForecastAccuracyFilter filter = new ForecastAccuracyFilter { 
                customer_list = Service.CAT.Master.MasterCustomer.GetList(),
                store_list = Service.CAT.Master.MasterStore.GetListStoreNo(),
            };
            return View(filter);
        }

        public ActionResult ForecastAccuracySummaryPage()
        {
            this.PaginatorBoot.Remove("SessionTRN");
            return ForecastAccuracySummaryPageXt();
        }

        public ActionResult ForecastAccuracySummaryPageXt()
        {
            Func<ForecastAccuracyFilter, List<Data.Domain.ForecastAccuracySummaryList>> func = delegate(ForecastAccuracyFilter filter)
            {
                var param = Request["params"];
                if (!string.IsNullOrEmpty(param))
                {
                    JavaScriptSerializer ser = new JavaScriptSerializer();
                    filter = ser.Deserialize<ForecastAccuracyFilter>(param);
                }

                var list = Service.CAT.ForecastAccuracySummaryList.GetList(filter.customer_id, filter.month_id, filter.store_id, filter.year);
                return list.ToList();
            };

            var paging = PaginatorBoot.Manage("SessionTRN", func).Pagination.ToJsonResult();
            return Json(paging, JsonRequestBehavior.AllowGet);
        }

        public ActionResult DownloadForecastAccuracySummary(ForecastAccuracyFilter filter)
        {
            Guid guid = Guid.NewGuid();
            Helper.Service.CAT.DownloadReportForecastAccuracySummary data = new Helper.Service.CAT.DownloadReportForecastAccuracySummary();

            Session[guid.ToString()] = data.DownloadToExcel(filter);

            return Json(guid.ToString(), JsonRequestBehavior.AllowGet);
        }
    }
}