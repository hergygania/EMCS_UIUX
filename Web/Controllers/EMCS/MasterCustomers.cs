using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using App.Data.Caching;
using App.Data.Domain;
using App.Domain;
using App.Web.App_Start;
using System.Globalization;
using Newtonsoft.Json;
using System.Web.Script.Serialization;
using System.Configuration;
using System.Net;
using App.Data.Domain.EMCS;

namespace App.Web.Controllers.EMCS
{
    public partial class EmcsController
    {
        // GET: DailyReport
        [AuthorizeAcces(ActionType = AuthorizeAcces.IsRead)]
        public ActionResult CustomerList()
        {
            ViewBag.IsAdminDTS = AuthorizeAcces.AllowCreated;
            ViewBag.AllowRead = AuthorizeAcces.AllowRead;
            ViewBag.AllowCreate = AuthorizeAcces.AllowCreated;
            ViewBag.AllowUpdate = AuthorizeAcces.AllowUpdated;
            ViewBag.AllowDelete = AuthorizeAcces.AllowDeleted;

            Session["AllowRead_DeliveryRequisition"] = AuthorizeAcces.AllowRead;
            Session["AllowCreated_DeliveryRequisition"] = AuthorizeAcces.AllowCreated;
            Session["AllowUpdated_DeliveryRequisition"] = AuthorizeAcces.AllowUpdated;
            Session["AllowDeleted_DeliveryRequisition"] = AuthorizeAcces.AllowDeleted;

            var userId = User.Identity.GetUserId();
            var detail = Service.DTS.DeliveryRequisition.GetDetailUser(userId);
            ViewBag.userFullName = detail.FullName;
            ViewBag.userPhone = detail.Phone;
            ViewBag.userID = userId;

            return View();
        }

        [AuthorizeAcces(ActionType = AuthorizeAcces.IsRead, UrlMenu = "CustomerList")]
        public ActionResult CustomerEmcsPage()
        {
            this.PaginatorBoot.Remove("SessionTRN");
            return CustomerEmcsPageXt();
        }

        [AuthorizeAcces(ActionType = AuthorizeAcces.IsRead, UrlMenu = "CustomerList")]
        public ActionResult CustomerEmcsPageXt()
        {
            Func<MasterSearchForm, IList<App.Data.Domain.EMCS.MasterCustomers>> func = delegate (MasterSearchForm crit)
            {
                List<App.Data.Domain.EMCS.MasterCustomers> list = Service.EMCS.MasterCustomer.GetList(crit);
                return list.ToList();
            };

            ActionResult paging = PaginatorBoot.Manage("SessionTRN", func).Pagination.ToJsonResult();
            return Json(paging, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetCustomerData(string key)
        {
            var item = Service.DTS.MasterUsers.GetListFilter(key);
            return Json(item, JsonRequestBehavior.AllowGet);
        }
    }
}