using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
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

        public ActionResult BORespondTime()
        {
            PaginatorBoot.Remove("SessionTRN");
            var model = new BORespondTimeView
            {
                PickupStartDate = new DateTime(DateTime.Now.Year, 1, 1),
                PickupEndDate = DateTime.Now,
            };
            return View(model);
        }

        public ActionResult BORespondTimePage()
        {
            PaginatorBoot.Remove("SessionTRN");
            return BORespondTimePageXt();
        }

        public ActionResult BORespondTimePageXt()
        {
            Func<BORespondTimeView, List<RptBORespondTime>> func = delegate(BORespondTimeView crit)
            {
                string param = Request["params"];
                if (!string.IsNullOrEmpty(param))
                {
                    var ser = new JavaScriptSerializer();
                    crit = ser.Deserialize<BORespondTimeView>(param);
                }

                List<RptBORespondTime> list = BORespondTimes.GetList(
                    crit.PartNo,
                    crit.Quantity,
                    crit.BinLoc,
                    crit.Weight,
                    crit.Length,
                    crit.Width,
                    crit.PickupStartDate,
                    crit.PickupEndDate
                    );
                return list.OrderBy(a => a.borsp_UpdatedOn).ThenBy(a => a.borsp_ID).ToList();
            };

            ActionResult paging = PaginatorBoot.Manage("SessionTRN", func).Pagination.ToJsonResult();
            return Json(paging, JsonRequestBehavior.AllowGet);
        }

     

        public void ExportToExcelBORespondTime(
            string partNo, 
            int? quantity, 
            string binLoc, 
            int? weight, 
            int? length, 
            int? width, 
            DateTime? pickupStartDate, 
            DateTime? pickupEndDate)
        {
            List<RptBORespondTime> list = BORespondTimes.GetList(
                   partNo,
                   quantity,
                   binLoc,
                   weight,
                   length,
                   width,
                   pickupStartDate,
                   pickupEndDate
                   );

            DataTable dt = DataTableHelper.ConvertTo(list);
            ExportToExcel(dt, "BoRespondTime");

        }
    }
}