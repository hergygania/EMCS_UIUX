using App.Data.Domain;
using App.Domain;
using App.Web.App_Start;
using App.Web.Helper;
using App.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
namespace App.Web.Controllers
{
    public partial class MasterController
    {
        #region Generic Initilize
        private MasterGeneric InitilizeGeneric(int genericId)
        {
            var generic = new MasterGeneric();
            if (genericId == 0)
            {
                return generic;
            }
            generic = Service.Master.MasterGeneric.GetId(genericId);
            return generic;
        }
        #endregion
        // GET: Generic

        //[AuthorizeAcces(ActionType = AuthorizeAcces.IsRead)]
        public ActionResult MasterGeneric()
        {
            ViewBag.AllowRead = AuthorizeAcces.AllowRead;
            ViewBag.AllowCreate = AuthorizeAcces.AllowCreated;
            ViewBag.AllowUpdate = AuthorizeAcces.AllowUpdated;
            ViewBag.AllowDelete = AuthorizeAcces.AllowDeleted;
            PaginatorBoot.Remove("SessionTRN");
            return View();
        }

        [AuthorizeAcces(ActionType = AuthorizeAcces.IsRead, UrlMenu = "MasterGeneric")]
        public ActionResult MasterGenericPage()
        {
            PaginatorBoot.Remove("SessionTRN");
            return MasterGenericPageXt();
        }

        [AuthorizeAcces(ActionType = AuthorizeAcces.IsRead, UrlMenu = "MasterGeneric")]
        public ActionResult MasterGenericPageXt()
        {
            Func<MasterSearchForm, IList<MasterGeneric>> func = delegate(MasterSearchForm crit)
            {
                List<MasterGeneric> list = Service.Master.MasterGeneric.GetGenericList(crit);
                return list.OrderBy(o => o.Code).ToList();
            };

            ActionResult paging = PaginatorBoot.Manage("SessionTRN", func).Pagination.ToJsonResult();
            return Json(paging, JsonRequestBehavior.AllowGet);
        }

        [AuthorizeAcces(ActionType = AuthorizeAcces.IsRead, UrlMenu = "MasterGeneric")]
        [HttpGet]
        public ActionResult MasterGenericCreate()
        {
            ViewBag.crudMode = "I";
            var GenericData = InitilizeGeneric(0);
            return PartialView("MasterGeneric.iud", GenericData);
        }


        #region Region Edit
        [AuthorizeAcces(ActionType = AuthorizeAcces.IsRead, UrlMenu = "MasterGeneric")]
        [HttpGet]
        public ActionResult MasterGenericEdit(int id)
        {
            ViewBag.crudMode = "U";
            var region = InitilizeGeneric(id);
            if (region == null)
            {
                return HttpNotFound();
            }

            return PartialView("MasterGeneric.iud", region);
        }

        [AuthorizeAcces(ActionType = AuthorizeAcces.IsUpdated, UrlMenu = "MasterGeneric")]
        [HttpPost, ValidateInput(false)]
        [ValidateAntiForgeryToken]
        public ActionResult MasterGenericEdit(MasterGeneric items)
        {
            var ResultValue = Service.Master.EmailRecipients.ValidateInputHtmlInjection(items.Value, "`^<>");
            var ResultDescription = Service.Master.EmailRecipients.ValidateInputHtmlInjection(items.Description, "`^<>");
            if (!ResultValue)
            {
                return JsonMessage("Please Enter a Valid Value", 1, "i");
            }
            if (!ResultDescription)
            {
                return JsonMessage("Please Enter a Valid Description", 1, "i");
            }

            ViewBag.crudMode = "U";
            if (ModelState.IsValid)
            {
                items.Code = Common.Sanitize(items.Code);
                items.Name = Common.Sanitize(items.Name);
                items.Value = Common.Sanitize(items.Value);
                items.Description = Common.Sanitize(items.Description);

                Service.Master.MasterGeneric.crud(items, ViewBag.crudMode);
                return JsonCRUDMessage(ViewBag.crudMode);
            }
            return Json(new { success = false });
        }

        #endregion


    }
}