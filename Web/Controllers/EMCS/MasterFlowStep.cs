using System.Web.Mvc;
using App.Data.Domain.EMCS;
using App.Web.Models.EMCS;

namespace App.Web.Controllers.EMCS
{
    public partial class EmcsController
    {
        public JsonResult GetDataFlowStep(GridListFilter crit)
        {
            var data = Service.EMCS.MasterFlowStep.GetListSp(crit);
            return Json(data, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult FlowStepCreate(long idFlow, long idStep = 0)
        {
            ViewBag.crudMode = "I";
            var data = new FlowStepModel();
            data.IdFlow = idFlow;
            return PartialView("Modal.FormFlowStep", data);
        }

        [HttpGet]
        public ActionResult FlowStepEdit(long idFlow, long idStep)
        {
            ViewBag.crudMode = "U";
            var data = Service.EMCS.MasterFlowStep.GetDataById(idStep);
            var detail = new FlowStepModel();
            detail.Id = data.Id;
            detail.IdFlow = data.IdFlow;
            detail.Step = data.Step;
            detail.AssignType = data.AssignType;
            detail.AssignTo = data.AssignTo;
            detail.Sort = data.Sort;
            return PartialView("Modal.FormFlowStep", detail);
        }

        [HttpPost]
        public ActionResult FlowStepCreate(MasterFlowStep items)
        {
            if (ModelState.IsValid)
            {
                ViewBag.crudMode = "I";
                var detail = new MasterFlowStep();
                detail.Step = items.Step;
                detail.AssignType = items.AssignType;
                detail.AssignTo = items.AssignTo;
                detail.Sort = items.Sort;
                Service.EMCS.MasterFlowStep.Crud(items, ViewBag.crudMode);
                return JsonCRUDMessage(ViewBag.crudMode);
            }
            return Json(new { success = false });
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult FlowStepEdit(MasterFlowStep form)
        {
            ViewBag.crudMode = "U";
            if (ModelState.IsValid)
            {
                var step = Service.EMCS.MasterFlowStep.GetDataById(form.Id);
                step.Step = form.Step;
                step.Sort = form.Sort;
                step.AssignTo = form.AssignTo;
                step.AssignType = form.AssignType;
                Service.EMCS.MasterFlowStep.Crud(step, ViewBag.crudMode);
                return JsonCRUDMessage(ViewBag.crudMode);
            }
            return Json(new { success = false });
        }

        [HttpPost]
        public ActionResult FlowStepDeleteById(long id)
        {
            var data = Service.EMCS.MasterFlowStep.GetDataById(id);
            Service.EMCS.MasterFlowStep.Crud(data, "D");
            return JsonCRUDMessage("D", new { data });
        }
    }
}