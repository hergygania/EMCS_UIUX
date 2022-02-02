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
        //
        // GET: /ForecastAccuracyDetail/
        public ActionResult ForecastAccuracyDetail()
        {
            ForecastAccuracyFilter filter = new ForecastAccuracyFilter
            {
                customer_list = Service.CAT.Master.MasterCustomer.GetList(),
                store_list = Service.CAT.Master.MasterStore.GetListStoreNo(),
            };
            return View(filter);
        }

        public ActionResult ForecastAccuracyDetailPage()
        {
            this.PaginatorBoot.Remove("SessionTRN");
            return ForecastAccuracyDetailPageXt();
        }

        public ActionResult ForecastAccuracyDetailPageXt()
        {
            Func<ForecastAccuracyFilter, List<Data.Domain.ForecastAccuracyDetailList>> func = delegate(ForecastAccuracyFilter filter)
            {
                var param = Request["params"];
                if (!string.IsNullOrEmpty(param))
                {
                    JavaScriptSerializer ser = new JavaScriptSerializer();
                    filter = ser.Deserialize<ForecastAccuracyFilter>(param);
                }

                var list = Service.CAT.ForecastAccuracyDetailList.GetList(filter.customer_id, filter.month_id, filter.store_id, filter.year);
                return list.ToList();
            };

            var paging = PaginatorBoot.Manage("SessionTRN", func).Pagination.ToJsonResult();
            return Json(paging, JsonRequestBehavior.AllowGet);
        }

        public ActionResult DownloadForecastAccuracyDetail(ForecastAccuracyFilter filter)
        {
            Guid guid = Guid.NewGuid();
            Helper.Service.CAT.DownloadReportForcastAccuracyDetail data = new Helper.Service.CAT.DownloadReportForcastAccuracyDetail();

            Session[guid.ToString()] = data.DownloadToExcel(filter);

            return Json(guid.ToString(), JsonRequestBehavior.AllowGet);
        }
    }
}