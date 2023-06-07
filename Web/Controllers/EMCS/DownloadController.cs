using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using App.Web.Models.EMCS;
using System.Diagnostics;
using System.Threading;
using System.IO;
using Rotativa;
using NPOI.XSSF.UserModel;
using NPOI.SS.UserModel;
using System.Data;
using App.Domain;
<<<<<<< HEAD
<<<<<<< HEAD
using App.Data.Domain.EMCS;
using Newtonsoft.Json;
using App.Web.Helper;
using App.Web.App_Start;
<<<<<<< HEAD
=======

>>>>>>> 639d8d0 (Intial commit)
=======
using App.Data.Domain.EMCS;
using Newtonsoft.Json;
using App.Web.Helper;
>>>>>>> b773f28 (intial commit for changes from himanshu and vijendra)
=======
>>>>>>> d3e2e7a (Tasks from P1-CIPL , P1-CL , P!-SS , P!-SI , P1-BL/AWB & P1-PEB_NPE)

namespace App.Web.Controllers.EMCS
{
    public class DownloadController : Controller
    {
        public readonly EmcsController InitGoodReceive = new EmcsController();

        public CiplModel InitModelCipl(long id)
        {
            var detail = new CiplModel();
            detail.Data = Service.EMCS.SvcCipl.CiplGetById(id);
            detail.Forwarder = Service.EMCS.SvcCipl.CiplForwaderGetById(id);
            detail.DataItem = Service.EMCS.SvcCipl.CiplItemGetById(id);
            detail.TemplateHeader = Service.EMCS.DocumentStreamGenerator.GetCiplInvoicePlHeaderData(id);
            detail.TemplateDetail = Service.EMCS.DocumentStreamGenerator.GetCiplInvoicePlDetailData(id);
            detail.DataRequest = Service.EMCS.SvcRequestCipl.GetRequestById(id);

            List<string> items = new List<string>();
            foreach (var item in detail.DataItem.GroupBy(a => a.ReferenceNo))
            {
                items.Add(item.Key);
            }

            var detailTotalData = Service.EMCS.SvcTotalCipl.GetById(id);
            ViewBag.Quantity = detailTotalData.TotalPackage;
            ViewBag.Collies = detail.TemplateHeader.TotalCaseNumber;
            ViewBag.grossWeight = detailTotalData.TotalGrossWeight;
            ViewBag.netWeight = detailTotalData.TotalNetWeight;
            ViewBag.volume = detailTotalData.TotalVolume;
            ViewBag.refs = string.Join(",", items);
            ViewBag.Currency = detail.DataItem.Count != 0 ? detail.DataItem.FirstOrDefault()?.Currency : "";
            ViewBag.IdCust = detail.DataItem.Count != 0 ? detail.DataItem.FirstOrDefault()?.IdCustomer : "";

            return detail;
        }

        public string BaseUrl()
        {
            if (Request.Url != null)
            {
                string baseUrl = Request.Url.GetLeftPart(UriPartial.Authority);
                return baseUrl;
            }

            return null;
        }
<<<<<<< HEAD
<<<<<<< HEAD

=======
        
>>>>>>> 639d8d0 (Intial commit)
=======

>>>>>>> b773f28 (intial commit for changes from himanshu and vijendra)
        public CargoModel InitModelCargo(long id)
        {
            var detail = new CargoModel();
            detail.Data = Service.EMCS.SvcCargo.GetCargoById(id);
            detail.DataItem = Service.EMCS.SvcCargo.GetCargoDetailData(id);
            detail.TemplateHeader = Service.EMCS.DocumentStreamGenerator.GetCargoHeaderData(id);
            detail.TemplateDetail = Service.EMCS.DocumentStreamGenerator.GetCargoDetailData(id);

            return detail;
        }

        public GoodReceiveModel InitModelRg(long id)
        {
            GoodReceiveModel data = InitGoodReceive.InitGoodReceive(id);
            data.DetailGr = Service.EMCS.DocumentStreamGenerator.GetGrDetailData(id);
            return data;
        }
<<<<<<< HEAD
<<<<<<< HEAD
        public ActionResult GeneratePdf(CiplModel detail, string view, string typeDoc = "Invoice", GoodReceiveModel gr = null, CargoModel cargo = null,CargoSiModel cargoSiModel = null,CargoSsModel cargoSsModel = null)
=======

        public ActionResult GeneratePdf(CiplModel detail, string view, string typeDoc = "Invoice", GoodReceiveModel gr = null, CargoModel cargo = null)
>>>>>>> 639d8d0 (Intial commit)
=======
        public ActionResult GeneratePdf(CiplModel detail, string view, string typeDoc = "Invoice", GoodReceiveModel gr = null, CargoModel cargo = null,CargoSiModel cargoSiModel = null,CargoSsModel cargoSsModel = null)
>>>>>>> d3e2e7a (Tasks from P1-CIPL , P1-CL , P!-SS , P!-SI , P1-BL/AWB & P1-PEB_NPE)
        {
            string headerBlank = Server.MapPath("~/Views/Download/CustomBlankHeader.html");//Path of PrintHeader.html File

            string headerContent;
            if (typeDoc == "Invoice")
            {
<<<<<<< HEAD
<<<<<<< HEAD
=======
>>>>>>> b773f28 (intial commit for changes from himanshu and vijendra)
                var qrCodeInvoiceValue = TempData["QrCodeUrlInvoice"];
                if (qrCodeInvoiceValue == null)
                {
                    string strQrCodeUrlInvoice = Common.GenerateQrCode(detail.Data.Id, "downloadInvoice");
                    qrCodeInvoiceValue = strQrCodeUrlInvoice;
                }
                ViewBag.QrCodeUrlInvoice = qrCodeInvoiceValue;
<<<<<<< HEAD
=======
>>>>>>> 639d8d0 (Intial commit)
=======
>>>>>>> b773f28 (intial commit for changes from himanshu and vijendra)
                headerContent = RenderPartialViewToString("~/Views/Download/CustomHeaderCiplInv.cshtml", detail);
            }
            else if (typeDoc == "Pl")
            {
<<<<<<< HEAD
<<<<<<< HEAD
=======
>>>>>>> b773f28 (intial commit for changes from himanshu and vijendra)
                var qrCodelPLValue = TempData["QrCodeUrlPL"];
                if (qrCodelPLValue == null)
                {
                    string strQrCodeUrlPL = Common.GenerateQrCode(detail.Data.Id, "DownloadPl");
                    qrCodelPLValue = strQrCodeUrlPL;
                }
                ViewBag.QrCodeUrlPL = qrCodelPLValue;
<<<<<<< HEAD
=======
>>>>>>> 639d8d0 (Intial commit)
=======
>>>>>>> b773f28 (intial commit for changes from himanshu and vijendra)
                headerContent = RenderPartialViewToString("~/Views/Download/CustomHeaderCiplInvPl.cshtml", detail);
            }
            else if (typeDoc == "Rg")
            {
<<<<<<< HEAD
<<<<<<< HEAD
=======
>>>>>>> b773f28 (intial commit for changes from himanshu and vijendra)
                var qrCodeUrlGRValue = TempData["QrCodeUrlGR"];
                if (qrCodeUrlGRValue == null)
                {
                    string strQrCodeUrlGR = Common.GenerateQrCode(detail.Data.Id, "DownloadRg");
                    qrCodeUrlGRValue = strQrCodeUrlGR;
                }
                ViewBag.QrCodeUrlGR = qrCodeUrlGRValue;
<<<<<<< HEAD
=======
>>>>>>> 639d8d0 (Intial commit)
=======
>>>>>>> b773f28 (intial commit for changes from himanshu and vijendra)
                headerContent = RenderPartialViewToString("~/Views/Download/CustomHeaderRg.cshtml", gr);
            }
            else if (typeDoc == "Cargo")
            {
                headerContent = RenderPartialViewToString("~/Views/Download/CustomHeaderCargo.cshtml", cargo);
            }
<<<<<<< HEAD
<<<<<<< HEAD
=======
>>>>>>> d3e2e7a (Tasks from P1-CIPL , P1-CL , P!-SS , P!-SI , P1-BL/AWB & P1-PEB_NPE)
            else if (typeDoc == "Si")
            {
                headerContent = RenderPartialViewToString("~/Views/Download/CustomHeaderSi.cshtml", cargoSiModel);
            }
<<<<<<< HEAD
            else if (typeDoc == "Ss")
            {
                headerContent = RenderPartialViewToString("~/Views/Download/CustomHeaderSs.cshtml", cargoSsModel);
            }
            else
            {
                var qrCodeValue = TempData["QrCodeUrlEDI"];
                if (qrCodeValue == null)
                {
                    string strQrCodeUrlEDI = Common.GenerateQrCode(detail.Data.Id, "downloadedi");
                    qrCodeValue = strQrCodeUrlEDI;
                }
                ViewBag.QrCodeUrlEDI = qrCodeValue;
=======
=======
>>>>>>> d3e2e7a (Tasks from P1-CIPL , P1-CL , P!-SS , P!-SI , P1-BL/AWB & P1-PEB_NPE)
            else if (typeDoc == "Ss")
            {
                headerContent = RenderPartialViewToString("~/Views/Download/CustomHeaderSs.cshtml", cargoSsModel);
            }
            else
<<<<<<< HEAD
<<<<<<< HEAD
            {
>>>>>>> 639d8d0 (Intial commit)
=======
            {               
=======
            {
>>>>>>> d3e2e7a (Tasks from P1-CIPL , P1-CL , P!-SS , P!-SI , P1-BL/AWB & P1-PEB_NPE)
                var qrCodeValue = TempData["QrCodeUrlEDI"];
                if (qrCodeValue == null)
                {
                    string strQrCodeUrlEDI = Common.GenerateQrCode(detail.Data.Id, "downloadedi");
                    qrCodeValue = strQrCodeUrlEDI;
                }
                ViewBag.QrCodeUrlEDI = qrCodeValue;
>>>>>>> b773f28 (intial commit for changes from himanshu and vijendra)
                headerContent = RenderPartialViewToString("~/Views/Download/CustomHeaderCiplEdi.cshtml", detail);
            }

            using (var sw = new StreamWriter(new FileStream(headerBlank, FileMode.Create, FileAccess.Write)))
            {
                sw.Write(headerContent);
                string customSwitches = string.Format("--header-html  \"{0}\" " +
                                                      "--page-size A4 " +
                                                      "--dpi 96 " +
                                                      "--print-media-type " +
                                                      "--outline " +
                                                      "--header-spacing \"1\"  " +
                                                      "--header-font-size \"10\" ", headerBlank);

                var dataview = new ViewAsPdf(view, detail)
                {
                    IsJavaScriptDisabled = false,
                    CustomSwitches = customSwitches,
                    PageOrientation = Rotativa.Options.Orientation.Portrait,
                    PageSize = Rotativa.Options.Size.A4,
                    PageMargins = new Rotativa.Options.Margins(55, 3, 32, 3),
                };

                if (typeDoc == "Rg")
                {
                    dataview = new ViewAsPdf(view, gr)
                    {
                        IsJavaScriptDisabled = false,
                        CustomSwitches = customSwitches,
                        PageOrientation = Rotativa.Options.Orientation.Portrait,
                        PageSize = Rotativa.Options.Size.A4,
                        PageMargins = new Rotativa.Options.Margins(55, 3, 32, 3),
                    };
                }
<<<<<<< HEAD
<<<<<<< HEAD
=======
>>>>>>> d3e2e7a (Tasks from P1-CIPL , P1-CL , P!-SS , P!-SI , P1-BL/AWB & P1-PEB_NPE)
                else if (typeDoc == "Si")
                {
                    dataview = new ViewAsPdf(view, cargoSiModel)
                    {
                        IsJavaScriptDisabled = false,
                        CustomSwitches = customSwitches,
                        PageOrientation = Rotativa.Options.Orientation.Portrait,
                        PageSize = Rotativa.Options.Size.A4,
                        PageMargins = new Rotativa.Options.Margins(55, 3, 32, 3),
                    };
                }
                else if (typeDoc == "Ss")
                {
                    dataview = new ViewAsPdf(view, cargoSsModel)
                    {
                        IsJavaScriptDisabled = false,
                        CustomSwitches = customSwitches,
                        PageOrientation = Rotativa.Options.Orientation.Portrait,
                        PageSize = Rotativa.Options.Size.A4,
                        PageMargins = new Rotativa.Options.Margins(55, 3, 32, 3),
                    };
                }
<<<<<<< HEAD
=======
>>>>>>> 639d8d0 (Intial commit)
=======
>>>>>>> d3e2e7a (Tasks from P1-CIPL , P1-CL , P!-SS , P!-SI , P1-BL/AWB & P1-PEB_NPE)
                else if (typeDoc == "Cargo")
                {
                    dataview = new ViewAsPdf(view, cargo)
                    {
                        IsJavaScriptDisabled = false,
                        CustomSwitches = customSwitches,
                        PageOrientation = Rotativa.Options.Orientation.Portrait,
                        PageSize = Rotativa.Options.Size.A4,
                        PageMargins = new Rotativa.Options.Margins(55, 3, 32, 3),
                    };
                }

                sw.Flush();
                sw.Close();

                return dataview;
            }
        }

        protected string RenderPartialViewToString(string viewName, object model)
        {
<<<<<<< HEAD
<<<<<<< HEAD
            try
            {
                if (string.IsNullOrEmpty(viewName))
                    viewName = ControllerContext.RouteData.GetRequiredString("action");

                if (model != null)
                    ViewData.Model = model;

                using (StringWriter sw = new StringWriter())
                {
                    ViewEngineResult viewResult = ViewEngines.Engines.FindPartialView(ControllerContext, viewName);
                    ViewContext viewContext = new ViewContext(ControllerContext, viewResult.View, ViewData, TempData, sw);
                    viewResult.View.Render(viewContext, sw);

                    return sw.GetStringBuilder().ToString();
                }
            }
            catch (Exception ex)
            {
                throw ex;
=======
            if (string.IsNullOrEmpty(viewName))
                viewName = ControllerContext.RouteData.GetRequiredString("action");

            if (model != null)
                ViewData.Model = model;

            using (StringWriter sw = new StringWriter())
=======
            try
>>>>>>> d3e2e7a (Tasks from P1-CIPL , P1-CL , P!-SS , P!-SI , P1-BL/AWB & P1-PEB_NPE)
            {
                if (string.IsNullOrEmpty(viewName))
                    viewName = ControllerContext.RouteData.GetRequiredString("action");

<<<<<<< HEAD
                return sw.GetStringBuilder().ToString();
>>>>>>> 639d8d0 (Intial commit)
=======
                if (model != null)
                    ViewData.Model = model;

                using (StringWriter sw = new StringWriter())
                {
                    ViewEngineResult viewResult = ViewEngines.Engines.FindPartialView(ControllerContext, viewName);
                    ViewContext viewContext = new ViewContext(ControllerContext, viewResult.View, ViewData, TempData, sw);
                    viewResult.View.Render(viewContext, sw);

                    return sw.GetStringBuilder().ToString();
                }
            }
            catch (Exception ex)
            {
                throw ex;
>>>>>>> d3e2e7a (Tasks from P1-CIPL , P1-CL , P!-SS , P!-SI , P1-BL/AWB & P1-PEB_NPE)
            }
        }

        public ActionResult DownloadInvoice(long id)
        {
            var detail = InitModelCipl(id);
            ViewBag.typeDocument = "INVOICE";
            string View = "DownloadInvoice";
            return GeneratePdf(detail, View);
        }

        public ActionResult DownloadPl(long id)
        {
            var detail = InitModelCipl(id);
            ViewBag.TypeDocument = "PACKING LIST";
            string View = "DownloadPl";
            return GeneratePdf(detail, View, "Pl");
        }
<<<<<<< HEAD
<<<<<<< HEAD
=======
>>>>>>> d3e2e7a (Tasks from P1-CIPL , P1-CL , P!-SS , P!-SI , P1-BL/AWB & P1-PEB_NPE)
        //public ActionResult DownloadSi(long id)
        //{
        //    var detail = InitModelCargo(id);
        //    ViewBag.TypeDocument = "Shipping Information";
        //    string View = "DownloadSi";
        //    return GeneratePdf(detail, View, "Si");
        //}
<<<<<<< HEAD
=======

>>>>>>> 639d8d0 (Intial commit)
=======
>>>>>>> d3e2e7a (Tasks from P1-CIPL , P1-CL , P!-SS , P!-SI , P1-BL/AWB & P1-PEB_NPE)
        public ActionResult DownloadEdi(long id)
        {
            var detail = InitModelCipl(id);
            ViewBag.typeDocument = "Export Delivery Instruction";
            return GeneratePdf(detail, "DownloadEdi", "Edi");
        }

        public ActionResult DownloadRg(long id)
        {
            var detail = InitModelRg(id);
            ViewBag.typeDocument = "Export Receipt Goods";
            return GeneratePdf(null, "DownloadRg", "Rg", detail);
        }

        public ActionResult DownloadCargo(long id)
        {
            var detail = InitModelCargo(id);
            return GeneratePdf(null, "DownloadCargo", "Cargo", null, detail);
        }

        public ActionResult DownloadSs(long id)
        {
<<<<<<< HEAD
<<<<<<< HEAD
=======
>>>>>>> d3e2e7a (Tasks from P1-CIPL , P1-CL , P!-SS , P!-SI , P1-BL/AWB & P1-PEB_NPE)
            string strQrCodeUrlEDI = Common.GenerateQrCode(id, "downloadss");
            ViewBag.QrCodeUrlSS = strQrCodeUrlEDI;
            TempData["QrCodeUrlSS"] = strQrCodeUrlEDI;
            TempData.Peek("QrCodeUrlSS");
            ViewBag.AllowRead = AuthorizeAcces.AllowRead;
            ViewBag.AllowCreate = AuthorizeAcces.AllowCreated;
            ViewBag.AllowUpdate = AuthorizeAcces.AllowUpdated;
            ViewBag.AllowDelete = AuthorizeAcces.AllowDeleted;
            ViewBag.WizardData = Service.EMCS.SvcWizard.GetWizardData("ss", id);
            ViewBag.CargoID = id;

            ViewBag.TemplateHeader = Service.EMCS.DocumentStreamGenerator.GetCargoHeaderData(id);
            ViewBag.TemplateDetail = Service.EMCS.DocumentStreamGenerator.GetCargoDetailData(id);

            CargoSsModel data = new CargoSsModel
            {
                Header = Service.EMCS.DocumentStreamGenerator.GetCargoSsHeader(id),
                Detail = Service.EMCS.DocumentStreamGenerator.GetCargoSsDetail(id)
            };
            return GeneratePdf(null, "DownloadSs", "Ss", null, null,null, data);
        }
        public ActionResult DownloadSi(int id)
        {
           
            try
            {
                string strQrCodeUrlEDI = Common.GenerateQrCode(id, "downloadsl");
                ViewBag.QrCodeUrlSI = strQrCodeUrlEDI;
                TempData["QrCodeUrlSI"] = strQrCodeUrlEDI;
                TempData.Peek("QrCodeUrlSI");
                ViewBag.AllowRead = AuthorizeAcces.AllowRead;
                ViewBag.AllowCreate = AuthorizeAcces.AllowCreated;
                ViewBag.AllowUpdate = AuthorizeAcces.AllowUpdated;
                ViewBag.AllowDelete = AuthorizeAcces.AllowDeleted;
                ViewBag.CargoID = id;
                var GetCargoId = Service.EMCS.SvcShippingInstruction.GetIdSi(id);
                long NewCargoId = Convert.ToInt64(GetCargoId.IdCl);
                ViewBag.IdCargo = NewCargoId;
                ViewBag.WizardData = Service.EMCS.SvcWizard.GetWizardData("si", id);

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
                return GeneratePdf(null, "DownloadSi", "Si", null, null, data);
            }
            catch (Exception ex)
            {
                throw ex;
            }

           
<<<<<<< HEAD
        }


=======
            var detail = InitModelCargo(id);
            return View(detail);
=======
>>>>>>> d3e2e7a (Tasks from P1-CIPL , P1-CL , P!-SS , P!-SI , P1-BL/AWB & P1-PEB_NPE)
        }


>>>>>>> 639d8d0 (Intial commit)
        public ActionResult DownloadStatement(int id)
        {
            string serverPath = HttpContext.Server.MapPath("~/Phantomjs/");
            string filename = DateTime.Now.ToString("ddMMyyyy_hhmmss") + ".pdf";
            string urlAction = this.BaseUrl() + "/emcs/DownloadInvoice/" + id;
            string pdfPath = Server.MapPath("~/Reports/PDF/");

            new Thread(x =>
            {
                ExecuteCommand("cd " + serverPath + @" & phantomjs rasterize.js " + urlAction + " " + pdfPath + @" ""A4""");
            }).Start();

            var filePath = Path.Combine(HttpContext.Server.MapPath("~/Phantomjs/"), filename);

            var bytes = DoWhile(filePath);

            return File(bytes, "application/pdf", filename);
        }

<<<<<<< HEAD
<<<<<<< HEAD

        public FileResult DownloadCIPLItem(long id)
        {
=======
        
        public FileResult DownloadCIPLItem(long id)
        {           
>>>>>>> 639d8d0 (Intial commit)
=======

        public FileResult DownloadCIPLItem(long id)
        {
>>>>>>> b773f28 (intial commit for changes from himanshu and vijendra)
            try
            {
                var data = Service.EMCS.DocumentStreamGenerator.GetStreamCiplItem(id);
                MemoryStream output = new MemoryStream();
                output = Service.EMCS.DocumentStreamGenerator.DownloaddataExcel(data);
                return File(output.ToArray(),   //The binary data of the XLS file
                    "application/vnd.ms-excel",//MIME type of Excel files
                    "CIPLItem.xlsx");
                //Return the result to the end user
                //Suggested file name in the "Save as" dialog which will be displayed to the end user
            }
            catch (Exception ex)
            {
                //Write the Workbook to a memory stream
                MemoryStream output = new MemoryStream();
                //Return the result to the end user
                return File(output.ToArray(),   //The binary data of the XLS file
                 "application/vnd.ms-excel",//MIME type of Excel files
                 "CIPLItem.xlsx");    //Suggested file name in the "Save as" dialog which will be displayed to the end user
            }
        }

        public FileResult DownloadToExcel(string guid)
        {
            return Session[guid] as FileResult;
        }
        public ActionResult DownloadExcel(long id)
        {
            Guid guid = Guid.NewGuid();
            Session[guid.ToString()] = DownloadCIPLItem(id);
            return Json(guid.ToString(), JsonRequestBehavior.AllowGet);
        }
        public ActionResult ShowPdf(long id)
        {
            return View();
        }
<<<<<<< HEAD
<<<<<<< HEAD

=======
        
>>>>>>> 639d8d0 (Intial commit)
=======

>>>>>>> b773f28 (intial commit for changes from himanshu and vijendra)
        private void ExecuteCommand(string command)
        {
            try
            {
                ProcessStartInfo ProcessInfo;

                ProcessInfo = new ProcessStartInfo("cmd.exe", "/K " + command);
                ProcessInfo.CreateNoWindow = true;
                ProcessInfo.UseShellExecute = false;

                Process.Start(ProcessInfo);
            }
            catch (Exception)
            {
                // ignored
            }
        }

        public ViewResult FileToPdf(int id)
        {
            var viewModel = "";
            return View(viewModel);
        }

        private byte[] DoWhile(string filePath)
        {
            byte[] bytes = new byte[0];
            bool fail = true;

            while (fail)
            {
                try
                {
                    using (FileStream file = new FileStream(filePath, FileMode.Open, FileAccess.Read))
                    {
                        bytes = new byte[file.Length];
                        file.Read(bytes, 0, (int)file.Length);
                    }

                    fail = false;
                }
                catch
                {
                    Thread.Sleep(1000);
                }
            }

            System.IO.File.Delete(filePath);
            return bytes;
        }

        [HttpPost]
<<<<<<< HEAD
<<<<<<< HEAD
        public JsonResult UploadCIPLItem(long idCIPL, string idReference, string refCIPL, string ciplFormModelFormData)
        {
            try
            {
                CiplFormModel objCiplFormModel = new CiplFormModel();
                if (idCIPL == 0)
                {
                    objCiplFormModel = JsonConvert.DeserializeObject<CiplFormModel>(ciplFormModelFormData);

                }

                if (UploadFile("CIPLItem", "TemplateCatepillarSparePart"))
                {
                    return GetFileNameCIPLItem(idCIPL, idReference, refCIPL, objCiplFormModel);
=======
        public JsonResult UploadCIPLItem(long idCIPL,string idReference,string refCIPL)
=======
        public JsonResult UploadCIPLItem(long idCIPL, string idReference, string refCIPL, string ciplFormModelFormData)
>>>>>>> b773f28 (intial commit for changes from himanshu and vijendra)
        {
            try
            {
                CiplFormModel objCiplFormModel = new CiplFormModel();
                if (idCIPL == 0)
                {
                    objCiplFormModel = JsonConvert.DeserializeObject<CiplFormModel>(ciplFormModelFormData);

                }

                if (UploadFile("CIPLItem", "TemplateCatepillarSparePart"))
                {
<<<<<<< HEAD
                    return GetFileNameCIPLItem(idCIPL, idReference, refCIPL);
>>>>>>> 639d8d0 (Intial commit)
=======
                    return GetFileNameCIPLItem(idCIPL, idReference, refCIPL, objCiplFormModel);
>>>>>>> b773f28 (intial commit for changes from himanshu and vijendra)
                }
                else
                {
                    return Json(new { status = false, msg = "Upload File gagal" });
                }
            }
            catch (Exception err)
            {
                return Json(new { status = false, msg = err.Message });
            }
        }

<<<<<<< HEAD
<<<<<<< HEAD
        public JsonResult GetFileNameCIPLItem(long idCIPL, string idReference, string refCIPL, CiplFormModel objCiplFormModel)
=======
        public JsonResult GetFileNameCIPLItem(long idCIPL, string idReference,string refCIPL)
>>>>>>> 639d8d0 (Intial commit)
=======
        public JsonResult GetFileNameCIPLItem(long idCIPL, string idReference, string refCIPL, CiplFormModel objCiplFormModel)
>>>>>>> b773f28 (intial commit for changes from himanshu and vijendra)
        {
            try
            {
                XSSFWorkbook xssf;
                ISheet sheet;
                string FullPath = @"~\Content\EMCS\Templates\TemplateCatepillarSparePart.xlsx";

                using (FileStream file = new FileStream(Server.MapPath(FullPath), FileMode.Open, FileAccess.Read))
                {
                    string extension = Path.GetExtension(FullPath);
                    if (extension == ".xlsx")
                    {
                        xssf = new XSSFWorkbook(file);
                        sheet = xssf.GetSheet("CIPLItem");
<<<<<<< HEAD
<<<<<<< HEAD

                        var tuple = CheckFileData(sheet, refCIPL, idReference);
                        if (tuple.Item2 == "success")
                        {
                            if (idCIPL == 0)
                            {
                                objCiplFormModel.Data.ReferenceNo = idReference;
                                var data = Service.EMCS.SvcCipl.InsertCipl(objCiplFormModel.Forwader, objCiplFormModel.Data, "I", "Draft");
                                if (data != null)
                                {
                                    idCIPL = data.Select(x => x.Id).First();
                                }

                            }
                            if (GetCIPLItemDataTable(sheet, idCIPL, idReference))
                            {
                                ViewBag.crudMode = "I";

                                return Json(new { idCIPL = idCIPL, status = true, msg = "Upload file successfully" });
=======
                        
=======

>>>>>>> b773f28 (intial commit for changes from himanshu and vijendra)
                        var tuple = CheckFileData(sheet, refCIPL, idReference);
                        if (tuple.Item2 == "success")
                        {
                            if (idCIPL == 0)
                            {
                                objCiplFormModel.Data.ReferenceNo = idReference;
                                var data = Service.EMCS.SvcCipl.InsertCipl(objCiplFormModel.Forwader, objCiplFormModel.Data, "I", "Draft");
                                if (data != null)
                                {
                                    idCIPL = data.Select(x => x.Id).First();
                                }

                            }
                            if (GetCIPLItemDataTable(sheet, idCIPL, idReference))
                            {
                                ViewBag.crudMode = "I";
<<<<<<< HEAD
                                return Json(new { status = true, msg = "Upload file successfully" });
>>>>>>> 639d8d0 (Intial commit)
=======

                                return Json(new { idCIPL = idCIPL, status = true, msg = "Upload file successfully" });
>>>>>>> b773f28 (intial commit for changes from himanshu and vijendra)
                            }
                            return Json(new { status = true, msg = "Upload file successfully" });
                        }
                        else
                        {
<<<<<<< HEAD
<<<<<<< HEAD
                            return Json(new { status = false, msg = tuple.Item1 });
=======
                            return Json(new { status = false, msg = tuple.Item1});
>>>>>>> 639d8d0 (Intial commit)
=======
                            return Json(new { status = false, msg = tuple.Item1 });
>>>>>>> b773f28 (intial commit for changes from himanshu and vijendra)
                        }
                    }
                    else
                    {
                        return Json(new { status = false, msg = "File Extenstion is not Valid" });
                    }
                }
            }
            catch (Exception err)
            {
                throw;
            }
        }
        public bool UploadFile(string dir, string defaultFileNamexlsx)
        {
            try
            {
                if (Request.Files.Count > 0)
                {
                    var file = Request.Files[0];

                    if (file != null && file.ContentLength > 0)
                    {
                        return UploadCIPLItemFile(file, dir, defaultFileNamexlsx);
                    }
                }
                return false;
            }
            catch (Exception err)
            {
                return false;
            }
        }
<<<<<<< HEAD
<<<<<<< HEAD
        public Tuple<string, string> CheckFileData(ISheet sheet, string refCIPL, string idReference)
        {

            var tuple = new Tuple<string, string>("", "");
=======
        public Tuple<string, string> CheckFileData(ISheet sheet,string refCIPL, string idReference)
        {

            var tuple = new Tuple<string, string>("","");
>>>>>>> 639d8d0 (Intial commit)
=======
        public Tuple<string, string> CheckFileData(ISheet sheet, string refCIPL, string idReference)
        {

            var tuple = new Tuple<string, string>("", "");
>>>>>>> b773f28 (intial commit for changes from himanshu and vijendra)
            for (var i = 1; i <= sheet.LastRowNum; i++)
            {
                if (sheet.GetRow(i) != null)
                {
                    for (int cellnum = 0; cellnum <= 18; cellnum++)
                    {
                        if (idReference == "")
                        {
                            string message = "failed";
                            string messagedesc = "Please Choose Reference";
                            tuple = new Tuple<string, string>(messagedesc, message);
                        }
                        if (cellnum == 10)
                        {
                            string message = "success";
                            string messagedesc = "";
                            tuple = new Tuple<string, string>(messagedesc, message);
                        }
                        else
                        {
                            if (cellnum == 2)
                            {
                                tuple = CheckDecVal(sheet, i, cellnum, 0);
                            }
                            else
                            {
                                tuple = CheckStringVal(sheet, i, cellnum, 0, refCIPL);
                            }
                        }

<<<<<<< HEAD
<<<<<<< HEAD

=======
                      
>>>>>>> 639d8d0 (Intial commit)
=======

>>>>>>> b773f28 (intial commit for changes from himanshu and vijendra)
                        if (tuple.Item2 != "success")
                        {
                            break;
                        }
<<<<<<< HEAD
<<<<<<< HEAD
                    }
=======
                    }                 
>>>>>>> 639d8d0 (Intial commit)
=======
                    }
>>>>>>> b773f28 (intial commit for changes from himanshu and vijendra)
                }
                if (tuple.Item2 != "success")
                {
                    break;
                }
            }
            return tuple;


        }
<<<<<<< HEAD
<<<<<<< HEAD
        public Tuple<string, string> CheckStringVal(ISheet sheet, int numRow, int cellNum, int isNum, string refCIPL)
=======
        public Tuple<string, string> CheckStringVal(ISheet sheet, int numRow, int cellNum, int isNum,string refCIPL)
>>>>>>> 639d8d0 (Intial commit)
=======
        public Tuple<string, string> CheckStringVal(ISheet sheet, int numRow, int cellNum, int isNum, string refCIPL)
>>>>>>> b773f28 (intial commit for changes from himanshu and vijendra)
        {
            string message = "";
            string value = "";
            string messagedesc = "";
            try
            {
<<<<<<< HEAD
<<<<<<< HEAD

                if (sheet.GetRow(numRow).GetCell(cellNum) != null)
                {
                    //value = sheet.GetRow(numRow).GetCell(cellNum).StringCellValue;
                    //Note : above condition is not working for string cell value if cell value is null
                    value = Convert.ToString(sheet.GetRow(numRow).GetCell(cellNum));
                    if (cellNum == 0)
=======
              
                if (sheet.GetRow(numRow).GetCell(cellNum)!=null)
                {
                    value = sheet.GetRow(numRow).GetCell(cellNum).StringCellValue;
                    if (cellNum == 0 )
>>>>>>> 639d8d0 (Intial commit)
=======

                if (sheet.GetRow(numRow).GetCell(cellNum) != null)
                {
                    //value = sheet.GetRow(numRow).GetCell(cellNum).StringCellValue;
                    //Note : above condition is not working for string cell value if cell value is null
                    value = Convert.ToString(sheet.GetRow(numRow).GetCell(cellNum));
                    if (cellNum == 0)
>>>>>>> b773f28 (intial commit for changes from himanshu and vijendra)
                    {
                        if (!refCIPL.Contains(value))
                        {
                            message = "failed";
                            messagedesc = "Data Column " + GetCellName(cellNum) + " Row " + numRow + " Reference CIPL Not Match with App";
                        }
                        else
                        {
                            message = "success";
                            messagedesc = "";
                        }
                    }
                    else if (cellNum > 0 && cellNum < 7 || cellNum > 8 && cellNum < 13)
                    {
                        message = "success";
                        messagedesc = "";
                    }
                    else if (cellNum == 7 || cellNum == 8 || cellNum == 10 || cellNum >= 13)
                    {

<<<<<<< HEAD
<<<<<<< HEAD
                        if (isDecimal(value))
=======
                        if (isDecimal(value))                        
>>>>>>> 639d8d0 (Intial commit)
=======
                        if (isDecimal(value))
>>>>>>> b773f28 (intial commit for changes from himanshu and vijendra)
                        {
                            message = "success";
                            messagedesc = "";
                        }
                        // It's a decimal
                        else
                        {
                            message = "failed";
                            messagedesc = "Data Column " + GetCellName(cellNum) + " Row " + numRow + " not decimal value";
                        }
<<<<<<< HEAD
<<<<<<< HEAD
                    }
                }
                else
                {
=======
                    }                 
                }
                else
                {                   
>>>>>>> 639d8d0 (Intial commit)
=======
                    }
                }
                else
                {
>>>>>>> b773f28 (intial commit for changes from himanshu and vijendra)
                    if (cellNum == 10)
                    {
                        message = "failed";
                        messagedesc = "Data Column " + GetCellName(cellNum) + " Row " + numRow + " Empty";
                    }
                    else
                    {
                        message = "success";
                        messagedesc = "";
<<<<<<< HEAD
<<<<<<< HEAD
                    }
                }

=======
                    }                  
                }
             
>>>>>>> 639d8d0 (Intial commit)
=======
                    }
                }

>>>>>>> b773f28 (intial commit for changes from himanshu and vijendra)
            }
            catch (Exception e)
            {
                message = "failed";
                messagedesc = "Data Column " + GetCellName(cellNum) + " Row " + numRow + " Not Character value";
            }
            var tuple = new Tuple<string, string>(messagedesc, message);
            return tuple;
        }
        public bool isDecimal(string value)
        {

            try
            {
                Decimal.Parse(value);
                return true;
            }
            catch
            {
                return false;
            }
        }
        public string GetCellName(int cell)
        {
<<<<<<< HEAD
<<<<<<< HEAD

            string coloum = Service.EMCS.SvcCipl.GetCellDataName(cell);

=======
            
            string coloum = Service.EMCS.SvcCipl.GetCellDataName(cell);
            
>>>>>>> 639d8d0 (Intial commit)
=======

            string coloum = Service.EMCS.SvcCipl.GetCellDataName(cell);

>>>>>>> b773f28 (intial commit for changes from himanshu and vijendra)
            return coloum;
        }

        public Tuple<string, string> CheckDecVal(ISheet sheet, int numRow, int cellNum, int isNum)
        {

            string message = "";
            string messagedesc = "";
            string value = "";
            try
<<<<<<< HEAD
<<<<<<< HEAD
            {
=======
            {              
>>>>>>> 639d8d0 (Intial commit)
=======
            {
>>>>>>> b773f28 (intial commit for changes from himanshu and vijendra)
                if (sheet.GetRow(numRow).GetCell(cellNum) != null)
                {
                    value = sheet.GetRow(numRow).GetCell(cellNum).NumericCellValue.ToString();
                    message = "success";
                    messagedesc = "";
                }
                else
<<<<<<< HEAD
<<<<<<< HEAD
                {
=======
                {                 
>>>>>>> 639d8d0 (Intial commit)
=======
                {
>>>>>>> b773f28 (intial commit for changes from himanshu and vijendra)
                    message = "failed";
                    messagedesc = "Data Column " + GetCellName(cellNum) + " Row " + numRow + " Empty";
                }

            }
            catch (Exception e)
            {
                message = "failed";
                messagedesc = "Data Column " + GetCellName(cellNum) + " Row " + numRow + " Not Numeric Value";
            }
            var tuple = new Tuple<string, string>(messagedesc, message);
            return tuple;
        }

        public bool UploadCIPLItemFile(System.Web.HttpPostedFileBase file, string dir, string defaultFileNamexlsx)
        {
            var fileName = Path.GetFileName(file.FileName);

            // Get Mime Type
            var ext = Path.GetExtension(fileName);
            if (ext == ".xlsx")
            {
                var path = @"~\Content\EMCS\Templates\";
                bool isExists = System.IO.Directory.Exists(Server.MapPath(path));

                if (!isExists)
                    Directory.CreateDirectory(path);

                var fullPath = Path.Combine(Server.MapPath(path), defaultFileNamexlsx + ".xlsx");


                if (System.IO.File.Exists(fullPath))
                {
                    System.IO.File.Delete(fullPath);
                }

                file.SaveAs(fullPath);
                return true;
            }
            else
            {
                return false;
            }
        }

        public bool GetCIPLItemDataTable(ISheet sheet, long idCIPL, string idReference)
        {
            try
            {
                // TempTableCIPLItem
                DataTable dt = new DataTable("CiplItem");
                dt.Columns.Add("Id");
                dt.Columns.Add("IdCipl");
                dt.Columns.Add("IdReference");
                dt.Columns.Add("ReferenceNo");
                dt.Columns.Add("IdCustomer");
                dt.Columns.Add("Name");
                dt.Columns.Add("Uom");
                dt.Columns.Add("PartNumber");
                dt.Columns.Add("Sn");
                dt.Columns.Add("JCode");
                dt.Columns.Add("Ccr");
                dt.Columns.Add("CaseNumber");
                dt.Columns.Add("Type");
                dt.Columns.Add("IdNo");
                dt.Columns.Add("YearMade");
                dt.Columns.Add("Quantity");
                dt.Columns.Add("UnitPrice");
                dt.Columns.Add("ExtendedValue");
                dt.Columns.Add("Length");
                dt.Columns.Add("Width");
                dt.Columns.Add("Height");
                dt.Columns.Add("Volume");
                dt.Columns.Add("GrossWeight");
                dt.Columns.Add("NetWeight");
                dt.Columns.Add("Currency");
                dt.Columns.Add("CreateBy");
                dt.Columns.Add("CreateDate");
                dt.Columns.Add("UpdateBy");
                dt.Columns.Add("UpdateDate");
                dt.Columns.Add("IsDelete");
                dt.Columns.Add("CoO");
                dt.Columns.Add("IdParent");
                dt.Columns.Add("SIBNumber");
<<<<<<< HEAD
<<<<<<< HEAD
                dt.Columns.Add("WONumber");
=======
                dt.Columns.Add("WONumber"); 
>>>>>>> 639d8d0 (Intial commit)
=======
                dt.Columns.Add("WONumber");
>>>>>>> b773f28 (intial commit for changes from himanshu and vijendra)
                dt.Columns.Add("Claim");
                dt.Columns.Add("ASNNumber");

                DataRow dr;
                for (var i = 1; i <= sheet.LastRowNum; i++)
                {
                    if (sheet.GetRow(i) != null)
                    {
<<<<<<< HEAD
<<<<<<< HEAD
                        dr = AddDataRow(dt, sheet, i, idCIPL, idReference);
=======
                        dr = AddDataRow(dt, sheet, i, idCIPL,idReference);
>>>>>>> 639d8d0 (Intial commit)
=======
                        dr = AddDataRow(dt, sheet, i, idCIPL, idReference);
>>>>>>> b773f28 (intial commit for changes from himanshu and vijendra)
                    }
                }

                var CIPLItem = Service.EMCS.SvcCipl.CiplItemGetById(idCIPL);
                foreach (var items in CIPLItem)
                {
                    items.IsDelete = true;
                    Service.EMCS.SvcCipl.removeCiplItem(items);
                }

                Service.EMCS.MasterCIPLItem.InsertBulk("CiplItem", dt, (HeaderCIPLItem().Count - 1));
                return true;

            }
            catch (Exception ex)
            {
<<<<<<< HEAD
<<<<<<< HEAD

=======
                
>>>>>>> 639d8d0 (Intial commit)
=======

>>>>>>> b773f28 (intial commit for changes from himanshu and vijendra)
                return false;
            }
        }

        private List<string> HeaderCIPLItem()
        {
            List<string> header = new List<string>();
            header.Add("Id");
            header.Add("IdCipl");
            header.Add("IdReference");
            header.Add("ReferenceNo");
            header.Add("IdCustomer");
            header.Add("Name");
            header.Add("Uom");
            header.Add("PartNumber");
            header.Add("Sn");
            header.Add("JCode");
            header.Add("Ccr");
            header.Add("CaseNumber");
            header.Add("Type");
            header.Add("IdNo");
            header.Add("YearMade");
            header.Add("Quantity");
            header.Add("UnitPrice");
            header.Add("ExtendedValue");
            header.Add("Length");
            header.Add("Width");
            header.Add("Height");
            header.Add("Volume");
            header.Add("GrossWeight");
            header.Add("NetWeight");
            header.Add("Currency");
            header.Add("CreateBy");
            header.Add("CreateDate");
            header.Add("UpdateBy");
            header.Add("UpdateDate");
            header.Add("IsDelete");
            header.Add("CoO");
            header.Add("IdParent");
            header.Add("SIBNumber");
            header.Add("WONumber");
            header.Add("Claim");
            header.Add("ASNNumber");
            return header;
        }

        public DataRow AddDataRow(DataTable dt, ISheet sheet, int i, long idCIPL, string idReference)
        {
            DataRow dataRow = dt.NewRow();
<<<<<<< HEAD
<<<<<<< HEAD
            var idReferenceCIPL = Service.EMCS.SvcCipl.GetIdReference(idReference);
            var Refno = GetStringVal(sheet, i, 0, 0);
=======
            var idReferenceCIPL = Service.EMCS.SvcCipl.GetIdReference(idReference); 
            var Refno = GetStringVal(sheet, i, 0, 0);  
>>>>>>> 639d8d0 (Intial commit)
=======
            var idReferenceCIPL = Service.EMCS.SvcCipl.GetIdReference(idReference);
            var Refno = GetStringVal(sheet, i, 0, 0);
>>>>>>> b773f28 (intial commit for changes from himanshu and vijendra)
            dataRow["Id"] = i;
            dataRow["IdCipl"] = idCIPL;
            dataRow["IdReference"] = idReferenceCIPL;
            dataRow["ReferenceNo"] = Refno;
            dataRow["IdCustomer"] = "";
            dataRow["Name"] = GetStringVal(sheet, i, 1, 0);
            dataRow["Uom"] = GetStringVal(sheet, i, 3, 0);
            dataRow["PartNumber"] = GetStringVal(sheet, i, 4, 0);
            dataRow["Sn"] = "";
            dataRow["JCode"] = GetStringVal(sheet, i, 5, 0);
            dataRow["Ccr"] = "";
            dataRow["CaseNumber"] = GetStringVal(sheet, i, 11, 0);
            dataRow["Type"] = GetStringVal(sheet, i, 12, 0);
            dataRow["IdNo"] = "";
            dataRow["YearMade"] = "";
            dataRow["Quantity"] = GetDecVal(sheet, i, 2, 0);
            dataRow["UnitPrice"] = GetStringVal(sheet, i, 7, 0);
<<<<<<< HEAD
<<<<<<< HEAD
            dataRow["ExtendedValue"] = GetStringVal(sheet, i, 8, 0);
=======
            dataRow["ExtendedValue"] = GetStringVal(sheet, i,8, 0);          
>>>>>>> 639d8d0 (Intial commit)
=======
            dataRow["ExtendedValue"] = GetStringVal(sheet, i, 8, 0);
>>>>>>> b773f28 (intial commit for changes from himanshu and vijendra)
            dataRow["Length"] = GetStringVal(sheet, i, 13, 0);
            dataRow["Width"] = GetStringVal(sheet, i, 14, 0);
            dataRow["Height"] = GetStringVal(sheet, i, 15, 0);
            dataRow["Volume"] = GetStringVal(sheet, i, 16, 0);
            dataRow["GrossWeight"] = GetStringVal(sheet, i, 18, 0);
            dataRow["NetWeight"] = GetStringVal(sheet, i, 17, 0);
            dataRow["Currency"] = GetStringVal(sheet, i, 9, 0);
            dataRow["CreateBy"] = SiteConfiguration.UserName;
            dataRow["CreateDate"] = DateTime.Now;
            dataRow["UpdateBy"] = SiteConfiguration.UserName;
            dataRow["UpdateDate"] = DateTime.Now;
            dataRow["IsDelete"] = false;
            dataRow["CoO"] = GetStringVal(sheet, i, 6, 0);
            dataRow["IdParent"] = 0;
            dataRow["SIBNumber"] = "";
            dataRow["WONumber"] = "";
            dataRow["Claim"] = "";
            string ASNNumber = "";
            if (GetStringVal(sheet, i, 10, 0) == "")
            {
                ASNNumber = GetDecVal(sheet, i, 10, 0).ToString();
            }
            else
            {
                ASNNumber = GetStringVal(sheet, i, 10, 0);
            }

            dataRow["ASNNumber"] = ASNNumber;

            if (!String.IsNullOrEmpty(Refno))
            {
                dt.Rows.Add(dataRow);
            }

            return dataRow;
        }

        public string GetStringVal(ISheet sheet, int numRow, int cellNum, int isNum)
        {
            try
            {

                string val;
<<<<<<< HEAD
<<<<<<< HEAD
                val = Convert.ToString(sheet.GetRow(numRow).GetCell(cellNum));
=======
                val = sheet.GetRow(numRow).GetCell(cellNum).StringCellValue;
>>>>>>> 639d8d0 (Intial commit)
=======
                val = Convert.ToString(sheet.GetRow(numRow).GetCell(cellNum));
>>>>>>> b773f28 (intial commit for changes from himanshu and vijendra)
                return val;
            }
            catch (Exception ex)
            {
                return "";
            }
        }
        public double GetDecVal(ISheet sheet, int numRow, int cellNum, int isNum)
        {
            try
            {
<<<<<<< HEAD
<<<<<<< HEAD
=======
>>>>>>> b773f28 (intial commit for changes from himanshu and vijendra)
                double val = 0;
                //val = sheet.GetRow(numRow).GetCell(cellNum).NumericCellValue;
                if (sheet.GetRow(numRow).GetCell(cellNum) != null)
                {
                    val = sheet.GetRow(numRow).GetCell(cellNum).NumericCellValue;
                }
<<<<<<< HEAD

                return val;
            }
            catch (Exception)
=======
                double val;
                val = sheet.GetRow(numRow).GetCell(cellNum).NumericCellValue;

                return val;
            }
            catch (Exception )
>>>>>>> 639d8d0 (Intial commit)
=======

                return val;
            }
            catch (Exception)
>>>>>>> b773f28 (intial commit for changes from himanshu and vijendra)
            {
                return 0;
            }
        }

<<<<<<< HEAD
<<<<<<< HEAD

=======
     
>>>>>>> 639d8d0 (Intial commit)
=======

>>>>>>> b773f28 (intial commit for changes from himanshu and vijendra)
    }
}