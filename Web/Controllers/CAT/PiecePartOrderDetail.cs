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
        public ActionResult PiecePartOrderDetail()
        {
            var model = new App.Web.Models.CAT.RptPiecePartOrderSummaryFilter();

            model.RangeWeek = Service.CAT.Report.RptPiecePartOrderDetailList.GetRangeWeek();
            return View(model);
        }

        public ActionResult PiecePartOrderDetailPage()
        {
            this.PaginatorBoot.Remove("SessionRptPiecePartOrderDetailList");
            return PiecePartOrderDetailPageXt();
        }

        public ActionResult PiecePartOrderDetailPageXt()
        {
            Func<RptPiecePartOrderSummaryFilter, List<Data.Domain.RptPiecePartOrderDetailList>> func = delegate(RptPiecePartOrderSummaryFilter filter)
            {
                var param = Request["params"];
                if (!string.IsNullOrEmpty(param))
                {
                    JavaScriptSerializer ser = new JavaScriptSerializer();
                    filter = ser.Deserialize<RptPiecePartOrderSummaryFilter>(param);
                }

                var list = Service.CAT.Report.RptPiecePartOrderDetailList.SP_GetList(filter.ref_part_no, filter.model, filter.prefix, filter.smcs, filter.component, filter.mod, filter.DateFilter);
                return list.ToList();
            };

            var paging = PaginatorBoot.Manage("SessionRptPiecePartOrderDetailList", func).Pagination.ToJsonResult();
            return Json(paging, JsonRequestBehavior.AllowGet);
        }

        public ActionResult DownloadPiecePartOrderDetail(RptPiecePartOrderSummaryFilter filter)
        {
            Guid guid = Guid.NewGuid();
            Helper.Service.CAT.DownloadReportPiecePartOrderDetail data = new Helper.Service.CAT.DownloadReportPiecePartOrderDetail();

            Session[guid.ToString()] = data.DownloadToExcel(filter);

            return Json(guid.ToString(), JsonRequestBehavior.AllowGet);
        }
    }
}