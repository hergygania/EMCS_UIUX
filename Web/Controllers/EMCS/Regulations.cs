using System.Web.Mvc;
using App.Web.App_Start;

namespace App.Web.Controllers.EMCS
{
    public partial class EmcsController
    {

        [AuthorizeAcces(ActionType = AuthorizeAcces.IsRead)]
        public ActionResult Regulations()
        {
            ViewBag.AllowRead = AuthorizeAcces.AllowRead;
            ViewBag.AllowCreate = AuthorizeAcces.AllowCreated;
            ViewBag.AllowUpdate = AuthorizeAcces.AllowUpdated;
            ViewBag.AllowDelete = AuthorizeAcces.AllowDeleted;
            PaginatorBoot.Remove("SessionTRN");
            return View();
        }

        public JsonResult GetRegulationList()
        {
            var data = Service.EMCS.SvcRegulation.GetRegulationList();
            return Json(data, JsonRequestBehavior.AllowGet);
        }
    }
}