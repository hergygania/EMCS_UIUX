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
        public ActionResult DocumentConsolidateInvoice()
        {
            ViewBag.AllowRead = AuthorizeAcces.AllowRead;

            PaginatorBoot.Remove("SessionTRN");
            var userName = Domain.SiteConfiguration.UserName;
            var model = new ReportFilterView
            {
                StartDate = new DateTime(DateTime.Now.Year, 1, 1),
                EndDate = DateTime.Now,
                HubList = GetListHubByUserName(userName),
                AreaList = GetListAreaByUserName(userName),
                StoreNumberList = GetListStoreByUserName(userName),
                Customers = GetCustomersDocConsolidateInvoice()

            };
            return View(model);
        }

        public ActionResult DocumentConsolidateInvoicePage()
        {
            PaginatorBoot.Remove("SessionTRN");
            return DocumentConsolidateInvoicePageXt();
        }

        public ActionResult DocumentConsolidateInvoicePageXt()
        {
            Func<ReportFilterView, List<RptDocumentConsolidateInvoice>> func =
                delegate(ReportFilterView crit)
                {
                    string param = Request["params"];
                    if (!string.IsNullOrEmpty(param))
                    {
                        var ser = new JavaScriptSerializer();
                        crit = ser.Deserialize<ReportFilterView>(param);
                    }
                    var userId = User.Identity.GetUserId();
                    List<RptDocumentConsolidateInvoice> list = DocumentConsolidateInvoices
                        .GetList(crit.GroupType, crit.FilterBy,
                        crit.StoreNumber, crit.CustomerId,userId);
                    return list.OrderBy(a => a.rcinv_UpdatedOn).ThenBy(a => a.rcinv_Store).ToList();
                };

            ActionResult paging = PaginatorBoot.Manage("SessionTRN", func).Pagination.ToJsonResult();
            return Json(paging, JsonRequestBehavior.AllowGet);
        }



        //public void ExportToExcelDciv(string groupType, string filterBy, string storeNumber, DateTime? startDate,
        //        DateTime? endDate, string[] custId)
        //{
        //    string[] customer = { };
        //    customer = custId != null ? (custId[0] != "null" ? custId[0].Split(',').ToArray() : null) : null;
        //    List<RptDocumentConsolidateInvoice> list = DocumentConsolidateInvoices.GetList(groupType, filterBy, storeNumber, startDate,
        //        endDate, customer);
        //    DataTable dt = DataTableHelper.ConvertTo(list);
        //    ExportToExcel(dt, "DocumentConsolidateInvoice");

        //}

        public ActionResult ExportToExcelDciv(string groupType, string filterBy, string storeNumber, string[] custId)
        {
            string[] customer = { };
            customer = custId != null ? (custId[0] != "null" ? custId[0].Split(',').ToArray() : null) : null;
            var userId = User.Identity.GetUserId();
            List<RptDocumentConsolidateInvoice> listCurrent = DocumentConsolidateInvoices.GetList(groupType, filterBy, storeNumber, customer,userId);

            ViewBag.Title = "Document Consolidate Invoice";
            var sb = new System.Text.StringBuilder();
            sb.Append("<b>Document Consolidate Invoice</b>");
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
                         "<th>Customer ID</th>" +
                          "<th>Customer Name</th>" +
                          "</tr>");


                foreach (var e in listCurrent.ToList())
                {
                    var docDate = (e.rcinv_DocDate.HasValue) ? e.rcinv_DocDate.Value.ToString("dd MMM yyyy") : "";
                    var docValue = (e.rcinv_DocValue != null)
                        ? FormatStringNumber(e.rcinv_DocValue)
                        : "";
                    no++;
                    sb.Append("<tr>" +
                              "<td>" + no + "</td>" +
                              "<td>" + e.rcinv_StoreName + "&nbsp;</td>" +
                              "<td>" + e.rcinv_AreaName + "&nbsp;</td>" +
                              "<td>" + e.rcinv_HubName + "&nbsp;</td>" +
                              "<td>" + e.rcinv_RefDoc + "&nbsp;</td>" +
                              "<td>" + e.rcinv_CustPONo + "&nbsp;</td>" +
                              "<td style='align:right'>&nbsp;" + docDate + "</td>" +
                              "<td style='align:right'>&nbsp;" + docValue + "</td>" +
                             "<td>" + e.rcinv_CustID + "</td>" +
                              "<td>" + e.rcinv_CustName + "</td>" +
                               "</tr>");
                }
                sb.Append("</table>");
            }
            return View("excel", sb);

        }



        private IEnumerable<SelectListItem> GetCustomersDocConsolidateInvoice()
        {
            List<RptDocumentConsolidateInvoice> list = DocumentConsolidateInvoices.GetListCustomers();
            List<SelectListItem> listItem = list.Select(doc => new SelectListItem
            {
                Text = doc.rcinv_CustID + "-" + doc.rcinv_CustName,
                Value = doc.rcinv_CustName
            }).ToList();

            return listItem;
        }
    }
}