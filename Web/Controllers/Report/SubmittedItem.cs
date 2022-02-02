using System;
using System.Collections.Generic;
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
        public ActionResult SubmittedItem()
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

        public ActionResult SubmittedItemPage()
        {
            PaginatorBoot.Remove("SessionTRN");
            return SubmittedItemPageXt();
        }

        public ActionResult SubmittedItemPageXt()
        {
            Func<ReportFilterView, List<RptSubmittedItem>> func =
                delegate(ReportFilterView crit)
            {
                string param = Request["params"];
                if (!string.IsNullOrEmpty(param))
                {
                    var ser = new JavaScriptSerializer();
                    crit = ser.Deserialize<ReportFilterView>(param);
                }

                List<RptSubmittedItem> list = SubmittedItems.GetList
                    (crit.GroupType, crit.FilterBy,
                    crit.StoreNumber, crit.StartDate,
                    crit.EndDate);
                return list.OrderBy(a => a.sbmitm_CreatedOn).ThenBy(a => a.sbmitm_ID).ToList();
            };

            ActionResult paging = PaginatorBoot.Manage("SessionTRN", func).Pagination.ToJsonResult();
            return Json(paging, JsonRequestBehavior.AllowGet);
        }


        //public void ExportToExcelSubmittedItem
        //    (string groupType, string filterType, 
        //    string storeNumber, DateTime? startDate,
        //    DateTime? endDate)
        //{
        //    List<RptSubmittedItem> list = SubmittedItems.GetList(groupType, filterType, storeNumber, startDate,
        //        endDate);
        //    var dataInventory = list.OrderBy(a => a.sbmitm_UpdatedOn).ThenBy(a => a.sbmitm_StoreNo).ToList();
        //    var dt = DataTableHelper.ConvertTo(dataInventory);
        //    ExportToExcel(dt, "SubmittedItem");

        //}

        public ActionResult ExportToExcelSubmittedItem
            (string groupType, string filterBy,
            string storeNumber, DateTime? startDate,
            DateTime? endDate)
        {
            List<RptSubmittedItem> list = SubmittedItems.GetList
                (groupType, filterBy, storeNumber, startDate,
                endDate);
            var listCurrent = list.OrderBy(a => a.sbmitm_UpdatedOn).ThenBy(a => a.sbmitm_StoreNo).ToList();
            
            ViewBag.Title = "Submitted Item";
            var sb = new System.Text.StringBuilder();
            sb.Append("<b>Submitted Item</b>");
            if (listCurrent != null)
            {

                var no = 0;
                sb.Append("<table border=1  cellspacing='3'>");
                sb.Append("<tr>" +
                          "<th>No</th>" +
                          "<th>SOS</th>" +
                          "<th>Store</th>" +
                          "<th>Part Number</th>" +
                          "<th>Description</th>" +
                          "<th>Document</th>" +
                          "<th style='align:right'> Date</th>" +
                          "<th>TRXCD1</th>" +
                          "<th style='align:right'>Quantity</th>" +
                          "<th>UNCS</th>" +
                          "<th>GRSSWT</th>" +

                          "</tr>");


                foreach (var e in listCurrent.ToList())
                {
                    var date = (e.sbmitm_EntryDate.HasValue) ? e.sbmitm_EntryDate.Value.ToString("dd MMM yyyy") : "";

                    no++;
                    sb.Append("<tr>" +
                              "<td>" + no + "</td>" +
                              "<td>&nbsp;" + e.sbmitm_SOS + "&nbsp;</td>" +
                              "<td>" + e.sbmitm_StoreNo + "&nbsp;</td>" +
                              "<td>" + e.sbmitm_PartNumber + "&nbsp;</td>" +
                              "<td>" + e.sbmitm_Description + "&nbsp;</td>" +
                              "<td>" + e.sbmitm_Document + "&nbsp;</td>" +
                              "<td style='align:right'>&nbsp;" + date + "</td>" +
                              "<td>" + e.sbmitm_TRXCD1 + "&nbsp;</td>" +
                              "<td style='align:right'>&nbsp;" + e.sbmitm_QTY + "</td>" +
                              "<td>" + e.sbmitm_UNCS + "&nbsp;</td>" +
                              "<td>" + e.sbmitm_GRSSWT + "&nbsp;</td>" +
                              "</tr>");
                }
                sb.Append("</table>");
            }
            return View("excel", sb);

        }

    }
}