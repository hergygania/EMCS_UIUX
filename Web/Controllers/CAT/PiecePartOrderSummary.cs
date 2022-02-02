using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using App.Web.Models.CAT;

namespace App.Web.Controllers.CAT
{
    public partial class CATController
    {
        //
        // GET: /PiecePartOrderSummary/
        public ActionResult PiecePartOrderSummary()
        {
            var model = new App.Web.Models.CAT.RptPiecePartOrderSummaryFilter();

            model.RangeWeek = Service.CAT.Report.RptPiecePartOrderDetailList.GetRangeWeek();
            return View(model);
        }
                                  
        public ActionResult PiecePartOrderSummaryPage()
        {
            this.PaginatorBoot.Remove("SessionTRN");
            return PiecePartOrderSummaryPageXt();
        }

        public ActionResult PiecePartOrderSummaryPageXt()
        {
            Func<RptPiecePartOrderSummaryFilter, IList<Data.Domain.RptPiecePartOrderSummaryList>> func = delegate(RptPiecePartOrderSummaryFilter filter)
            {
                var param = Request["params"];
                if (!string.IsNullOrEmpty(param))
                {
                    JavaScriptSerializer ser = new JavaScriptSerializer();
                    filter = ser.Deserialize<RptPiecePartOrderSummaryFilter>(param);
                }

                var list = Service.CAT.Report.RptPiecePartOrderSummaryList.GetList(filter.ref_part_no, filter.model, filter.prefix, filter.smcs, filter.component, filter.mod, filter.DateFilter);
                return list.ToList();
            };

            var paging = PaginatorBoot.Manage("SessionTRN", func).Pagination.ToJsonResult();
            return Json(paging, JsonRequestBehavior.AllowGet);
        }

        public ActionResult DownloadPiecePartOrderSummary(RptPiecePartOrderSummaryFilter filter)
        {
            Guid guid = Guid.NewGuid();
            Helper.Service.CAT.DownloadReportPiecePartOrderSummary data = new Helper.Service.CAT.DownloadReportPiecePartOrderSummary();

            Session[guid.ToString()] = data.DownloadToExcel(filter);

            return Json(guid.ToString(), JsonRequestBehavior.AllowGet);
        }
    }
}