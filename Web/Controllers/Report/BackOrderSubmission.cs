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

        [AuthorizeAcces(ActionType = AuthorizeAcces.IsRead, UrlMenu = "BackorderSubmission")]
        [AuthorizeAcces(ActionType = AuthorizeAcces.IsRead)]
        public ActionResult BackOrderSubmission()
        {
            ViewBag.AllowRead = AuthorizeAcces.AllowRead;
            ViewBag.AllowCreate = AuthorizeAcces.AllowCreated;
            ViewBag.AllowUpdate = AuthorizeAcces.AllowUpdated;
            ViewBag.AllowDelete = AuthorizeAcces.AllowDeleted;
            PaginatorBoot.Remove("SessionTRN");
            var model = new ReportFilterView();
            var userName = Domain.SiteConfiguration.UserName;
            model.StartDate = new DateTime(DateTime.Now.Year, 1, 1);
            model.EndDate = DateTime.Now;
            model.HubList = GetListHubByUserName(userName);
            model.AreaList = GetListAreaByUserName(userName);
            model.StoreNumberList = GetListStoreByUserName(userName);
            model.Customers = GetCustomersBackorder();// new SelectList(BackorderSubmissions.GetListCustomers(), "bcksms_CustID", "bcksms_CustName");
            return View(model);
        }
        [AuthorizeAcces(ActionType = AuthorizeAcces.IsRead, UrlMenu = "BackorderSubmission")]
        public ActionResult BackorderSubmissionPage()
        {
            PaginatorBoot.Remove("SessionTRN");
            return BackorderSubmissionPageXt();
        }
        [AuthorizeAcces(ActionType = AuthorizeAcces.IsRead, UrlMenu = "BackorderSubmission")]

        public ActionResult BackorderSubmissionPageXt()
        {
            Func<ReportFilterView, List<RptBackorderSubmission>> func = delegate(ReportFilterView crit)
            {
                string param = Request["params"];
                if (!string.IsNullOrEmpty(param))
                {
                    var ser = new JavaScriptSerializer();
                    crit = ser.Deserialize<ReportFilterView>(param);
                }

                var userId = User.Identity.GetUserId();
                List<RptBackorderSubmission> list = BackorderSubmissions.GetList(crit.GroupType, crit.FilterBy, crit.StoreNumber, crit.CustomerId,userId);
                return list.OrderBy(a => a.bcksms_UpdatedOn).ThenBy(a => a.bcksms_Store).ToList();
            };

            ActionResult paging = PaginatorBoot.Manage("SessionTRN", func).Pagination.ToJsonResult();
            return Json(paging, JsonRequestBehavior.AllowGet);
        }


        //public void ExportToExcelBackorde(string groupType, string filterBy, string storeNumber, DateTime? startDate,
        //        DateTime? endDate, string[] custId)
        //{
        //    string[] customer = new string[] { };
        //    customer = custId != null ? (custId[0] != "null" ? custId[0].Split(',').ToArray() : null) : null;
        //    List<RptBackorderSubmission> list = BackorderSubmissions.GetList(groupType, filterBy, storeNumber, startDate,
        //        endDate, customer);
        //    var data = list.OrderBy(a => a.bcksms_UpdatedOn).ThenBy(a => a.bcksms_Store).ToList();
        //    DataTable dt = DataTableHelper.ConvertTo(data);
        //    ExportToExcel(dt, "BackOrderSubmission");
        //}
        [AuthorizeAcces(ActionType = AuthorizeAcces.IsRead, UrlMenu = "BackorderItem")]
        public ActionResult ExportToExcelBackorde(string groupType, string filterBy, string storeNumber,string[] custId)
        {
            string[] customer = new string[] { };
            var userId = User.Identity.GetUserId();
            customer = custId != null ? (custId[0] != "null" ? custId[0].Split(',').ToArray() : null) : null;
            List<RptBackorderSubmission> listCurrent = BackorderSubmissions.GetList(groupType, filterBy, storeNumber, customer,userId);

            ViewBag.Title = "Backorder Submission";
            var sb = new System.Text.StringBuilder();
            sb.Append("<b>Backorder Submission</b>");
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
                          "<th>SOS</th>" +
                          "<th>Part No</th>" +
                          "<th>Customer Name</th>" +
                          "<th>Description</th>" +
                          "<th style='align:right'>Quantity</th>" +
                          "<th>Facility</th>" +
                          "<th>Transfer Order</th>" +
                          "<th>FAC Order No</th>" +
                          "<th>Comments</th>" +
                          "<th>Customer Id</th>" +
                          "<th>Customer Name</th>" +
                          "</tr>");


                foreach (var e in listCurrent.ToList())
                {
                    var docDate = (e.bcksms_DocDate.HasValue) ? e.bcksms_DocDate.Value.ToString("dd MMM yyyy") : "";

                    no++;
                    sb.Append("<tr>" +
                              "<td>" + no + "</td>" +
                              "<td>" + e.bcksms_StoreName + "&nbsp;</td>" +
                              "<td>" + e.bcksms_AreaName + "&nbsp;</td>" +
                              "<td>" + e.bcksms_HubName + "&nbsp;</td>" +
                              "<td>" + e.bcksms_RefDoc + "&nbsp;</td>" +
                              "<td>" + e.bcksms_CustPONo + "&nbsp;</td>" +
                              "<td style='align:right'>&nbsp;" + docDate + "</td>" +
                              "<td>&nbsp;" + e.bcksms_SOS + "</td>" +
                              "<td>&nbsp;" + e.bcksms_PartNo + "&nbsp;</td>" +
                              "<td>&nbsp;" + e.bcksms_CustName + "&nbsp;</td>" +
                              "<td>&nbsp;" + e.bcksms_Description + "&nbsp;</td>" +
                              "<td style='align:right'>&nbsp;" + e.bcksms_BackorderQty+ "</td>" +
                              "<td>&nbsp;" + e.bcksms_Facility + "&nbsp;</td>" +
                              "<td>&nbsp;" + e.bcksms_TransferOrdNo + "&nbsp;</td>" +
                              "<td>&nbsp;" + e.bcksms_FacOrdNo + "&nbsp;</td>" +
                              "<td>&nbsp;" + e.bcksms_Comments + "&nbsp;</td>" +
                              "<td>&nbsp;" + e.bcksms_CustID + "&nbsp;</td>" +
                              "<td>&nbsp;" + e.bcksms_CustName + "&nbsp;</td>" +
                              "</tr>");
                }
                sb.Append("</table>");
            }
            return View("excel", sb);

        }

        private IEnumerable<SelectListItem> GetCustomersBackorder()
        {
            List<RptBackorderSubmission> list = BackorderSubmissions.GetListCustomers();
            List<SelectListItem> listItem = list.Select(doc => new SelectListItem
            {
                Text = doc.bcksms_CustID + "-" + doc.bcksms_CustName,
                Value = doc.bcksms_CustID
            }).ToList();

            return listItem;
        }
    }
}