using System.Web.Mvc;
using App.Web.App_Start;
using App.Data.Domain.EMCS;
using App.Web.Models.EMCS;

namespace App.Web.Controllers.EMCS
{
    public partial class EmcsController
    {
        public ActionResult Flow()
        {
            ViewBag.AllowRead = AuthorizeAcces.AllowRead;
            ViewBag.AllowCreate = AuthorizeAcces.AllowCreated;
            ViewBag.AllowUpdate = AuthorizeAcces.AllowUpdated;
            ViewBag.AllowDelete = AuthorizeAcces.AllowDeleted;
            PaginatorBoot.Remove("SessionTRN");
            return View();
        }

        public JsonResult GetDataFlow(GridListFilter crit)
        {
            var data = Service.EMCS.MasterFlow.GetListSp(crit);
            return Json(data, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult FlowCreate()
        {
            ViewBag.crudMode = "I";
            var bannerData = Service.EMCS.MasterFlow.GetDataById(0);
            return PartialView("Modal.FormFlow", bannerData);
        }

        [HttpPost]
        public ActionResult FlowCreate(FlowModel items)
        {
            ViewBag.crudMode = "I";
            MasterFlow item = new MasterFlow();
            item.Name = items.Name;
            item.Id = items.Id;
            item.Type = items.Type ?? "";
            item.IsDelete = items.IsDelete;
            Service.EMCS.MasterFlow.Crud(item, ViewBag.crudMode);
            return JsonCRUDMessage(ViewBag.crudMode);
        }

        [HttpGet]
        public ActionResult FlowEdit(long id)
        {
            ViewBag.crudMode = "U";
            var item = Service.EMCS.MasterFlow.GetDataById(id);
            if (item == null)
            {
                return HttpNotFound();
            }
            return PartialView("Modal.FormFlow", item);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult FlowEdit(FlowModel items)
        {
            ViewBag.crudMode = "U";
            if (ModelState.IsValid)
            {
                MasterFlow flow = Service.EMCS.MasterFlow.GetDataById(items.Id);
                flow.Name = items.Name;
                Service.EMCS.MasterFlow.Crud(flow, ViewBag.crudMode);
                return JsonCRUDMessage(ViewBag.crudMode);
            }
            return Json(new { success = false });
        }

        [HttpGet]
        public ActionResult FlowDelete(long id)
        {
            ViewBag.crudMode = "D";
            var item = new MasterFlow();

            return PartialView("Modal.FormFlow", item);
        }

        [HttpPost]
        //[ValidateAntiForgeryToken]
        public ActionResult FlowDelete(FlowModel items)
        {
            ViewBag.crudMode = "D";
            MasterFlow item = Service.EMCS.MasterFlow.GetDataById(items.Id);
            Service.EMCS.MasterFlow.Crud(
                item,
                ViewBag.crudMode);
            return JsonCRUDMessage(ViewBag.crudMode);
        }
    }
}