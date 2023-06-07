using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using App.Web.Models.CAT;
using System.Web.Script.Serialization;

namespace App.Web.Controllers.CAT
{
    public partial class CATController
    {
        public ActionResult SummaryPEX()
        {
            ViewBag.SOS = Service.CAT.Master.MasterSOS.GetList();
            return View();
        }

        public ActionResult SummaryPEXPage()
        {
            this.PaginatorBoot.Remove("SessionTRN");
            return SummaryPEXPageXt();
        }

        public ActionResult SummaryPEXPageXt()
        {
            Func<RptSummaryPEXFilter, List<Data.Domain.RptSummaryPEXList>> func = delegate(RptSummaryPEXFilter filter)
            {
                var param = Request["params"];
                if (!string.IsNullOrEmpty(param))
                {
                    JavaScriptSerializer ser = new JavaScriptSerializer();
                    filter = ser.Deserialize<RptSummaryPEXFilter>(param);
                }

                var list = Service.CAT.Report.RptSummaryPEXList.GetList(filter.ref_part_no, filter.model, filter.component, filter.SOS);
                return list.ToList();
            };

            var paging = PaginatorBoot.Manage("SessionTRN", func).Pagination.ToJsonResult();
            return Json(paging, JsonRequestBehavior.AllowGet);
        }

        public ActionResult DownloadSummaryPEX(RptSummaryPEXFilter filter)
        {
            Guid guid = Guid.NewGuid();
            Helper.Service.CAT.DownloadReportSummaryPEX data = new Helper.Service.CAT.DownloadReportSummaryPEX();

            Session[guid.ToString()] = data.DownloadToExcel(filter);

            return Json(guid.ToString(), JsonRequestBehavior.AllowGet);
        }
    }
}