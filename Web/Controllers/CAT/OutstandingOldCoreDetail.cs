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
        public ActionResult OutstandingOldCoreDetail()
        {
            var filter = new RptOutstandingOldCoreDetailFilter { 
                StoreList = Service.CAT.Master.MasterStore.GetList(),
                SOSList = Service.CAT.Master.MasterSOS.GetList(),
                CustomerList = Service.CAT.Master.MasterCustomer.GetList(),
                RangeWeek = Service.CAT.Report.RptOutstandingOldCoreDetailList.GetRangeWeek()
        };
            return View(filter);
        }

        public ActionResult OutstandingOldCoreDetailPage()
        {
            this.PaginatorBoot.Remove("SessionTRN");
            return OutstandingOldCoreDetailPageXt();
        }

        public ActionResult OutstandingOldCoreDetailPageXt()
        {
            Func<App.Data.Domain.CAT.RptOutstandingOldCoreDetailFilter, List<Data.Domain.SP_RptOutstandingOldCoreDetail>> func = delegate(App.Data.Domain.CAT.RptOutstandingOldCoreDetailFilter filter)
            {
                var param = Request["params"];
                if (!string.IsNullOrEmpty(param))
                {
                    JavaScriptSerializer ser = new JavaScriptSerializer();
                    filter = ser.Deserialize<App.Data.Domain.CAT.RptOutstandingOldCoreDetailFilter>(param);
                }

                var list = Service.CAT.Report.RptOutstandingOldCoreDetailList.SP_GetList(filter);
                return list.ToList();
            };

            var paging = PaginatorBoot.Manage("SessionTRN", func).Pagination.ToJsonResult();
            return Json(paging, JsonRequestBehavior.AllowGet);
        }

        public ActionResult DownloadOutstandingOldCoreDetail(App.Data.Domain.CAT.RptOutstandingOldCoreDetailFilter filter)
        {
            Guid guid = Guid.NewGuid();
            Helper.Service.CAT.DownloadReportOutstandingOldCoreDetail data = new Helper.Service.CAT.DownloadReportOutstandingOldCoreDetail();

            Session[guid.ToString()] = data.DownloadToExcel(filter);

            return Json(guid.ToString(), JsonRequestBehavior.AllowGet);
        }
    }
}