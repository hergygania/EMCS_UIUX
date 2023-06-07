using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using System.Web.WebPages;
using App.Web.App_Start;
using App.Web.Models;

namespace App.Web.Controllers.Vetting
{
	public partial class VettingController
	{
        private static string FormatDateStringTracking = "yyyy-MM-dd HH:mm:ss";
        private static string FormatDateStringSAP = "dd.MM.yyyy";

        #region PartsOrder paging
        [AuthorizeAcces(ActionType = AuthorizeAcces.IsRead, UrlMenu = "returnToVendor")]
        public ActionResult PartsOrderPage()

		{
			PaginatorBoot.Remove("SessionTRNDetail");
			return PartsOrderPageXt();
		}
        [AuthorizeAcces(ActionType = AuthorizeAcces.IsRead, UrlMenu = "returnToVendor")]
        public ActionResult PartsOrderPageXt()
		{
			Func<PartsOrderView, List<Data.Domain.PartsOrder>> func = delegate(PartsOrderView crit)
			{
				var param = Request["params"];
				if(!string.IsNullOrEmpty(param))
				{
					JavaScriptSerializer ser = new JavaScriptSerializer();
					crit = ser.Deserialize<PartsOrderView>(param);
				}

				int freightId = (crit.Freight + "").ToLower() == "air" ? 2 : 1;

				crit.JCode = crit.selJCode == null ? null : string.Join("|", crit.selJCode.ToArray());
				crit.AgreementType = crit.selAgreementType == null ? null : string.Join("|", crit.selAgreementType.ToArray());
				//crit.FreightShippId=null;

				var partList = Service.Vetting.PartsOrder.GetList(freightId,crit.FreightShippId, crit.VettingRoute,crit.ShipmentMode, crit.InvoiceNo, crit.DateSta, crit.DateFin, crit.JCode, crit.AgreementType, crit.StoreNumber, crit.DANumber);
				var list =
				(
					from c in partList
					from si in Service.Master.ShippingInstruction.GetList().Where(w => w.ShippingInstructionID == c.ShippingInstructionID).DefaultIfEmpty()
					select new { c, shipnm = si == null ? "" : si.Description }
				)
				.GroupBy(g => new { g.c.InvoiceNo, g.c.InvoiceDate, g.c.JCode })
				.Select(g => new Data.Domain.PartsOrder
				{
					InvoiceNo = g.Key.InvoiceNo,
					InvoiceDate = g.Key.InvoiceDate,
					JCode = g.Key.JCode,
					AgreementType = g.Max(m => m.c.AgreementType),
					StoreNumber = g.Max(m => m.c.StoreNumber),
					ShippingIDASN = g.Max(m => m.c.ShippingIDASN),
					ShippingInstruction = g.Max(m => m.shipnm),
					//DA = Service.SOVetting.CkbDeliveryStatus.VettingProcessGetNumberDa(g.Key.InvoiceNo),
                    DA = g.Max(m => m.c.DA),
                    TotalAmount = g.Max(m => m.c.TotalAmount),
					PartsOrderID = g.Max(m => m.c.PartsOrderID),
					ModifiedBy = g.Max(m => m.c.ModifiedBy),
					ModifiedDate = g.Max(m => m.c.ModifiedDate)
				}).ToList();

				return list.OrderByDescending(o => o.ModifiedDate).ThenBy(o => o.InvoiceNo).ToList();
			};

			var paging = PaginatorBoot.Manage("SessionTRNDetail", func).Pagination.ToJsonResult();
			return Json(paging, JsonRequestBehavior.AllowGet);
		}

        [AuthorizeAcces(ActionType = AuthorizeAcces.IsRead, UrlMenu = "quick")]
        public ActionResult PartsDetailHSPage()
		{
			PaginatorBoot.Remove("SessionTRNDetail");
			return PartsDetailHSPageXt();
		}

		public ActionResult PartsDetailHSPageXt()
		{
			Func<PartsOrderView, List<Data.Domain.PartsOrderDetail>> func = delegate(PartsOrderView crit)
			{
				var param = Request["params"];
				if(!string.IsNullOrEmpty(param))
				{
					JavaScriptSerializer ser = new JavaScriptSerializer();
					crit = ser.Deserialize<PartsOrderView>(param);
				}

				int freightId = (crit.Freight + "").ToLower() == "air" ? 2 : 1;

				//var partList = Service.Vetting.PartsOrderDetail.GetHsList(freightId, crit.VettingRoute, crit.CommodityID, crit.InvoiceNo, crit.DateSta, crit.DateFin, crit.PartsNumber);
				var partList = Service.Vetting.PartsOrderDetail.GetHsList(freightId, crit.VettingRoute, crit.CommodityID, crit.InvoiceNo, crit.DateSta, crit.DateFin, crit.PartsNumber, crit.CaseNo);
				var list = partList.OrderByDescending(o => o.PartsNumber).ThenBy(o => o.HSCode).ToList();
				//var list = partList
				//.GroupBy(g => new { g.InvoiceNo, g.InvoiceDate, g.PartsNumber })
				//.Select(g => new Data.Domain.PartsOrderDetail
				//{
				//	InvoiceNo = g.Key.InvoiceNo,
				//	InvoiceDate = g.Key.InvoiceDate,
				//	PartsNumber = g.Key.PartsNumber,
				//	PartsName = g.Max(m => m.PartsName),
				//	HSCode = g.Max(m => m.HSCode),
				//	HSDescription = g.Max(m => m.HSDescription),
				//	InvoiceItemQty = g.Max(m => m.InvoiceItemQty),
				//	PartGrossWeight = g.Max(m => m.PartGrossWeight),
				//	DetailID = g.Max(m => m.DetailID),
				//	ModifiedBy = g.Max(m => m.ModifiedBy),
				//	ModifiedDate = g.Max(m => m.ModifiedDate)
				//}).ToList();


				return list;//.OrderByDescending(o => o.PartsNumber).ThenBy(o => o.HSCode).ToList();
			};

			var paging = PaginatorBoot.Manage("SessionTRNDetail", func).Pagination.ToJsonResult();
			return Json(paging, JsonRequestBehavior.AllowGet);
		}

		#endregion

		#region parts case info paging
		public ActionResult PartsCasePage()
		{
			PaginatorBoot.Remove("SessionCaseInfo");
			return PartsCasePageXt();
		}

		public ActionResult PartsCasePageXt()
		{
			Func<PartsOrderView, List<Data.Domain.PartsOrderCase>> func = delegate(PartsOrderView crit)
			{
				var param = Request["params"];
				if(!string.IsNullOrEmpty(param))
				{
					JavaScriptSerializer ser = new JavaScriptSerializer();
					crit = ser.Deserialize<PartsOrderView>(param);
				}

				var list = Service.Vetting.PartsOrderCase.GetList(crit.PartsOrderID);
				return list.OrderBy(o => o.CaseNo).ToList();
			};

			var paging = PaginatorBoot.Manage("SessionCaseInfo", func).Pagination.ToJsonResult();
			return Json(paging, JsonRequestBehavior.AllowGet);
		}
		#endregion


		#region parts detail paging
		public ActionResult PartsDetailPage()
		{
			PaginatorBoot.Remove("SessionDetail");
			return PartsDetailPageXt();
		}

		public ActionResult PartsDetailPageXt()
		{
			Func<PartsOrderView, List<Data.Domain.PartsOrderDetail>> func = delegate(PartsOrderView crit)
			{
				var param = Request["params"];
				if(!string.IsNullOrEmpty(param))
				{
					JavaScriptSerializer ser = new JavaScriptSerializer();
					crit = ser.Deserialize<PartsOrderView>(param);
				}

				var list = Service.Vetting.PartsOrderDetail.GetList(crit.PartsOrderID, crit.ReturnToVendor, crit.VettingRoute);
				return list.OrderBy(o => o.CaseNo).ToList();
			};

			var paging = PaginatorBoot.Manage("SessionDetail", func).Pagination.ToJsonResult();

            return Json(paging, JsonRequestBehavior.AllowGet);
		}
		#endregion


		[HttpGet]
		public ActionResult PartsOrderListCheck()
		{
			var model = new PartsOrderView { VettingRoute = 1 };
			return PartialView("PartsOrder.listCheck", model);
		}

		[HttpGet]
		public ActionResult PartsOrderHSCheck()
		{
			var model = new PartsOrderView { VettingRoute = 2 };
			return PartialView("PartsOrder.listCheck.hs", model);
		}

		[HttpGet]
		public ActionResult PartsOrderEdit(int id)
		{
			ViewBag.crudMode = "U";
			var item = Service.Vetting.PartsOrder.GetId(id);

			if(item == null)
			{
				return HttpNotFound();
			}

			//load cache partorder mapping to minimize loading on modal
			//var _cache = new Data.Caching.MemoryCacheManager();
			//var list = _cache.Get<List<Data.Domain.PartsMapping>>("App.imex.PartsMapping");
			//if(list==null) 
			//	list = Service.Imex.PartsMapping.GetList();

			var emailList = Service.Master.EmailRecipients.GetList().Where(w => w.Purpose.ToLower() == "returntovendor").ToList();
			if(emailList.Count() > 0)
			{
				item.Email = emailList[0].EmailAddress;
			}

            item.DA = Service.SOVetting.CkbDeliveryStatus.VettingProcessGetNumberDa(item.InvoiceNo);


            return PartialView("PartsOrder.iud", item);
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		public ActionResult PartsOrderEdit(Data.Domain.PartsOrder item)
		{
			ViewBag.crudMode = "U";
			//item.Status = (byte)(item.Status ? 1 : 0);

			//if(ModelState.IsValid)
			//{
				var tbl = Service.Vetting.PartsOrder.GetId(item.PartsOrderID);
				tbl.IsHazardous=item.IsHazardous;
				Service.Vetting.PartsOrder.Update(tbl, ViewBag.crudMode);

				return JsonCRUDMessage(ViewBag.crudMode);
			//}
			//var nsg = Web.Helper.Error.ModelStateErrors(ModelState);
			//return Json(new { success = false, Msg = nsg });
		}

		[HttpPost]
		//[ValidateAntiForgeryToken]
		public ActionResult partsOrderOmEdit(Data.Domain.PartsOrderDetail item, string email)
		{
			ViewBag.crudMode = "U";
			if(!item.OMID.HasValue)
				return Json(new { success = false, Msg = "Om is required" });

			var tbl = Service.Vetting.PartsOrderDetail.GetId(item.DetailID);
			tbl.OMID= item.OMID;
			tbl.ReturnToVendor = item.ReturnToVendor;
			tbl.Remark = item.Remark;
			Service.Vetting.PartsOrderDetail.Update(tbl, ViewBag.crudMode);

			//send email
			if(item.ReturnToVendor == 1)
			{
				var dataDetail = new List<Data.Domain.PartsOrderDetail>();
				dataDetail.Add(tbl);
				if(!string.IsNullOrEmpty(tbl.PartsNumber))
				{
					var partlist = Service.Master.PartsLists.GetList().Where(w=>w.PartsNumber==tbl.PartsNumber).FirstOrDefault();
					tbl.PartsName = partlist == null ? "-" : partlist.PartsName;
				}
				PartsOrderEmail(email, dataDetail, "Return-To-Vendor (Partial)");
			}
			return JsonCRUDMessage(ViewBag.crudMode);
		}

		[HttpPost]
		public ActionResult partsOrderReturnToVendor(long id,string email, string remark)
		{
			Service.Vetting.PartsOrderDetail.UpdateReturnToVendor(id);

			var dataDetail = Service.Vetting.PartsOrderDetail.GetList(id,null,null);
			PartsOrderEmail(email, dataDetail, "Return-To-Vendor (All)");
			return JsonCRUDMessage("U");
		}
		[HttpPost]
		public ActionResult partsOrderReturnToVendorId(long id)
		{
			Service.Vetting.PartsOrderDetail.UpdateReturnToVendorId(id);
			return JsonCRUDMessage("U");
		}

		private int PartsOrderEmail(string emailTo,
			List<Data.Domain.PartsOrderDetail> dataDetail,
			string subject)
		{
			var emailCC = "";
			var strUrlAddress = Request.Url.Scheme + "://" + Request.Url.Authority.ToString() + "/";

			string tbl = "<style>table,th,td{border:1px solid black;border-collapse:collapse;} th,td{padding:5px;}</style>";
			tbl = tbl + "<table style='width:99%'><tr>" +
				 "<th>CaseNo</th>" +
				 "<th>PartsNumber</th>" +
				 "<th>PartsName</th>" +
				 "<th>COO</th>" +
				 "<th style='text-align:right'>Qty</th>" +
				 "<th style='text-align:right'>Gross WT</th>" +
				 "<th style='text-align:right'>UnitPrice</th>" +
				 "<th>Invoice No</th>" +
				 "<th style='white-space:nowrap;text-align:right'>Invoice Date</th>" +
				 "</tr>";
			foreach(var f in dataDetail)
			{
				tbl = tbl + "<tr>" +
					 "<td>" + f.CaseNo + "</td>" +
					 "<td>" + f.PartsNumber + "</td>" +
					 "<td>" + f.PartsName + "</td>" +
					 "<td>" + f.COODescription + "</td>" +
					 "<td style='text-align:right'>" + f.InvoiceItemQty + "</td>" +
					 "<td style='text-align:right'>" + f.PartGrossWeight + "</td>" +
					 "<td style='text-align:right'>" + f.UnitPrice + "</td>" +
					 "<td>" + f.InvoiceNo + "</td>" +
					 "<td style='text-align:right'>" + f.InvoiceDate.ToString("dd MMM yyyy") + "</td>" +
					 "</tr>";
			}
			tbl = tbl + "</table>";

			string emailBody = "Dear Sir,<br><br>" +
				"For your information, please be verified 'Return-To-Vendor' below:<br/> " +
				//"<br/>BL/AWB : <b>" + item.BLAWB + "</b>" +
				//(item.ATD.HasValue ? "<br/>ATD Date : " + item.ATD.Value.ToString("dd MMMM yyyy") : "") +
				"<br/><br/>" +
				tbl +
				"<br/><br/><br/>For more detail, click here " + strUrlAddress + " enter to the application.<br/><br/>Has been submitted at <b>" + DateTime.Now.ToString("dd-MMM-yyyy HH:mm") + "</b> by: pis";

			var _email = new Data.Domain.Email
			{
				EmailDate = DateTime.Today,
				EmailFrom = "Pis",
				EmailFromAddress = strUrlAddress,
				EmailSubject = subject,
				EmailBody = emailBody,
				EmailTo = emailTo,
				Status = "OUTBOX"
			};

			var documents = new List<Data.Domain.EmailAttachment>();

			try
			{
				App.Service.Master.Emails.Update(_email, documents, "I");
			}
			catch { }

			try
			{
				Framework.Email.SendAsync(subject, emailTo, emailCC, emailBody);
			}
			catch { }

			return 1;
		}
        //[AuthorizeAcces(ActionType = AuthorizeAcces.IsRead)]
        public JsonResult DownloadPartsOrderToExcel(string freight, int vettingRoute)
        {
            Guid guid = Guid.NewGuid();
            Helper.Service.DownloadPartsOrder data = new Helper.Service.DownloadPartsOrder();

            Session[guid.ToString()] = data.DownloadToExcel(freight, vettingRoute);

            return Json(guid.ToString(), JsonRequestBehavior.AllowGet);
        }

        public ActionResult ShipmentStatusPage()
        {
            PaginatorBoot.Remove("SessionTRNShipmentDetail");
            return ShipmentStatusPageXt();
        }

        public ActionResult ShipmentStatusPageXt()
        {
            Func<PartsOrderView, List<Data.Domain.SOVetting.CKBDeliveryStatusTrackWithDaString>> func = delegate (PartsOrderView crit)
            {
                List<Data.Domain.SOVetting.CKBDeliveryStatusTrackWithDaString> tmpDataTracking = new List<Data.Domain.SOVetting.CKBDeliveryStatusTrackWithDaString>();
                var param = Request["params"];
                if (!string.IsNullOrEmpty(param))
                {
                    JavaScriptSerializer ser = new JavaScriptSerializer();
                    crit = ser.Deserialize<PartsOrderView>(param);
                }

                int freightId = (crit.Freight + "").ToLower() == "air" ? 2 : 1;
                var partList = Service.Vetting.PartsOrderDetail.GetHsList(freightId, crit.VettingRoute, crit.CommodityID, crit.InvoiceNo, crit.DateSta, crit.DateFin, crit.PartsNumber, crit.CaseNo);
                var list = partList.OrderByDescending(o => o.PartsNumber).ThenBy(o => o.HSCode).ToList();


                if (!crit.DANumber.IsEmpty())
                {
                    var daList = crit.DANumber.Split(new[] { ',', ' ' }, StringSplitOptions.RemoveEmptyEntries);
                    foreach (var da in daList)
                    {
                        
                        Data.Domain.SOVetting.CKBDeliveryStatus dataCkb = Service.SOVetting.CkbDeliveryStatus.GetLastTracking(da);
                        if (dataCkb == null) continue;
                        var tempCkbDa = new Data.Domain.SOVetting.CKBDeliveryStatusTrackWithDaString();
                        tempCkbDa.origin = dataCkb.origin;
                        tempCkbDa.destination = dataCkb.destination;
                        tempCkbDa.customer = dataCkb.customer;
                        tempCkbDa.receiver = dataCkb.receiver;
                        tempCkbDa.no_sequence = dataCkb.no_sequence;
                        tempCkbDa.tracking_station = dataCkb.tracking_station;
                        tempCkbDa.tracking_status_id = dataCkb.tracking_status_id;
                        tempCkbDa.tracking_date = (dataCkb.tracking_date.HasValue) ? dataCkb.tracking_date.Value.ToString(FormatDateStringTracking) : "";
                        tempCkbDa.tracking_status_desc = dataCkb.tracking_status_desc;
                        tempCkbDa.city = dataCkb.city;
                        tempCkbDa.datetime_updated = (dataCkb.tracking_date.HasValue) ? dataCkb.tracking_date.Value.ToString(FormatDateStringTracking) : "";
                        tempCkbDa.dano = da;
                        tmpDataTracking.Add(tempCkbDa);
                    }

                }
                else
                {
                    var daList = Service.SOVetting.CkbDeliveryStatus.VettingProcessGetNumberDa(crit.InvoiceNo).Split(new[] { ',', ' ' }, StringSplitOptions.RemoveEmptyEntries);
                    foreach (var da in daList)
                    {
                        Data.Domain.SOVetting.CKBDeliveryStatus dataCkb = Service.SOVetting.CkbDeliveryStatus.GetLastTracking(da);
                        if (dataCkb == null) continue;
                        var tempCkbDa = new Data.Domain.SOVetting.CKBDeliveryStatusTrackWithDaString();
                         tempCkbDa.origin = dataCkb.origin;
                        tempCkbDa.destination = dataCkb.destination;
                        tempCkbDa.customer = dataCkb.customer;
                        tempCkbDa.receiver = dataCkb.receiver;
                        tempCkbDa.no_sequence = dataCkb.no_sequence;
                        tempCkbDa.tracking_station = dataCkb.tracking_station;
                        tempCkbDa.tracking_status_id = dataCkb.tracking_status_id;
                        tempCkbDa.tracking_date = (dataCkb.tracking_date.HasValue) ? dataCkb.tracking_date.Value.ToString(FormatDateStringTracking) : "";
                        tempCkbDa.tracking_status_desc = dataCkb.tracking_status_desc;
                        tempCkbDa.city = dataCkb.city;
                        tempCkbDa.datetime_updated = (dataCkb.tracking_date.HasValue) ? dataCkb.tracking_date.Value.ToString(FormatDateStringTracking) : "";
                        tempCkbDa.dano = da;
                        tmpDataTracking.Add(tempCkbDa);
                    }
                }

                return tmpDataTracking;

            };

            var paging = PaginatorBoot.Manage("SessionTRNShipmentDetail", func).Pagination.ToJsonResult();
            return Json(paging, JsonRequestBehavior.AllowGet);
        }

    }
}