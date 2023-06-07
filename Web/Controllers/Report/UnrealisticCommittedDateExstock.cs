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
using App.Web.App_Start;

namespace App.Web.Controllers
{
    public partial class ReportController
    {
        //[myAuthorize(Roles = "ReportPartCounter")]
        [AuthorizeAcces(ActionType = AuthorizeAcces.IsRead)]
        public ActionResult UnrealisticCommittedDateExstock()
        {
            ViewBag.AllowRead = AuthorizeAcces.AllowRead;
            ViewBag.AllowCreate = AuthorizeAcces.AllowCreated;
            ViewBag.AllowUpdate = AuthorizeAcces.AllowUpdated;
            ViewBag.AllowDelete = AuthorizeAcces.AllowDeleted;
            PaginatorBoot.Remove("SessionTRN");
            var userName = Domain.SiteConfiguration.UserName;
            var model = new ReportFilterView
            {
                StartDate = new DateTime(DateTime.Now.Year, 1, 1),
                EndDate = DateTime.Now,
                HubList = GetListHubByUserName(userName),
                AreaList = GetListAreaByUserName(userName),
                StoreNumberList = GetListStoreByUserName(userName),
                Customers = GetCustomersDateExstock(),// new SelectList(UnrealisticCommittedDateExstocks.GetListCustomers(), "urcdx_CustID", "urcdx_CustName")

            };
            return View(model);
        }
        [AuthorizeAcces(ActionType = AuthorizeAcces.IsRead, UrlMenu = "UnrealisticCommittedDateExstock")]
        public ActionResult UnrealisticCommittedDateExstockPage()
        {
            PaginatorBoot.Remove("SessionTRN");
            return UnrealisticCommittedDateExstockPageXt();
        }
        [AuthorizeAcces(ActionType = AuthorizeAcces.IsRead, UrlMenu = "UnrealisticCommittedDateExstock")]
        public ActionResult UnrealisticCommittedDateExstockPageXt()
        {
            Func<ReportFilterView, List<RptUnrealisticCommittedDateExstock>> func = delegate(ReportFilterView crit)
            {
                string param = Request["params"];
                if (!string.IsNullOrEmpty(param))
                {
                    var ser = new JavaScriptSerializer();
                    crit = ser.Deserialize<ReportFilterView>(param);
                }

                var userId = User.Identity.GetUserId();
                List<RptUnrealisticCommittedDateExstock> list = UnrealisticCommittedDateExstocks.GetList(crit.GroupType, crit.FilterBy, crit.StoreNumber, crit.CustomerId, userId);
                return list.OrderBy(a => a.urcdx_UpdatedBy).ThenBy(a => a.urcdx_Store).ToList();
            };

            ActionResult paging = PaginatorBoot.Manage("SessionTRN", func).Pagination.ToJsonResult();
            return Json(paging, JsonRequestBehavior.AllowGet);
        }



        //public void ExportToExcelCommitedDateExstock(string groupType, string filterBy, string storeNumber, DateTime? startDate,
        //        DateTime? endDate, string[] custId)
        //{
        //    string[] customer = new string[] { };
        //    customer = custId != null ? (custId[0] != "null" ? custId[0].Split(',').ToArray() : null) : null;
        //    var list = UnrealisticCommittedDateExstocks.GetList(groupType, filterBy, storeNumber, startDate,
        //        endDate, customer);
        //    DataTable dt = DataTableHelper.ConvertTo(list);
        //    ExportToExcel(dt, "CommitedDateExstock");
        //}
        [AuthorizeAcces(ActionType = AuthorizeAcces.IsRead, UrlMenu = "UnrealisticCommittedDateExstock")]
        public ActionResult ExportToExcelCommitedDateExstock(string groupType, string filterBy, string storeNumber, string[] custId)
        {
            string[] customer = new string[] { };
            var userId = User.Identity.GetUserId();
            customer = custId != null ? (custId[0] != "null" ? custId[0].Split(',').ToArray() : null) : null;
            var listCurrent = UnrealisticCommittedDateExstocks.GetList(groupType, filterBy, storeNumber, customer, userId);

            ViewBag.Title = "Unrealistic Committed Date Exstock";
            var sb = new System.Text.StringBuilder();
            sb.Append("<b>Unrealistic Committed Date Exstock</b>");
            if (listCurrent != null)
            {

                var no = 0;
                sb.Append("<table border=1  cellspacing='3'>");
                sb.Append("<tr>" +
                          "<th>No</th>" +
                          "<th>Store</th>" +
                          "<th>Area</th>" +
                          "<th>Hub</th>" +
                          "<th>Reference Document</th>" +
                          "<th>Customer PO Number</th>" +
                          "<th style='align:right'>Document Date</th>" +
                          "<th style='align:right'>Document Value</th>" +
                          "<th>Gap Of Date</th>" +
                          "<th>Customer ID</th>" +
                          "<th>Customer Name</th>" +
                          "<th>Commited Date</th>" +
                          "</tr>");


                foreach (var e in listCurrent.ToList())
                {
                    var docDate = e.urcdx_DocDate;
                    var commitedDate = (e.urcdx_CommittedDate.HasValue) ? e.urcdx_CommittedDate.Value.ToString("dd MMM yyyy") : "";
                    var docValue = (e.urcdx_DocValue != null)
                        ? FormatStringNumber(e.urcdx_DocValue)
                        : "";
                    no++;
                    sb.Append("<tr>" +
                              "<td>" + no + "</td>" +
                              "<td>" + e.urcdx_StoreName + "&nbsp;</td>" +
                              "<td>" + e.urcdx_AreaName + "&nbsp;</td>" +
                              "<td>" + e.urcdx_HubName + "&nbsp;</td>" +
                              "<td>" + e.urcdx_RefDoc + "&nbsp;</td>" +
                              "<td>" + e.urcdx_CustPONo + "&nbsp;</td>" +
                              "<td style='align:right'>&nbsp;" + docDate + "</td>" +
                              "<td style='align:right'>&nbsp;" + docValue + "</td>" +
                              "<td>" + e.urcdx_GapOfDate + "</td>" +
                              "<td>" + e.urcdx_CustID + "</td>" +
                              "<td>" + e.urcdx_CustName + "</td>" +
                               "<td style='align:right'>&nbsp;" + commitedDate + "</td>" +
                              "</tr>");
                }
                sb.Append("</table>");
            }
            return View("excel", sb);

        }


        private IEnumerable<SelectListItem> GetCustomersDateExstock()
        {
            var list = UnrealisticCommittedDateExstocks.GetListCustomers();
            List<SelectListItem> listItem = list.Select(doc => new SelectListItem
            {
                Text = doc.urcdx_CustID + "-" + doc.urcdx_CustName,
                Value = doc.urcdx_CustID
            }).ToList();

            return listItem;
        }
    }
}