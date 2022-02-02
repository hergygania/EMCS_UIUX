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
        public ActionResult OrderConfirmation()
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
                Customers = GetCustomersOrderConfirmation()
            };
            return View(model);
        }

        public ActionResult OrderConfirmationPage()
        {
            PaginatorBoot.Remove("SessionTRN");
            return OrderConfirmationPageXt();
        }

        public ActionResult OrderConfirmationPageXt()
        {
            Func<ReportFilterView, List<RptOrderConfirmation>> func = delegate(ReportFilterView crit)
            {
                string param = Request["params"];
                if (!string.IsNullOrEmpty(param))
                {
                    var ser = new JavaScriptSerializer();
                    crit = ser.Deserialize<ReportFilterView>(param);
                }

                List<RptOrderConfirmation> list = OrderConfirmations.GetList(crit.GroupType, crit.StoreNumber, crit.StartDate,
                    crit.EndDate, crit.CustomerId);
                return list.OrderBy(a => a.ordcnf_UpdatedBy).ThenBy(a => a.ordcnf_Store).ToList();
            };

            ActionResult paging = PaginatorBoot.Manage("SessionTRN", func).Pagination.ToJsonResult();
            return Json(paging, JsonRequestBehavior.AllowGet);
        }



        public void ExportToExcelDocordcnf(string groupType, string storeNumber, DateTime? startDate,
                DateTime? endDate, string[] custId)
        {
            string[] customer = new string[] { };
            customer = custId != null ? (custId[0] != "null" ? custId[0].Split(',').ToArray() : null) : null;
            List<RptOrderConfirmation> list =
                OrderConfirmations.GetList(groupType, storeNumber, startDate,
                endDate, customer);

            DataTable dt = DataTableHelper.ConvertTo(list);
            ExportToExcel(dt, "OrderConfirmation");
        }

        private IEnumerable<SelectListItem> GetCustomersOrderConfirmation()
        {
            IEnumerable<RptOrderConfirmation> list = OrderConfirmations.GetListCustomers();
            List<SelectListItem> listItem = list.Select(doc => new SelectListItem
            {
                Text = doc.ordcnf_CustID + "-" + doc.ordcnf_CustName,
                Value = doc.ordcnf_CustID
            }).ToList();

            return listItem;
        }
    }
}