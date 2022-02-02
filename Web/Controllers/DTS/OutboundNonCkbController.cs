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
        // GET: Outbound
        [AuthorizeAcces(ActionType = AuthorizeAcces.IsRead)]
        public ActionResult OutboundNonCkb()
        {
            ViewBag.AllowRead = AuthorizeAcces.AllowRead;
            ViewBag.AllowCreate = AuthorizeAcces.AllowCreated;
            ViewBag.AllowUpdate = AuthorizeAcces.AllowUpdated;
            ViewBag.AllowDelete = AuthorizeAcces.AllowDeleted;

            Session["AllowRead_OutboundNonCkb"] = AuthorizeAcces.AllowRead;
            Session["AllowCreated_OutboundNonCkb"] = AuthorizeAcces.AllowCreated;
            Session["AllowUpdated_OutboundNonCkb"] = AuthorizeAcces.AllowUpdated;
            Session["AllowDeleted_OutboundNonCkb"] = AuthorizeAcces.AllowDeleted;

            return View();
        }
        [AuthorizeAcces(ActionType = AuthorizeAcces.IsRead,UrlMenu = "OutboundNonCKB")]
        public ActionResult OutboundNonCkbManagementPage()
        {
            PaginatorBoot.Remove("SessionTRN");
            return OutboundNonCkbManagementPageXt();
        }
        [AuthorizeAcces(ActionType = AuthorizeAcces.IsRead, UrlMenu = "OutboundNonCKB")]
        public ActionResult OutboundNonCkbManagementPageXt()
        {
            Func<App.Data.Domain.DTS.OutboundFilter, List<Data.Domain.ShipmentOutbound>> func = delegate (App.Data.Domain.DTS.OutboundFilter filter)
            {
                var searchName = Request.Form["searchName"] ?? "";
                var list = Service.DTS.ShipmentOutbound.GetListFilterNonCkb(searchName);
                return list.ToList();
            };

            ActionResult paging = PaginatorBoot.Manage("SessionTRN", func).Pagination.ToJsonResult();
            return Json(paging, JsonRequestBehavior.AllowGet);
        }

    }
}