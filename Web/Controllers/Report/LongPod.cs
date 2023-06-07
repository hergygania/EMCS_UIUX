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
        public ActionResult LongPod()
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

        public ActionResult LongPodPage()
        {
            PaginatorBoot.Remove("SessionTRN");
            return LongPodPageXt();
        }

        public ActionResult LongPodPageXt()
        {
            Func<ReportFilterView, List<RptLongPOD>> func = delegate(ReportFilterView crit)
            {
                string param = Request["params"];
                if (!string.IsNullOrEmpty(param))
                {
                    var ser = new JavaScriptSerializer();
                    crit = ser.Deserialize<ReportFilterView>(param);
                }

                var userId = User.Identity.GetUserId();
                List<RptLongPOD> list = LongPods.GetList(crit.GroupType, crit.FilterBy, crit.StoreNumber, crit.StartDate,
                        crit.EndDate, userId);
                return list.OrderBy(a => a.lpod_CreatedOn).ThenBy(a => a.lpod_ID).ToList();
            };

            ActionResult paging = PaginatorBoot.Manage("SessionTRN", func).Pagination.ToJsonResult();
            return Json(paging, JsonRequestBehavior.AllowGet);
        }


        //public void ExportToExcelLongPod(string groupType, string filterType,
        //    string storeNumber, DateTime? startDate,
        //    DateTime? endDate)
        //{
        //    List<RptLongPOD> list = LongPods.GetList(groupType, filterType, storeNumber, startDate,
        //        endDate);
        //    DataTable dt = DataTableHelper.ConvertTo(list);
        //    ExportToExcel(dt, "LongPod");
        //}

        public ActionResult ExportToExcelLongPod(string groupType,
            string filterBy,
            string storeNumber, DateTime? startDate,
            DateTime? endDate)
        {
            var userId = User.Identity.GetUserId();
            List<RptLongPOD> listCurrent = LongPods.GetList(groupType, filterBy, storeNumber, startDate,
                endDate, userId);
            ViewBag.Title = "Long POD";
            var sb = new System.Text.StringBuilder();
            sb.Append("<b>Long POD</b>");
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
                          "<th>Invoice NO</th>" +
                          "<th style='align:right'>Invoice Date</th>" +
                          "<th style='align:right'>POD Date</th>" +
                          "<th>Dealer Net</th>" +
                          "<th style='align:right'>Weight</th>" +
                          "<th>ASN NO</th>" +
                          "</tr>");


                foreach (var e in listCurrent.ToList())
                {
                    var orderDate = (e.lpod_OrderDate.HasValue) ? e.lpod_OrderDate.Value.ToString("dd MMM yyyy") : "";
                    var invoiceDate = (e.lpod_InvoiceDate.HasValue) ? e.lpod_InvoiceDate.Value.ToString("dd MMM yyyy") : "";
                    var lpodRgDate = (e.lpod_PODDate.HasValue) ? e.lpod_PODDate.Value.ToString("dd MMM yyyy") : "";

                    no++;
                    sb.Append("<tr>" +
                              "<td>" + no + "</td>" +
                              "<td>" + e.lpod_StoreNo + "&nbsp;</td>" +
                              "<td>&nbsp;" + e.lpod_SOS + "&nbsp;</td>" +
                              "<td>&nbsp;" + e.lpod_PartsNumber + "&nbsp;</td>" +
                              "<td>" + e.lpod_Description + "&nbsp;</td>" +
                              "<td>" + e.lpod_Document + "&nbsp;</td>" +
                              "<td>" + e.lpod_QTY + "&nbsp;</td>" +
                              "<td>&nbsp;" + orderDate + "&nbsp;</td>" +
                              "<td>" + e.lpod_InvoiceNo + "&nbsp;</td>" +
                              "<td>&nbsp;" + invoiceDate + "&nbsp;</td>" +
                              "<td>&nbsp;" + lpodRgDate + "&nbsp;</td>" +
                              "<td>" + e.lpod_UNCS + "&nbsp;</td>" +
                              "<td>" + e.lpod_Weight + "&nbsp;</td>" +
                              "<td>" + e.lpod_ASNNo + "&nbsp;</td>" +
                              "</tr>");
                }
                sb.Append("</table>");
            }
            return View("excel", sb);

        }
    }
}