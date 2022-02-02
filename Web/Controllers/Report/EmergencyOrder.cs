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
        public ActionResult EmergencyOrder()
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
                Customers = GetCustomersEmergencyOrder(userName)
            };
            return View(model);
        }
        [AuthorizeAcces(ActionType = AuthorizeAcces.IsRead, UrlMenu = "EmergencyOrder")]
        public ActionResult EmergencyOrderPage()
        {
            PaginatorBoot.Remove("SessionTRN");
            return EmergencyOrderPageXt();
        }
        [AuthorizeAcces(ActionType = AuthorizeAcces.IsRead, UrlMenu = "EmergencyOrder")]
        public ActionResult EmergencyOrderPageXt()
        {
            Func<ReportFilterView, List<RptEmergencyOrder>> func =
                delegate(ReportFilterView crit)
                {
                    string param = Request["params"];
                    if (!string.IsNullOrEmpty(param))
                    {
                        var ser = new JavaScriptSerializer();
                        crit = ser.Deserialize<ReportFilterView>(param);
                    }
                    var userId = User.Identity.GetUserId();
                    List<RptEmergencyOrder> list = EmergencyOrders.GetList(crit.GroupType, crit.FilterBy, crit.StoreNumber,crit.CustomerId,userId);
                    return list.OrderBy(a => a.emgor_CreatedOn).ThenBy(a => a.emgor_ID).ToList();
                };

            ActionResult paging = PaginatorBoot.Manage("SessionTRN", func).Pagination.ToJsonResult();
            return Json(paging, JsonRequestBehavior.AllowGet);
        }



        //public void ExportToExcelDocEmgoe(string groupType, string filterType, string storeNumber, DateTime? startDate,
        //        DateTime? endDate, string[] custId)
        //{
        //    string[] customer = new string[] { };
        //    customer = custId != null ? (custId[0] != "null" ? custId[0].Split(',').ToArray() : null) : null;
        //    var list = EmergencyOrders.GetList(groupType,filterType, storeNumber, startDate,
        //        endDate, customer);
        //    DataTable dt = DataTableHelper.ConvertTo(list);
        //    ExportToExcel(dt, "EmergencyOrder");
        //}
        [AuthorizeAcces(ActionType = AuthorizeAcces.IsRead, UrlMenu = "EmergencyOrder")]
        public ActionResult ExportToExcelDocEmgoe(string groupType, string filterBy, string storeNumber, string[] custId)
        {
            string[] customer = new string[] { };
            customer = custId != null ? (custId[0] != "null" ? custId[0].Split(',').ToArray() : null) : null;
            var userId = User.Identity.GetUserId();
            var listCurrent = EmergencyOrders.GetList(groupType, filterBy, storeNumber, customer, userId);

            ViewBag.Title = "Emergency Order";
            var sb = new System.Text.StringBuilder();
            sb.Append("<b>Emergency Order</b>");
            if (listCurrent != null)
            {

                var no = 0;
                sb.Append("<table border=1  cellspacing='3'>");
                sb.Append("<tr>" +
                          "<th>No</th>" +
                          "<th>Plant</th>" +
                          "<th>Area</th>" +
                          "<th>Hub</th>" +
                          "<th>SO Number Customer</th>" +
                          "<th>Customer PO Number</th>" +
                          "<th style='align:right'>Document Date</th>" +
                          "<th>SLoc</th>" +
                          "<th>Part No</th>" +
                          "<th>Description</th>" +
                          "<th style='align:right'>Back Order Quantity</th>" +
                          "<th>Facility</th>" +
                          "<th>Delivery Document</th>" +
                            "<th>Customer ID</th>" +
                          "<th>Customer Name</th>" +
                          "</tr>");


                foreach (var e in listCurrent.ToList())
                {
                    var docDate = (e.emgor_DocDate.HasValue) ? e.emgor_DocDate.Value.ToString("dd MMM yyyy") : "";

                    no++;
                    sb.Append("<tr>" +
                              "<td>" + no + "</td>" +
                              "<td>" + e.emgor_StoreName + "&nbsp;</td>" +
                              "<td>" + e.emgor_AreaName + "&nbsp;</td>" +
                              "<td>" + e.emgor_HubName + "&nbsp;</td>" +
                              "<td>" + e.emgor_RefDoc + "&nbsp;</td>" +
                              "<td>" + e.emgor_CustPONo + "&nbsp;</td>" +
                              "<td style='align:right'>&nbsp;" + docDate + "</td>" +
                              "<td>" + e.emgor_SOS + "&nbsp;</td>" +
                              "<td>" + e.emgor_PartNo + "&nbsp;</td>" +
                              "<td>" + e.emgor_Description + "&nbsp;</td>" +
                              "<td style='align:right'>&nbsp;" + e.emgor_BackorderQty + "</td>" +
                              "<td>" + e.emgor_Facility + "&nbsp;</td>" +
                              "<td>" + e.emgor_Comments + "&nbsp;</td>" +
                              "<td>" + e.emgor_CustID + "</td>" +
                              "<td>" + e.emgor_CustName + "</td>" +
                              "</tr>");
                }
                sb.Append("</table>");
            }
            return View("excel", sb);

        }

        private IEnumerable<SelectListItem> GetCustomersEmergencyOrder(string username)
        {
            List<RptEmergencyOrder> list = EmergencyOrders.GetListCustomers(username);
            List<SelectListItem> listItem = list.Select(doc => new SelectListItem
            {
                Text = doc.emgor_CustID+"-"+doc.emgor_CustName,
                Value = doc.emgor_CustID
            }).ToList();

            return listItem;
        }
    }
}