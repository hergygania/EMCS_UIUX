using System;
using System.Web.Mvc;
using App.Data.Domain.POST;
using App.Service.POST;
using App.Web.App_Start;
namespace App.Web.Controllers.POST
{
    public partial class PostController
    {

        #region view
        public ActionResult ListVendor()
        {
            ApplicationTitle();
            ViewBag.AllowRead = AuthorizeAcces.AllowRead;
            ViewBag.AllowCreate = AuthorizeAcces.AllowCreated;
            ViewBag.AllowUpdate = AuthorizeAcces.AllowUpdated;
            ViewBag.AllowDelete = AuthorizeAcces.AllowDeleted;
            PaginatorBoot.Remove("SessionTRN");
            return View();
        }
        #endregion

        #region Get Data
        public JsonResult GetDataVendor(PaginationParam param)
        {
            try
            {
                var data = Service.POST.MtVendor.GetData(param);
                return Json(new { status = "SUCCESS", result = data }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { status = "FAILED", result = ex.InnerException.Message }, JsonRequestBehavior.AllowGet);
            }

        }

        public JsonResult GetDataVendorById(int Id)
        {
            try
            {
                var data = Service.POST.MtVendor.GetVendorById(Id);
                return Json(new { status = "SUCCESS", result = data }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { status = "FAILED", result = ex.InnerException.Message }, JsonRequestBehavior.AllowGet);
            }

        }
        public JsonResult GetDataVendorByName(string name)
        {
            try
            {
                var data = Service.POST.MtVendor.GetVendorByName(name);
                return Json(new { status = "SUCCESS", result = data }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { status = "FAILED", result = ex.InnerException.Message }, JsonRequestBehavior.AllowGet);
            }

        }
        #endregion

        #region CRUD

        [HttpPost]
        public JsonResult MasterVendorCreate(Data.Domain.POST.MtVendor items)
        {
            try
            {
                ViewBag.crudMode = CrudModeInsert;
                Service.POST.MtVendor.Crud(items, ViewBag.crudMode);
                return Json(new { status = "SUCCESS" }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { status = "FAILED", result = ex.InnerException.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public JsonResult MasterVendorEdit(Data.Domain.POST.MtVendor items)
        {
            try
            {
                ViewBag.crudMode = CrudModeUpdate;
                Data.Domain.POST.MtVendor vendor = Service.POST.MtVendor.GetVendorById(items.Id);
                vendor.Id = items.Id;
                vendor.Name = items.Name;
                vendor.NPWP = items.NPWP;
                vendor.Telephone = items.Telephone;
                vendor.Address = items.Address;
                vendor.City = items.City;
                vendor.Code = items.Code;
                Service.POST.MtVendor.Crud(vendor, ViewBag.crudMode);
                return Json(new { status = "SUCCESS" }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { status = "FAILED", result = ex.InnerException.Message }, JsonRequestBehavior.AllowGet);
            }

        }

        [HttpPost]
        public JsonResult MasterVendorDeleteById(int Id)
        {
            try
            {
                Data.Domain.POST.MtVendor item = Service.POST.MtVendor.GetVendorById(Id);
                Service.POST.MtVendor.Crud(
                    item,
                    CrudModeDelete);
                return Json(new { status = "SUCCESS" }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { status = "FAILED", result = ex.InnerException.Message }, JsonRequestBehavior.AllowGet);
            }
        }
        #endregion






    }
}