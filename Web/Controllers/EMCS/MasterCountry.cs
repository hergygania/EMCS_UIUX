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
        public ActionResult CountryList()
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

        [AuthorizeAcces(ActionType = AuthorizeAcces.IsRead, UrlMenu = "CountryList")]
        public ActionResult CountryEmcsPage()
        {
            this.PaginatorBoot.Remove("SessionTRN");
            return CountryEmcsPageXt();
        }

        [AuthorizeAcces(ActionType = AuthorizeAcces.IsRead, UrlMenu = "CountryList")]
        public ActionResult CountryEmcsPageXt()
        {
            Func<MasterSearchForm, IList<MasterCountry>> func = delegate (MasterSearchForm crit)
            {
                List<MasterCountry> list = Service.EMCS.MasterCountry.GetList(crit);
                return list.ToList();
            };

            ActionResult paging = PaginatorBoot.Manage("SessionTRN", func).Pagination.ToJsonResult();
            return Json(paging, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult SetAsEmbargoCountry(MasterCountry item)
        {
            try
            {
                ViewBag.crudMode = "U";
                if (ModelState.IsValid)
                {
                    var existing = Service.EMCS.MasterCountry.GetCountry(item.Id);
                    existing.IsEmbargoCountry = item.IsEmbargoCountry;
                    Service.EMCS.MasterCountry.Crud(existing, ViewBag.crudMode);
                    return JsonCRUDMessage(ViewBag.crudMode);
                }

                return Json(new { status = true, msg = "" });
            }
            catch (Exception err)
            {
                return Json(new { status = false, msg = err.Message });
            }
        }

        public JsonResult GetCountryData(string key)
        {
            var item = Service.DTS.MasterUsers.GetListFilter(key);
            return Json(item, JsonRequestBehavior.AllowGet);
        }

        public JsonResult UpdateCountry(string key)
        {
            var item = Service.DTS.MasterCustomers.GetListFilter(key);
            return Json(item, JsonRequestBehavior.AllowGet);
        }
    }
}