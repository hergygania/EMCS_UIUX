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
        public ActionResult PartReleaseInformation()
        {
            PaginatorBoot.Remove("SessionTRN");
            var model = new ReportFilterView
            {
                StartDate = new DateTime(DateTime.Now.Year, 1, 1),
                EndDate = DateTime.Now,
                Customers = GetCustomers()
            };
            return View(model);
        }

        public ActionResult PartReleaseInformationPage()
        {
            PaginatorBoot.Remove("SessionTRN");
            return PartReleaseInformationPageXt();
        }

        public ActionResult PartReleaseInformationPageXt()
        {
            Func<ReportFilterView, List<RptPartReleaseInformation>> func = delegate(ReportFilterView crit)
            {
                string param = Request["params"];
                if (!string.IsNullOrEmpty(param))
                {
                    var ser = new JavaScriptSerializer();
                    crit = ser.Deserialize<ReportFilterView>(param);
                }

                List<RptPartReleaseInformation> list = PartReleaseInformations.GetList(crit.GroupType, crit.StoreNumber, crit.StartDate,
                    crit.EndDate, crit.CustomerId);
                return list.OrderBy(a => a.rpartrel_UpdatedBy).ThenBy(a => a.rpartrel_Store).ToList();
            };

            ActionResult paging = PaginatorBoot.Manage("SessionTRN", func).Pagination.ToJsonResult();
            return Json(paging, JsonRequestBehavior.AllowGet);
        }



        public void ExportToExcelPartRelease(string groupType, string storeNumber, DateTime? startDate,
                DateTime? endDate, string[] custId)
        {
            string[] customer = new string[] { };
            customer = custId != null ? (custId[0] != "null" ? custId[0].Split(',').ToArray() : null) : null;
            var list = PartReleaseInformations.GetList(groupType, storeNumber, startDate,
                endDate, customer);
            DataTable dt = DataTableHelper.ConvertTo(list);
            ExportToExcel(dt, "PartReleaseInformation");
        }
    }
}