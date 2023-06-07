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
using Microsoft.Ajax.Utilities;

namespace App.Web.Controllers
{
    public partial class ReportController
    {
        public ActionResult PSSIntransit()
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
                Customers = GetCustomers()
            };
            return View(model);
        }

        public ActionResult PSSIntransitPage()
        {
            PaginatorBoot.Remove("SessionTRN");
            return PSSIntransitPageXt();
        }

        public ActionResult PSSIntransitPageXt()
        {
            Func<ReportFilterView, List<RptPSSIntransit>> func = delegate(ReportFilterView crit)
            {
                string param = Request["params"];
                if (!string.IsNullOrEmpty(param))
                {
                    var ser = new JavaScriptSerializer();
                    crit = ser.Deserialize<ReportFilterView>(param);
                }

                List<RptPSSIntransit> list = PSSIntransits.GetList(crit.GroupType, crit.FilterBy, crit.StoreNumber, crit.StartDate,
                    crit.EndDate, crit.CustomerId);
                return list.OrderBy(a => a.pss_UpdatedOn).ThenBy(a => a.pss_ID).ToList();
            };

            ActionResult paging = PaginatorBoot.Manage("SessionTRN", func).Pagination.ToJsonResult();
            return Json(paging, JsonRequestBehavior.AllowGet);
        }

        public void ExportToExcelPSSIntransit(string groupType, string filterBy, string storeNumber, DateTime? startDate, DateTime? endDate, string[] custId)
        {
            string[] customer = new string[] { };
            customer = custId != null ? (custId[0] != "null" ? custId[0].Split(',').ToArray() : null) : null;
            List<RptPSSIntransit> list = PSSIntransits.GetList(groupType, filterBy, storeNumber, startDate,
                endDate, customer);
            var dataInventory = list.OrderBy(a => a.pss_UpdatedOn).ThenBy(a => a.pss_Store).ToList();
            DataTable dt = DataTableHelper.ConvertTo(dataInventory);
            ExportToExcel(dt, "PSSIntransit");
        }


    }
}