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
        // GET: MasterKurs
        [AuthorizeAcces(ActionType = AuthorizeAcces.IsRead)]
        public ActionResult Kurs()
        {
            ViewBag.AllowRead = AuthorizeAcces.AllowRead;
            
            PaginatorBoot.Remove("SessionTRN");

            return View();
        }

        [AuthorizeAcces(ActionType = AuthorizeAcces.IsRead, UrlMenu = "Kurs")]
        public ActionResult KursPage()
        {
            this.PaginatorBoot.Remove("SessionTRN");
            return KursPageXt();
        }

        [AuthorizeAcces(ActionType = AuthorizeAcces.IsRead, UrlMenu = "Kurs")]
        public ActionResult KursPageXt()
        {
            Func<MasterSearchForm, IList<Data.Domain.EMCS.MasterKurs>> func = delegate (MasterSearchForm crit)
            {
                List<App.Data.Domain.EMCS.MasterKurs> list = Service.EMCS.MasterKurs.GetKursList(crit);
                return list.ToList();
            };

            ActionResult paging = PaginatorBoot.Manage("SessionTRN", func).Pagination.ToJsonResult();
            return Json(paging, JsonRequestBehavior.AllowGet);
        }
    }
}