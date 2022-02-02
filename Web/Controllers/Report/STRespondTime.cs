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
        public ActionResult STRespondTime()
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
            };
            return View(model);
        }

        public ActionResult STRespondTimePage()
        {
            PaginatorBoot.Remove("SessionTRN");
            return STRespondTimePageXt();
        }

        public ActionResult STRespondTimePageXt()
        {
          
                 Func<ReportFilterView, List<RptSTRespondTime>> func = delegate(ReportFilterView crit)
            {
                string param = Request["params"];
                if (!string.IsNullOrEmpty(param))
                {
                    var ser = new JavaScriptSerializer();
                    crit = ser.Deserialize<ReportFilterView>(param);
                }

                List<RptSTRespondTime> list = STRespondTimes.GetList(crit.GroupType, crit.FilterBy, crit.StoreNumber, crit.StartDate,
                    crit.EndDate);
                return list.OrderBy(a => a.strsp_UpdatedOn).ThenBy(a => a.strsp_UpdatedOn).ToList();
            };


            ActionResult paging = PaginatorBoot.Manage("SessionTRN", func).Pagination.ToJsonResult();
            return Json(paging, JsonRequestBehavior.AllowGet);
        }

        public void ExportToExcelSTRespondTimer(
         string groupType, string filterBy, string storeNumber, DateTime? startDate,
                DateTime? endDate)
        {
            var list = STRespondTimes.GetList(groupType, filterBy, storeNumber, startDate,
                endDate);

            DataTable dt = DataTableHelper.ConvertTo(list);
            ExportToExcel(dt, "PSSIntransit");
        }

   
    }
}