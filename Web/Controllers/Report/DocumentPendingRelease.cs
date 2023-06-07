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
        [AuthorizeAcces(ActionType = AuthorizeAcces.IsRead, UrlMenu = "DocumentPendingRelease")]
        [AuthorizeAcces(ActionType = AuthorizeAcces.IsRead)]
        public ActionResult DocumentPendingRelease()
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
                Customers = GetCustomersDocPendingRelease(),//new SelectList(DocumentPendingReleases.GetListCustomers(), "rpnd_CustID", "rpnd_CustName")

            };
            return View(model);
        }
        [AuthorizeAcces(ActionType = AuthorizeAcces.IsRead, UrlMenu = "DocumentPendingRelease")]
        public ActionResult DocumentPendingReleasePage()
        {
            PaginatorBoot.Remove("SessionTRN");
            return DocumentPendingReleasePageXt();
        }
        [AuthorizeAcces(ActionType = AuthorizeAcces.IsRead, UrlMenu = "DocumentPendingRelease")]
        public ActionResult DocumentPendingReleasePageXt()
        {
            Func<ReportFilterView, List<RptDocumentPendingRelease>> func =
                delegate(ReportFilterView crit)
                {
                    string param = Request["params"];
                    if (!string.IsNullOrEmpty(param))
                    {
                        var ser = new JavaScriptSerializer();
                        crit = ser.Deserialize<ReportFilterView>(param);
                    }
                    var userId = User.Identity.GetUserId();
                    List<RptDocumentPendingRelease> list = DocumentPendingReleases
                        .GetList(crit.GroupType, crit.FilterBy, crit.StoreNumber, crit.CustomerId,userId);
                    return list.OrderBy(a => a.rpnd_UpdatedBy).ThenBy(a => a.rpnd_Store).ToList();
                };

            ActionResult paging = PaginatorBoot.Manage("SessionTRN", func).Pagination.ToJsonResult();
            return Json(paging, JsonRequestBehavior.AllowGet);
        }



        //public void ExportToExcelDocPendingRelease(string groupType, string filterType, string storeNumber, DateTime? startDate,
        //        DateTime? endDate, string[] custId)
        //{
        //    string[] customer = new string[] { };
        //    customer = custId != null ? (custId[0] != "null" ? custId[0].Split(',').ToArray() : null) : null;
        //    var list = DocumentPendingReleases.GetList(groupType, filterType, storeNumber, startDate,
        //        endDate, customer);

        //    DataTable dt = DataTableHelper.ConvertTo(list);
        //    ExportToExcel(dt, "DocumentPendingRelease");
        //}
        
        [AuthorizeAcces(ActionType = AuthorizeAcces.IsRead, UrlMenu = "DocumentPendingRelease")]
        public ActionResult ExportToExcelDocPendingRelease(string groupType, string filterBy, string storeNumber, string[] custId)
        {
            var userId = User.Identity.GetUserId();
            string[] customer = new string[] { };
            customer = custId != null ? (custId[0] != "null" ? custId[0].Split(',').ToArray() : null) : null;
            var listCurrent = DocumentPendingReleases.GetList(groupType, filterBy, storeNumber, customer, userId);
            ViewBag.Title = "Document Pending Release";
            var sb = new System.Text.StringBuilder();
            sb.Append("<b>Document Pending Release</b>");
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
                          "<th>Remarks</th>" +
                          "</tr>");


                foreach (var e in listCurrent.ToList())
                {
                    var docDate = (e.rpnd_DocDate.HasValue) ? e.rpnd_DocDate.Value.ToString("dd MMM yyyy") : "";
                    var docValue = (e.rpnd_DocValue != null)
                       ? FormatStringNumber(e.rpnd_DocValue)
                       : "";
                    no++;
                    sb.Append("<tr>" +
                              "<td>" + no + "</td>" +
                              "<td>" + e.rpnd_StoreName + "&nbsp;</td>" +
                              "<td>" + e.rpnd_AreaName + "&nbsp;</td>" +
                              "<td>" + e.rpnd_HubName + "&nbsp;</td>" +
                              "<td>" + e.rpnd_RefDoc + "&nbsp;</td>" +
                              "<td>" + e.rpnd_CustPONo + "&nbsp;</td>" +
                              "<td style='align:right'>&nbsp;" + docDate + "</td>" +
                              "<td style='align:right'>&nbsp;" + docValue + "</td>" +
                               "<td>" + e.rpnd_CustID + "</td>" +
                              "<td>" + e.rpnd_CustName + "</td>" +
                              "<td>" + e.rpnd_Remarks + "</td>" +
                              "</tr>");
                }
                sb.Append("</table>");
            }
            return View("excel", sb);

        }


        private IEnumerable<SelectListItem> GetCustomersDocPendingRelease()
        {
            List<RptDocumentPendingRelease> list = DocumentPendingReleases.GetListCustomers();
            List<SelectListItem> listItem = list.Select(doc => new SelectListItem
            {
                Text = doc.rpnd_CustID+"-"+doc.rpnd_CustName,
                Value = doc.rpnd_CustID
            }).ToList();

            return listItem;
        }
    }
}