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
        public ActionResult DocumentReprint()
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
                Customers = GetCustomersDocReprint(),// new SelectList(DocumentReprints.GetListCustomers(), "docrep_CustID", "docrep_CustName")
            };
            return View(model);
        }
        [AuthorizeAcces(ActionType = AuthorizeAcces.IsRead, UrlMenu = "DocumentReprint")]
        public ActionResult DocumentReprintPage()
        {
            PaginatorBoot.Remove("SessionTRN");
            return DocumentReprintPageXt();
        }
        [AuthorizeAcces(ActionType = AuthorizeAcces.IsRead, UrlMenu = "DocumentReprint")]
        public ActionResult DocumentReprintPageXt()
        {
            Func<ReportFilterView, List<RptDocumentReprint>> func = delegate(ReportFilterView crit)
            {
                string param = Request["params"];
                if (!string.IsNullOrEmpty(param))
                {
                    var ser = new JavaScriptSerializer();
                    crit = ser.Deserialize<ReportFilterView>(param);
                }
                var userId = User.Identity.GetUserId();
                List<RptDocumentReprint> list = DocumentReprints.GetList(crit.GroupType, crit.FilterBy, crit.StoreNumber, crit.CustomerId, userId);
                return list.OrderBy(a => a.docrep_UpdatedOn).ThenBy(a => a.docrep_Store).ToList();
            };

            ActionResult paging = PaginatorBoot.Manage("SessionTRN", func).Pagination.ToJsonResult();
            return Json(paging, JsonRequestBehavior.AllowGet);
        }


        //public void ExportToExcelDocReprint(string groupType, string storeNumber, DateTime? startDate,
        //        DateTime? endDate, string[] custId)
        //{
        //    string[] customer = new string[] { };
        //    customer = custId != null ? (custId[0] != "null" ? custId[0].Split(',').ToArray() : null) : null;
        //    var list = DocumentReprints.GetList(groupType, storeNumber, startDate,
        //        endDate, customer);
        //    DataTable dt = DataTableHelper.ConvertTo(list);
        //    ExportToExcel(dt, "DocumentReprint");
        //}
        
        [AuthorizeAcces(ActionType = AuthorizeAcces.IsRead, UrlMenu = "DocumentReprint")]
        public ActionResult ExportToExcelDocReprint(string groupType
            , string filterBy, string storeNumber, string[] custId)
        {
            string[] customer = new string[] { };
            customer = custId != null ? (custId[0] != "null" ? custId[0].Split(',').ToArray() : null) : null;
            var userId = User.Identity.GetUserId();
            var listCurrent = DocumentReprints.GetList(groupType, filterBy, storeNumber, customer, userId);

            ViewBag.Title = "Document Reprint";
            var sb = new System.Text.StringBuilder();
            sb.Append("<b>Document Reprint</b>");
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
                          "<th>Print Count</th>" +
                          "<th>Remarks</th>" +
                          "</tr>");


                foreach (var e in listCurrent.ToList())
                {
                    var docDate = (e.docrep_DocDate.HasValue) ? e.docrep_DocDate.Value.ToString("dd MMM yyyy") : "";
                    var docValue = (e.docrep_DocValue != null)
                        ? FormatStringNumber(e.docrep_DocValue)
                        : "";
                    no++;
                    sb.Append("<tr>" +
                              "<td>" + no + "</td>" +
                              "<td>" + e.docrep_StoreName + "&nbsp;</td>" +
                              "<td>" + e.docrep_AreaName + "&nbsp;</td>" +
                              "<td>" + e.docrep_HubName + "&nbsp;</td>" +
                              "<td>" + e.docrep_RefDoc + "&nbsp;</td>" +
                              "<td>" + e.docrep_CustPONo + "&nbsp;</td>" +
                              "<td style='align:right'>&nbsp;" + docDate + "</td>" +
                              "<td style='align:right'>&nbsp;" + docValue + "</td>" +
                               "<td>" + e.docrep_CustID + "</td>" +
                              "<td>" + e.docrep_CustName + "</td>" +
                              "<td>" + e.docrep_PrintCount + "</td>" +
                             "<td>" + e.docrep_Remark + "</td>" +
                              "</tr>");
                }
                sb.Append("</table>");
            }
            return View("excel", sb);

        }


        private IEnumerable<SelectListItem> GetCustomersDocReprint()
        {
            IEnumerable<RptDocumentReprint> list = DocumentReprints.GetListCustomers();
            List<SelectListItem> listItem = list.Select(doc => new SelectListItem
            {
                Text = doc.docrep_CustID + "-" + doc.docrep_CustName,
                Value = doc.docrep_CustID
            }).ToList();

            return listItem;
        }
    }
}