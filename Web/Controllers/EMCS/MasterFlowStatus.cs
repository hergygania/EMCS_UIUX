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
        public ActionResult FlowStatusPage()
        {
            PaginatorBoot.Remove("SessionTRN");
            return FlowStatusPageXt();
        }

        public ActionResult FlowStatusPageXt()
        {
            Func<MasterSearchForm, IList<MasterBanner>> func = delegate (MasterSearchForm crit)
            {
                List<MasterBanner> list = Service.EMCS.MasterBanner.GetBannerList(crit);
                return list.OrderBy(o => o.UpdateDate).ToList();
            };

            ActionResult paging = PaginatorBoot.Manage("SessionTRN", func).Pagination.ToJsonResult();
            return Json(paging, JsonRequestBehavior.AllowGet);

        }

        public JsonResult GetDataFlowStatus(GridListFilter crit)
        {
            var data = Service.EMCS.MasterFlowStatus.GetListSp(crit);
            return Json(data, JsonRequestBehavior.AllowGet);
        }


        [HttpGet]
        public ActionResult FlowStatusCreate(long idStep, long idStatus = 0)
        {
            ViewBag.crudMode = "I";
            var data = new FlowStatusModel();
            data.IdStep = idStep;
            return PartialView("Modal.FormFlowStatus", data);
        }

        [HttpPost]
        public ActionResult FlowStatusCreate(MasterFlowStatus items)
        {
            if (ModelState.IsValid)
            {
                ViewBag.crudMode = "I";
                Service.EMCS.MasterFlowStatus.Crud(items, ViewBag.crudMode);
                return JsonCRUDMessage(ViewBag.crudMode);
            }
            return Json(new { success = false });
        }

        [HttpGet]
        public ActionResult FlowStatusEdit(long id)
        {
            ViewBag.crudMode = "U";
            var item = Service.EMCS.MasterFlowStatus.GetDataById(id);
            var data = new FlowStatusModel();
            data.Id = item.Id;
            data.IdStep = item.IdStep;
            data.Status = item.Status;
            data.ViewByUser = item.ViewByUser;

            return PartialView("Modal.FormFlowStatus", data);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult FlowStatusEdit(MasterFlowStatus items)
        {
            ViewBag.crudMode = "U";
            if (ModelState.IsValid)
            {
                MasterFlowStatus status = Service.EMCS.MasterFlowStatus.GetDataById(items.Id);
                status.Status = items.Status;
                status.ViewByUser = items.ViewByUser;
                Service.EMCS.MasterFlowStatus.Crud(status, ViewBag.crudMode);
                return JsonCRUDMessage(ViewBag.crudMode);
            }
            return Json(new { success = false });
        }

        [HttpPost]
        public ActionResult FlowStatusDeleteById(long idStatus)
        {
            MasterFlowStatus item = Service.EMCS.MasterFlowStatus.GetDataById(idStatus);
            List<MasterFlowNext> itemNext = Service.EMCS.MasterFlowStatus.GetDataByStatusId(idStatus);
            Service.EMCS.MasterFlowStatus.Crud(item, "D");
            foreach (var data in itemNext)
            {
                Service.EMCS.MasterFlowNext.Crud(data, "D");
            }
            return JsonCRUDMessage("D");
        }
    }
}