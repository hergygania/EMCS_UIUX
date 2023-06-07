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
using Area = App.Service.Master.Area;
using Hub = App.Service.Master.Hub;
using App.Web.App_Start;

namespace App.Web.Controllers
{
    public partial class ReportController
    {
        [AuthorizeAcces(ActionType = AuthorizeAcces.IsRead)]
        public ActionResult DocumentAmend()
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
                Customers = GetCustomersDocAmend(userName)
            };
            return View(model);
        }

        public ActionResult DocumentAmendPage()
        {
            PaginatorBoot.Remove("SessionTRN");
            return DocumentAmendPageXt();
        }

        public ActionResult DocumentAmendPageXt()
        {
            Func<ReportFilterView, List<RptDocumentAmend>> func = delegate(ReportFilterView crit)
            {
                string param = Request["params"];
                if (!string.IsNullOrEmpty(param))
                {
                    var ser = new JavaScriptSerializer();
                    crit = ser.Deserialize<ReportFilterView>(param);
                }

                var userId = User.Identity.GetUserId();
                List<RptDocumentAmend> list = DocumentAmends.GetList(crit.GroupType, crit.FilterBy, crit.StoreNumber, crit.CustomerId, userId);
                return list.OrderBy(a => a.amdoc_UpdatedOn).ThenBy(a => a.amdoc_Type).ToList();
            };

            ActionResult paging = PaginatorBoot.Manage("SessionTRN", func).Pagination.ToJsonResult();
            return Json(paging, JsonRequestBehavior.AllowGet);
        }


        public ActionResult ExportToExcelDocAmend(string groupType, string filterBy, string storeNumber, string[] custId)
        {
            string[] customer = new string[] { };
            customer = custId != null ? (custId[0] != "null" ? custId[0].Split(',').ToArray() : null) : null;
            var userId = User.Identity.GetUserId();

            List<RptDocumentAmend> listCurrent = DocumentAmends.GetList(groupType, filterBy, storeNumber, customer, userId);

            ViewBag.Title = "Document Amend";
            var sb = new System.Text.StringBuilder();
            sb.Append("<b>Document Amend</b>");
            if (listCurrent != null)
            {

                var no = 0;
                sb.Append("<table border=1  cellspacing='3'>");
                sb.Append("<tr>" +
                          "<th>No</th>" +
                          "<th>Type</th>" +
                          "<th>Source WO</th>" +
                          "<th>Qty On Hand</th>" +
                          "<th>Stock</th>" +
                          "<th>Status Stock</th>" +
                          "<th>Plant</th>" +
                          "<th>PO Number</th>" +
                          "<th>Dealer Net</th>" +
                          "<th>Part No</th>" +
                           "<th style='align:right'>Quantity</th>" +
                          "<th>Status Stock ST No</th>" +
                          "<th>Activity Date</th>" +
                          "<th>Stock QTY On Hand</th>" +
                          "<th>Outstanding QTY</th>" +
                          "<th>QTY In Process</th>" +
                          "<th>Return QTY In Process</th>" +
                          "<th>Note</th>" +
                          "<th>SO Created By</th>" +
                          "<th>Pack QTY</th>" +
                          "</tr>");


                foreach (var e in listCurrent.ToList())
                {

                    no++;
                    sb.Append("<tr>" +
                              "<td>" + no + "</td>" +
                              "<td>" + e.amdoc_Type + "&nbsp;</td>" +
                              "<td>&nbsp;" + e.amdoc_ST1 + "&nbsp;</td>" +
                              "<td>" + e.amdoc_QYHNDST1 + "&nbsp;</td>" +
                              "<td>" + e.amdoc_TAST1 + "&nbsp;</td>" +
                              "<td>" + e.amdoc_StatusStockST1 + "&nbsp;</td>" +
                              "<td>" + e.amdoc_STNo + "&nbsp;</td>" +
                              "<td>&nbsp;" + e.amdoc_DocNo + "&nbsp;</td>" +
                              "<td>&nbsp;" + e.amdoc_SOS + "&nbsp;</td>" +
                              "<td>&nbsp;" + e.amdoc_PartNo + "&nbsp;</td>" +
                              "<td>" + e.amdoc_Qty1 + "&nbsp;</td>" +
                              "<td>" + e.amdoc_StatusStockSTNo + "&nbsp;</td>" +
                              "<td>" + e.amdoc_LSACJ8 + "&nbsp;</td>" +
                              "<td>" + e.amdoc_QYHND + "&nbsp;</td>" +
                              "<td>" + e.amdoc_QYOR + "&nbsp;</td>" +
                              "<td>" + e.amdoc_QYPCS + "&nbsp;</td>" +
                              "<td>" + e.amdoc_RTQYPC + "&nbsp;</td>" +
                              "<td>" + e.amdoc_Note + "&nbsp;</td>" +
                              "<td>" + e.amdoc_UserID + "&nbsp;</td>" +
                              "<td>" + e.amdoc_PXQY2 + "&nbsp;</td>" +
                              "</tr>");
                }
                sb.Append("</table>");
            }
            return View("excel", sb);

        }

        private IEnumerable<SelectListItem> GetCustomersDocAmend(string username)
        {
            List<RptDocumentAmend> list = DocumentAmends.GetListCustomers(username);
            List<SelectListItem> listItem = list.Select(doc => new SelectListItem
            {
                Text = doc.amdoc_UserID,
                Value = doc.amdoc_UserID
            }).ToList();

            return listItem;
        }
    }
}