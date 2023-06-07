using App.Data.Caching;
using App.Data.Domain;
using App.Domain;
using App.Web.App_Start;
using App.Web.Models;
using System.Globalization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using Newtonsoft.Json;
using App.Web.Models.DTS;

namespace App.Web.Controllers.DTS
{
    public partial class DtsController
    {
        [AuthorizeAcces(ActionType = AuthorizeAcces.IsRead)]
        public ActionResult InboundNonCkb()
        {
            ViewBag.AllowRead = AuthorizeAcces.AllowRead;
            ViewBag.AllowCreate = AuthorizeAcces.AllowCreated;
            ViewBag.AllowUpdate = AuthorizeAcces.AllowUpdated;
            ViewBag.AllowDelete = AuthorizeAcces.AllowDeleted;

            Session["AllowRead_InboundNonCkb"] = AuthorizeAcces.AllowRead;
            Session["AllowCreated_InboundNonCkb"] = AuthorizeAcces.AllowCreated;
            Session["AllowUpdated_InboundNonCkb"] = AuthorizeAcces.AllowUpdated;
            Session["AllowDeleted_InboundNonCkb"] = AuthorizeAcces.AllowDeleted;

            return View();
        }
        [AuthorizeAcces(ActionType = AuthorizeAcces.IsRead,UrlMenu = "InboundNonCKB")]
        public ActionResult InboundNonCkbManagementPage()
        {
            PaginatorBoot.Remove("SessionTRN");
            return InboundNonCkbManagementPageXt();
        }
        [AuthorizeAcces(ActionType = AuthorizeAcces.IsRead, UrlMenu = "InboundNonCKB")]
        public ActionResult InboundNonCkbManagementPageXt()
        {
            Func<App.Data.Domain.DTS.InboundFilter, List<Data.Domain.ShipmentInbound>> func = delegate (App.Data.Domain.DTS.InboundFilter filter)
            {
                var searchName = Request.Form["searchName"] ?? "";
                var list = Service.DTS.ShipmentInbound.GetListFilterNonCkb(searchName);
                return list.ToList();
            };

            ActionResult paging = PaginatorBoot.Manage("SessionTRN", func).Pagination.ToJsonResult();
            return Json(paging, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        [AuthorizeAcces(ActionType = AuthorizeAcces.IsRead,UrlMenu = "Inbound")]
        public ActionResult FormInbound()
        {
            ViewBag.formType = "insert";
            var PONo = Request.QueryString["ID"];
            var SN = Request.QueryString["SN"];
            if (PONo != null)
            {
                ViewBag.formType = "edit";
                ViewBag.detail = Service.DTS.ShipmentInbound.GetId(PONo, SN);
            }
            return View();
        }

        [HttpPost]
        public ActionResult SubmitInbound(ShipmentInbound formColl)
        {
            string dataDetail = Request.Form["details"];
            string AjuNo = Request.Form["AjuNo"].ToString();
            string PONo = Request.Form["PONo"].ToString();
            string SN = Request.Form["SerialNumber"].ToString();
            //var inboundDetail = new ShipmentInboundDetail();
            string formType = Request.Form["formType"];

            if (formType == "edit")
            {
                var inbound = Service.DTS.ShipmentInbound.GetId(PONo,SN);
                inbound = formColl;
                if (inbound.ATAPort != null) inbound.ATAPort = ConvertDateData(Request.Form["ATAPort"]);
                if (inbound.ETAPort != null) inbound.ETAPort = ConvertDateData(Request.Form["ETAPort"]);
                if (inbound.ATACakung != null) inbound.ATACakung = ConvertDateData(Request.Form["ATACakung"]);
                if (inbound.ETACakung != null) inbound.ETACakung = ConvertDateData(Request.Form["ETACakung"]);
                Service.DTS.ShipmentInbound.crud(inbound, "U");
                var inboundDetail = Service.DTS.ShipmentInboundDetail.getDetailList(inbound.PONo, inbound.SerialNumber).FirstOrDefault();
                return JsonCRUDMessage("U");
            }
            else
            {
                if (formColl.ATAPort != null) formColl.ATAPort = ConvertDateData(Request.Form["ATAPort"]);
                if (formColl.ETAPort != null) formColl.ETAPort = ConvertDateData(Request.Form["ETAPort"]);
                if (formColl.ATACakung != null) formColl.ATACakung = ConvertDateData(Request.Form["ATACakung"]);
                if (formColl.ETACakung != null) formColl.ETACakung = ConvertDateData(Request.Form["ETACakung"]);

                formColl.IsCkb = false;

                Service.DTS.ShipmentInbound.crud(formColl, "I");

                var newDataDetail = JsonConvert.DeserializeObject<List<JsonDetail>>(dataDetail);
                var newDetail = new ShipmentInboundDetail();

                if (newDataDetail.Count() > 0)
                {
                    foreach (var itemDetail in newDataDetail)
                    {
                        newDetail.AjuNo = formColl.AjuNo;
                        newDetail.PONo = formColl.PONo;
                        //newDetail.PODate = formColl.PODate;
                        newDetail.MSONo = formColl.MSONo;
                        newDetail.ATACakung = formColl.ATACakung;
                        newDetail.ETACakung = formColl.ETACakung;
                        newDetail.ATAPort = formColl.ATAPort;
                        newDetail.ETAPort = formColl.ETAPort;
                        newDetail.SerialNumber = formColl.SerialNumber;

                        if (itemDetail.OnBoardVesselActual != "") newDetail.OnBoardVesselActual = ConvertDateData(itemDetail.OnBoardVesselActual);
                        if (itemDetail.OnBoardVesselPlan != "") newDetail.OnBoardVesselPlan = ConvertDateData(itemDetail.OnBoardVesselPlan);
                        if (itemDetail.PLBInActual != "") newDetail.PLBInActual = ConvertDateData(itemDetail.PLBInActual);
                        if (itemDetail.PLBInPlan != "") newDetail.PLBInPlan = ConvertDateData(itemDetail.PLBInPlan);
                        if (itemDetail.PLBOutActual != "") newDetail.PLBOutActual = ConvertDateData(itemDetail.PLBOutActual);
                        if (itemDetail.PLBOutPlan != "") newDetail.PLBOutPlan = ConvertDateData(itemDetail.PLBOutPlan);
                        if (itemDetail.RTSPlan != "") newDetail.RTSPlan = ConvertDateData(itemDetail.RTSPlan);
                        if (itemDetail.RTSActual != "") newDetail.RTSActual = ConvertDateData(itemDetail.RTSActual);
                        if (itemDetail.PortInActual != "") newDetail.PortInActual = ConvertDateData(itemDetail.PortInActual);
                        if (itemDetail.PortInPlan != "") newDetail.PortInPlan = ConvertDateData(itemDetail.PortInPlan);
                        if (itemDetail.PortOutPlan != "") newDetail.PortOutPlan = ConvertDateData(itemDetail.PortOutPlan);
                        if (itemDetail.PortOutActual != "") newDetail.PortOutActual = ConvertDateData(itemDetail.PortOutActual);
                        if (itemDetail.YardInPlan != "") newDetail.YardInPlan = ConvertDateData(itemDetail.YardInPlan);
                        if (itemDetail.YardInActual != "") newDetail.YardInActual = ConvertDateData(itemDetail.YardInActual);
                        if (itemDetail.YardInPlan != "") newDetail.YardOutPlan = ConvertDateData(itemDetail.YardInPlan);
                        if (itemDetail.YardOutActual != "") newDetail.YardOutActual = ConvertDateData(itemDetail.YardOutActual);
                        Service.DTS.ShipmentInboundDetail.crud(newDetail, "I");
                    }
                }

                return JsonCRUDMessage("I");
            }
        }

        public DateTime ConvertDateData(string dateData)
        {
            try
            {
                string newDateData = DateTime.ParseExact(dateData, "dd MMM yyyy", CultureInfo.InvariantCulture).ToString("MM/dd/yyyy", CultureInfo.InvariantCulture);
                return DateTime.ParseExact(newDateData, "MM/dd/yyyy", CultureInfo.InvariantCulture);
            }
            catch (Exception err)
            {
                throw err;
            }
        }

        public string ConvertDateTostring(DateTime? dateData)
        {
            var newDate = Convert.ToDateTime(dateData).ToString("dd MMM yyyy");
            return newDate;
        }

        [HttpPost]
        public JsonResult SubmitDetailInbound(ShipmentInboundDetail formColl)
        {
            var newDetail = new ShipmentInboundDetail();
            newDetail = formColl;
            Service.DTS.ShipmentInboundDetail.crud(newDetail, "I");
            return JsonCRUDMessage("I");
        }

        public ActionResult InboundDetailPage()
        {
            PaginatorBoot.Remove("SessionTRN");
            return InboundDetailPageXt();
        }

        public ActionResult InboundDetailPageXt()
        {
            Func<App.Data.Domain.DTS.InboundFilter, List<Data.Domain.ShipmentInboundDetail>> func = delegate (App.Data.Domain.DTS.InboundFilter filter)
            {
                var param = Request["params"];
                string PONo = "";
                string SN = "";
                if (!string.IsNullOrEmpty(param))
                {
                    JavaScriptSerializer ser = new JavaScriptSerializer();
                    filter = ser.Deserialize<App.Data.Domain.DTS.InboundFilter>(param);
                    PONo = filter.PoNumber;
                    SN = filter.SerialNumber;
                }

                var list = Service.DTS.ShipmentInboundDetail.getDetailList(PONo, SN);
                return list.ToList();
            };

            ActionResult paging = PaginatorBoot.Manage("SessionTRN", func).Pagination.ToJsonResult();
            return Json(paging, JsonRequestBehavior.AllowGet);
        }
    }
}