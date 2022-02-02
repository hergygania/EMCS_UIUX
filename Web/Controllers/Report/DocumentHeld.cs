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
        public ActionResult DocumentHeld()
        {
            ViewBag.AllowRead = AuthorizeAcces.AllowRead;
            ViewBag.AllowCreate = AuthorizeAcces.AllowCreated;
            ViewBag.AllowUpdate = AuthorizeAcces.AllowUpdated;
            ViewBag.AllowDelete = AuthorizeAcces.AllowDeleted;
            PaginatorBoot.Remove("SessionTRN");
            var userName = Domain.SiteConfiguration.UserName;
            var model = new ReportFilterView();
            model.StartDate = new DateTime(DateTime.Now.Year, 1, 1);
            model.EndDate = DateTime.Now;
            model.HubList = GetListHubByUserName(userName);
            model.AreaList = GetListAreaByUserName(userName);
            model.StoreNumberList = GetListStoreByUserName(userName);
            model.Customers = GetCustomersDocHeld();
            return View(model);
        }
        [AuthorizeAcces(ActionType = AuthorizeAcces.IsRead, UrlMenu = "DocumentHeld")]
        public ActionResult DocumentHeldPage()
        {
            PaginatorBoot.Remove("SessionTRN");
            return DocumentHeldPageXt();
        }
        [AuthorizeAcces(ActionType = AuthorizeAcces.IsRead, UrlMenu = "DocumentHeld")]
        public ActionResult DocumentHeldPageXt()
        {
            Func<ReportFilterView, List<RptDocumentHeld>> func = delegate(ReportFilterView crit)
            {
                string param = Request["params"];
                if (!string.IsNullOrEmpty(param))
                {
                    var ser = new JavaScriptSerializer();
                    crit = ser.Deserialize<ReportFilterView>(param);
                }
                var userId = User.Identity.GetUserId();
                List<RptDocumentHeld> list = DocumentHelds.GetList(crit.GroupType, crit.FilterBy, crit.StoreNumber, crit.CustomerId,userId);
                return list.OrderBy(a => a.rhld_UpdatedOn).ThenBy(a => a.rhld_Store).ToList();
            };

            ActionResult paging = PaginatorBoot.Manage("SessionTRN", func).Pagination.ToJsonResult();
            return Json(paging, JsonRequestBehavior.AllowGet);
        }



        //public void ExportToExcelDocHeld(
        // string groupType,string filterType, string storeNumber, DateTime? startDate,
        //        DateTime? endDate, string[] custId)
        //{
        //    string[] customer = new string[] { };
        //    customer = custId != null ? (custId[0] != "null" ? custId[0].Split(',').ToArray() : null) : null;
        //    var list = DocumentHelds.GetList(groupType,filterType, storeNumber, startDate,
        //        endDate, customer);
        //    var dt = DataTableHelper.ConvertTo(list);
        //    ExportToExcel(dt, "DocumentHeld");
        //}

        [AuthorizeAcces(ActionType = AuthorizeAcces.IsRead, UrlMenu = "DocumentHeld")]
        public ActionResult ExportToExcelDocHeld(string groupType, string filterBy, string storeNumber, string[] custId)
        {
            var userId = User.Identity.GetUserId();
         
            string[] customer = new string[] { };
            customer = custId != null ? (custId[0] != "null" ? custId[0].Split(',').ToArray() : null) : null;
            var listCurrent = DocumentHelds.GetList(groupType, filterBy, storeNumber, customer, userId);

            ViewBag.Title = "Document Held";
            var sb = new System.Text.StringBuilder();
            sb.Append("<b>Document Held</b>");
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
                    var docDate = (e.rhld_DocDate.HasValue) ? e.rhld_DocDate.Value.ToString("dd MMM yyyy") : "";
                    var docValue = (e.rhld_DocValue != null)
                        ? FormatStringNumber(e.rhld_DocValue)
                        : "";
                    no++;
                    sb.Append("<tr>" +
                              "<td>" + no + "</td>" +
                              "<td>" + e.rhld_StoreName + "&nbsp;</td>" +
                              "<td>" + e.rhld_AreaName + "&nbsp;</td>" +
                              "<td>" + e.rhld_HubName + "&nbsp;</td>" +
                              "<td>" + e.rhld_RefDoc + "&nbsp;</td>" +
                              "<td>" + e.rhld_CustPONo + "&nbsp;</td>" +
                              "<td style='align:right'>&nbsp;" + docDate + "</td>" +
                              "<td style='align:right'>&nbsp;" + docValue + "</td>" +
                              "<td>" + e.rhld_CustID + "</td>" +
                              "<td>" + e.rhld_CustName + "</td>" +
                              "<td>" + e.rhld_Remarks + "</td>" +
                              "</tr>");
                }
                sb.Append("</table>");
            }
            return View("excel", sb);

        }


        private IEnumerable<SelectListItem> GetCustomersDocHeld()
        {
            List<RptDocumentHeld> list = DocumentHelds.GetListCustomers();
            List<SelectListItem> listItem = list.Select(doc => new SelectListItem
            {
                Text = doc.rhld_CustID + "-" + doc.rhld_CustName,
                Value = doc.rhld_CustID
            }).ToList();

            return listItem;
        }

    }
}