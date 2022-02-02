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
using Newtonsoft.Json;
using App.Web.Models.DTS;
using System.Web.Script.Serialization;

namespace App.Web.Controllers.DTS
{
    public partial class DtsController
    {
        private ShipmentInbound InitilizeShipmentInbound(string itemid, string SN)
        {
            var item = new ShipmentInbound();

            if (itemid == "")
                return item;

            item = Service.DTS.ShipmentInbound.GetId(itemid, SN);
            return item;
        }

        // GET: Inbound
        [AuthorizeAcces(ActionType = AuthorizeAcces.IsRead)]
        public ActionResult Inbound()
        {
            ViewBag.AllowRead = AuthorizeAcces.AllowRead;
            ViewBag.AllowCreate = AuthorizeAcces.AllowCreated;
            ViewBag.AllowUpdate = AuthorizeAcces.AllowUpdated;
            ViewBag.AllowDelete = AuthorizeAcces.AllowDeleted;

            Session["AllowRead_Inbound"] = AuthorizeAcces.AllowRead;
            Session["AllowCreated_Inbound"] = AuthorizeAcces.AllowCreated;
            Session["AllowUpdated_Inbound"] = AuthorizeAcces.AllowUpdated;
            Session["AllowDeleted_Inbound"] = AuthorizeAcces.AllowDeleted;

            return View();
        }

        public ActionResult PartialListDetail(string InboundID, string InboundSN)
        {
            ViewBag.InboundID = InboundID;
            ViewBag.listDetail = Service.DTS.ShipmentInboundDetail.getDetailList(InboundID, InboundSN);
            return PartialView();
        }

        public ActionResult InboundManagementPage()
        {
            PaginatorBoot.Remove("SessionTRN");
            return InboundManagementPageXt();
        }

        public ActionResult InboundManagementPageXt()
        {
            Func<App.Data.Domain.DTS.InboundFilter, List<Data.Domain.ShipmentInbound>> func = delegate (App.Data.Domain.DTS.InboundFilter filter)
            {
                var param = Request["params"];
                if (!string.IsNullOrEmpty(param))
                {
                    JavaScriptSerializer ser = new JavaScriptSerializer();
                    filter = ser.Deserialize<App.Data.Domain.DTS.InboundFilter>(param);
                }

                var list = Service.DTS.ShipmentInbound.GetListFilter(filter);
                return list.ToList();
            };

            ActionResult paging = PaginatorBoot.Manage("SessionTRN", func).Pagination.ToJsonResult();
            return Json(paging, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetDataList()
        {
            var list = Demo.Data.Inbound.GetLis();

            return Json(list, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetDataDetailList(string PONo)
        {
            var list = Service.DTS.ShipmentInboundDetail.GetList(PONo);
            return Json(list, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult getDetailDataInbound()
        {
            var key = Request.Form["Key"];
            //var SN = Request.Form["SN"];
            var detail = Service.DTS.ShipmentInbound.GetDetail(key);
            var DetailLast = Service.DTS.ShipmentInbound.GetLastDetail(detail.PONo);

            var inboundDetail = new ShipmentInboundDetailSingle();
            inboundDetail.AJUNo = detail.AjuNo;
            inboundDetail.MSONo = detail.MSONo;
            inboundDetail.PONo = detail.PONo;
            inboundDetail.PODate = detail.PODate;
            inboundDetail.LoadingPort = detail.LoadingPort;
            inboundDetail.DischargePort = detail.DischargePort;
            inboundDetail.Status = detail.Status;
            inboundDetail.SerialNumber = detail.SerialNumber;
            inboundDetail.BatchNumber = detail.BatchNumber;
            inboundDetail.Model = detail.Model;
            inboundDetail.ModelDescription = detail.ModelDescription;
            inboundDetail.Position = detail.Position;
            inboundDetail.Notes = detail.Notes;
            inboundDetail.Remark = detail.Remark;
            inboundDetail.ATAPort = detail.ATAPort;
            inboundDetail.ETAPort = detail.ETAPort;
            inboundDetail.ATACakung = detail.ATACakung;
            inboundDetail.ETACakung = detail.ETACakung;
            inboundDetail.RTSActual = DetailLast.RTSActual;
            inboundDetail.RTSPlan = DetailLast.RTSPlan;
            inboundDetail.VesselActual = DetailLast.OnBoardVesselActual;
            inboundDetail.VesselPlan = DetailLast.OnBoardVesselPlan;
            inboundDetail.PortInPlan = DetailLast.PortInPlan;
            inboundDetail.PortInActual = DetailLast.PortInActual;
            inboundDetail.PortOutActual = DetailLast.PortOutActual;
            inboundDetail.PortOutPlan = DetailLast.PortOutPlan;
            inboundDetail.PLBInActual = DetailLast.PLBInActual;
            inboundDetail.PLBInPlan = DetailLast.PLBInPlan;
            inboundDetail.PLBOutActual = DetailLast.PLBOutActual;
            inboundDetail.PLBOutPlan = DetailLast.PLBOutPlan;
            inboundDetail.YardInActual = DetailLast.YardInActual;
            inboundDetail.YardInPlan = DetailLast.YardInPlan;
            inboundDetail.YardOutActual = DetailLast.YardOutActual;
            inboundDetail.YardOutPlan = DetailLast.YardOutPlan;
            inboundDetail.Plant = detail.Plant ?? "-";

            var HistoryList = Service.DTS.ShipmentInbound.ListHistory(detail.PONo);
            inboundDetail.DetailList = HistoryList.Select(a => new DetailList
            {
                PLBInActual = a.PLBInActual,
                PLBInPlan = a.PLBInPlan,
                PLBOutActual = a.PLBOutActual,
                PLBOutPlan = a.PLBOutPlan,
                RTSActual = a.RTSActual,
                RTSPlan = a.RTSPlan,
                PortInActual = a.PortInActual,
                PortInPlan = a.PortInPlan,
                PortOutActual = a.PortOutActual,
                PortOutPlan = a.PortOutPlan,
                YardInActual = a.YardInActual,
                YardInPlan = a.YardInPlan,
                YardOutActual = a.YardOutActual,
                YardOutPlan = a.YardOutPlan,
                VesselActual = a.OnBoardVesselActual,
                VesselPlan = a.OnBoardVesselPlan
            }).ToList();

            return Json(inboundDetail, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult InboundDelete()
        {
            try
            {
                var PO = Request.Form["PONo"].ToString();
                var SN = Request.Form["SerialnumberInbound"].ToString();
                ShipmentInbound item = Service.DTS.ShipmentInbound.GetId(PO,SN);
                Service.DTS.ShipmentInbound.crud(item, "D");
                return JsonCRUDMessage("D");
            }
            catch (Exception ex)
            {
                return Json(new { result = "failed", message = ex.Message }, JsonRequestBehavior.DenyGet);
            }
        }

        [HttpPost]
        public JsonResult submitRemarkInbound(ShipmentInbound formColl)
        {
            var item = Service.DTS.ShipmentInbound.GetId(formColl.PONo, formColl.SerialNumber);
            item.Remark = formColl.Remark.Trim();
            try
            {
                Service.DTS.ShipmentInbound.crud(item, "U");
                return JsonCRUDMessage("U");
            }
            catch (Exception ex)
            {
                return Json(new { result = "failed", message = ex.Message }, JsonRequestBehavior.DenyGet);
            }
        }

        [HttpPost]
        public JsonResult submitPositionInbound(ShipmentInbound formColl)
        {
            var item = Service.DTS.ShipmentInbound.GetId(formColl.PONo, formColl.SerialNumber);
            item.Position = formColl.Position;
            Service.DTS.ShipmentInbound.crud(item, "U");
            return JsonCRUDMessage("U");
        }

        public ActionResult DownloadInbound(App.Data.Domain.DTS.InboundFilter filter)
        {
            Guid guid = Guid.NewGuid();
            Helper.Service.DTS.DownloadInboundController data = new Helper.Service.DTS.DownloadInboundController();

            Session[guid.ToString()] = data.DownloadToExcel(filter);

            return Json(guid.ToString(), JsonRequestBehavior.AllowGet);
        }

        public FileResult DownloadToExcelInbound(string guid)
        {
            return Session[guid] as FileResult;
        }

    }
}