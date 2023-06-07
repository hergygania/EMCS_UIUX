using App.Data.Domain;
using App.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;

namespace App.Web.Controllers.PartTracking
{
    public partial class PartTrackingController : App.Framework.Mvc.BaseController
    {
        public ActionResult PopupMilestone()
        {
            return PartialView("PopupMilestone");
        }

        public JsonResult GetFilterBy(int index)
        {

            if (index == 1)
            {
                var filterData = Service.Master.Hub.GetList();
                return Json(filterData, JsonRequestBehavior.AllowGet);
            }
            else
            {
                var filterData = Service.Master.Area.GetList();
                return Json(filterData, JsonRequestBehavior.AllowGet);
            }
        }

        public JsonResult GetCustomerList(string term, string prcuno)
        {
            var modelList = Service.PartTracking.V_GET_CUSTOMER.GetList(term, prcuno);
            return Json(new { modelList }, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetCustomerListFilter(string term, string cust_group)
        {
            var list = new List<Data.Domain.SAP.Customer>();

            if (!string.IsNullOrEmpty(term))
                list = Service.SAP.Customer.GetSelectCustomer(term, cust_group);

            //int totRec = 0;
            //int offset = pageSize * (page - 1);

            return Json(new { list }, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetMilestoneList()
        {
            var model = new V_POPUP_DETAIL();
            var param = Request["params"];
            if (!string.IsNullOrEmpty(param))
            {
                JavaScriptSerializer ser = new JavaScriptSerializer();
                model = ser.Deserialize<Data.Domain.V_POPUP_DETAIL>(param);
            }

            var modelList = Service.PartTracking.Milestone.GetMilestoneList(model);
            return Json(modelList, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetStore(string filter_type, int? id)
        {
            var filterData = Service.PartTracking.V_CUSTORDER_HEADER.GetStore(filter_type, id);
            return Json(filterData, JsonRequestBehavior.AllowGet);
        }

        //public JsonResult GetMaterialDescription()
        //{
        //    var data = Service.SAP.MaterialDescription.GetMaterialDescription();
        //    return Json(data, JsonRequestBehavior.AllowGet);
        //}

        public JsonResult GetMaterialDescription(string searchTerm, int pageSize, int page)
        {
            var list = new List<Data.Domain.SAP.MaterialDescription>();

            if (!string.IsNullOrEmpty(searchTerm))
                list = Service.SAP.MaterialDescription.GetSelectMaterialDescription(searchTerm, pageSize, page);

            int totRec = 0;
            int offset = pageSize * (page - 1);

            Select2PagedResult jsonResult = new Select2PagedResult();
            jsonResult.Results = new List<Select2Result>();

            foreach (Data.Domain.SAP.MaterialDescription a in list)
            {
                jsonResult.Results.Add(new Select2Result { id = a.MaterialType.ToString(), text = a.MaterialDesc.ToString() });
            }

            return new Select2().SelectToResult(jsonResult, pageSize, offset, totRec);
        }


        public ActionResult DetailSupplierPage()
        {
            this.PaginatorBoot.Remove("SessionTRNSupp");
            return DetailSupplierPageXt();
        }


        public ActionResult DetailSupplierPageXt()
        {
            Func<Data.Domain.V_POPUP_DETAIL, IList<Data.Domain.V_SUPPLIER_STATUS>> func = delegate(Data.Domain.V_POPUP_DETAIL model)
            {
                var param = Request["params"];
                if (!string.IsNullOrEmpty(param))
                {
                    JavaScriptSerializer ser = new JavaScriptSerializer();
                    model = ser.Deserialize<Data.Domain.V_POPUP_DETAIL>(param);
                }

                var list = Service.PartTracking.V_SUPPLIER_STATUS.GetDetailSupplierList(model);
                return list.ToList();
            };

            var paging = PaginatorBoot.Manage("SessionTRNSupp", func).Pagination.ToJsonResult();
            return Json(paging, JsonRequestBehavior.AllowGet);
        }


        public ActionResult DetailForwarderPage()
        {
            this.PaginatorBoot.Remove("SessionTRNDetFwd");
            return DetailForwarderPageXt();
        }


        public ActionResult DetailForwarderPageXt()
        {
            Func<Data.Domain.V_POPUP_DETAIL, IList<Data.Domain.V_FORWADER_STATUS>> func = delegate(Data.Domain.V_POPUP_DETAIL model)
            {
                var param = Request["params"];
                if (!string.IsNullOrEmpty(param))
                {
                    JavaScriptSerializer ser = new JavaScriptSerializer();
                    model = ser.Deserialize<Data.Domain.V_POPUP_DETAIL>(param);
                }

                var list = Service.PartTracking.V_FORWADER_STATUS.GetDetailForwarderList(model);
                return list.ToList();
            };

            var paging = PaginatorBoot.Manage("SessionTRNDetFwd", func).Pagination.ToJsonResult();
            return Json(paging, JsonRequestBehavior.AllowGet);
        }

        public ActionResult DetailTrakindoPage()
        {
            this.PaginatorBoot.Remove("SessionTRNDetTrak");
            return DetailTrakindoPageXt();
        }


        public ActionResult DetailTrakindoPageXt()
        {
            Func<Data.Domain.V_POPUP_DETAIL, IList<Data.Domain.V_TRAKINDO_STATUS>> func = delegate(Data.Domain.V_POPUP_DETAIL model)
            {
                var param = Request["params"];
                if (!string.IsNullOrEmpty(param))
                {
                    JavaScriptSerializer ser = new JavaScriptSerializer();
                    model = ser.Deserialize<Data.Domain.V_POPUP_DETAIL>(param);
                }

                var list = Service.PartTracking.V_TRAKINDO_STATUS.GetDetailTrakindoList(model);
                return list.ToList();
            };

            var paging = PaginatorBoot.Manage("SessionTRNDetTrak", func).Pagination.ToJsonResult();
            return Json(paging, JsonRequestBehavior.AllowGet);
        }

        [AllowAnonymous]
        [Route("detail-part-and-case-{guid}")]
        public ActionResult DetailPartAndCase(string guid)
        {
            string sxNo = Request["sxNo"];
            string partNo = Request["partNo"];
            string caseNo = Request["caseNo"];

            sxNo = (sxNo + "") == "-" || string.IsNullOrEmpty(sxNo) ? "" : sxNo.Trim();
            partNo = (partNo + "") == "-" || string.IsNullOrEmpty(partNo) ? "" : partNo.Trim();
            caseNo = (caseNo + "") == "-" || string.IsNullOrEmpty(caseNo) ? "" : caseNo.Trim();

            string searchby_ = string.IsNullOrEmpty(sxNo) ? "" : "Invoice No: " + sxNo;
            string searchby = searchby_ + (string.IsNullOrEmpty(partNo) ? "" : (string.IsNullOrEmpty(searchby_) ? "" : "  and  ") + "PartNumber: " + partNo);
            searchby = string.IsNullOrEmpty(caseNo) ? searchby : "Case No: " + caseNo;

            ViewBag.searchBy = searchby;
            ViewBag.searchNote = "";
            ViewBag.PartsOrderID = null;
            ViewBag.PartsNo = "";

            try
            {
                var list = Service.PartTracking.PartsOrder.GetAllList(sxNo, partNo, caseNo);

                if (list.partsOrder.Count > 0 && list.partsOrder.Where(w => w.InvoiceNo.Trim().Contains(sxNo)).Count() == 0
                && !string.IsNullOrEmpty(sxNo) && !string.IsNullOrEmpty(partNo))
                {
                    ViewBag.searchNote = "Related Invoice used by PartNumber: " + partNo;
                }

                if (!string.IsNullOrEmpty(sxNo) && list.partsOrder.Count > 0)
                    ViewBag.PartsOrderID = list.partsOrder[0].PartsOrderID;
                if (!string.IsNullOrEmpty(partNo) && list.partsOrderDetail.Count > 0)
                    ViewBag.PartsNo = list.partsOrderDetail[0].PartsNumber;

                return View("PartAndCase.view", list);
            }
            catch (Exception ex)
            {
                return Content(ex.Message);
            }
        }

        public JsonResult DetailPartSurvey(long? partsOrderId, string partsNo)
        {
            var list = Service.PartTracking.PartsOrder.GetSurveyList(partsOrderId, partsNo);

            return Json(list, JsonRequestBehavior.AllowGet);
        }

    }
}