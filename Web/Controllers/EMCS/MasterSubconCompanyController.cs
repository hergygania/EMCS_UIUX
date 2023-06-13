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

        [AuthorizeAcces(ActionType = AuthorizeAcces.IsRead, UrlMenu = "Subcon")]
        public ActionResult Subcon()
        {
            ViewBag.AllowRead = AuthorizeAcces.AllowRead;
            ViewBag.AllowCreate = AuthorizeAcces.AllowCreated;
            ViewBag.AllowUpdate = AuthorizeAcces.AllowUpdated;
            ViewBag.AllowDelete = AuthorizeAcces.AllowDeleted;
            PaginatorBoot.Remove("SessionTRN");
            return View();
        }

        //[AuthorizeAcces(ActionType = AuthorizeAcces.IsCreated, UrlMenu = "Subcon")]
        [HttpGet]
        public ActionResult SubconCreate()
        {
            try
            {
                ViewBag.crudMode = "I";
                //var Data = new SubconModel();
                return PartialView("ModelFormSubcon");

            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        [HttpPost]
        public ActionResult SubconCreate(MasterSubConCompany subconmodel)
        {
            try
            {
                var data = Service.EMCS.MasterSubcon.InsertSubcon(subconmodel);
                ViewBag.crudMode = "I";

                return JsonCRUDMessage(ViewBag.crudMode);

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [AuthorizeAcces(ActionType = AuthorizeAcces.IsRead, UrlMenu = "Subcon")]
        public ActionResult SubconPage()
        {
            PaginatorBoot.Remove("SessionTRN");
            return SubconPageXt();
        }

        [AuthorizeAcces(ActionType = AuthorizeAcces.IsRead, UrlMenu = "Subcon")]
        public ActionResult SubconPageXt()
        {
            Func<MasterSearchForm, IList<MasterSubConCompany>> func = delegate (MasterSearchForm crit)
            {
                List<MasterSubConCompany> list = Service.EMCS.MasterSubcon.GetSubconList(crit);
                return list.OrderBy(o => o.Id).ToList();
            };

            ActionResult paging = PaginatorBoot.Manage("SessionTRN", func).Pagination.ToJsonResult();
            return Json(paging, JsonRequestBehavior.AllowGet);

        }
        [AuthorizeAcces(ActionType = AuthorizeAcces.IsUpdated, UrlMenu = "Subcon")]
        [HttpGet]
        public ActionResult SubconEdit(long Id)
        {
            ViewBag.crudMode = "U";
            var data = Service.EMCS.MasterSubcon.GetDataById(Id);
            return PartialView("ModelFormSubcon",data);
        }
        [AuthorizeAcces(ActionType = AuthorizeAcces.IsCreated, UrlMenu = "Subcon")]
        [HttpPost, ValidateInput(false)]
        [ValidateAntiForgeryToken]
        public ActionResult SubconEdit(MasterSubConCompany objModel)
        {
            ViewBag.crudMode = "U";
            var data = Service.EMCS.MasterSubcon.InsertSubcon(objModel);
            return JsonCRUDMessage(ViewBag.crudMode);
        }
        public ActionResult SubconDelete(long Id)
        {
            ViewBag.crudMode = "D";
            Service.EMCS.MasterSubcon.DeleteData(Id);
            return JsonCRUDMessage(ViewBag.crudMode);
        }



    }
}
