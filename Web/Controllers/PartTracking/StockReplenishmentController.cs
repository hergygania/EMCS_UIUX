using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using App.Data.Domain;
using App.Framework.Mvc;
using App.Web.Models;
using App.Web.App_Start;

namespace App.Web.Controllers.PartTracking
{
	public partial class PartTrackingController
	{
		private StockReplenishmentHeaderViewModel InitilizeStockReplenishmentHeader()
		{
			var model = new StockReplenishmentHeaderViewModel();
			model.HubList = Service.Master.Hub.GetList();
			model.AreaList = Service.Master.Area.GetList();
			model.StoreNumberList = new List<Store>();
			model.OrderProfileList = new List<OrderProfile>();
			model.doc_date_s = DateTime.Now.ToString("dd MMM yyyy") + " - " + DateTime.Now.ToString("dd MMM yyyy");
			model.doc_date_start = DateTime.Now;
			model.doc_date_end = DateTime.Now;
			return model;
		}

        //[myAuthorize(Roles = "PartTracking")]
        [AuthorizeAcces(ActionType = AuthorizeAcces.IsRead)]
		public ActionResult StockReplenishment()
		{
            ViewBag.AllowRead = AuthorizeAcces.AllowRead;
            ViewBag.AllowCreate = AuthorizeAcces.AllowCreated;
            ViewBag.AllowUpdate = AuthorizeAcces.AllowUpdated;
            ViewBag.AllowDelete = AuthorizeAcces.AllowDeleted;
			var model = InitilizeStockReplenishmentHeader();
			PaginatorBoot.Remove("SessionTRNStock");
			return View(model);
		}

		public ActionResult StockReplenishmentPage()
		{
			this.PaginatorBoot.Remove("SessionTRNStock");
			return StockReplenishmentPageXt();
		}


		public ActionResult StockReplenishmentPageXt()
		{
			Func<Data.Domain.V_STOCKORDER_HEADER, IList<Data.Domain.V_STOCKORDER_HEADER>> func = delegate(Data.Domain.V_STOCKORDER_HEADER model)
			{
				var param = Request["params"];
				if(!string.IsNullOrEmpty(param))
				{
					JavaScriptSerializer ser = new JavaScriptSerializer();
					model = ser.Deserialize<Data.Domain.V_STOCKORDER_HEADER>(param);
				}

				var list = Service.PartTracking.V_STOCKORDER_HEADER.GetList(model);
				return list.ToList();
			};

			var paging = PaginatorBoot.Manage("SessionTRNStock", func).Pagination.ToJsonResult();
			return Json(paging, JsonRequestBehavior.AllowGet);
		}

		[HttpGet]
		public ActionResult StockReplenishmentDetail(string rporne, string ordsos, string stno, string orderNo, string invNo, string docDate, string invDate, string recvDate, string docSts, string trackingid, string podDt, string pupDt)
		{
			//'&invNo=' + invno + '&docDate=' + docDate + '&invDate=' + invDate + '&recvDate=' + recvDate + '&docSts=' + row.doc_status);
			PaginatorBoot.Remove("SessionTRNDet");
			DateTime ? _date=null;

			var model = new Data.Domain.V_STOCKORDER_HEADER
			{
				RPORNE = rporne,
				ORDSOS = ordsos,
                store_no = stno,
                order_number = orderNo,
				supply_docinv = invNo == "null" ? "" : invNo,
				doc_date = string.IsNullOrEmpty(docDate) ? _date : Convert.ToDateTime(docDate),
				supply_docinv_date = string.IsNullOrEmpty(invDate) || invDate == "undefined" ? _date : Convert.ToDateTime(invDate),
				receiveDate = string.IsNullOrEmpty(recvDate) || recvDate == "undefined" ? _date : Convert.ToDateTime(recvDate),
				doc_status = docSts,
				tracking_id = trackingid == "null" ? "" : trackingid,
				podDate= string.IsNullOrEmpty(podDt) || podDt == "undefined" ? _date : Convert.ToDateTime(podDt),
				pupDate = string.IsNullOrEmpty(pupDt) || podDt == "undefined" ? _date : Convert.ToDateTime(pupDt)
			};
			//if(!string.IsNullOrEmpty(rporne)) model = Service.PartTracking.V_STOCKORDER_HEADER.GetOne(rporne, ordsos, stno, orderNo);
            //model.store_name = Service.Master.Stores.GetNo(stno).StoreNameCap;
            model.store_name = Service.Master.Stores.GetStoreByPlant(stno).StoreNameCap;
			return PartialView("StockReplenishmentDetail", model);
		}

		public ActionResult StockReplenishmentDetailPage()
		{
			this.PaginatorBoot.Remove("SessionTRNDet");
			return StockReplenishmentDetailPageXt();
		}

		public ActionResult StockReplenishmentDetailPageXt()
		{
			Func<Data.Domain.V_STOCKORDER_HEADER, IList<Data.Domain.V_STOCKODER_DETAIL>> func = delegate(Data.Domain.V_STOCKORDER_HEADER model)
			{
				var param = Request["params"];
				if(!string.IsNullOrEmpty(param))
				{
					JavaScriptSerializer ser = new JavaScriptSerializer();
					model = ser.Deserialize<Data.Domain.V_STOCKORDER_HEADER>(param);
				}
				var list = Service.PartTracking.V_STOCKORDER_HEADER.GetDetailList(model);

				Session["SessionTRNDet.Count"] = 0;
				Session["SessionTRNDet.qty"] = 0;
				Session["SessionTRNDet.Weight"] = 0;
				if(list.Count > 0)
				{
					var lisSum = (from p in list
												group p by 1 into g
												select new
												{
													qty = g.Sum(m => Convert.ToInt32(m.qty)),
													weight = g.Sum(m => Convert.ToDecimal(m.weight))
												}).ToList();

					if(lisSum.Count > 0)
					{
						Session["SessionTRNDet.Count"] = list.Count;
						Session["SessionTRNDet.Qty"] = lisSum[0].qty;
						Session["SessionTRNDet.Weight"] = lisSum[0].weight;
					}
				}

				return list.ToList();
			};

			var paging = PaginatorBoot.Manage("SessionTRNDet", func).Pagination.ToJsonResult();
			return Json(paging, JsonRequestBehavior.AllowGet);

		}

		[AcceptVerbs(HttpVerbs.Get)]
		public ActionResult StockReplenishmentPopupPartDetail(string partNumber, string caseNo, string jcode, string sos, string invoiceNo, string invoiceDate, string docNumber, string source, string partDesc)
		{
			var model = new V_STOCKODER_DETAIL();
			model.part_no = partNumber;
			model.part_desc = partDesc;
			model.source = source;
			model.case_number = caseNo;
			model.JCode = jcode;
			model.sos = sos;
			model.invoice_no = invoiceNo;
			if(invoiceDate != null && invoiceDate != "null")
				model.invoice_date = Convert.ToDateTime(invoiceDate);
			model.doc_number = docNumber;
			return PartialView("StockReplenishmentPopupPartDetail", model);
		}

		public JsonResult GetStockReplenishmentSum()
		{
			var _qty = Convert.ToInt32(Session["SessionTRNDet.Qty"]);
			var _wt = Convert.ToDecimal(Session["SessionTRNDet.Weight"]);
			var _rw = Convert.ToInt32(Session["SessionTRNDet.Count"]);

			return Json(new { qty = _qty, weight = _wt, rows = _rw }, JsonRequestBehavior.AllowGet);
		}

		public JsonResult StockReplenishmentGetOrderProfile(string orderClass)
		{
			var modelList = Service.Master.OrderProfile.GetList(orderClass);
			return Json(modelList, JsonRequestBehavior.AllowGet);
		}
	}
}