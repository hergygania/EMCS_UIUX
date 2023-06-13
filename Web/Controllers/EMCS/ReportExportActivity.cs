using System;
using System.Web.Mvc;
using App.Web.App_Start;
using System.IO;

namespace App.Web.Controllers.EMCS
{
    public partial class EmcsController
    {

        [AuthorizeAcces(ActionType = AuthorizeAcces.IsRead)]
        public ActionResult ExportActivity()
        {
            ViewBag.AllowRead = AuthorizeAcces.AllowRead;
            ViewBag.AllowCreate = AuthorizeAcces.AllowCreated;
            ViewBag.AllowUpdate = AuthorizeAcces.AllowUpdated;
            ViewBag.AllowDelete = AuthorizeAcces.AllowDeleted;
            PaginatorBoot.Remove("SessionTRN");
            return View();
        }

        [AuthorizeAcces(ActionType = AuthorizeAcces.IsRead, UrlMenu = "ExportActivity")]
        public ActionResult GetTrendExport(int startYear, int endYear, string filter)
        {
            var data = Service.EMCS.SvcRExportActivity.TrendExportList(startYear, endYear, filter);
            return Json(data, JsonRequestBehavior.AllowGet);
        }

        [AuthorizeAcces(ActionType = AuthorizeAcces.IsRead, UrlMenu = "ExportActivity")]
        public ActionResult GetExportByCategory(int startYear, int endYear)
        {
            var data = Service.EMCS.SvcRExportActivity.ExportByCategoryList(startYear, endYear);
            return Json(data, JsonRequestBehavior.AllowGet);
        }

        [AuthorizeAcces(ActionType = AuthorizeAcces.IsRead, UrlMenu = "ExportActivity")]
        public ActionResult GetSalesVsNonSales(int startYear, int endYear)
        {
            var data = Service.EMCS.SvcRExportActivity.SalesVsNonSalesList(startYear, endYear);
            return Json(data, JsonRequestBehavior.AllowGet);
        }

        [AuthorizeAcces(ActionType = AuthorizeAcces.IsRead, UrlMenu = "ExportActivity")]
        public ActionResult GetTotalExport(int year)
        {
            var data = Service.EMCS.SvcRExportActivity.TotalExportList(year);
            return Json(data, JsonRequestBehavior.AllowGet);
        }

        [AuthorizeAcces(ActionType = AuthorizeAcces.IsRead, UrlMenu = "ExportActivity")]
        public ActionResult TotalBig5CommoditiesList(Domain.MasterSearchForm crit)
        {
            var model = Service.EMCS.SvcRExportActivity.TotalBig5CommoditiesList(crit);
            return Json(model, JsonRequestBehavior.AllowGet);
        }

        [AuthorizeAcces(ActionType = AuthorizeAcces.IsRead, UrlMenu = "ExportActivity")]
        public ActionResult DownloadTrendExport(int startYear, int endYear, string filter)
        {
            var data = Service.EMCS.SvcRExportActivity.TrendExportList(startYear, endYear, filter);
            string fileExcel = Server.MapPath("~\\Content\\EMCS\\Templates\\TemplateActivityReport_TrendExport.xls");
            MemoryStream output = Service.EMCS.SvcRExportActivity.GetTrendExportStream(data, fileExcel);
            return File(output.ToArray(), "application/vnd.ms-excel", "TrendExport_" + DateTime.Now.ToString("ddMMyyyyhhhmmss") + ".xls");
        }

        [AuthorizeAcces(ActionType = AuthorizeAcces.IsRead, UrlMenu = "ExportActivity")]
        public ActionResult DownloadExportByCategory(int startYear, int endYear)
        {
            var data = Service.EMCS.SvcRExportActivity.ExportByCategoryList(startYear, endYear);
            string fileExcel = Server.MapPath("~\\Content\\EMCS\\Templates\\TemplateActivityReport_ExportByCategory.xls");
            MemoryStream output = Service.EMCS.SvcRExportActivity.GetExportByCategoryStream(data, fileExcel);

            return File(output.ToArray(), "application/vnd.ms-excel", "ExportByCategory_" + DateTime.Now.ToString("ddMMyyyyhhhmmss") + ".xls");
        }

        [AuthorizeAcces(ActionType = AuthorizeAcces.IsRead, UrlMenu = "ExportActivity")]
        public ActionResult DownloadSalesVsNonSales(int startYear, int endYear)
        {
            var data = Service.EMCS.SvcRExportActivity.SalesVsNonSalesList(startYear, endYear);
            string fileExcel = Server.MapPath("~\\Content\\EMCS\\Templates\\TemplateActivityReport_SalesVSNonSales.xls");
            MemoryStream output = Service.EMCS.SvcRExportActivity.GetSalesVsNonSalesStream(data, fileExcel);
            return File(output.ToArray(), "application/vnd.ms-excel", "SalesVSNonSales_" + DateTime.Now.ToString("ddMMyyyyhhhmmss") + ".xls");
        }
        [AuthorizeAcces(ActionType = AuthorizeAcces.IsRead, UrlMenu = "ExportActivity")]
        public ActionResult DownloadTotalExport(int year)
        {
            var data = Service.EMCS.SvcRExportActivity.TotalExportList(year);
            string fileExcel = Server.MapPath("~\\Content\\EMCS\\Templates\\TemplateActivityReport_TotalExport.xls");
            MemoryStream output = Service.EMCS.SvcRExportActivity.GetTotalExportStream(data, fileExcel);
            return File(output.ToArray(), "application/vnd.ms-excel", "TotalExportTransaction_" + DateTime.Now.ToString("ddMMyyyyhhhmmss") + ".xls");
        }
        [AuthorizeAcces(ActionType = AuthorizeAcces.IsRead, UrlMenu = "ExportActivity")]
        public ActionResult DownloadBig5Commodities(Domain.MasterSearchForm crit)
        {
            var data = Service.EMCS.SvcRExportActivity.TotalBig5CommoditiesList(crit);
            string fileExcel = Server.MapPath("~\\Content\\EMCS\\Templates\\TemplateTotal5BigCommodities.xls");
            MemoryStream output = Service.EMCS.SvcRExportActivity.GetTotalBig5CommoditiesStream(data, fileExcel);
            return File(output.ToArray(), "application/vnd.ms-excel", "Total5BigCommodities_" + DateTime.Now.ToString("ddMMyyyyhhhmmss") + ".xls");
        }
    }
}