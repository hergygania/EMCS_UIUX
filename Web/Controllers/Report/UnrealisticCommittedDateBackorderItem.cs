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
        public ActionResult UnrealisticCommittedDateBackorderItem()
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
                Customers = GetCustomersCommitedDateBackorderItem(),//new SelectList(UnrealisticCommittedDateBackorderItems.GetListCustomers(), "ucdbi_CustID", "ucdbi_CustName")

            };
            return View(model);
        }
        [AuthorizeAcces(ActionType = AuthorizeAcces.IsRead)]
        public ActionResult UnrealisticCommittedDateBackorderItemPage()
        {
            PaginatorBoot.Remove("SessionTRN");
            return UnrealisticCommittedDateBackorderItemPageXt();
        }
        [AuthorizeAcces(ActionType = AuthorizeAcces.IsRead)]
        public ActionResult UnrealisticCommittedDateBackorderItemPageXt()
        {
            Func<ReportFilterView, List<RptUnrealisticCommittedDateBackorderItem>> func = delegate(ReportFilterView crit)
            {
                string param = Request["params"];
                if (!string.IsNullOrEmpty(param))
                {
                    var ser = new JavaScriptSerializer();
                    crit = ser.Deserialize<ReportFilterView>(param);
                }

                var userId = User.Identity.GetUserId();
                List<RptUnrealisticCommittedDateBackorderItem> list = UnrealisticCommittedDateBackorderItems.GetList(crit.GroupType, crit.FilterBy, crit.StoreNumber,
                    crit.CustomerId, userId);
                return list.OrderBy(a => a.ucdbi_UpdatedBy).ThenBy(a => a.ucdbi_Store).ToList();
            };

            ActionResult paging = PaginatorBoot.Manage("SessionTRN", func).Pagination.ToJsonResult();
            return Json(paging, JsonRequestBehavior.AllowGet);
        }



        //public void ExportToExcelCommitedDateBackorderIteme(string groupType, string filterType, string storeNumber, DateTime? startDate,
        //        DateTime? endDate, string[] custId)
        //{
        //    string[] customer = new string[] { };
        //    customer = custId != null ? (custId[0] != "null" ? custId[0].Split(',').ToArray() : null) : null;
        //    var list = UnrealisticCommittedDateBackorderItems.GetList(groupType,filterType, storeNumber, startDate,
        //        endDate, customer);
        //    DataTable dt = DataTableHelper.ConvertTo(list);
        //    ExportToExcel(dt, "CommitedDateBackorderIteme");
        //}
        [AuthorizeAcces(ActionType = AuthorizeAcces.IsRead, UrlMenu = "OutstandingBackorderItem")]
        public ActionResult ExportToExcelCommitedDateBackorderIteme(string groupType, string filterBy, string storeNumber, string[] custId)
        {
            string[] customer = new string[] { };
            customer = custId != null ? (custId[0] != "null" ? custId[0].Split(',').ToArray() : null) : null;
            var userId = User.Identity.GetUserId();
            var listCurrent = UnrealisticCommittedDateBackorderItems.GetList(groupType, filterBy, storeNumber, customer, userId);
            ViewBag.Title = "Unrealistic Committed Date Backorder";
            var sb = new System.Text.StringBuilder();
            sb.Append("<b>Unrealistic Committed Date Backorder</b>");
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
                          "<th>Commited Date</th>" +
                          "<th>SOS</th>" +
                          "<th>Part Number</th>" +
                          "<th>Order Method</th>" +
                          "<th>Facility</th>" +
                          "<th>Lead Time</th>" +
                          "<th>Gap Of Date</th>" +
                          "<th>Customer ID</th>" +
                          "<th>Customer Name</th>" +
                          "</tr>");


                foreach (var e in listCurrent.ToList())
                {
                 //   var docDate = (e.ucdbi_DocDate.HasValue) ? e.ucdbi_DocDate.Value.ToString("dd MMM yyyy") : "";
                    var commitedDate = (e.ucdbi_CommittedDate.HasValue) ? e.ucdbi_CommittedDate.Value.ToString("dd MMM yyyy") : "";
                    var docValue = (e.ucdbi_DocValue != null)
                        ? FormatStringNumber(e.ucdbi_DocValue)
                        : "";
                    no++;
                    sb.Append("<tr>" +
                              "<td>" + no + "</td>" +
                              "<td>" + e.ucdbi_Store + "&nbsp;</td>" +
                              "<td>" + e.ucdbi_AreaName + "&nbsp;</td>" +
                              "<td>" + e.ucdbi_HubName + "&nbsp;</td>" +
                              "<td>" + e.ucdbi_RefDoc + "&nbsp;</td>" +
                              "<td>" + e.ucdbi_CustPONo + "&nbsp;</td>" +
                              "<td style='align:right'>&nbsp;" + e.ucdbi_DocDate + "</td>" +
                              "<td>" + docValue + "</td>" +
                              "<td style='align:right'>&nbsp;" + commitedDate + "</td>" +
                              "<td>" + e.ucdbi_SOS + "</td>" +
                              "<td>" + e.ucdbi_PartNo + "</td>" +
                              "<td>" + e.ucdbi_OrderMethod + "</td>" +
                              "<td>" + e.ucdbi_Facility + "</td>" +
                              "<td>" + e.ucdbi_LeadTime + "</td>" +
                              "<td>" + e.ucdbi_GapOfDate + "</td>" +
                              "<td>" + e.ucdbi_CustID + "</td>" +
                              "<td>" + e.ucdbi_CustName + "</td>" +
                              "</tr>");
                }
                sb.Append("</table>");
            }
            return View("excel", sb);

        }

        private IEnumerable<SelectListItem> GetCustomersCommitedDateBackorderItem()
        {
            List<RptUnrealisticCommittedDateBackorderItem> list = UnrealisticCommittedDateBackorderItems.GetListCustomers();
            List<SelectListItem> listItem = list.Select(doc => new SelectListItem
            {
                Text = doc.ucdbi_CustID + "-" + doc.ucdbi_CustName,
                Value = doc.ucdbi_CustID
            }).ToList();

            return listItem;
        }
    }
}