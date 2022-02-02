using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using App.Data.Domain;
using App.Service.Report;
using App.Web.Helper.Extensions;
using App.Web.Models;

namespace App.Web.Controllers
{
    public partial class ReportController
    {
        public ActionResult PODtoBOFill()
        {
            PaginatorBoot.Remove("SessionTRN");
            var model = new PODtoBOFillView
            {
                StartDate = new DateTime(DateTime.Now.Year, 1, 1),
                EndDate = DateTime.Now,
            };
            return View(model);
        }

        public ActionResult PODtoBOFillPage()
        {
            PaginatorBoot.Remove("SessionTRN");
            return PODtoBOFillPageXt();
        }

        public ActionResult PODtoBOFillPageXt()
        {
            Func<PODtoBOFillView, List<RptPODtoBOFill>> func = delegate(PODtoBOFillView crit)
            {
                string param = Request["params"];
                if (!string.IsNullOrEmpty(param))
                {
                    var ser = new JavaScriptSerializer();
                    crit = ser.Deserialize<PODtoBOFillView>(param);
                }

                List<RptPODtoBOFill> list = PODtoBOFills.GetList(
                    crit.DocNo,
                    crit.PartNo,
                    crit.Sos,
                    crit.StartDate,
                    crit.EndDate,
                    crit.Quantity
                    );
                return list.OrderBy(a => a.podbo_UpdatedOn).ThenBy(a => a.podbo_ID).ToList();
            };

            ActionResult paging = PaginatorBoot.Manage("SessionTRN", func).Pagination.ToJsonResult();
            return Json(paging, JsonRequestBehavior.AllowGet);
        }

        public void ExportToExcelPodToFill(string docNo, string partNo, string sos, DateTime? startDate, DateTime? endDate, int? quantity)
        {

            List<RptPODtoBOFill> list = PODtoBOFills.GetList(
                    docNo,
                    partNo,
                    sos,
                    startDate,
                    endDate,
                    quantity
                    );

            DataTable dt = DataTableHelper.ConvertTo(list);
            ExportToExcel(dt, "PODtoBOFill");
        }


    }
}