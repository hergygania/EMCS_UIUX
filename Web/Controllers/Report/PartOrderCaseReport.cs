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
        public ActionResult PartOrderCaseReport()
        {
            ViewBag.AllowRead = AuthorizeAcces.AllowRead;
            ViewBag.AllowCreate = AuthorizeAcces.AllowCreated;
            ViewBag.AllowUpdate = AuthorizeAcces.AllowUpdated;
            ViewBag.AllowDelete = AuthorizeAcces.AllowDeleted;
            PaginatorBoot.Remove("SessionTRN");
            var userName = Domain.SiteConfiguration.UserName;
            var model = new PartOrderCaseFilterView
            {
                InvoiceStartDate = new DateTime(DateTime.Now.Year, 1, 1),
                InvoiceEndDate = DateTime.Now,
                CaseDescriptions = GetSelectListItem(PartOrderCaseReports.GetCaseDescription()),
                CaseTypes = GetSelectListItem(PartOrderCaseReports.GetCaseTypes()),
                HubList = Hub.GetList(),
                AreaList = Area.GetList(),
                StoreNumberList = new List<Store>(),
            };
            return View(model);
        }
        [AuthorizeAcces(ActionType = AuthorizeAcces.IsRead, UrlMenu = "PartOrderCaseReport")]
        public ActionResult PartOrderCaseReportPage()
        {
            PaginatorBoot.Remove("SessionTRN");
            return PartOrderCaseReportPageXt();
        }
        [AuthorizeAcces(ActionType = AuthorizeAcces.IsRead, UrlMenu = "PartOrderCaseReport")]
        public ActionResult PartOrderCaseReportPageXt()
        {
            Func<PartOrderCaseFilterView, List<V_PART_ORDER_CASE_REPORT>> func = delegate(PartOrderCaseFilterView crit)
            {
                string param = Request["params"];
                if (!string.IsNullOrEmpty(param))
                {
                    var ser = new JavaScriptSerializer();
                    crit = ser.Deserialize<PartOrderCaseFilterView>(param);
                }

                List<V_PART_ORDER_CASE_REPORT> list = PartOrderCaseReports.GetList
                    (
                        crit.GroupType, crit.FilterBy,
                        crit.StoreNumber, crit.CaseNo,
                        crit.CaseType, crit.CaseDescription,
                        crit.InvoiceNo, crit.InvoiceStartDate,
                        crit.InvoiceEndDate, crit.WeightFrom,
                        crit.WeightTo, crit.LengthFrom,
                        crit.LengthTo, crit.WideFrom,
                        crit.WideTo, crit.HeightFrom, crit.HeightFrom);
                return list.OrderBy(a => a.EntryDate).ThenBy(a => a.StoreName).ToList();
            };

            ActionResult paging = PaginatorBoot.Manage("SessionTRN", func).Pagination.ToJsonResult();
            return Json(paging, JsonRequestBehavior.AllowGet);
        }


        //public void ExportToExcelPartOrderCase(
        //    string groupType, string filterBy,
        //    string storeNo, string caseNo,
        //    string caseType, string caseDescription,
        //    string invoiceNo, DateTime? invoiceStartDate,
        //    DateTime? invoiceEndDate, decimal? weightFrom,
        //    decimal? weightTo, decimal? lengthFrom,
        //    decimal? lengthTo, decimal? wideFrom,
        //    decimal? wideTo, decimal? heightFrom,
        //    decimal? heightTo)
        //{
        //    List<V_PART_ORDER_CASE_REPORT> list = PartOrderCaseReports.GetList(
        //        groupType, filterBy,
        //        storeNo, caseNo,
        //        caseType, caseDescription,
        //        invoiceNo, invoiceStartDate,
        //        invoiceEndDate, weightFrom,
        //        weightTo, lengthFrom,
        //        lengthTo, wideFrom,
        //        wideTo, heightFrom,
        //        heightTo);
        //    DataTable dt = DataTableHelper.ConvertTo(list);
        //    ExportToExcel(dt, "PartOrderCaseReport");
        //}

        public ActionResult ExportToExcelPartOrderCase(
            string groupType, string filterBy,
            string storeNo, string caseNo,
            string caseType, string caseDescription,
            string invoiceNo, DateTime? invoiceStartDate,
            DateTime? invoiceEndDate, decimal? weightFrom,
            decimal? weightTo, decimal? lengthFrom,
            decimal? lengthTo, decimal? wideFrom,
            decimal? wideTo, decimal? heightFrom,
            decimal? heightTo)
        {
            //var listCurrent = Paginator.GetPagination<Data.Domain.Shipment>("SessionTRN").ToList();
            var listCurrent = PartOrderCaseReports.GetList(
                  groupType, filterBy,
                storeNo, caseNo,
                caseType, caseDescription,
                invoiceNo, invoiceStartDate,
                invoiceEndDate, weightFrom,
                weightTo, lengthFrom,
                lengthTo, wideFrom,
                wideTo, heightFrom,
                heightTo);

            //var x= (listCurrent)(new System.Collections.Generic.Mscorlib_CollectionDebugView<<>f__AnonymousType9<App.Data.Domain.Shipment,bool,App.Data.Domain.ShipmentManifest,App.Data.Domain.ShipmentManifestDetail,App.Data.Domain.PartsOrderCase>>(listCurrent as System.Collections.Generic.List<<>f__AnonymousType9<App.Data.Domain.Shipment,bool,App.Data.Domain.ShipmentManifest,App.Data.Domain.ShipmentManifestDetail,App.Data.Domain.PartsOrderCase>>)).Items[0];

            ViewBag.Title = "Part Order Case";
            var sb = new System.Text.StringBuilder();
            sb.Append("<b>Part Order Case</b>");
            if (listCurrent != null)
            {
                var no = 0;
                sb.Append("<table border=1  cellspacing='3'>");
                sb.Append("<tr>" +
                          "<th>No</th>" +
                          "<th>Case ID</th>" +
                          "<th>Parts Order ID</th>" +
                          "<th>Invoice No</th>" +
                          "<th style='align:right'>Invoice Date</th>" +
                          "<th>Case No</th>" +
                          "<th>Shipping ID ASN</th>" +
                          "<th>Case Type</th>" +
                          "<th>Case Description</th>" +
                          "<th style='align:right'>Weight (KG)</th>" +
                          "<th style='align:right'>Length (CM)</th>" +
                          "<th style='align:right'>Wide (CM)</th>" +
                          "<th style='align:right'>Height (CM)</th>" +
                          "<th>Route ID</th>" +
                          "</tr>");


                foreach (var e in listCurrent.ToList())
                {
                     no++;
                    sb.Append("<tr>" +
                              "<td>" + no + "</td>" +
                              "<td>" + e.CaseID + "&nbsp;</td>" +
                              "<td>" + e.PartsOrderID + "&nbsp;</td>" +
                              "<td>" + e.InvoiceNo + "&nbsp;</td>" +
                              "<td style='align:right'>&nbsp;" + e.InvoiceDate.ToString("dd MMM yyyy") + "</td>" +
                              "<td>" + e.CaseNo + "</td>" +
                              "<td>" + e.ShippingIDASN + "</td>" +
                              "<td>&nbsp;" + e.CaseType + "</td>" +
                              "<td>&nbsp;" + e.CaseDescription + "</td>" +
                              "<td style='align:right'>&nbsp;" + e.WeightKG + "</td>" +
                              "<td style='align:right'>&nbsp;" + e.LengthCM + "</td>" +
                              "<td style='align:right'>&nbsp;" + e.WideCM + "</td>" +
                              "<td style='align:right'>&nbsp;" + e.HeightCM + "</td>" +
                              "<td>" + e.RouteID + "&nbsp;</td>" +
                              "</tr>");
                }
                sb.Append("</table>");
            }
            return View("excel", sb);

        }

    }
}