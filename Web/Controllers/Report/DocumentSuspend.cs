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
        public ActionResult DocumentSuspend()
        {
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
                Customers = GetCustomersDocSuspend(),//new SelectList(DocumentSuspends.GetListCustomers(), "rspnd_CustID", "rspnd_CustName")
            };
            return View(model);
        }

        public ActionResult DocumentSuspendPage()
        {
            PaginatorBoot.Remove("SessionTRN");
            return DocumentSuspendPageXt();
        }

        public ActionResult DocumentSuspendPageXt()
        {
            Func<ReportFilterView, List<RptDocumentSuspend>> func = delegate(ReportFilterView crit)
            {
                string param = Request["params"];
                if (!string.IsNullOrEmpty(param))
                {
                    var ser = new JavaScriptSerializer();
                    crit = ser.Deserialize<ReportFilterView>(param);
                }
                var userId = User.Identity.GetUserId();
                List<RptDocumentSuspend> list = DocumentSuspends.GetList(crit.GroupType, crit.FilterBy, crit.StoreNumber, crit.CustomerId,userId);
                return list.OrderBy(a => a.rspnd_UpdatedOn).ThenBy(a => a.rspnd_Store).ToList();
            };

            ActionResult paging = PaginatorBoot.Manage("SessionTRN", func).Pagination.ToJsonResult();
            return Json(paging, JsonRequestBehavior.AllowGet);
        }




        public ActionResult ExportToExcelDocSuspend(
            string groupType, string filterBy,
            string storeNumber, string[] custId)
        {
            var userId = User.Identity.GetUserId();
            string[] customer = new string[] { };
            customer = custId != null ? (custId[0] != "null" ? custId[0].Split(',').ToArray() : null) : null;
            var listCurrent = DocumentSuspends.GetList(groupType, filterBy, storeNumber, customer,userId);

            ViewBag.Title = "Document Suspend";
            var sb = new System.Text.StringBuilder();
            sb.Append("<b>Document Suspend</b>");
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
                    var docDate = (e.rspnd_DocDate.HasValue) ? e.rspnd_DocDate.Value.ToString("dd MMM yyyy") : "";
                    var docValue = (e.rspnd_DocValue != null)
                        ? FormatStringNumber(e.rspnd_DocValue)
                        : "";
                    no++;
                    sb.Append("<tr>" +
                              "<td>" + no + "</td>" +
                              "<td>" + e.rspnd_StoreName + "&nbsp;</td>" +
                              "<td>" + e.rspnd_AreaName + "&nbsp;</td>" +
                              "<td>" + e.rspnd_HubName + "&nbsp;</td>" +
                              "<td>" + e.rspnd_RefDoc + "&nbsp;</td>" +
                              "<td>" + e.rspnd_CustPONo + "&nbsp;</td>" +
                              "<td style='align:right'>&nbsp;" + docDate + "</td>" +
                              "<td style='align:right'>&nbsp;" + docValue + "</td>" +
                               "<td>" + e.rspnd_CustID + "</td>" +
                              "<td>" + e.rspnd_CustName + "</td>" +
                              "</tr>");
                }
                sb.Append("</table>");
            }
            return View("excel", sb);

        }

        private IEnumerable<SelectListItem> GetCustomersDocSuspend()
        {
            List<RptDocumentSuspend> list = DocumentSuspends.GetListCustomers();
            List<SelectListItem> listItem = list.Select(doc => new SelectListItem
            {
                Text = doc.rspnd_CustID + "-" + doc.rspnd_CustName,
                Value = doc.rspnd_CustID
            }).ToList();

            return listItem;
        }
    }
}