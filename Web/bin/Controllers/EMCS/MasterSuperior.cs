using App.Web.App_Start;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using App.Web.Models.EMCS;
using App.Data.Domain.EMCS;
using System.Web.Script.Serialization;

namespace App.Web.Controllers.EMCS
{
    public partial class EmcsController
    {
        #region Initilize
        private MasterSuperior InitilizeSuperior(long id)
        {
            //var item = Service.EMCS.MasterSuperior.GetDataById(id);
            //SuperiorModel result = new SuperiorModel();
            //Data.Domain.EMCS.SPSuperior data = new Data.Domain.EMCS.SPSuperior();
            //if (item != null)
            //{
            //    data.Id = item.Id;
            //    data.EmployeeUsername = item.EmployeeUsername;
            //    data.EmployeeName = item.EmployeeName;
            //    data.SuperiorUsername = item.SuperiorUsername;
            //    data.SuperiorName = item.SuperiorName;
            //}

            MasterSuperior superior = new MasterSuperior();
            if (id == 0)
            {
                return superior;
            }

            superior = Service.EMCS.MasterSuperior.GetDataById(id);
            return superior;
        }
        #endregion

        // GET: MasterSuperior
        [AuthorizeAcces(ActionType = AuthorizeAcces.IsRead)]
        public ActionResult SuperiorList()
        {
            ViewBag.AllowRead = AuthorizeAcces.AllowRead;
            ViewBag.AllowCreate = AuthorizeAcces.AllowCreated;
            ViewBag.AllowUpdate = AuthorizeAcces.AllowUpdated;
            ViewBag.AllowDelete = AuthorizeAcces.AllowDeleted;
            PaginatorBoot.Remove("SessionTRN");

            return View();
        }

        //[AuthorizeAcces(ActionType = AuthorizeAcces.IsRead, UrlMenu = "Superior")]
        public ActionResult SuperiorListPage()
        {
            this.PaginatorBoot.Remove("SessionTRN");
            return SuperiorListPageXt();
        }

        //[AuthorizeAcces(ActionType = AuthorizeAcces.IsRead, UrlMenu = "Superior")]
        public ActionResult SuperiorListPageXt()
        {
            var data = new App.Data.Domain.EMCS.SuperiorListFilter();
            data.Username = Request["SearchName"];
            Func<SuperiorListFilter, List<SPSuperior>> func = delegate (SuperiorListFilter filter)
            {
                var param = Request["params"];
                if (!string.IsNullOrEmpty(param))
                {
                    JavaScriptSerializer ser = new JavaScriptSerializer();
                    filter = ser.Deserialize<SuperiorListFilter>(param);
                }
                var list = Service.EMCS.MasterSuperior.GetSuperiorList(data);
                return list.ToList();
            };
            ActionResult paging = PaginatorBoot.Manage("SessionTRN", func).Pagination.ToJsonResult();
            return Json(paging, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult SuperiorCreate()
        {
            ViewBag.crudMode = "I";
            var superiorData = new SuperiorModel();
            superiorData.Superior = InitilizeSuperior(0);
            var crit = new Domain.MasterSearchForm();
            superiorData.EmployeeList = Service.EMCS.MasterSuperior.GetUserList();
            //superiorData.SuperiorList = Service.EMCS.MasterSuperior.GetUserList();
            return PartialView("Modal.FormSuperior", superiorData);
        }

        [HttpPost]
        public ActionResult SuperiorCreate(SuperiorModel form)
        {
            if (ModelState.IsValid)
            {

                var item = new Data.Domain.EMCS.MasterSuperior();

                item.EmployeeUsername = form.Superior.EmployeeUsername;
                item.EmployeeName = form.Superior.EmployeeName;
                item.SuperiorUsername = form.Superior.SuperiorUsername;
                item.SuperiorName = form.Superior.SuperiorName;

                ViewBag.crudMode = "I";
                Service.EMCS.MasterSuperior.CrudSp(item, ViewBag.crudMode);
                return JsonCRUDMessage(ViewBag.crudMode);
            }
            return Json(new { success = false });
        }

        [HttpGet]
        public ActionResult SuperiorEdit(long id)
        {
            ViewBag.crudMode = "U";
            PaginatorBoot.Remove("SessionTRN");
            var superiorData = new SuperiorModel();
            superiorData.Superior = InitilizeSuperior(id);
            superiorData.EmployeeList = Service.EMCS.MasterSuperior.GetUserList();

            if (superiorData.Superior == null)
            {
                return HttpNotFound();
            }
            return PartialView("Modal.FormSuperior", superiorData);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult SuperiorEdit(SuperiorModel form)
        {
            ViewBag.crudMode = "U";
            if (ModelState.IsValid)
            {
                var item = new Data.Domain.EMCS.MasterSuperior();

                item.Id = form.Superior.Id;
                item.EmployeeUsername = form.Superior.EmployeeUsername;
                item.EmployeeName = form.Superior.EmployeeName;
                item.SuperiorUsername = form.Superior.SuperiorUsername;
                item.SuperiorName = form.Superior.SuperiorName;

                Service.EMCS.MasterSuperior.CrudSp(item, ViewBag.crudMode);
                return JsonCRUDMessage(ViewBag.crudMode);
            }
            return Json(new { success = false });
        }

        [HttpPost]
        public JsonResult SuperiorDelete(long Id)
        {
            try
            {
                var item = new Data.Domain.EMCS.MasterSuperior();

                var superiorData = new SuperiorModel();
                superiorData.Superior = InitilizeSuperior(Id);

                item.Id = Id;
                item.EmployeeUsername = superiorData.Superior.EmployeeUsername;
                item.EmployeeName = superiorData.Superior.EmployeeName;
                item.SuperiorUsername = superiorData.Superior.SuperiorUsername;
                item.SuperiorName = superiorData.Superior.SuperiorName;
                Service.EMCS.MasterSuperior.CrudSp(item, "D");
                return JsonCRUDMessage("D", item);
            }
            catch (Exception e)
            {
                return Json(new { success = false });
            }
        }
    }
}