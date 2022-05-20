using App.Web.App_Start;
using System;
using System.Web.Mvc;
using App.Web.Models.EMCS;
using System.IO;
using App.Web.Helper;

namespace App.Web.Controllers.EMCS
{
    public partial class EmcsController
    {
        [AuthorizeAcces(ActionType = AuthorizeAcces.IsRead)]
        public ActionResult ShippingSummaryList()
        {
            ApplicationTitle();
            ViewBag.AllowRead = AuthorizeAcces.AllowRead;
            ViewBag.AllowCreate = AuthorizeAcces.AllowCreated;
            ViewBag.AllowUpdate = AuthorizeAcces.AllowUpdated;
            ViewBag.AllowDelete = AuthorizeAcces.AllowDeleted;
            PaginatorBoot.Remove("SessionTRN");
            return View();
        }
        [AuthorizeAcces(ActionType = AuthorizeAcces.IsRead, UrlMenu = "ShippingSummaryList")]
        public JsonResult GetShippingSummary(GridListFilterModel filter)
        {
            var dataFilter = new Data.Domain.EMCS.GridListFilter();
            dataFilter.Limit = filter.Limit;
            dataFilter.Order = filter.Order;
            dataFilter.Term = filter.Term;
            dataFilter.Sort = filter.Sort;
            dataFilter.Offset = filter.Offset;

            var data = Service.EMCS.SVCShippingSummary.SSList(dataFilter);
            return Json(data, JsonRequestBehavior.AllowGet);
        }
        public ActionResult ShippingSummaryView(long cargoId = 1)
        {
            ApplicationTitle();
            string strQrCodeUrlEDI = Common.GenerateQrCode(cargoId, "downloadss");
            ViewBag.QrCodeUrlSS = strQrCodeUrlEDI;
            TempData["QrCodeUrlSS"] = strQrCodeUrlEDI;
            TempData.Peek("QrCodeUrlSS");
            ViewBag.AllowRead = AuthorizeAcces.AllowRead;
            ViewBag.AllowCreate = AuthorizeAcces.AllowCreated;
            ViewBag.AllowUpdate = AuthorizeAcces.AllowUpdated;
            ViewBag.AllowDelete = AuthorizeAcces.AllowDeleted;
            ViewBag.WizardData = Service.EMCS.SvcWizard.GetWizardData("ss", cargoId);
            ViewBag.CargoID = cargoId;

            ViewBag.TemplateHeader = Service.EMCS.DocumentStreamGenerator.GetCargoHeaderData(cargoId);
            ViewBag.TemplateDetail = Service.EMCS.DocumentStreamGenerator.GetCargoDetailData(cargoId);

            CargoSsModel data = new CargoSsModel
            {
                Header = Service.EMCS.DocumentStreamGenerator.GetCargoSsHeader(cargoId),
                Detail = Service.EMCS.DocumentStreamGenerator.GetCargoSsDetail(cargoId)
            };
            return View(data);
        }

        public ActionResult ReportSs(long clId, string reportType)
        {
            string fileExcel = Server.MapPath("~\\Content\\EMCS\\Templates\\TemplateSS.xls");
            string fileName = "SSDocument_" + DateTime.Now.ToString("yyyyMMddHHmmss");
            string filePath = Server.MapPath("~\\Content\\EMCS\\Templates\\" + fileName);
            MemoryStream output = Service.EMCS.DocumentStreamGenerator.GetStream(clId, fileExcel, filePath, reportType);

            return File(output.ToArray(), "application/pdf", "SSDocument_" + DateTime.Now.ToString("ddMMyyyyhhhmmss") + ".pdf");    //Suggested file name in the "Save as" dialog which will be displayed to the end user
        }
    }
}