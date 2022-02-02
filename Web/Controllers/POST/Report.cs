using System;
using System.Linq;
using System.Web.Mvc;
using App.Data.Domain.POST;
using App.Web.App_Start;
using Newtonsoft.Json;

namespace App.Web.Controllers.POST
{
    public partial class PostController
    {
        public ActionResult LeadTime()
        {
            ApplicationTitle();
            ViewBag.AllowRead = AuthorizeAcces.AllowRead;
            ViewBag.AllowCreate = AuthorizeAcces.AllowCreated;
            ViewBag.AllowUpdate = AuthorizeAcces.AllowUpdated;
            ViewBag.AllowDelete = AuthorizeAcces.AllowDeleted;
            ViewBag.Menu = 3;
            PaginatorBoot.Remove("SessionTRN");
            return View();
        }


        #region table List
        public JsonResult GetListReportSla(SearchReport model)
        {
            try
            {
                model.order = string.IsNullOrEmpty(model.order) ? "ASC" : model.order;
                model.sort = string.IsNullOrEmpty(model.sort) ? "PO_Number" : model.sort;
                model.limit = model.limit == 0 ? 10 : model.limit;

                var userLogin = HttpContext.User.Identity.Name;
                var rows = Service.POST.Report.GetListReportSla(userLogin, model).ToList();
                var total = Service.POST.Report.GetTotalRowSla(userLogin, model);
                return Json(new { status = "SUCCESS", total, rows }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { status = "FAILED", result = ex.InnerException.Message }, JsonRequestBehavior.AllowGet);
            }
        }
        #endregion
    }
}