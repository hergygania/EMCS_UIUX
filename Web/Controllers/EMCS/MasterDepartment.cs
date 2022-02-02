using App.Data.Domain;
using App.Domain;
using App.Web.App_Start;
using App.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using App.Web.Models.EMCS;
using App.Data.Domain.EMCS;

namespace App.Web.Controllers.EMCS
{
    public partial class EmcsController
    {
        // GET: MasterDepartment
        public ActionResult Department()
        {
            ViewBag.AllowRead = AuthorizeAcces.AllowRead;

            PaginatorBoot.Remove("SessionTRN");

            return View();
        }

        public ActionResult DepartmentPage()
        {
            this.PaginatorBoot.Remove("SessionTRN");
            return DepartmentPageXt();
        }

        public ActionResult DepartmentPageXt()
        {
            Func<MasterSearchForm, IList<Data.Domain.EMCS.MasterDepartment>> func = delegate (MasterSearchForm crit)
            {
                List<App.Data.Domain.EMCS.MasterDepartment> list = Service.EMCS.MasterDepartment.GetKursList(crit);
                return list.ToList();
            };

            ActionResult paging = PaginatorBoot.Manage("SessionTRN", func).Pagination.ToJsonResult();
            return Json(paging, JsonRequestBehavior.AllowGet);
        }
    }
}