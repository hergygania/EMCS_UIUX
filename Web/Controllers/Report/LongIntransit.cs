using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
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

        //[myAuthorize(Roles = "ReportSupply")]
        [AuthorizeAcces(ActionType = AuthorizeAcces.IsRead)]
        public ActionResult LongIntransit()
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
            };
            return View(model);
        }

        public ActionResult LongIntransitPage()
        {
            PaginatorBoot.Remove("SessionTRN");
            return LongIntransitPageXt();
        }

        public ActionResult LongIntransitPageXt()
        {
            Func<ReportFilterView, List<RptLongIntransit>> func =
                delegate(ReportFilterView crit)
                {
                    string param = Request["params"];
                    if (!string.IsNullOrEmpty(param))
                    {
                        var ser = new JavaScriptSerializer();
                        crit = ser.Deserialize<ReportFilterView>(param);
                    }

                    var userId = User.Identity.GetUserId();
                    List<RptLongIntransit> list = LongIntransits.
                        GetList(crit.GroupType, crit.FilterBy, crit.StoreNumber, crit.StartDate,
                        crit.EndDate,userId);
                return list.OrderBy(a => a.lint_UpdatedOn).ThenBy(a => a.lint_ID).ToList();
            };

            ActionResult paging = PaginatorBoot.Manage("SessionTRN", func).Pagination.ToJsonResult();
            return Json(paging, JsonRequestBehavior.AllowGet);
        }


        //public void ExportToExcelLongIntransit(string groupType, string filterType,
        //    string storeNumber, DateTime? startDate,
        //    DateTime? endDate)
        //{
        //    List<RptLongIntransit> list = LongIntransits.GetList(groupType, filterType, storeNumber, startDate,
        //        endDate);
        //    var dataInventory = list.OrderBy(a => a.lint_UpdatedOn).ThenBy(a => a.lint_StoreNo).ToList();
        //    var dt = DataTableHelper.ConvertTo(dataInventory);
        //    ExportToExcel(dt, "LongIntransit");
        //}

        public ActionResult ExportToExcelLongIntransit(string groupType,
            string filterBy,
            string storeNumber, DateTime? startDate,
            DateTime? endDate)
        {
            var userId = User.Identity.GetUserId();
            List<RptLongIntransit> listCurrent = LongIntransits.
                GetList(groupType, filterBy, storeNumber, startDate,
                endDate,userId);
         
            ViewBag.Title = "Long Intransit";
            var sb = new System.Text.StringBuilder();
            sb.Append("<b>Long Intransit</b>");
            if (listCurrent != null)
            {

                var no = 0;
                sb.Append("<table border=1  cellspacing='3'>");
                sb.Append("<tr>" +
                          "<th>No</th>" +
                          "<th>Plant</th>" +
                          "<th>SLoc</th>" +
                          "<th>Part Number</th>" +
                          "<th>Description</th>" +
                          "<th>PO Number</th>" +
                          "<th>QTY</th>" +
                          "<th style='align:right'>Order Date</th>" +
                          "<th style='align:right'>Invoice Date</th>" +
                          "<th style='align:right'>POD Date</th>" +
                          "<th>Dealer Net</th>" +
                          "<th>Weight</th>" +
                          "<th>Case NO</th>" +
                          "<th>ASN NO</th>" +
                          "<th>Invoice NO</th>" +
                          "<th>BM</th>" +
                          "<th>Inv Date-POD Date</th>" +
                          "</tr>");


                foreach (var e in listCurrent.ToList())
                {
                    var orderDate = (e.lint_OrderDate.HasValue) ? e.lint_OrderDate.Value.ToString("dd MMM yyyy") : "";
                    var invoiceDate = (e.lint_InvoiceDate.HasValue) ? e.lint_InvoiceDate.Value.ToString("dd MMM yyyy") : "";
                    var lintPodDate = (e.lint_PODDate.HasValue) ? e.lint_PODDate.Value.ToString("dd MMM yyyy") : "";

                    no++;
                    sb.Append("<tr>" +
                              "<td>" + no + "</td>" +
                              "<td>" + e.lint_StoreNo + "&nbsp;</td>" +
                              "<td>" + e.lint_SOS + "&nbsp;</td>" +
                              "<td>&nbsp;" + e.lint_PartsNumber + "&nbsp;</td>" +
                              "<td>" + e.lint_Description + "&nbsp;</td>" +
                              "<td>" + e.lint_Document + "&nbsp;</td>" +
                              "<td>&nbsp;" + e.lint_QTY + "&nbsp;</td>" +
                                  "<td>&nbsp;" + orderDate + "&nbsp;</td>" +
                              "<td>&nbsp;" + invoiceDate + "&nbsp;</td>" + 
                              "<td>&nbsp;" + lintPodDate + "&nbsp;</td>" +
                              "<td>" + e.lint_UNCS + "&nbsp;</td>" +
                              "<td>" + e.lint_Weight + "&nbsp;</td>" +
                              "<td>" + e.lint_CaseNo + "&nbsp;</td>" +
                              "<td>" + e.lint_ASNNo + "&nbsp;</td>" +
                              "<td>" + e.lint_InvoiceNo + "&nbsp;</td>" +
                              "<td>" + e.lint_BM + "&nbsp;</td>" +
                              "<td>" + e.lint_PODDate + "&nbsp;</td>" +
                              "</tr>");
                }
                sb.Append("</table>");
            }
            return View("excel", sb);

        }
    }
}