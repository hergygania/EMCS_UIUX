using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using App.Data.Domain;
using App.Service.Report;
using App.Web.Models;
using App.Web.App_Start;

namespace App.Web.Controllers
{
    public partial class ReportController
    {
        [AuthorizeAcces(ActionType = AuthorizeAcces.IsRead)]
        public ActionResult WHDocumentReprint()
        {
            ViewBag.AllowRead = AuthorizeAcces.AllowRead;
            ViewBag.AllowCreate = AuthorizeAcces.AllowCreated;
            ViewBag.AllowUpdate = AuthorizeAcces.AllowUpdated;
            ViewBag.AllowDelete = AuthorizeAcces.AllowDeleted;
            PaginatorBoot.Remove("SessionTRN");
            var model = new WHDocumentReprintView
            {
                StartDate = new DateTime(DateTime.Now.Year, 1, 1),
                EndDate = DateTime.Now,
            };
            return View(model);
        }

        public ActionResult WHDocumentReprintPage()
        {
            PaginatorBoot.Remove("SessionTRN");
            return WHDocumentReprintPageXt();
        }

        public ActionResult WHDocumentReprintPageXt()
        {
            Func<WHDocumentReprintView, List<RptWHDocumentReprint>> func = delegate(WHDocumentReprintView crit)
            {
                string param = Request["params"];
                if (!string.IsNullOrEmpty(param))
                {
                    var ser = new JavaScriptSerializer();
                    crit = ser.Deserialize<WHDocumentReprintView>(param);
                }

                List<RptWHDocumentReprint> list = WHDocumentReprints.GetList(
                    crit.DocNo,
                    crit.Sos,
                    crit.PartNo,
                    crit.BinLoc,
                    crit.StartDate,
                    crit.EndDate);
                return list.OrderBy(a => a.whdocrep_UpdatedOn).ThenBy(a => a.whdocrep_DocNo).ToList();
            };

            ActionResult paging = PaginatorBoot.Manage("SessionTRN", func).Pagination.ToJsonResult();
            return Json(paging, JsonRequestBehavior.AllowGet);
        }


        public ActionResult ExportToExcelWHDocReprint()
        {
            Response.AddHeader("Content-Type", "application/vnd.ms-excel");
            return PartialView("_tableWHDocumentReprint");
        }
    }
}