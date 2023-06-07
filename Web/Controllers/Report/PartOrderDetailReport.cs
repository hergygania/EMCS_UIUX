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
        public ActionResult PartOrderDetailReport()
        {
            ViewBag.AllowRead = AuthorizeAcces.AllowRead;
            ViewBag.AllowCreate = AuthorizeAcces.AllowCreated;
            ViewBag.AllowUpdate = AuthorizeAcces.AllowUpdated;
            ViewBag.AllowDelete = AuthorizeAcces.AllowDeleted;
            PaginatorBoot.Remove("SessionTRN");
            var userName = Domain.SiteConfiguration.UserName;
            var model = new PartOrderDetailFilterView
            {
                InvoiceStartDate = new DateTime(DateTime.Now.Year, 1, 1),
                InvoiceEndDate = DateTime.Now,
                CooDescriptions =  PartOrderDetailReports.GetCooDescriptions(),
                HubList = Hub.GetList(),
                AreaList = Area.GetList(),
                StoreNumberList = new List<Store>(),
            };
            return View(model);
        }

        public ActionResult PartOrderDetailReportPage()
        {
            PaginatorBoot.Remove("SessionTRN");
            return PartOrderDetailReportPageXt();
        }

        public ActionResult PartOrderDetailReportPageXt()
        {
            Func<PartOrderDetailFilterView, List<V_PART_ORDER_DETAIL_REPORT>> func = delegate(PartOrderDetailFilterView crit)
            {
                string param = Request["params"];
                if (!string.IsNullOrEmpty(param))
                {
                    var ser = new JavaScriptSerializer();
                    crit = ser.Deserialize<PartOrderDetailFilterView>(param);
                }

                List<V_PART_ORDER_DETAIL_REPORT> list = PartOrderDetailReports.GetList(
                    crit.GroupType, crit.FilterBy, crit.StoreNumber, crit.InvoiceNo,
                    crit.InvoiceStartDate,crit.InvoiceEndDate,
                    crit.CaseNo,crit.PartNumber,
                    crit.PartName,crit.Coo,
                    crit.CustomerReff,crit.SOS);
                return list.OrderBy(a => a.EntryDate).ThenBy(a => a.StoreName).ToList();
            };

            ActionResult paging = PaginatorBoot.Manage("SessionTRN", func).Pagination.ToJsonResult();
            return Json(paging, JsonRequestBehavior.AllowGet);
        }

        
        //public void ExportToExcelPartOrderDetail(
        //    string groupType, string filterBy, string storeNo,
        //    string invoiceNo, DateTime? startDate, DateTime? endDate,
        //    string caseNo, string partNo,
        //    string partName, string coo,
        //    string customerReff, string sos)
        //{
        //    var list = PartOrderDetailReports.GetList(
        //        groupType, filterBy,  storeNo,
        //    invoiceNo,  startDate,  endDate,
        //    caseNo, partNo,
        //    partName, coo,
        //    customerReff, sos);
        //    var dt = DataTableHelper.ConvertTo(list);
        //    ExportToExcel(dt, "PartOrderDetailReport");
        //}


        public ActionResult ExportToExcelPartOrderDetail(
        string groupType, string filterBy, string storeNo,
            string invoiceNo, DateTime? startDate, DateTime? endDate,
            string caseNo, string partNo,
            string partName, string coo,
            string customerReff, string sos)
        {
            var listCurrent = PartOrderDetailReports.GetList(
                groupType, filterBy, storeNo,
            invoiceNo, startDate, endDate,
            caseNo, partNo,
            partName, coo,
            customerReff, sos);


            ViewBag.Title = "Part Order Detail";
            var sb = new System.Text.StringBuilder();
            sb.Append("<b>Part Order Detail</b>");
            if (listCurrent != null)
            {
                var no = 0;
                sb.Append("<table border=1  cellspacing='3'>");
                sb.Append("<tr>" +
                          "<th>No</th>" +
                          "<th>Detail ID</th>" +
                          "<th>Parts Order ID</th>" +
                          "<th>Invoice No</th>" +
                          "<th style='align:right'>Invoice Date</th>" +
                          "<th>Prim PSO</th>" +
                          "<th>Case No</th>" +
                          "<th>Parts Number</th>" +
                          "<th>COO</th>" +
                          "<th>COO Description</th>" +
                          "<th>Invoice Item No</th>" +
                          "<th>Parts Description Short</th>" +
                          "<th style='align:right'>Invoice Item Qty</th>" +
                          "<th>Customer Reff</th>" +
                          "<th style='align:right'>Part Gross Weight</th>" +
                          "<th style='align:right'>Charges Discount Amount</th>" +
                          "<th style=''>BE Code</th>" +
                          "<th style=''>Order CLS Code/th>" +
                          "<th style='align:right'>Profile</th>" +
                          "<th style='align:right'>Unit Price</th>" +
                          "<th>OM ID</th>" +
                          "</tr>");


                foreach (var e in listCurrent.ToList())
                {
                    no++;
                    sb.Append("<tr>" +
                              "<td>" + no + "</td>" +
                              "<td>" + e.DetailID + "&nbsp;</td>" +
                              "<td>" + e.PartsOrderID + "&nbsp;</td>" +
                              "<td>" + e.InvoiceNo + "&nbsp;</td>" +
                              "<td style='align:right'>&nbsp;" + e.InvoiceDate.ToString("dd MMM yyyy") + "</td>" +
                              "<td>" + e.PrimPSO + "</td>" +
                              "<td>" + e.CaseNo + "</td>" +
                              "<td>&nbsp;" + e.PartsNumber + "</td>" +
                              "<td>&nbsp;" + e.COO + "</td>" +
                              "<td style=''>" + e.COODescription + "&nbsp;</td>" +
                              "<td style=''>" + e.InvoiceItemNo + "&nbsp;</td>" +
                              "<td style=''>" + e.PartsDescriptionShort + "&nbsp;</td>" +
                              "<td style='align:right'>&nbsp;" + e.InvoiceItemQty + "</td>" +
                              "<td style=''>" + e.CustomerReff + "&nbsp;</td>" +
                              "<td style='align:right'>&nbsp;" + e.PartGrossWeight + "</td>" +
                              "<td style='align:right'>&nbsp;" + e.ChargesDiscountAmount + "</td>" +
                              "<td>" + e.BECode + "&nbsp;</td>" +
                              "<td>" + e.OrderCLSCode + "&nbsp;</td>" +
                              "<td>" + e.Profile + "&nbsp;</td>" +
                              "<td style='align:right'>&nbsp;" + e.UnitPrice + "</td>" +
                              "<td>" + e.OMID + "&nbsp;</td>" +
                              "</tr>");
                }
                sb.Append("</table>");
            }
            return View("excel", sb);

        }

    }
}