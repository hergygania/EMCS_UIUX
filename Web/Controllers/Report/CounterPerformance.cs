using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using App.Data.Domain;
using App.Service.Report;
using App.Web.Helper.Extensions;
using App.Web.Models;

namespace App.Web.Controllers
{
    public partial class ReportController
    {
        public ActionResult CounterPerformance()
        {
            PaginatorBoot.Remove("SessionTRN");
            var userName = Domain.SiteConfiguration.UserName;
            var model = new ReportFilterView
            {
                StartDate = new DateTime(DateTime.Now.Year, 1, 1),
                EndDate = DateTime.Now,
                HubList = GetListHubByUserName(userName),
                AreaList = GetListAreaByUserName(userName),
                StoreNumberList = GetListStoreByUserName(userName),
                Customers = GetUserCounterPerformances()//new SelectList(CounterPerformances.GetListUser(), "ctprf_DBSUserID","ctprf_DBSUserID""ctprf_DBSUserName")
            };
            return View(model);
        }

        public ActionResult CounterPerformancePage()
        {
            PaginatorBoot.Remove("SessionTRN");
            return CounterPerformancePageXt();
        }

        public ActionResult CounterPerformancePageXt()
        {
            Func<ReportFilterView, List<RptCounterPerformance>> func = delegate(ReportFilterView crit)
            {
                string param = Request["params"];
                if (!string.IsNullOrEmpty(param))
                {
                    var ser = new JavaScriptSerializer();
                    crit = ser.Deserialize<ReportFilterView>(param);
                }

                List<RptCounterPerformance> list = CounterPerformances.GetList(crit.GroupType, crit.FilterBy, crit.StoreNumber, crit.StartDate,
                    crit.EndDate, crit.CustomerId);
                return list.OrderBy(a => a.ctprf_UpdatedOn).ThenBy(a => a.ctprf_Store).ToList();
            };

            ActionResult paging = PaginatorBoot.Manage("SessionTRN", func).Pagination.ToJsonResult();
            return Json(paging, JsonRequestBehavior.AllowGet);
        }

        private IEnumerable<SelectListItem> GetUserCounterPerformances()
        {
            List<RptCounterPerformance> list = CounterPerformances.GetListUser();
            List<SelectListItem> listItem = list.Select(doc => new SelectListItem
            {
                Text = doc.ctprf_DBSUserID +"-"+ doc.ctprf_DBSUserName,
                Value = doc.ctprf_DBSUserID
            }).ToList();

            return listItem;
        }
        public void ExportToExcelCounterPerformance(string groupType, string filterBy, string storeNumber, DateTime? startDate,
                DateTime? endDate, string[] custId)
        {
            var list = CounterPerformances.GetList(groupType, filterBy, storeNumber, startDate,
                endDate, custId);
            DataTable dt = DataTableHelper.ConvertTo(list);
            ExportToExcel(dt, "CounterPerformance");
        }

    }
}