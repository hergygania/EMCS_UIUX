using App.Data.Domain;
using App.Domain;
using App.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;

namespace App.Web.Controllers.PartTracking
{
    public partial class PartTrackingController
    {
        private CustomerOrderViewModel InitilizeCustomerOrder()
        {
            var custOrder = new CustomerOrderViewModel();
            var currentUserCustGroup = "KALREZ";
            custOrder.CustomerList = Service.PartTracking.V_GET_CUSTOMER.GetList(string.Empty, currentUserCustGroup);            
            return custOrder;
        }


        public ActionResult CustomerOrder()
        {
            var orderData = InitilizeCustomerOrder();
            PaginatorBoot.Remove("SessionTRN");
            return View(orderData);
        }

        public ActionResult CustomerOrderPage(string search, string sort, string order, string limit, string offset)
        {
            this.PaginatorBoot.Remove("SessionTRN");
            return CustomerOrderPageXt(search, sort, order, limit, offset);
        }


        public ActionResult CustomerOrderPageXt(string search, string sort, string order, string limit, string offset)
        {
            Func<Data.Domain.V_CLIENTORDER_HEADER, IList<Data.Domain.V_CLIENTORDER_HEADER>> func = delegate(Data.Domain.V_CLIENTORDER_HEADER model)
            {
                var param = Request["params"];
                if (!string.IsNullOrEmpty(param))
                {
                    JavaScriptSerializer ser = new JavaScriptSerializer();
                    model = ser.Deserialize<Data.Domain.V_CLIENTORDER_HEADER>(param);
                }

                var list = Service.PartTracking.V_CLIENTORDER_HEADER.GetList(model);
                return list.ToList();
            };

            var paging = PaginatorBoot.Manage<Data.Domain.V_CLIENTORDER_HEADER, Data.Domain.V_CLIENTORDER_HEADER>("SessionTRN", func).Pagination.ToJsonResult();

            return Json(paging, JsonRequestBehavior.AllowGet);
        }

        public ActionResult CustomerOrderDetail(string id)
        {
            var model = new V_CUSTORDER_HEADER();
            if (!string.IsNullOrEmpty(id))
                model = Service.PartTracking.V_CUSTORDER_HEADER.GetOne(id);
            PaginatorBoot.Remove("SessionTRN");
            return View(model);
        }

        public ActionResult CustomerOrderDetailPage(string search, string sort, string order, string limit, string offset)
        {
            this.PaginatorBoot.Remove("SessionTRN");
            return CustomerOrderDetailPageXt(search, sort, order, limit, offset);
        }


        public ActionResult CustomerOrderDetailPageXt(string search, string sort, string order, string limit, string offset)
        {
            string ref_doc_no = string.Empty;
            Func<MasterSearchForm, IList<V_CUSTODER_DETAIL>> func = delegate(MasterSearchForm crit)
            {
                var param = Request["params"];
                if (!string.IsNullOrEmpty(param))
                {
                    JavaScriptSerializer ser = new JavaScriptSerializer();
                    ref_doc_no = ser.Deserialize<string>(param);
                }

                List<V_CUSTODER_DETAIL> list = Service.PartTracking.V_CUSTORDER_HEADER.GetDetailList(ref_doc_no);
                return list.ToList();
            };

            ActionResult paging = PaginatorBoot.Manage("SessionTRN", func).Pagination.ToJsonResult();
            return Json(paging, JsonRequestBehavior.AllowGet);
        }
    }
}