using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using App.Domain;
using App.Data.Domain.EMCS;
using App.Web.Models.EMCS;

namespace App.Web.Controllers.EMCS
{
    public partial class EmcsController
    {
       public ActionResult FlowNextPage()
        {
            PaginatorBoot.Remove("SessionTRN");
            return FlowNextPageXt();
        }

        public ActionResult FlowNextPageXt()
        {

            // ReSharper disable once UnusedAnonymousMethodSignature
            Func<MasterSearchForm, IList<SpFlowNext>> func = delegate (MasterSearchForm crit)
            {
                long IdStep = 1;
                long IdStatus = 1;
                List<SpFlowNext> list = Service.EMCS.SvcFlowNext.GetList(IdStep, IdStatus);
                return list.OrderBy(o => o.UpdateDate).ToList();
            };

            ActionResult paging = PaginatorBoot.Manage("SessionTRN", func).Pagination.ToJsonResult();
            return Json(paging, JsonRequestBehavior.AllowGet);



        }

        [HttpGet]
        public ActionResult FlowNextCreate(long idStatus)
        {
            ViewBag.crudMode = "I";
            Service.EMCS.MasterFlowNext.GetDataById(0);
            var detail = new FlowNextModel();
            detail.IdStatus = idStatus;
            return PartialView("Modal.FormFlowNext", detail);
        }

        [HttpPost]
        public ActionResult FlowNextCreate(MasterFlowNext items)
        {
            if (ModelState.IsValid)
            {
                ViewBag.crudMode = "I";
                Service.EMCS.MasterFlowNext.Crud(items, ViewBag.crudMode);
                return JsonCRUDMessage(ViewBag.crudMode);
            }
            return Json(new { success = false });
        }

        [HttpGet]
        public ActionResult FlowNextEdit(long id)
        {
            ViewBag.crudMode = "U";
            var detail = Service.EMCS.MasterFlowNext.GetDataById(id);

            var data = new FlowNextModel();
            data.Id = detail.Id;
            data.IdStep = detail.IdStep;
            data.IdStatus = detail.IdStatus;

            return PartialView("Modal.FormFlowNext", data);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult FlowNextEdit(MasterFlowNext items)
        {
            ViewBag.crudMode = "U";
            if (ModelState.IsValid)
            {
                MasterFlowNext banner = Service.EMCS.MasterFlowNext.GetDataById(items.Id);
                banner.IdStep = items.IdStep;
                Service.EMCS.MasterFlowNext.Crud(banner, ViewBag.crudMode);
                return JsonCRUDMessage(ViewBag.crudMode);
            }
            return Json(new { success = false });
        }

        [HttpPost]
        //[ValidateAntiForgeryToken]
        public JsonResult FlowNextDelete(long id)
        {
            ViewBag.crudMode = "D";
            var items = Service.EMCS.MasterFlowNext.GetDataById(id);
            try
            {
                Service.EMCS.MasterFlowNext.Crud(items, ViewBag.crudMode);
                return JsonCRUDMessage(ViewBag.crudMode);
            }
            catch (Exception)
            {
                return JsonMessage("Failed to delete flow Next", 1, items);
            }
        }

        public JsonResult GetDataFlowNext(GridListFilter crit)
        {
            var data = Service.EMCS.MasterFlowNext.GetListSp(crit);
            return Json(data, JsonRequestBehavior.AllowGet);
        }
    }
}