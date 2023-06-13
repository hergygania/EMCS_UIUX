using App.Data.Domain.EMCS;
using App.Domain;
using App.Web.App_Start;
using App.Web.Models.EMCS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace App.Web.Controllers.EMCS
{
    public partial class EmcsController
    {

        [AuthorizeAcces(ActionType = AuthorizeAcces.IsRead, UrlMenu = "Vendor")]
        public ActionResult Vendor()
        {
            ViewBag.AllowRead = AuthorizeAcces.AllowRead;
            ViewBag.AllowCreate = AuthorizeAcces.AllowCreated;
            ViewBag.AllowUpdate = AuthorizeAcces.AllowUpdated;
            ViewBag.AllowDelete = AuthorizeAcces.AllowDeleted;
            PaginatorBoot.Remove("SessionTRN");
            return View();
        }

        [HttpGet]
        public ActionResult VendorCreate()
        {
            try
            {
                ViewBag.crudMode = "I";
                return PartialView("ModelFormVendor");

            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        [HttpPost]
        public ActionResult VendorCreate(Vendor objmodel)
        {
            try
            {
                var data = Service.EMCS.MasterVendorService.InsertVendor(objmodel);
                ViewBag.crudMode = "I";

                return JsonCRUDMessage(ViewBag.crudMode);
                //return Json(new { success = false });

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [AuthorizeAcces(ActionType = AuthorizeAcces.IsRead, UrlMenu = "Vendor")]
        public ActionResult VendorPage()
        {
            PaginatorBoot.Remove("SessionTRN");
            return VendorPageXt();
        }

        [AuthorizeAcces(ActionType = AuthorizeAcces.IsRead, UrlMenu = "Vendor")]
        public ActionResult VendorPageXt()
        {
            Func<MasterSearchForm, IList<Vendor>> func = delegate (MasterSearchForm crit)
            {
                List<Vendor> list = Service.EMCS.MasterVendorService.GetVendorList(crit);
                   
                return list.OrderBy(o => o.Id).ToList();
            };

            ActionResult paging = PaginatorBoot.Manage("SessionTRN", func).Pagination.ToJsonResult();
            return Json(paging, JsonRequestBehavior.AllowGet);

        }
        [AuthorizeAcces(ActionType = AuthorizeAcces.IsUpdated, UrlMenu = "Vendor")]
        [HttpGet]
        public ActionResult VendorEdit(long Id)
        {
            ViewBag.crudMode = "U";
            var data = Service.EMCS.MasterVendorService.GetDataById(Id);
            return PartialView("ModelFormVendor", data);
        }
        [AuthorizeAcces(ActionType = AuthorizeAcces.IsCreated, UrlMenu = "Vendor")]
        [HttpPost, ValidateInput(false)]
        [ValidateAntiForgeryToken]
        public ActionResult VendorEdit(Vendor objModel)
        {
            ViewBag.crudMode = "U";
            var data = Service.EMCS.MasterVendorService.InsertVendor(objModel);
            return JsonCRUDMessage(ViewBag.crudMode);
            //return Json(new { success = false });
        }
        public ActionResult VendorDelete(long Id)
        {
            ViewBag.crudMode = "D";
            Service.EMCS.MasterVendorService.DeleteData(Id);
            return JsonCRUDMessage(ViewBag.crudMode);
        }
        public ActionResult VendorView(long Id) 
        {
            ViewBag.crudMode = "V";
            var data = Service.EMCS.MasterVendorService.GetDataById(Id);
            return PartialView("ModelFormVendor",data);
        }
        public JsonResult GetVendorById(long Id)
        {
            var data = Service.EMCS.MasterVendorService.GetDataById(Id);
            return Json(data, JsonRequestBehavior.AllowGet);
        }



    }
}