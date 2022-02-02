using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
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

        [AuthorizeAcces(ActionType = AuthorizeAcces.IsRead, UrlMenu = "OutstandingBackorderItem")]
        [AuthorizeAcces(ActionType = AuthorizeAcces.IsRead)]
        public ActionResult OutstandingBackorderItem()
        {
            ViewBag.AllowRead = AuthorizeAcces.AllowRead;
            ViewBag.AllowCreate = AuthorizeAcces.AllowCreated;
            ViewBag.AllowUpdate = AuthorizeAcces.AllowUpdated;
            ViewBag.AllowDelete = AuthorizeAcces.AllowDeleted;
            var userName = Domain.SiteConfiguration.UserName;
            PaginatorBoot.Remove("SessionTRN");
            var model = new ReportFilterView();
            model.StartDate = new DateTime(DateTime.Now.Year, 1, 1);
            model.EndDate = DateTime.Now;
            model.HubList = GetListHubByUserName(userName);
            model.AreaList = GetListAreaByUserName(userName);
            model.StoreNumberList = GetListStoreByUserName(userName);
            model.Customers = GetCustomersOBItem();// new SelectList(OutstandingBackorderItems.GetListCustomers(), "obkitm_CustNo", "obkitm_CustName");
            return View(model);
        }
        [AuthorizeAcces(ActionType = AuthorizeAcces.IsRead, UrlMenu = "OutstandingBackorderItem")]
        public ActionResult OutstandingBackorderItemPage()
        {
            PaginatorBoot.Remove("SessionTRN");
            return OutstandingBackorderItemPageXt();
        }
        [AuthorizeAcces(ActionType = AuthorizeAcces.IsRead, UrlMenu = "OutstandingBackorderItem")]
        public ActionResult OutstandingBackorderItemPageXt()
        {
            Func<ReportFilterView, List<RptOutstandingBackorderItem>> func = delegate(ReportFilterView crit)
            {
                string param = Request["params"];
                if (!string.IsNullOrEmpty(param))
                {
                    var ser = new JavaScriptSerializer();
                    crit = ser.Deserialize<ReportFilterView>(param);
                }

                var userId = User.Identity.GetUserId();
                List<RptOutstandingBackorderItem> list = OutstandingBackorderItems.
                    GetList(crit.GroupType, crit.FilterBy, crit.StoreNumber, crit.CustomerId,userId);
                return list.OrderBy(a => a.obkitm_UpdatedOn).ThenBy(a => a.obkitm_ID).ToList();
            };

            ActionResult paging = PaginatorBoot.Manage("SessionTRN", func).Pagination.ToJsonResult();
            return Json(paging, JsonRequestBehavior.AllowGet);
        }



        //public void ExportToExcelOutstandingBackorderItem(string groupType,string filterBy, string storeNumber, DateTime? startDate,
        //        DateTime? endDate, string[] custId)
        //{
        //    string[] customer = new string[] { };
        //    customer = custId != null ? (custId[0] != "null" ? custId[0].Split(',').ToArray() : null) : null;
        //    List<RptOutstandingBackorderItem> list = OutstandingBackorderItems.GetList(groupType,filterBy, storeNumber, startDate,
        //        endDate, customer);


        //    DataTable dt = DataTableHelper.ConvertTo(list);
        //    ExportToExcel(dt, "OutstandingBackorderItem");
        //}
        [AuthorizeAcces(ActionType = AuthorizeAcces.IsRead, UrlMenu = "OutstandingBackorderItem")]
        public ActionResult ExportToExcelOutstandingBackorderItem(string groupType, string filterBy, string storeNumber, string[] custId)
        {
            string[] customer = new string[] { };
            customer = custId != null ? (custId[0] != "null" ? custId[0].Split(',').ToArray() : null) : null;
            var userId = User.Identity.GetUserId();
            List<RptOutstandingBackorderItem> listCurrent = OutstandingBackorderItems.GetList(groupType, filterBy, storeNumber, customer,userId);

            ViewBag.Title = "Outstanding Back Order Item";
            var sb = new System.Text.StringBuilder();
            sb.Append("<b>Outstanding Back Order Item</b>");
            if (listCurrent != null)
            {

                var no = 0;
                sb.Append("<table border=1  cellspacing='3'>");
                sb.Append("<tr>" +
                          "<th>No</th>" +
                          "<th>Ref Document</th>" +
                          "<th style='align:right'>Org Date</th>" +
                          "<th style='align:right'>Need by Date</th>" +
                          "<th style='align:right'>Commited Date</th>" +
                          "<th>Customer PO</th>" +
                          "<th>SOS</th>" +
                          "<th>Part Number</th>" +
                          "<th>Description</th>" +
                          "<th style='align:right'>Order Quantity</th>" +
                          "<th style='align:right'>Comodity</th>" +
                          "<th>Order Method</th>" +
                          "<th>Activity Ind</th>" +
                          "<th>SKNSKI</th>" +
                          "<th style='align:right'>Pack Qty</th>" +
                          "<th style='align:right'>Gross Wt</th>" +
                          "<th style='align:right'>HOSE ASM IND</th>" +
                          "<th style='align:right'>Unit List</th>" +
                          "<th style='align:right'>BO12M</th>" +
                          "<th style='align:right'>CALL12M</th>" +
                          "<th style='align:right'>DEMAND12M</th>" +
                          "<th style='align:right'>Model</th>" +
                          "<th style='align:right'>Serial Number</th>" +
                          "<th style='align:right'>Machine Id</th>" +
                          "<th style='align:right'>Customer Number</th>" +
                          "<th style='align:right'>Customer Name</th>" +
                          "<th style='align:right'>Facility</th>" +
                          "<th style='align:right'>Entry Class</th>" +
                          "<th style='align:right'>BO Ship IND</th>" +
                          "<th style='align:right'>Transfer Order No</th>" +
                          "<th style='align:right'>Fac Order No</th>" +
                          "<th style='align:right'>Comments</th>" +
                          "<th style='align:right'>DA CKB</th>" +
                          "<th style='align:right'>Pickup Date</th>" +
                          "<th style='align:right'>Lead Time</th>" +
                          "<th style='align:right'>Eta Date</th>" +
                          "<th style='align:right'>Ord To Cur Date</th>" +
                          "<th style='align:right'>Ord To Need By Date</th>" +
                          "<th style='align:right'>Ord To Commited Date</th>" +
                          "<th style='align:right'>Need By Date to Curr Date</th>" +
                          "<th style='align:right'>Commited By Date to Curr Date</th>" +
                          "</tr>");


                foreach (var e in listCurrent.ToList())
                {
                    //var orgDate = (string.IsNullOrEmpty(e.obkitm_OrgDate)) ? DateTime.TryParseExact(e.obkitm_OrgDate,"yyyyMMdd",CultureInfo.InvariantCulture) : "";
                    //var commitedDate = (e.obkitm_CommittedDate.HasValue) ? e.obkitm_CommittedDate.Value.ToString("dd MMM yyyy") : "";
                    //var needByDate = (e.obkitm_NeedByDate.HasValue) ? e.obkitm_NeedByDate.Value.ToString("dd MMM yyyy") : "";
                    //var etaDate = (e.obkitm_ETADate.HasValue) ? e.obkitm_ETADate.Value.ToString("dd MMM yyyy") : "";
                    //var pickupDate = (e.obkitm_PickupDate.HasValue) ? e.obkitm_PickupDate.Value.ToString("dd MMM yyyy") : "";
                    //var ordToCurrDate = (e.obkitm_OrdToCurrDate.HasValue) ? e.obkitm_OrdToCurrDate.Value.ToString("dd MMM yyyy") : "";
                    //var ordToNeedByDate = (e.obkitm_OrdToNeedByDate.HasValue) ? e.obkitm_OrdToNeedByDate.Value.ToString("dd MMM yyyy") : "";
                    //var ordToCommitedDate = (e.obkitm_OrdToCommitedDate.HasValue) ? e.obkitm_OrdToCommitedDate.Value.ToString("dd MMM yyyy") : "";
                    //var needByDateToCurrDate = (e.obkitm_NeedByDateToCurrDate.HasValue) ? e.obkitm_NeedByDateToCurrDate.Value.ToString("dd MMM yyyy") : "";
                    //var committedDateToCurrDate = (e.obkitm_CommitedDateToCurrDate.HasValue) ? e.obkitm_CommitedDateToCurrDate.Value.ToString("dd MMM yyyy") : "";
                   
                    no++;
                    sb.Append("<tr>" +
                              "<td>" + no + "</td>" +
                              "<td>" + e.obkitm_StoreName + "&nbsp;</td>" +
                              "<td>" + e.obkitm_RefDoc + "&nbsp;</td>" +
                              "<td style='align:right'>&nbsp;" + e.obkitm_OrgDate + "</td>" +
                              "<td style='align:right'>&nbsp;" + e.obkitm_NeedByDate + "</td>" +
                              "<td style='align:right'>&nbsp;" + e.obkitm_CommittedDate + "</td>" +
                              "<td>" + e.obkitm_CustPO + "&nbsp;</td>" +
                              "<td>" + e.obkitm_SOS + "&nbsp;</td>" +
                              "<td>" + e.obkitm_PartNo + "&nbsp;</td>" +
                              "<td>" + e.obkitm_Description + "&nbsp;</td>" +
                              "<td style='align:right'>&nbsp;" + e.obkitm_OrderQty + "</td>" +
                              "<td>" + e.obkitm_Commodity + "&nbsp;</td>" +
                              "<td>" + e.obkitm_OrderMethod + "&nbsp;</td>" +
                              "<td>" + e.obkitm_ActivityInd + "&nbsp;</td>" +
                              "<td>" + e.obkitm_SKNSKI + "&nbsp;</td>" +
                              "<td style='align:right'>&nbsp;" + e.obkitm_PackQty + "</td>" +
                              "<td style='align:right'>&nbsp;" + e.obkitm_GrossWt + "</td>" +
                              "<td style=''>" + e.obkitm_HoseAdmInd + "&nbsp;</td>" +
                              "<td style=''>" + e.obkitm_UnitList + "&nbsp;</td>" +
                              "<td style=''>" + e.obkitm_BO12M + "&nbsp;</td>" +
                              "<td style=''>" + e.obkitm_CALL12M + "&nbsp;</td>" +
                              "<td style=''>" + e.obkitm_Demand12M + "&nbsp;</td>" +
                              "<td style=''>" + e.obkitm_Model + "&nbsp;</td>" +
                              "<td style=''>" + e.obkitm_SerialNumber + "&nbsp;</td>" +
                              "<td style=''>" + e.obkitm_MachineID + "&nbsp;</td>" +
                              "<td style=''>" + e.obkitm_CustNo + "&nbsp;</td>" +
                              "<td style=''>" + e.obkitm_CustName + "&nbsp;</td>" +
                              "<td style=''>" + e.obkitm_Facility + "&nbsp;</td>" +
                              "<td style=''>" + e.obkitm_EntryClass + "&nbsp;</td>" +
                              "<td style=''>" + e.obkitm_BOShipInd + "&nbsp;</td>" +
                              "<td style=''>" + e.obkitm_TransferOrderNo + "&nbsp;</td>" +
                              "<td style=''>" + e.obkitm_FacOrdNo + "&nbsp;</td>" +
                              "<td style=''>" + e.obkitm_Comments + "&nbsp;</td>" +
                              "<td style=''>" + e.obkitm_DACKB + "&nbsp;</td>" +
                              "<td style=''>" + e.obkitm_PickupDate + "&nbsp;</td>" +
                              "<td style=''>" + e.obkitm_LeadTime + "&nbsp;</td>" +
                              "<td style=''>" + e.obkitm_ETADate + "&nbsp;</td>" +
                              "<td style=''>" + e.obkitm_OrdToCurrDate + "&nbsp;</td>" +
                              "<td style=''>" + e.obkitm_OrdToNeedByDate + "&nbsp;</td>" +
                              "<td style=''>" + e.obkitm_OrdToCommitedDate + "&nbsp;</td>" +
                              "<td style=''>" + e.obkitm_NeedByDateToCurrDate + "&nbsp;</td>" +
                              "<td style=''>" + e.obkitm_CommitedDateToCurrDate + "&nbsp;</td>" +
                              "</tr>");
                }
                sb.Append("</table>");
            }
            return View("excel", sb);

        }

        private IEnumerable<SelectListItem> GetCustomersOBItem()
        {
            List<RptOutstandingBackorderItem> list = OutstandingBackorderItems.GetListCustomers();
            List<SelectListItem> listItem = list.Select(doc => new SelectListItem
            {
                Text = doc.obkitm_CustNo + "-" + doc.obkitm_CustName,
                Value = doc.obkitm_CustNo
            }).ToList();

            return listItem;
        }
    }
}