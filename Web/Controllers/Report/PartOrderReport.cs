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
        public ActionResult PartOrderReport()
        {
            ViewBag.AllowRead = AuthorizeAcces.AllowRead;
            ViewBag.AllowCreate = AuthorizeAcces.AllowCreated;
            ViewBag.AllowUpdate = AuthorizeAcces.AllowUpdated;
            ViewBag.AllowDelete = AuthorizeAcces.AllowDeleted;
            PaginatorBoot.Remove("SessionTRN");
            var model = new PartOrderFilterView
            {
                InvoiceStartDate = new DateTime(DateTime.Now.Year, 1, 1),
                InvoiceEndDate = DateTime.Now,
                ShippingInstructions = Service.Master.ShippingInstruction.GetList(),
                AgreementTypes = GetSelectListItem(PartOrderReports.GetAgreementTypes()),
                JCodes = Service.Master.Stores.GetList(),
                HubList = Hub.GetList(),
                AreaList = Area.GetList(),
                StoreNumberList = new List<Store>(),
            };
            return View(model);
        }
        [AuthorizeAcces(ActionType = AuthorizeAcces.IsRead, UrlMenu = "PartOrderReport")]
        public ActionResult PartOrderReportPage()
        {
            PaginatorBoot.Remove("SessionTRN");
            return PartOrderReportPageXt();
        }
        [AuthorizeAcces(ActionType = AuthorizeAcces.IsRead, UrlMenu = "PartOrderReport")]
        public ActionResult PartOrderReportPageXt()
        {
            Func<PartOrderFilterView, List<V_PART_ORDER_REPORT>> func = delegate(PartOrderFilterView crit)
            {
                string param = Request["params"];
                if (!string.IsNullOrEmpty(param))
                {
                    var ser = new JavaScriptSerializer();
                    crit = ser.Deserialize<PartOrderFilterView>(param);
                }

                List<V_PART_ORDER_REPORT> list = PartOrderReports.GetList(
                    crit.InvoiceNo, crit.InvoiceStartDate, crit.InvoiceEndDate,
                    crit.GroupType, crit.FilterBy, crit.StoreNumber,
                    crit.JCode, crit.ShippingInstruction, crit.IsHazardous,
                    crit.AgreementType, crit.SOS
                    );
                return list.OrderBy(a => a.ModifiedDate).ThenBy(a => a.StoreName).ToList();
            };

            ActionResult paging = PaginatorBoot.Manage("SessionTRN", func).Pagination.ToJsonResult();
            return Json(paging, JsonRequestBehavior.AllowGet);
        }



        //public void ExportToExcelPartOrder(
        //    string invoiceNo, DateTime? invoiceStartDate,
        //    DateTime? invoiceEndDate, string groupType,
        //    string filterBy, string storeNumber,
        //    string jCode, int? shippingInstruction,
        //    bool? isHazardous, string agreementType, string sos
        //    )
        //{
        //    var list = PartOrderReports.GetList(
        //        invoiceNo, invoiceStartDate, invoiceEndDate,
        //            groupType, filterBy, storeNumber,
        //            jCode, shippingInstruction, isHazardous,
        //            agreementType, sos);
        //    var dt = DataTableHelper.ConvertTo(list);
        //    ExportToExcel(dt, "PartOrderReport");
        //}

        public ActionResult ExportToExcelPartOrder(string invoiceNo, DateTime? invoiceStartDate,
            DateTime? invoiceEndDate, string groupType,
            string filterBy, string storeNumber,
            string jCode, int? shippingInstruction,
            bool? isHazardous, string agreementType, string sos)
        {
            //var listCurrent = Paginator.GetPagination<Data.Domain.Shipment>("SessionTRN").ToList();
            var listCurrent = PartOrderReports.GetList(
                invoiceNo, invoiceStartDate, invoiceEndDate,
                groupType, filterBy, storeNumber,
                jCode, shippingInstruction, isHazardous,
                agreementType, sos);

            //var x= (listCurrent)(new System.Collections.Generic.Mscorlib_CollectionDebugView<<>f__AnonymousType9<App.Data.Domain.Shipment,bool,App.Data.Domain.ShipmentManifest,App.Data.Domain.ShipmentManifestDetail,App.Data.Domain.PartsOrderCase>>(listCurrent as System.Collections.Generic.List<<>f__AnonymousType9<App.Data.Domain.Shipment,bool,App.Data.Domain.ShipmentManifest,App.Data.Domain.ShipmentManifestDetail,App.Data.Domain.PartsOrderCase>>)).Items[0];

            ViewBag.Title = "Part Order";
            var sb = new System.Text.StringBuilder();
            sb.Append("<b>Part Order</b>");
            if (listCurrent != null)
            {
                var no = 0;
                sb.Append("<table border=1  cellspacing='3'>");
                sb.Append("<tr>" +
                          "<th>No</th>" +
                          "<th>Parts Order ID</th>" +
                          "<th>Invoice No </th>" +
                          "<th style='align:right'>Invoice Date</th>" +
                          "<th>J Code</th>" +
                          "<th>Store Number</th>" +
                          "<th>Shipping Instruction ID</th>" +
                          "<th style='align:right'>Total Amount</th>" +
                          "<th style='align:right'>Total FOB</th>" +
                          "<th>Is Hazardous</th>" +
                          "<th style='align:right'>Service Charges</th>" +
                          "<th style='align:right'>Core Deposit</th>" +
                          "<th style='align:right'>Other Charges</th>" +
                          "<th style='align:right'>Freight Charges</th>" +
                          "<th>Shipping IDASN</th>" +
                          "<th>Agreement Type</th>" +
                          "<th>Vetting Route</th>" +
                          "<th style='align:right'>Survey Date</th>" +
                          "<th style=''>Status</th>" +
                          "<th style=''>SOS</th>" +
                          "<th style='align:right'>HPL Receipt Date</th>" +
                          "</tr>");


                foreach (var e in listCurrent.ToList())
                {
                    var surveyDate = (e.SurveyDate.HasValue) ? e.SurveyDate.Value.ToString("dd MMM yyyy") : "";
                    var hplReceiptDate = (e.HPLReceiptDate.HasValue) ? e.HPLReceiptDate.Value.ToString("dd MMM yyyy") : "";
                    no++;
                    sb.Append("<tr>" +
                              "<td>" + no + "</td>" +
                              "<td>" + e.PartsOrderID + "&nbsp;</td>" +
                              "<td>" + e.InvoiceNo + "&nbsp;</td>" +
                              "<td style='align:right'>&nbsp;" + e.InvoiceDate.ToString("dd MMM yyyy") + "</td>" +
                              "<td>" + e.JCode + "</td>" +
                              "<td>" + e.StoreNumber + "</td>" +
                              "<td>&nbsp;" + e.ShippingInstructionID + "</td>" +
                              "<td style='align:right'>&nbsp;" + e.TotalAmount + "</td>" +
                              "<td style='align:right'>&nbsp;" + e.TotalFOB + "</td>" +
                              "<td>" + e.IsHazardous + "&nbsp;</td>" +
                              "<td style='align:right'>" + e.ServiceCharges + "&nbsp;</td>" +
                              "<td style='align:right'>" + e.CoreDeposit + "&nbsp;</td>" +
                              "<td style='align:right'>" + e.OtherCharges + "&nbsp;</td>" +
                              "<td style='align:right'>" + e.FreightCharges + "&nbsp;</td>" +
                              "<td>" + e.ShippingIDASN + "&nbsp;</td>" +
                              "<td>" + e.AgreementType + "&nbsp;</td>" +
                              "<td>" + e.VettingRoute + "&nbsp;</td>" +
                              "<td style='align:right'>" + surveyDate + "&nbsp;</td>" +
                              "<td style=''>&nbsp;" + e.Status + "</td>" +
                              "<td style=''>&nbsp;" + e.SOS + "</td>" +
                        "<td style='align:right'>" + hplReceiptDate + "&nbsp;</td>" +
                              "</tr>");
                }
                sb.Append("</table>");
            }
            return View("excel", sb);

        }

        public IEnumerable<SelectListItem> GetSelectListItem(IEnumerable<string> listString)
        {
            var list = listString.Select(data => new SelectListItem()
            {
                Text = data,
                Value = data,
            }).ToList();

            return list;
        }


    }
}