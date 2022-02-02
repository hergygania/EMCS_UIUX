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
        //[myAuthorize(Roles = "ReportSupply")]
        [AuthorizeAcces(ActionType = AuthorizeAcces.IsRead)]
        public ActionResult VendorConstraint()
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

        public ActionResult VendorConstraintPage()
        {
            PaginatorBoot.Remove("SessionTRN");
            return VendorConstraintPageXt();
        }

        public ActionResult VendorConstraintPageXt()
        {
            Func<ReportFilterView, List<RptVendorConstraint>> func =
                delegate(ReportFilterView crit)
                {
                    string param = Request["params"];
                    if (!string.IsNullOrEmpty(param))
                    {
                        var ser = new JavaScriptSerializer();
                        crit = ser.Deserialize<ReportFilterView>(param);
                    }

                    var userId = User.Identity.GetUserId();
                    List<RptVendorConstraint> list = VendorConstraints.GetList(crit.GroupType, crit.FilterBy,
                        crit.StoreNumber, crit.StartDate,
                        crit.EndDate, userId);
                    return list.OrderBy(a => a.vcon_CreatedOn).ThenBy(a => a.vcon_ID).ToList();
                };

            ActionResult paging = PaginatorBoot.Manage("SessionTRN", func).Pagination.ToJsonResult();
            return Json(paging, JsonRequestBehavior.AllowGet);
        }


        //public void ExportToExcelVendorConstraint(string groupType, string filterType,
        //    string storeNumber, DateTime? startDate,
        //    DateTime? endDate)
        //{
        //    List<RptVendorConstraint> list = VendorConstraints.GetList(groupType, filterType, storeNumber, startDate,
        //        endDate);
        //    List<RptVendorConstraint> dataInventory =
        //        list.OrderBy(a => a.vcon_UpdatedOn).ThenBy(a => a.vcon_StoreNo).ToList();
        //    DataTable dt = DataTableHelper.ConvertTo(dataInventory);
        //    ExportToExcel(dt, "VendorConstraint");
        //}


        public ActionResult ExportToExcelVendorConstraint
            (string groupType, string filterBy,
            string storeNumber, DateTime? startDate,
            DateTime? endDate)
        {
            var userId = User.Identity.GetUserId();
            List<RptVendorConstraint> listCurrent = VendorConstraints.GetList(groupType, filterBy, storeNumber, startDate,
                endDate, userId);

            ViewBag.Title = "Vendor Constraint";
            var sb = new System.Text.StringBuilder();
            sb.Append("<b>Vendor Constraint</b>");
            if (listCurrent != null)
            {

                var no = 0;
                sb.Append("<table border=1  cellspacing='3'>");
                sb.Append("<tr>" +
                          "<th>No</th>" +
                          "<th>RPORNE</th>" +
                          "<th>SOS</th>" +
                          "<th>Store</th>" +
                          "<th>Part Number</th>" +
                          "<th>Description</th>" +
                          "<th>Document</th>" +
                          "<th>QTY</th>" +
                           "<th style='align:right'>Order Date</th>" +
                          "<th style='align:right'>Invoice Date</th>" +
                          "<th>UNCS</th>" +
                          "<th>Weight</th>" +
                          "<th>Case NO</th>" +
                          "<th>ASN NO</th>" +
                          "<th>Invoice NO</th>" +
                          "</tr>");


                foreach (var e in listCurrent.ToList())
                {
                    var orderDate = (e.vcon_OrderDate.HasValue) ? e.vcon_OrderDate.Value.ToString("dd MMM yyyy") : "";
                    var invoiceDate = (e.vcon_InvoiceDate.HasValue) ? e.vcon_InvoiceDate.Value.ToString("dd MMM yyyy") : "";

                    no++;
                    sb.Append("<tr>" +
                              "<td>" + no + "</td>" +
                              "<td>" + e.vcon_RPORNE + "&nbsp;</td>" +
                              "<td>&nbsp;" + e.vcon_SOS + "&nbsp;</td>" +
                              "<td>" + e.vcon_StoreNo + "&nbsp;</td>" +
                              "<td>" + e.vcon_PartsNumber + "&nbsp;</td>" +
                              "<td>" + e.vcon_Description + "&nbsp;</td>" +
                              "<td>" + e.vcon_Document + "&nbsp;</td>" +
                              "<td>&nbsp;" + e.vcon_QTY + "&nbsp;</td>" +
                              "<td>&nbsp;" + orderDate + "&nbsp;</td>" +
                              "<td>&nbsp;" + invoiceDate + "&nbsp;</td>" +
                              "<td>" + e.vcon_UNCS + "&nbsp;</td>" +
                              "<td>" + e.vcon_Weight + "&nbsp;</td>" +
                              "<td>" + e.vcon_CaseNo + "&nbsp;</td>" +
                              "<td>" + e.vcon_ASNNo + "&nbsp;</td>" +
                              "<td>" + e.vcon_InvoiceNo + "&nbsp;</td>" +
                              "</tr>");
                }
                sb.Append("</table>");
            }
            return View("excel", sb);

        }
    }
}