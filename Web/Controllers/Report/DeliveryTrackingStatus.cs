using App.Data.Domain;
using App.Service.Report;
using App.Web.App_Start;
using App.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using DotNet.Highcharts.Enums;
using DotNet.Highcharts.Helpers;
using DotNet.Highcharts.Options;
using Point = DotNet.Highcharts.Options.Point;
using DotNet.Highcharts;
using System.Drawing;
using HChart = App.Web.Helper.Extensions.HighchartsExtensions;
using MST_DTS = App.Service.DeliveryTrackingStatus.MasterDTS;

namespace App.Web.Controllers
{
    public partial class ReportController
    {
        // GET: DeliveryTrackingStatus
        [AuthorizeAcces(ActionType = AuthorizeAcces.IsRead)]
        public ActionResult DeliveryTrackingStatus()
        {
            PaginatorBoot.Remove("SessionTRN");
            
            //Set Base URL
            var appUrl = HttpRuntime.AppDomainAppVirtualPath;
            if (appUrl == "/") appUrl = "";
            var baseUrl = string.Format("{0}://{1}{2}", Request.Url.Scheme, Request.Url.Authority, appUrl);
            ViewBag.baseUrl = baseUrl + "/";

            var model = new ReportFilterDeliveryTrackingStatusView();
            var userName = Domain.SiteConfiguration.UserName;
            model.ETD = new DateTime(DateTime.Now.Year, 1, 1);
            model.ATD = DateTime.Now;
            model.ETA = new DateTime(DateTime.Now.Year, 1, 1);
            model.ATA = DateTime.Now;
            model.ModaList = MST_DTS.GetMasterModa();
            model.OriginList = MST_DTS.GetMasterCItys();
            model.DestinationList = MST_DTS.GetMasterCItys();
            model.StatusList = MST_DTS.GetMasterStatus();
            model.UnitTypeList = MST_DTS.GetListSelectUnitType();

            return View(model);
        }

        [HttpGet]
        public ActionResult _deliveryTrackingStatus(int Moda, string From, string To, int Status, int UnitType, string ETD, string ATD, string ETA, string ATA, string NODA)
        {
            Highcharts chart = HChart.GetHighcharts(Moda, From, To, Status, UnitType, ETD, ATD, ETA, ATA, NODA);

            return View(chart);
        }
    }
}