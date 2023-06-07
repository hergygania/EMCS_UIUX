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
        //[myAuthorize(Roles = "ReportSupply")]
        [AuthorizeAcces(ActionType = AuthorizeAcces.IsRead)]
        public ActionResult AckMessage()
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

        public ActionResult AckMessagePage()
        {
            PaginatorBoot.Remove("SessionTRN");
            return AckMessagePageXt();
        }

        public ActionResult AckMessagePageXt()
        {
            Func<ReportFilterView, List<RptAckMessage>> func =
                delegate(ReportFilterView crit)
            {
                string param = Request["params"];
                if (!string.IsNullOrEmpty(param))
                {
                    var ser = new JavaScriptSerializer();
                    crit = ser.Deserialize<ReportFilterView>(param);
                }

                var userId = User.Identity.GetUserId();
                List<RptAckMessage> list = AckMessages.
                    GetList(crit.GroupType, crit.FilterBy, crit.StoreNumber,userId);
                return list.OrderBy(a => a.ackm_CreatedOn).ThenBy(a => a.ackm_ID).ToList();
            };

            ActionResult paging = PaginatorBoot.Manage("SessionTRN", func).Pagination.ToJsonResult();
            return Json(paging, JsonRequestBehavior.AllowGet);
        }


        //public void ExportToExcelAckMessage
        //    (string groupType, string filterType,
        //    string storeNumber, DateTime? startDate,
        //    DateTime? endDate)
        //{
        //    List<RptAckMessage> list = AckMessages.GetList(groupType, filterType, storeNumber);
        //    var dataInventory = list.OrderBy(a => a.ackm_UpdatedBy).ThenBy(a => a.ackm_ID).ToList();
        //    var dt = DataTableHelper.ConvertTo(dataInventory);
        //    ExportToExcel(dt, "AckMessage");
        //}


        public ActionResult ExportToExcelAckMessage
            (string groupType, string filterBy,
            string storeNumber)
        {
            var userId = User.Identity.GetUserId();
            List<RptAckMessage> list = AckMessages.
                GetList(groupType, filterBy, storeNumber, userId);
            var listCurrent = list.OrderBy(a => a.ackm_UpdatedBy).ThenBy(a => a.ackm_ID).ToList();
           
            ViewBag.Title = "Ack Message";
            var sb = new System.Text.StringBuilder();
            sb.Append("<b>Ack Message</b>");
            if (listCurrent != null)
            {

                var no = 0;
                sb.Append("<table border=1  cellspacing='3'>");
                sb.Append("<tr>" +
                          "<th>No</th>" +
                          "<th>Plant</th>" +
                          "<th>Area</th>" +
                          "<th>Hub</th>" +
                          "<th>PO Number</th>" +
                          "<th>Description</th>" +
                          "<th>Parts Number</th>" +
                          "<th>SLoc</th>" +
                          "<th>Dealer Net</th>" +
                          "<th>Gross Weight</th>" +
                          "<th>Message</th>" +
                          "</tr>");


                foreach (var e in listCurrent.ToList())
                {

                    no++;
                    sb.Append("<tr>" +
                              "<td>" + no + "</td>" +
                              "<td>" + e.ackm_StoreNo + "&nbsp;</td>" +
                              "<td>" + e.ackm_AreaID + "&nbsp;</td>" +
                              "<td>" + e.ackm_HubID + "&nbsp;</td>" +
                              "<td>" + e.ackm_Document + "&nbsp;</td>" +
                              "<td>" + e.ackm_Description + "&nbsp;</td>" +
                              "<td>" + e.ackm_PartsNumber + "&nbsp;</td>" +
                              "<td>&nbsp;" + e.ackm_SOS + "&nbsp;</td>" +
                              "<td>" + e.ackm_UNCS + "&nbsp;</td>" +
                              "<td>" + e.ackm_GRSSWT + "&nbsp;</td>" +
                              "<td>" + e.ackm_Message + "&nbsp;</td>" +
                              "</tr>");
                }
                sb.Append("</table>");
            }
            return View("excel", sb);

        }
    }
}