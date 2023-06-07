using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using System.Data.SqlClient;
using App.Data.Domain;
using System.Data.Entity;
using App.Domain;
using App.Web.Models;
using App.Web.App_Start;

namespace App.Web.Controllers.PartTracking
{

    public partial class PartTrackingController
	{
		private OrderThruCounterHeaderViewModel InitilizeOrderThruCounterHeader()
		{
			var orderHeader = new OrderThruCounterHeaderViewModel();
			orderHeader.HubList = Service.Master.Hub.GetList();
			orderHeader.AreaList = Service.Master.Area.GetList();
            orderHeader.StoreNumberList = new List<Store>();
			orderHeader.ModelList = new List<V_MODEL_LIST>();
			orderHeader.CustomerGroupList = new List<V_GET_CUSTOMER_GROUP>();
			orderHeader.CustomerList = new List<V_GET_CUSTOMER>();
			orderHeader.doc_date_s = DateTime.Now.ToString("dd MMM yyyy") + " - " + DateTime.Now.ToString("dd MMM yyyy");
			orderHeader.doc_date_start = DateTime.Now;
			orderHeader.doc_date_end = DateTime.Now;
			orderHeader.cust_po_date_s = DateTime.Now.ToString("dd MMM yyyy") + " - " + DateTime.Now.ToString("dd MMM yyyy");
			orderHeader.cust_po_date_start = DateTime.Now;
			orderHeader.cust_po_date_end = DateTime.Now;

			return orderHeader;
		}


        //[myAuthorize(Roles = "PartTracking")]
        [AuthorizeAcces(ActionType = AuthorizeAcces.IsRead)]
		public ActionResult OrderThruCounter()
		{
            ViewBag.AllowRead = AuthorizeAcces.AllowRead;
            ViewBag.AllowCreate = AuthorizeAcces.AllowCreated;
            ViewBag.AllowUpdate = AuthorizeAcces.AllowUpdated;
            ViewBag.AllowDelete = AuthorizeAcces.AllowDeleted;
			PaginatorBoot.Remove("SessionTRNThru");
			var orderData = InitilizeOrderThruCounterHeader();

			ViewBag.custGroupHeader = "";

			if(User.Identity.GetUserType().ToLower() == "ext-part")
			{
				var user = Service.Master.UserAcces.GetId(User.Identity.Name);
				orderData.cust_group_no = user.Cust_Group_No + "";
				ViewBag.custGroupHeader = user.Cust_Group_NoCap;
			}
			return View(orderData);
		}

		public ActionResult OrderThruCounterPage()
		{
			this.PaginatorBoot.Remove("SessionTRNThru");
			return OrderThruCounterPageXt();
		}

        public ActionResult OrderThruCounterPageXt()
		{
			Func<Data.Domain.V_CUSTORDER_HEADER, IList<Data.Domain.V_CUSTORDER_HEADER>> func = delegate(Data.Domain.V_CUSTORDER_HEADER model)
			{
				var param = Request["params"];
				if(!string.IsNullOrEmpty(param))
				{
					JavaScriptSerializer ser = new JavaScriptSerializer();
					model = ser.Deserialize<Data.Domain.V_CUSTORDER_HEADER>(param);
				}

				var list = Service.PartTracking.V_CUSTORDER_HEADER.GetList(model);
				return list.ToList();
			};

			var paging = PaginatorBoot.Manage("SessionTRNThru", func).Pagination.ToJsonResult();
			return Json(paging, JsonRequestBehavior.AllowGet);
		}

		public ActionResult OrderThruCounterDetail(
			string refdocno,
			string storeno,
			string custname,
			string custpono,
			string custpodate,
			string docdate,
			string wonumber,
			string storename,
			string needbydate,
			string commiteddate,
			string model,
			string serial,
			string doccurr,
			string docvalue,
			string docstatus)
		{

			PaginatorBoot.Remove("SessionTRNDet");
			DateTime? _date = null;

			var _model = new V_CUSTORDER_HEADER
			{
				ref_doc_no = (refdocno + "").Trim(),
				store_no = (storeno + "").Trim(),
				cust_name = (custname + "").Trim(),
				cust_po_no = (custpono + "").Trim(),
                cust_po_date = string.IsNullOrEmpty(custpodate) || custpodate == "undefined" ? _date : Convert.ToDateTime(custpodate),
                doc_date = Convert.ToDateTime(docdate),
				wo_number = (wonumber + "").Trim(),
				store_name = (storename + "").Trim(),
				need_by_date = string.IsNullOrEmpty(needbydate) || needbydate == "undefined" ? _date : Convert.ToDateTime(needbydate),
				commited_date = string.IsNullOrEmpty(commiteddate) || commiteddate == "undefined" ? _date : Convert.ToDateTime(commiteddate),
				model = (model + "").Trim(),
				serial = (serial + "").Trim(),
				doc_curr = (doccurr + "").Trim(),
				doc_value = Convert.ToDecimal(docvalue),
				doc_status = (docstatus + "").Trim()
			};

			//if(!string.IsNullOrEmpty(id)) _model = Service.PartTracking.V_CUSTORDER_HEADER.GetOne(id);
			return PartialView("OrderThruCounterDetail", _model);
		}

		public ActionResult OrderThruCounterDetailPage()
		{
			this.PaginatorBoot.Remove("SessionTRNDet");
			return OrderThruCounterDetailPageXt();
		}

       

		public ActionResult OrderThruCounterDetailPageXt()
		{
			string ref_doc_no = string.Empty;
			Func<MasterSearchForm, IList<V_CUSTODER_DETAIL>> func = delegate(MasterSearchForm crit)
			{
				var param = Request["params"];
				if(!string.IsNullOrEmpty(param))
				{
					JavaScriptSerializer ser = new JavaScriptSerializer();
					ref_doc_no = ser.Deserialize<string>(param);
				}

                var list = Service.PartTracking.V_CUSTORDER_HEADER.GetDetailList(ref_doc_no);
             
				
				Session["SessionTRNDet.Count"]=0;
				Session["SessionTRNDet.Ata"] = null;
				Session["SessionTRNDet.QtyOrd"] = null;
				Session["SessionTRNDet.QtyShi"] = null;
				Session["SessionTRNDet.weight"] = null;

                if (list.Count > 0)
                {
                    var lisSum = (from p in list
                                  group p by 1 into g
                                  select new
                                  {
                                      minAta = g.Min(m => m.ata.HasValue ? m.ata.Value.ToString("yyyyMMdd") : "19000101"),
                                      minEta = g.Min(m => m.eta.HasValue ? m.eta.Value : DateTime.Today),
                                      qtyOrder = g.Sum(m => m.qty_order),
                                      qtyShipped = g.Sum(m => m.qty_shipped),
                                      qtyBo = g.Sum(m => m.qty_bo),
                                      weight = g.Sum(m => m.weight)
                                  }).ToList();

                    if (lisSum.Count > 0)
                    {
                        //minAta = list.Min(m => m.ata.HasValue ? m.ata.Value.ToString("yyyyMMdd") : "19000101");
                        //var minEta = list.Min(m => m.eta.HasValue ? m.eta.Value : DateTime.Today);
                        var minAta = lisSum[0].minAta;
                        var minEta = lisSum[0].minEta;

                        if (minAta == "19000101" && minEta < DateTime.Today)
                            minAta = DateTime.Today.ToString("yyyyMMdd");

                        Session["SessionTRNDet.Count"] = list.Count;
                        Session["SessionTRNDet.Ata"] = minAta;
                        Session["SessionTRNDet.QtyOrd"] = lisSum[0].qtyOrder;
                        Session["SessionTRNDet.QtyShi"] = lisSum[0].qtyShipped;
                        Session["SessionTRNDet.weight"] = lisSum[0].weight;
                    }
                }
				
				return list.ToList();
			};

			ActionResult paging = PaginatorBoot.Manage("SessionTRNDet", func).Pagination.ToJsonResult();
			return Json(paging, JsonRequestBehavior.AllowGet);
		}

		public JsonResult GetOrderThruCounterDetailSum()
		{
			var dt = (Session["SessionTRNDet.Ata"] + "") == "19000101" ? null : Session["SessionTRNDet.Ata"];
			var _Ord = Convert.ToInt32(Session["SessionTRNDet.QtyOrd"]);
			var _Shi = Convert.ToInt32(Session["SessionTRNDet.QtyShi"]);
			var _wt = Convert.ToDecimal(Session["SessionTRNDet.weight"]);
			var _row = Convert.ToInt32(Session["SessionTRNDet.Count"]);

			return Json(new { ataDate = dt, qtyOrder = _Ord, qtyShipped = _Shi, weight=_wt,rows = _row }, JsonRequestBehavior.AllowGet);
		}

		public ActionResult OrderThruCounterPopupPartDetail()
		{
			return PartialView("OrderThruCounterPopupPartDetail");
		}

        [HttpPost]
        public JsonResult updateDocument(updDoc formColl)
        {
            try
            {
                var db = new Data.RepositoryFactory(new Data.EfDbContext());
                string query = "";

                var cekData = @"SELECT * FROM [dbo].[documentStatus] WHERE RFDCNO='" + formColl.docNumber + "'";
                var result  = db.DbContext.Database.SqlQuery<getCekDate>(cekData).Count();
                if(result == 0)
                {
                    query = @"INSERT INTO [dbo].[documentStatus] (RFDCNO, DOCUMENT_STATUS, LAST_UPDATE) VALUES ('"+formColl.docNumber+"','"+formColl.order_status+"','"+ DateTime.Now + "')";
                } else
                {
                    query = @"UPDATE [dbo].[documentStatus] SET DOCUMENT_STATUS = '"+formColl.order_status+ "', LAST_UPDATE = '" + DateTime.Now+"' WHERE RFDCNO = '"+formColl.docNumber+"'";
                }
                db.DbContext.Database.ExecuteSqlCommand(query);
                return Json(new { result = "success", msg = result, rfdcno = formColl.docNumber }, JsonRequestBehavior.DenyGet);
            }
            catch (Exception ex)
            {
                return Json(new { result = "failed", message = ex.Message }, JsonRequestBehavior.DenyGet);
            }
        }

        public JsonResult OrderThruCounterGetCustomerGroupList(string term)
		{
			var modelList = Service.PartTracking.V_GET_CUSTOMER_GROUP.GetList(term);
			return Json(new { modelList }, JsonRequestBehavior.AllowGet);
		}

        #region customer
        [AuthorizeAcces(ActionType = AuthorizeAcces.IsRead)]
        public ActionResult OrderThruCounters()
        {
            PaginatorBoot.Remove("SessionTRNThru");
            var orderData = InitilizeOrderThruCounterHeader();

            ViewBag.custGroupHeader = "";

            if (User.Identity.GetUserType().ToLower() == "ext-part")
            {
                var user = Service.Master.UserAcces.GetId(User.Identity.Name);
                orderData.cust_group_no = user.Cust_Group_No + "";
                ViewBag.custGroupHeader = user.Cust_Group_NoCap;
            }
            return View(orderData);
        }

        public ActionResult OrderThruCountersPage()
        {
            this.PaginatorBoot.Remove("SessionTRNThru");
            return OrderThruCountersPageXt();
        }

        public ActionResult OrderThruCountersPageXt()
        {
            Func<Data.Domain.V_CUSTORDER_HEADERS, IList<Data.Domain.V_CUSTORDER_HEADERS>> func = delegate (Data.Domain.V_CUSTORDER_HEADERS model)
            {
                var param = Request["params"];
                if (!string.IsNullOrEmpty(param))
                {
                    JavaScriptSerializer ser = new JavaScriptSerializer();
                    model = ser.Deserialize<Data.Domain.V_CUSTORDER_HEADERS>(param);
                }

                var list = Service.PartTracking.V_CUSTORDER_HEADERS.GetList(model);
                return list.ToList();
            };

            var paging = PaginatorBoot.Manage("SessionTRNThru", func).Pagination.ToJsonResult();
            return Json(paging, JsonRequestBehavior.AllowGet);
        }

        public ActionResult OrderThruCounterDetails(
            string refdocno,
            string storeno,
            string custname,
            string custpono,
//            string custpodate,
            string docdate,
            string wonumber,
            string storename,
            string needbydate,
            string commiteddate,
            string model,
            string serial,
            string doccurr,
            string docvalue,
            string docstatus)
        {

            PaginatorBoot.Remove("SessionTRNDet");
            DateTime? _date = null;

            var _model = new V_CUSTORDER_HEADER
            {
                ref_doc_no = (refdocno + "").Trim(),
                store_no = (storeno + "").Trim(),
                cust_name = (custname + "").Trim(),
                cust_po_no = (custpono + "").Trim(),
                //                cust_po_date = string.IsNullOrEmpty(custpodate) || custpodate == "undefined" ? _date : Convert.ToDateTime(custpodate),
                doc_date = Convert.ToDateTime(docdate),
                wo_number = (wonumber + "").Trim(),
                store_name = (storename + "").Trim(),
                need_by_date = string.IsNullOrEmpty(needbydate) || needbydate == "undefined" ? _date : Convert.ToDateTime(needbydate),
                commited_date = string.IsNullOrEmpty(commiteddate) || commiteddate == "undefined" ? _date : Convert.ToDateTime(commiteddate),
                model = (model + "").Trim(),
                serial = (serial + "").Trim(),
                doc_curr = (doccurr + "").Trim(),
                doc_value = Convert.ToDecimal(docvalue),
                doc_status = (docstatus + "").Trim()
            };

            //if(!string.IsNullOrEmpty(id)) _model = Service.PartTracking.V_CUSTORDER_HEADER.GetOne(id);
            return PartialView("OrderThruCounterDetails", _model);
            //            return PartialView("OrderThruCounterDetails");
        }

        public ActionResult OrderThruCounterDetailsPage()
        {
            this.PaginatorBoot.Remove("SessionTRNDet");
            return OrderThruCounterDetailsPageXt();
        }


        public ActionResult OrderThruCounterDetailsPageXt()
        {
            string ref_doc_no = string.Empty;
            Func<MasterSearchForm, IList<V_CUSTODER_DETAIL>> func = delegate (MasterSearchForm crit)
            {
                var param = Request["params"];
                if (!string.IsNullOrEmpty(param))
                {
                    JavaScriptSerializer ser = new JavaScriptSerializer();
                    ref_doc_no = ser.Deserialize<string>(param);
                }

                var list = Service.PartTracking.V_CUSTORDER_HEADER.GetDetailList(ref_doc_no);

                Session["SessionTRNDet.Count"] = 0;
                Session["SessionTRNDet.Ata"] = null;
                Session["SessionTRNDet.QtyOrd"] = null;
                Session["SessionTRNDet.QtyShi"] = null;
                Session["SessionTRNDet.weight"] = null;

                if (list.Count > 0)
                {
                    var lisSum = (from p in list
                                  group p by 1 into g
                                  select new
                                  {
                                      minAta = g.Min(m => m.ata.HasValue ? m.ata.Value.ToString("yyyyMMdd") : "19000101"),
                                      minEta = g.Min(m => m.eta.HasValue ? m.eta.Value : DateTime.Today),
                                      qtyOrder = g.Sum(m => m.qty_order),
                                      qtyShipped = g.Sum(m => m.qty_shipped),
                                      qtyBo = g.Sum(m => m.qty_bo),
                                      weight = g.Sum(m => m.weight)
                                  }).ToList();

                    if (lisSum.Count > 0)
                    {
                        //minAta = list.Min(m => m.ata.HasValue ? m.ata.Value.ToString("yyyyMMdd") : "19000101");
                        //var minEta = list.Min(m => m.eta.HasValue ? m.eta.Value : DateTime.Today);
                        var minAta = lisSum[0].minAta;
                        var minEta = lisSum[0].minEta;

                        if (minAta == "19000101" && minEta < DateTime.Today)
                            minAta = DateTime.Today.ToString("yyyyMMdd");

                        Session["SessionTRNDet.Count"] = list.Count;
                        Session["SessionTRNDet.Ata"] = minAta;
                        Session["SessionTRNDet.QtyOrd"] = lisSum[0].qtyOrder;
                        Session["SessionTRNDet.QtyShi"] = lisSum[0].qtyShipped;
                        Session["SessionTRNDet.weight"] = lisSum[0].weight;
                    }
                }

                return list.ToList();
            };

            ActionResult paging = PaginatorBoot.Manage("SessionTRNDet", func).Pagination.ToJsonResult();
            return Json(paging, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetOrderThruCounterDetailsSum()
        {
            var dt = (Session["SessionTRNDet.Ata"] + "") == "19000101" ? null : Session["SessionTRNDet.Ata"];
            var _Ord = Convert.ToInt32(Session["SessionTRNDet.QtyOrd"]);
            var _Shi = Convert.ToInt32(Session["SessionTRNDet.QtyShi"]);
            var _wt = Convert.ToDecimal(Session["SessionTRNDet.weight"]);
            var _row = Convert.ToInt32(Session["SessionTRNDet.Count"]);

            return Json(new { ataDate = dt, qtyOrder = _Ord, qtyShipped = _Shi, weight = _wt, rows = _row }, JsonRequestBehavior.AllowGet);
        }

        #endregion

        #region new
        [AuthorizeAcces(ActionType = AuthorizeAcces.IsRead)]
        public ActionResult OrderThruCounterNew()
        {
            ViewBag.AllowRead = AuthorizeAcces.AllowRead;
            ViewBag.AllowCreate = AuthorizeAcces.AllowCreated;
            ViewBag.AllowUpdate = AuthorizeAcces.AllowUpdated;
            ViewBag.AllowDelete = AuthorizeAcces.AllowDeleted;
            PaginatorBoot.Remove("SessionTRNThru");
            var orderData = InitilizeOrderThruCounterHeader();

            ViewBag.custGroupHeader = "";

            if (User.Identity.GetUserType().ToLower() == "ext-part")
            {
                var user = Service.Master.UserAcces.GetId(User.Identity.Name);
                orderData.cust_group_no = user.Cust_Group_No + "";
                ViewBag.custGroupHeader = user.Cust_Group_NoCap;
            }
            return View(orderData);
        }

        public ActionResult OrderThruCounterNewPage()
        {
            this.PaginatorBoot.Remove("SessionTRNThru");
            return OrderThruCounterNewPageXt();
        }

        public ActionResult OrderThruCounterNewPageXt()
        {
            Func<Data.Domain.V_CUSTORDER_HEADER_NEW, IList<Data.Domain.V_CUSTORDER_HEADER_NEW>> func = delegate (Data.Domain.V_CUSTORDER_HEADER_NEW model)
            {
                var param = Request["params"];
                if (!string.IsNullOrEmpty(param))
                {
                    JavaScriptSerializer ser = new JavaScriptSerializer();
                    model = ser.Deserialize<Data.Domain.V_CUSTORDER_HEADER_NEW>(param);
                }

                var list = Service.PartTracking.V_CUSTORDER_HEADER_NEW.GetList(model);
                return list.ToList();
            };

            var paging = PaginatorBoot.Manage("SessionTRNThru", func).Pagination.ToJsonResult();
            return Json(paging, JsonRequestBehavior.AllowGet);
        }

        #endregion
    }
}