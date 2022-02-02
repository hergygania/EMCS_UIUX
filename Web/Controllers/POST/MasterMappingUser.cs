using System;
using System.Web.Mvc;
using App.Service.POST;
using App.Web.App_Start;

namespace App.Web.Controllers.POST
{
    public partial class PostController
    {

        #region view
        public ActionResult MasterMappingUser()
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
        public JsonResult GetDataMappingUser(PaginationParam param)
        {
            try
            {
                var data = Service.POST.MasterMappingUser.GetData();
                return Json(new { status = "SUCCESS", result = data }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { status = "FAILED", result = ex.InnerException.Message }, JsonRequestBehavior.AllowGet);
            }

        }

        public JsonResult GetDataMappingUserById(int Id)
        {
            try
            {
                var data = Service.POST.MasterMappingUser.GetMappingUserById(Id);
                return Json(new { status = "SUCCESS", result = data }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { status = "FAILED", result = ex.InnerException.Message }, JsonRequestBehavior.AllowGet);
            }

        }

        public JsonResult GetSelectUser(string search)
        {
            try
            {
                var userLogin = HttpContext.User.Identity.Name;
                var data = Service.POST.MasterMappingUser.GetSelectUser(search);
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
        public JsonResult MasterMappingUserCreate(Data.Domain.POST.MtMappingUserBranch items)
        {
            try
            {
                ViewBag.crudMode = CrudModeInsert;
                var result = Service.POST.MasterMappingUser.Crud(items, ViewBag.crudMode);
                if (result != -1)
                    return Json(new { status = "SUCCESS" }, JsonRequestBehavior.AllowGet);

                return Json(new { status = "FAILED", result = "Data exist" }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { status = "FAILED", result = ex.InnerException.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public JsonResult MasterMappingUserEdit(Data.Domain.POST.MtMappingUserBranch items)
        {
            try
            {
                ViewBag.crudMode = CrudModeUpdate;
                Data.Domain.POST.MtMappingUserBranch MappingUser = Service.POST.MasterMappingUser.GetMappingUserById(items.ID);
                MappingUser.ID = items.ID;
                MappingUser.UserID = items.UserID;
                MappingUser.NamaCabang = items.NamaCabang;
                MappingUser.NPWP = items.NPWP;
                Service.POST.MasterMappingUser.Crud(MappingUser, ViewBag.crudMode);
                return Json(new { status = "SUCCESS" }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { status = "FAILED", result = ex.InnerException.Message }, JsonRequestBehavior.AllowGet);
            }

        }

        [HttpPost]
        public JsonResult MasterMappingUserDeleteById(int Id)
        {
            try
            {
                Data.Domain.POST.MtMappingUserBranch item = Service.POST.MasterMappingUser.GetMappingUserById(Id);
                Service.POST.MasterMappingUser.Crud(
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