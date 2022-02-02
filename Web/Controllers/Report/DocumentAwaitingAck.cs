using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using System.Xml;
using System.Xml.Serialization;
using App.Data.Domain;
using App.Service.Report;
using App.Web.Helper.Extensions;
using App.Web.Models;
using App.Web.App_Start;

namespace App.Web.Controllers
{
    public partial class ReportController
    {
        [AuthorizeAcces(ActionType = AuthorizeAcces.IsRead, UrlMenu = "DocumentAwaitingAck")]        
        public ActionResult DocumentAwaitingAck()
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
                Customers = GetCustomersDocAwaitingAck()
            };
            return View(model);
        }
        [AuthorizeAcces(ActionType = AuthorizeAcces.IsRead, UrlMenu = "DocumentAwaitingAck")]
        public ActionResult DocumentAwaitingAckPage()
        {
            PaginatorBoot.Remove("SessionTRN");
            return DocumentAwaitingAckPageXt();
        }
        [AuthorizeAcces(ActionType = AuthorizeAcces.IsRead, UrlMenu = "DocumentAwaitingAck")]
        public ActionResult DocumentAwaitingAckPageXt()
        {
            Func<ReportFilterView, List<RptDocumentAwaitingAck>> func = delegate(ReportFilterView crit)
            {
                string param = Request["params"];
                if (!string.IsNullOrEmpty(param))
                {
                    var ser = new JavaScriptSerializer();
                    crit = ser.Deserialize<ReportFilterView>(param);
                }

                var userId = User.Identity.GetUserId();
                List<RptDocumentAwaitingAck> list = DocumentAwaitingAcks.GetList(crit.GroupType, crit.FilterBy, crit.StoreNumber, crit.CustomerId, userId);
                return list.OrderBy(a => a.rack_UpdatedOn).ThenBy(a => a.rack_Store).ToList();
            };

            ActionResult paging = PaginatorBoot.Manage("SessionTRN", func).Pagination.ToJsonResult();
            return Json(paging, JsonRequestBehavior.AllowGet);
        }


        //public void ExportToExcelDocument(string groupType, string filterType, string storeNumber, DateTime? startDate, DateTime? endDate, string[] custId)
        //{
        //    string[] customer = new string[] { };
        //    customer = custId != null ? (custId[0] != "null" ? custId[0].Split(',').ToArray() : null) : null;
        //    List<RptDocumentAwaitingAck> list = DocumentAwaitingAcks.GetList(groupType, filterType, storeNumber, startDate,
        //        endDate, customer);
        //    var dataInventory = list.OrderBy(a => a.rack_UpdatedOn).ThenBy(a => a.rack_Store).ToList();
        //    var dt = DataTableHelper.ConvertTo(dataInventory);
        //    ExportToExcel(dt, "DocuentAwaitingAck");

        //}
        [AuthorizeAcces(ActionType = AuthorizeAcces.IsRead, UrlMenu = "DocumentAwaitingAck")]
        public ActionResult ExportToExcelDocument(string groupType, string filterBy, string storeNumber, string[] custId)
        {
            string[] customer = new string[] { };
            var userId = User.Identity.GetUserId();
            customer = custId != null ? (custId[0] != "null" ? custId[0].Split(',').ToArray() : null) : null;
            List<RptDocumentAwaitingAck> listCurrent = DocumentAwaitingAcks.GetList(groupType, filterBy, storeNumber, customer, userId);

            ViewBag.Title = "Document Awaiting Ack";
            var sb = new System.Text.StringBuilder();
            sb.Append("<b>Document Awaiting Ack</b>");
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
                          "<th>Customer ID</th>" +
                          "<th>Customer Name</th>" +
                           "</tr>");


                foreach (var e in listCurrent.ToList())
                {
                    var docDate = (e.rack_DocDate.HasValue) ? e.rack_DocDate.Value.ToString("dd MMM yyyy") : "";

                    no++;
                    sb.Append("<tr>" +
                              "<td>" + no + "</td>" +
                              "<td>" + e.rack_StoreName + "&nbsp;</td>" +
                              "<td>" + e.rack_AreaName + "&nbsp;</td>" +
                              "<td>" + e.rack_HubName + "&nbsp;</td>" +
                              "<td>" + e.rack_RefDoc + "&nbsp;</td>" +
                              "<td>" + e.rack_CustPONo + "&nbsp;</td>" +
                              "<td style='align:right'>&nbsp;" + docDate + "</td>" +
                              "<td>" + e.rack_CustID + "</td>" +
                              "<td>" + e.rack_CustName + "</td>" +
                              "</tr>");
                }
                sb.Append("</table>");
            }
            return View("excel", sb);

        }

        private IEnumerable<SelectListItem> GetCustomersDocAwaitingAck()
        {
            List<RptDocumentAwaitingAck> list = DocumentAwaitingAcks.GetListCustomers();
            List<SelectListItem> listItem = list.Select(doc => new SelectListItem
            {
                Text = doc.rack_CustID + "-" + doc.rack_CustName,
                Value = doc.rack_CustID
            }).ToList();

            return listItem;
        }

    }
}