using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using App.Web.App_Start;
using System.IO;
using App.Web.Models.EMCS;
using App.Web.Helper;

namespace App.Web.Controllers.EMCS
{
    public partial class EmcsController
    {
        [AuthorizeAcces(ActionType = AuthorizeAcces.IsRead)]
        public ActionResult ShippingInstructionList()
        {
            ApplicationTitle();
            ViewBag.AllowRead = AuthorizeAcces.AllowRead;
            ViewBag.AllowCreate = AuthorizeAcces.AllowCreated;
            ViewBag.AllowUpdate = AuthorizeAcces.AllowUpdated;
            ViewBag.AllowDelete = AuthorizeAcces.AllowDeleted;
            PaginatorBoot.Remove("SessionTRN");
            return View();
        }
        [AuthorizeAcces(ActionType = AuthorizeAcces.IsRead, UrlMenu = "ShippingInstructionList")]
        public JsonResult GetShippingInstructionList(GridListFilterModel filter)
        {
            var dataFilter = new Data.Domain.EMCS.GridListFilter();
            dataFilter.Limit = filter.Limit;
            dataFilter.Order = filter.Order;
            dataFilter.Term = filter.Term;
            dataFilter.Sort = filter.Sort;
            dataFilter.Offset = filter.Offset;

            var data = Service.EMCS.SvcShippingInstruction.SIList(dataFilter);
            return Json(data, JsonRequestBehavior.AllowGet);
        }

        public ActionResult SubmitSi(Data.Domain.EMCS.GridListFilter filter)
        {
            ApplicationTitle();
            var id = filter.Id;
            ViewBag.crudMode = "U";
            ViewBag.AllowRead = AuthorizeAcces.AllowRead;
            ViewBag.AllowCreate = AuthorizeAcces.AllowCreated;
            ViewBag.AllowUpdate = AuthorizeAcces.AllowUpdated;
            ViewBag.AllowDelete = AuthorizeAcces.AllowDeleted;
            ViewBag.WizardData = Service.EMCS.SvcWizard.GetWizardData("si", id);
            ViewBag.SpecialInstructionDefault = Service.EMCS.MasterParameter.GetSingleParam("SIDocumentRequired");
            PaginatorBoot.Remove("SessionTRN");

            var detail = new CargoModel();
            detail.Data = Service.EMCS.SvcCargo.GetCargoById(id);
            detail.DataItem = Service.EMCS.SvcCargo.GetCargoDetailData(id);
            return View(detail);
        }

        public ActionResult EditSi(Data.Domain.EMCS.GridListFilter filter)
        {
            ApplicationTitle();
            var id = filter.Id;
            ViewBag.crudMode = "U";
            ViewBag.AllowRead = AuthorizeAcces.AllowRead;
            ViewBag.AllowCreate = AuthorizeAcces.AllowCreated;
            ViewBag.AllowUpdate = AuthorizeAcces.AllowUpdated;
            ViewBag.AllowDelete = AuthorizeAcces.AllowDeleted;
            ViewBag.WizardData = Service.EMCS.SvcWizard.GetWizardData("si", id);
            ViewBag.SpecialInstructionDefault = Service.EMCS.MasterParameter.GetSingleParam("SIDocumentRequired");
            PaginatorBoot.Remove("SessionTRN");

            var detail = new CargoModel();
            detail.Data = Service.EMCS.SvcCargo.GetCargoById(id);
            detail.DataItem = Service.EMCS.SvcCargo.GetCargoDetailData(id);
            detail.DataSi = Service.EMCS.SvcShippingInstruction.GetById(id.ToString());
            return View(detail);
        }

        public ActionResult ShippingInstructionForm()
        {
            ApplicationTitle();
            ViewBag.AllowRead = AuthorizeAcces.AllowRead;
            ViewBag.AllowCreate = AuthorizeAcces.AllowCreated;
            ViewBag.AllowUpdate = AuthorizeAcces.AllowUpdated;
            ViewBag.AllowDelete = AuthorizeAcces.AllowDeleted;
            PaginatorBoot.Remove("SessionTRN");
            return View();
        }

        public ActionResult ShippingInstructionMethod()
        {
            ApplicationTitle();
            ViewBag.AllowRead = AuthorizeAcces.AllowRead;
            ViewBag.AllowCreate = AuthorizeAcces.AllowCreated;
            ViewBag.AllowUpdate = AuthorizeAcces.AllowUpdated;
            ViewBag.AllowDelete = AuthorizeAcces.AllowDeleted;
            PaginatorBoot.Remove("SessionTRN");
            return View();
        }

        public ActionResult ShippingInstructionView(long cargoId = 1 )
        {

            try
            {
                ApplicationTitle();
                string strQrCodeUrlEDI = Common.GenerateQrCode(cargoId, "downloadsl");
                ViewBag.QrCodeUrlSI = strQrCodeUrlEDI;
                TempData["QrCodeUrlSI"] = strQrCodeUrlEDI;
                TempData.Peek("QrCodeUrlSI");
                ViewBag.AllowRead = AuthorizeAcces.AllowRead;
                ViewBag.AllowCreate = AuthorizeAcces.AllowCreated;
                ViewBag.AllowUpdate = AuthorizeAcces.AllowUpdated;
                ViewBag.AllowDelete = AuthorizeAcces.AllowDeleted;
                ViewBag.CargoID = cargoId;
                var GetCargoId = Service.EMCS.SvcShippingInstruction.GetIdSi(cargoId);
                long NewCargoId = Convert.ToInt64(GetCargoId.IdCl);
                ViewBag.IdCargo = NewCargoId;
                ViewBag.WizardData = Service.EMCS.SvcWizard.GetWizardData("si", cargoId);
                PaginatorBoot.Remove("SessionTRN");

                Service.EMCS.DocumentStreamGenerator.GetCargoSi(NewCargoId);
                ViewBag.TemplateHeader = Service.EMCS.DocumentStreamGenerator.GetCargoHeaderData(NewCargoId);
                ViewBag.TemplateDetail = Service.EMCS.DocumentStreamGenerator.GetCargoDetailData(NewCargoId);

                CargoSiModel data = new CargoSiModel
                {
                    Header = Service.EMCS.DocumentStreamGenerator.GetCargoSiHeader(NewCargoId),
                    Detail = Service.EMCS.DocumentStreamGenerator.GetCargoSiDetail(NewCargoId),
                    Item = Service.EMCS.DocumentStreamGenerator.GetCargoSiDetailItem(NewCargoId),
                    ContainerTypes = Service.EMCS.DocumentStreamGenerator.GetCargoSiDetail(NewCargoId).Select(i => i.ContainerDescription).Distinct().ToList()
                };
                return View(data);
            }

            catch (Exception ex)
            {
                throw ex;
            }
        }

        public ActionResult ReportSi(long clId, string reportType)
        {
            string fileExcel = Server.MapPath("~\\Content\\EMCS\\Templates\\TemplateSI.xls");
            string fileName = "SIDocument_" + DateTime.Now.ToString("yyyyMMddHHmmss");
            string filePath = Server.MapPath("~\\Content\\EMCS\\Templates\\" + fileName);
            MemoryStream output = Service.EMCS.DocumentStreamGenerator.GetStream(clId, fileExcel, filePath, reportType);
            return File(output.ToArray(), "application/pdf", "SIDocument_" + DateTime.Now.ToString("ddMMyyyyhhhmmss") + ".pdf");    //Suggested file name in the "Save as" dialog which will be displayed to the end user
        }

        [HttpPost]
        public ActionResult GetDocumentSi(long idRequest)
        {
            List<Data.Domain.EMCS.Documents> documents = Service.EMCS.SvcShippingInstruction.GetDocumentsSi(idRequest);
            return Json(documents, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult UpdateSi(Data.Domain.EMCS.ShippingInstructions form)
        {
            ViewBag.crudMode = "U";
            var item = Service.EMCS.SvcShippingInstruction.GetById(form.IdCl);
            item.DocumentRequired = form.DocumentRequired;
            item.SpecialInstruction = form.SpecialInstruction;
            Service.EMCS.SvcRequestSi.Update(item, ViewBag.crudMode);

            return JsonCRUDMessage(ViewBag.crudMode);
        }

        [HttpPost]
        public ActionResult UploadDocumentSi(long idCargo, string noCargo, string typeDoc)
        {
            ViewBag.crudMode = "I";
            Data.Domain.EMCS.RequestCl step = Service.EMCS.SvcRequestCl.GetRequestCl(idCargo);
            if (step.IdCl != null)
            {
                string fileResult = DocumentSi(noCargo, typeDoc);
                if (fileResult != "")
                {
                    Service.EMCS.SvcShippingInstruction.InsertDocumentSi(step, fileResult, typeDoc);
                }
                else
                {
                    return Json(new { status = false, msg = "Upload File gagal" });
                }
            }

            return JsonCRUDMessage(ViewBag.crudMode);
        }

        public string DocumentSi(string noCargo, string typeDoc)
        {
            string fileName = "";
            string strFiles = "";

            if (Request.Files.Count > 0)
            {

                var file = Request.Files[0];

                if (file != null && file.ContentLength > 0)
                {
                    fileName = Path.GetFileName(file.FileName);
                    if (fileName != null) strFiles = fileName;
                    fileName = strFiles;

                    // Get Mime Type
                    var ext = Path.GetExtension(fileName);
                    var path = Server.MapPath("~/Upload/EMCS/SI/");
                    bool isExists = Directory.Exists(path);
                    fileName = noCargo + " " + typeDoc + ext;

                    if (!isExists)
                        Directory.CreateDirectory(path);

                    var fullPath = Path.Combine(path, fileName);


                    if (System.IO.File.Exists(fullPath))
                    {
                        System.IO.File.Delete(fullPath);
                    }

                    file.SaveAs(fullPath);
                    return fileName;
                }
            }
            return fileName;
        }

    }
}