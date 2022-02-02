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
        public ActionResult DocumentNonConsolidateInvoice()
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
                Customers = GetCustomersDocNonConsolidateInvoice(),//new SelectList(DocumentNonConsolidateInvoices.GetListCustomers(), "rncinv_CustID", "rncinv_CustName")
            };
            return View(model);
        }

        public ActionResult DocumentNonConsolidateInvoicePage()
        {
            PaginatorBoot.Remove("SessionTRN");
            return DocumentNonConsolidateInvoicePageXt();
        }

        public ActionResult DocumentNonConsolidateInvoicePageXt()
        {
            Func<ReportFilterView, List<RptDocumentNonConsolidateInvoice>> func =
                delegate(ReportFilterView crit)
                {
                    string param = Request["params"];
                    if (!string.IsNullOrEmpty(param))
                    {
                        var ser = new JavaScriptSerializer();
                        crit = ser.Deserialize<ReportFilterView>(param);
                    }
                    var userId = User.Identity.GetUserId();
                    List<RptDocumentNonConsolidateInvoice> list = DocumentNonConsolidateInvoices.GetList(crit.GroupType, crit.FilterBy,
                        crit.StoreNumber, crit.CustomerId, userId);
                    return list.OrderBy(a => a.rncinv_UpdatedOn).ThenBy(a => a.rncinv_Store).ToList();
                };

            ActionResult paging = PaginatorBoot.Manage("SessionTRN", func).Pagination.ToJsonResult();
            return Json(paging, JsonRequestBehavior.AllowGet);
        }



        //public void ExportToExcelDnciv(string groupType,string filterType, string storeNumber, DateTime? startDate,
        //        DateTime? endDate, string[] custId)
        //{
        //    string[] customer = { };
        //    customer = custId != null ? (custId[0] != "null" ? custId[0].Split(',').ToArray() : null) : null;
        //    List<RptDocumentNonConsolidateInvoice> list = DocumentNonConsolidateInvoices.GetList(groupType,filterType, storeNumber, startDate,
        //        endDate, customer);

        //    DataTable dt = DataTableHelper.ConvertTo(list);
        //    ExportToExcel(dt, "DocumentNonConsolidateInvoice");
        //}

        public ActionResult ExportToExcelDnciv(string groupType, string filterBy, string storeNumber, string[] custId)
        {
            var userId = User.Identity.GetUserId();
            string[] customer = { };
            customer = custId != null ? (custId[0] != "null" ? custId[0].Split(',').ToArray() : null) : null;
            List<RptDocumentNonConsolidateInvoice> listCurrent = DocumentNonConsolidateInvoices.GetList(groupType, filterBy, storeNumber, customer, userId);

            ViewBag.Title = "Document Non Consolidate Invoice";
            var sb = new System.Text.StringBuilder();
            sb.Append("<b>Document Non Consolidate Invoice</b>");
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
                    var docDate = (e.rncinv_DocDate.HasValue) ? e.rncinv_DocDate.Value.ToString("dd MMM yyyy") : "";
                    var docValue = (e.rncinv_DocValue != null)
                        ? FormatStringNumber(e.rncinv_DocValue)
                        : "";
                    no++;
                    sb.Append("<tr>" +
                              "<td>" + no + "</td>" +
                              "<td>" + e.rncinv_StoreName + "&nbsp;</td>" +
                              "<td>" + e.rncinv_AreaName + "&nbsp;</td>" +
                              "<td>" + e.rncinv_HubName + "&nbsp;</td>" +
                              "<td>" + e.rncinv_RefDoc + "&nbsp;</td>" +
                              "<td>" + e.rncinv_CustPONo + "&nbsp;</td>" +
                              "<td style='align:right'>&nbsp;" + docDate + "</td>" +
                              "<td style='align:right'>&nbsp;" + docValue + "</td>" +
                             "<td>" + e.rncinv_CustID + "</td>" +
                              "<td>" + e.rncinv_CustName + "</td>" +
                               "</tr>");
                }
                sb.Append("</table>");
            }
            return View("excel", sb);

        }


        private IEnumerable<SelectListItem> GetCustomersDocNonConsolidateInvoice()
        {
            List<RptDocumentNonConsolidateInvoice> list = DocumentNonConsolidateInvoices.GetListCustomers();
            List<SelectListItem> listItem = list.Select(doc => new SelectListItem
            {
                Text = doc.rncinv_CustID + "-" + doc.rncinv_CustName,
                Value = doc.rncinv_CustID
            }).ToList();

            return listItem;
        }
    }
}