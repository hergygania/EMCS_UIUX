using App.Web.App_Start;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace App.Web.Controllers.DTS
{
    public partial class DtsController
    {
        // GET: DailyReport
        [AuthorizeAcces(ActionType = AuthorizeAcces.IsRead)]
        public ActionResult DailyReport()
        {
            ViewBag.AllowRead = AuthorizeAcces.AllowRead;
            ViewBag.AllowCreate = AuthorizeAcces.AllowCreated;
            ViewBag.AllowUpdate = AuthorizeAcces.AllowUpdated;
            ViewBag.AllowDelete = AuthorizeAcces.AllowDeleted;

            Session["AllowRead_DailyReport"] = AuthorizeAcces.AllowRead;
            Session["AllowCreated_DailyReport"] = AuthorizeAcces.AllowCreated;
            Session["AllowUpdated_DailyReport"] = AuthorizeAcces.AllowUpdated;
            Session["AllowDeleted_DailyReport"] = AuthorizeAcces.AllowDeleted;
            return View();
        }
        [AuthorizeAcces(ActionType = AuthorizeAcces.IsRead,UrlMenu = "Inbound")]
        public ActionResult DailyReportInbound()
        {
            ViewBag.AllowRead = AuthorizeAcces.AllowRead;
            ViewBag.AllowCreate = AuthorizeAcces.AllowCreated;
            ViewBag.AllowUpdate = AuthorizeAcces.AllowUpdated;
            ViewBag.AllowDelete = AuthorizeAcces.AllowDeleted;

            Session["AllowRead_DailyReportInbound"] = AuthorizeAcces.AllowRead;
            Session["AllowCreated_DailyReportInbound"] = AuthorizeAcces.AllowCreated;
            Session["AllowUpdated_DailyReportInbound"] = AuthorizeAcces.AllowUpdated;
            Session["AllowDeleted_DailyReportInbound"] = AuthorizeAcces.AllowDeleted;
            return View();
        }
    }
}