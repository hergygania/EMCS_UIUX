using App.Data.Domain.DTS;
using App.Web.App_Start;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Dynamic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;
namespace App.Web.Controllers.DTS
{
    public partial class DtsController
    {
        // GET: InboundEviz
        [AuthorizeAcces(ActionType = AuthorizeAcces.IsRead, UrlMenu = "Freight")]
        public ActionResult InboundEviz()
        {
            //ViewBag.AllowRead = AuthorizeAcces.AllowRead;
            //ViewBag.AllowCreate = AuthorizeAcces.AllowCreated;
            //ViewBag.AllowUpdate = AuthorizeAcces.AllowUpdated;
            //ViewBag.AllowDelete = AuthorizeAcces.AllowDeleted;

            //Session["AllowRead_OutboundNonCkb"] = AuthorizeAcces.AllowRead;
            //Session["AllowCreated_OutboundNonCkb"] = AuthorizeAcces.AllowCreated;
            //Session["AllowUpdated_OutboundNonCkb"] = AuthorizeAcces.AllowUpdated;
            //Session["AllowDeleted_OutboundNonCkb"] = AuthorizeAcces.AllowDeleted;
            return View();
        }

        [AuthorizeAcces(ActionType = AuthorizeAcces.IsRead, UrlMenu = "Freight")]
        public ActionResult InboundEvizPage()
        {
            PaginatorBoot.Remove("SessionTRN");
            return InboundEvizPageXt();
        }

        [AuthorizeAcces(ActionType = AuthorizeAcces.IsRead, UrlMenu = "Freight")]
        public ActionResult InboundEvizPageXt()
        {
            Func<App.Data.Domain.DTS.InboundEvizFilter, List<Data.Domain.InboundEviz>> func = delegate (App.Data.Domain.DTS.InboundEvizFilter filter)
            {

                var param = Request["params"];
                if (!string.IsNullOrEmpty(param))
                {
                    JavaScriptSerializer ser = new JavaScriptSerializer();
                    filter = ser.Deserialize<App.Data.Domain.DTS.InboundEvizFilter>(param);
                }
                var list = Service.DTS.InboundEviz.GetListFilterNonCkb(filter);
                return list.ToList();
            };

            ActionResult paging = PaginatorBoot.Manage("SessionTRN", func).Pagination.ToJsonResult();
            return Json(paging, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult GetSalesModel()
        {
            var key = Request.Form["key"] ?? "";
            var type = Request.Form["type"] ?? "";
            var listData = Service.DTS.InboundEviz.GetSalesModel(type, key);
            var data = listData.ToList();
            return Json(data, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult GetSerialNumber()
        {
            var key = Request.Form["key"] ?? "";
            var type = Request.Form["type"] ?? "";
            var listData = Service.DTS.InboundEviz.GetSerialNumber(type, key);
            var data = listData.ToList();
            return Json(data, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult GetShipSource()
        {
            var key = Request.Form["key"] ?? "";
            var type = Request.Form["type"] ?? "";
            var listData = Service.DTS.InboundEviz.GetShipSource(type, key);
            var data = listData.ToList();
            return Json(data, JsonRequestBehavior.AllowGet);
        }
    }
}