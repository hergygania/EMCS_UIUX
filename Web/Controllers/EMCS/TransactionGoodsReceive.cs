using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using App.Data.Caching;
using App.Data.Domain;
using App.Domain;
using App.Web.Models;
using App.Web.App_Start;
using System.Globalization;
using Newtonsoft.Json;
using System.Web.Script.Serialization;
using System.Configuration;
using System.Net;
using App.Web.Models.EMCS;
using Spire.Xls;
using System.IO;
using App.Web.App_Start;
using App.Web.Helper;

namespace App.Web.Controllers.EMCS
{
    public partial class EmcsController
    {
        // GET: TransactionGoodsReceive
        #region Initialize Data
        public GoodReceiveModel InitGoodReceive(long id)
        {
            var item = Service.EMCS.SvcGoodsReceive.GetById(id);
            GoodReceiveModel result = new GoodReceiveModel();
            Data.Domain.EMCS.SpGoodReceive data = new Data.Domain.EMCS.SpGoodReceive();
            data.EstimationTimePickup = DateTime.Now;

            if (item != null)
            {
                data.Id = item.Id;
                data.GrNo = item.GrNo;
                data.PhoneNumber = item.PhoneNumber;

                data.Vendor = item.Vendor;
                data.VendorName = item.VendorName;
                data.VendorCode = item.VendorCode;
                data.VendorTelephone = item.VendorTelephone;
                data.VendorAddress = item.VendorAddress;
                data.VendorCity = item.VendorCity;

                data.VehicleMerk = item.VehicleMerk;
                data.VehicleType = item.VehicleType;
                data.KirNumber = item.KirNumber;
                data.KirExpire = item.KirExpire;
                data.Apar = item.Apar != null && item.Apar == true;
                data.Apd = item.Apd != null && item.Apd == true;

                data.KtpNumber = item.KtpNumber;
                data.SimNumber = item.SimNumber;
                data.PicName = item.PicName;
                data.StnkNumber = item.StnkNumber;
                data.NopolNumber = item.NopolNumber;
                data.Notes = item.Notes;
                data.EstimationTimePickup = item.EstimationTimePickup;
                data.CreateDate = item.CreateDate;
                data.CreateBy = item.CreateBy;
                data.UpdateDate = item.UpdateDate;
                data.SimExpiryDate = item.SimExpiryDate;
                data.UpdateBy = item.UpdateBy;
                data.IsDelete = item.IsDelete;
                data.PickupPoint = item.PickupPoint;
                data.PickupPic = item.PickupPic;
                data.PickupPicName = item.PickupPicName;
                data.PlantCode = item.PlantCode;
                data.PlantName = item.PlantName;
                data.RequestorName = item.RequestorName;
                data.RequestorEmail = item.RequestorEmail;
                data.TotalGrossWeight = item.TotalGrossWeight;
                data.TotalNetWeight = item.TotalNetWeight;
                data.TotalPackages = item.TotalPackages;
                data.TotalVolume = item.TotalVolume;
                data.SignedName = item.SignedName;
                data.SignedPosition = item.SignedPosition;
            }

            result.Data = data;
            result.DataItem = Service.EMCS.SvcGoodsReceive.GetGrItemList(id);
            result.DataRequest = Service.EMCS.SvcRequestGr.GetRequestByGrId(id);
            return result;
        }
        #endregion

        #region List GR
        [AuthorizeAcces(ActionType = AuthorizeAcces.IsRead)]
        public ActionResult GrList()
        {
            ApplicationTitle();
            PaginatorBoot.Remove("SessionTRN");
            return View();
        }
        #endregion

        #region Create GR
        [AuthorizeAcces(ActionType = AuthorizeAcces.IsRead, UrlMenu = "GRList")]
        [HttpPost, ValidateInput(false)]
        public JsonResult CreateGr(GoodReceiveModel form)
        {
            try
            {
                var ResultPicName = Service.Master.EmailRecipients.ValidateInputHtmlInjection(form.Data.PicName, "`^<>");
                var ResultPhoneNumber = Service.Master.EmailRecipients.ValidateInputHtmlInjection(form.Data.PhoneNumber, "`^<>");
                var ResultKtpNumber = Service.Master.EmailRecipients.ValidateInputHtmlInjection(form.Data.KtpNumber, "`^<>");
                var ResultSimNumber = Service.Master.EmailRecipients.ValidateInputHtmlInjection(form.Data.SimNumber, "`^<>");
                var ResultStnkNumber = Service.Master.EmailRecipients.ValidateInputHtmlInjection(form.Data.StnkNumber, "`^<>");
                var ResultNopolNumber = Service.Master.EmailRecipients.ValidateInputHtmlInjection(form.Data.NopolNumber, "`^<>");
                var ResultVehicleType = Service.Master.EmailRecipients.ValidateInputHtmlInjection(form.Data.VehicleType, "`^<>");
                var ResultVehicleMerk = Service.Master.EmailRecipients.ValidateInputHtmlInjection(form.Data.VehicleMerk, "`^<>");
                var ResultKirNumber = Service.Master.EmailRecipients.ValidateInputHtmlInjection(form.Data.KirNumber, "`^<>");
                var ResultNotes = Service.Master.EmailRecipients.ValidateInputHtmlInjection(form.Data.Notes, "`^<>");

                if (!ResultPicName)
                {
                    return JsonMessage("Please Enter a Valid PIC Name", 1, "i");
                }
                if (!ResultPhoneNumber)
                {
                    return JsonMessage("Please Enter a Valid Contact", 1, "i");
                }
                if (!ResultKtpNumber)
                {
                    return JsonMessage("Please Enter a Valid ID Card", 1, "i");
                }
                if (!ResultSimNumber)
                {
                    return JsonMessage("Please Enter a Valid Driving License", 1, "i");
                }
                if (!ResultStnkNumber)
                {
                    return JsonMessage("Please Enter a Valid No STNK", 1, "i");
                }
                if (!ResultNopolNumber)
                {
                    return JsonMessage("Please Enter a Valid License Plate", 1, "i");
                }
                if (!ResultVehicleType)
                {
                    return JsonMessage("Please Enter a Valid Vehicle Type", 1, "i");
                }
                if (!ResultVehicleMerk)
                {
                    return JsonMessage("Please Enter a Valid Vehicle Brand", 1, "i");
                }
                if (!ResultKirNumber)
                {
                    return JsonMessage("Please Enter a Valid KIR Number", 1, "i");
                }
                if (!ResultNotes)
                {
                    return JsonMessage("Please Enter a Valid Notes", 1, "i");
                }

                ViewBag.crudMode = (form.Data.Id == 0) ? "I" : "U";
                var item = new Data.Domain.EMCS.SpGoodReceive();
                long id = 0;
                if (ViewBag.crudMode == "U")
                {
                    item = Service.EMCS.SvcGoodsReceive.GetById(form.Data.Id);
                }

                if (form.Data.Status == "Draft")
                {
                    item.PicName = form.Data.PicName;
                    item.PhoneNumber = form.Data.PhoneNumber;
                    item.KtpNumber = form.Data.KtpNumber;
                    item.SimNumber = form.Data.SimNumber;
                    item.StnkNumber = form.Data.StnkNumber;
                    item.NopolNumber = form.Data.NopolNumber;
                    item.PickupPoint = form.Data.PickupPoint;
                    item.PickupPic = form.Data.PickupPic;

                    item.Vendor = form.Data.Vendor ?? "";
                    item.VehicleType = form.Data.VehicleType ?? "";
                    item.VehicleMerk = form.Data.VehicleMerk ?? "";
                    item.Apar = form.Data.Apar;
                    item.Apd = form.Data.Apd;
                    item.KirNumber = form.Data.KirNumber ?? "";
                    item.KirExpire = form.Data.KirExpire != null ? form.Data.KirExpire : null;
                    item.SimExpiryDate = form.Data.SimExpiryDate != null ? form.Data.SimExpiryDate : null;

                    item.Notes = form.Data.Notes;
                    item.EstimationTimePickup = form.Data.EstimationTimePickup;
                    id = Service.EMCS.SvcGoodsReceive.CrudSp(item, form.Data.Status, ViewBag.crudMode);
                    var data = InitGoodReceive(id);
                    return JsonCRUDMessage(ViewBag.crudMode, data);
                }
                else
                {
                    return CreateDataGr(item, form, id);
                }
            }
            catch (Exception err)
            {
                return Json(new { success = false, msg = err.Message });
            }
        }

        [AuthorizeAcces(ActionType = AuthorizeAcces.IsRead, UrlMenu = "GRList")]
        public ActionResult CreateGr()
        {
            ApplicationTitle();
            ViewBag.crudMode = "I";
            PaginatorBoot.Remove("SessionTRN");
            GoodReceiveModel data = InitGoodReceive(0);
            data.YesNo = YesNoList();
            return View("GRForm", data);
        }

        public JsonResult CreateDataGr(Data.Domain.EMCS.SpGoodReceive item, GoodReceiveModel form, long id)
        {
            if (ModelState.IsValid)
            {
                item.PicName = form.Data.PicName;
                item.PhoneNumber = form.Data.PhoneNumber;
                item.KtpNumber = form.Data.KtpNumber;
                item.SimNumber = form.Data.SimNumber;
                item.StnkNumber = form.Data.StnkNumber;
                item.NopolNumber = form.Data.NopolNumber;

                item.Vendor = form.Data.Vendor ?? "";
                item.VehicleType = form.Data.VehicleType ?? "";
                item.VehicleMerk = form.Data.VehicleMerk ?? "";
                item.Apar = form.Data.Apar;
                item.PickupPoint = form.Data.PickupPoint;
                item.PickupPic = form.Data.PickupPic;
                item.Apd = form.Data.Apd;
                item.KirNumber = form.Data.KirNumber ?? "";
                item.KirExpire = form.Data.KirExpire != null ? form.Data.KirExpire : null;
                item.SimExpiryDate = form.Data.SimExpiryDate != null ? form.Data.SimExpiryDate : null;

                item.Notes = form.Data.Notes;
                item.EstimationTimePickup = form.Data.EstimationTimePickup;
                id = Service.EMCS.SvcGoodsReceive.CrudSp(item, form.Data.Status, ViewBag.crudMode);
                var data = InitGoodReceive(id);
                return JsonCRUDMessage(ViewBag.crudMode, data);
            }
            return Json(new { success = false });
        }
        #endregion

        #region
        [HttpPost]
        public JsonResult RemoveGr(long id)
        {
            try
            {
                var gr = Service.EMCS.SvcGoodsReceive.GetData(id);
                string Action = "U";
                var grRequest = Service.EMCS.SvcRequestGr.GetRequestById(id);
                var grItem = Service.EMCS.SvcGoodsReceiveItem.GetByGrId(id);
                gr.IsDelete = true;
                grRequest.IsDelete = true;

                Service.EMCS.SvcGoodsReceive.Crud(gr, Action);
                Service.EMCS.SvcRequestGr.Crud(grRequest, Action);

                foreach (var items in grItem)
                {
                    items.IsDelete = true;
                    Service.EMCS.SvcGoodsReceiveItem.Crud(items, Action);
                }

                return JsonCRUDMessage(Action, gr);
            }
            catch (Exception)
            {
                return Json(new { success = false });
            }
        }
        #endregion

        #region Preview
        public ActionResult PreviewGr(long id)
        {
            ViewBag.crudMode = "U";
            PaginatorBoot.Remove("SessionTRN");
            GoodReceiveModel data = InitGoodReceive(id);
            ApplicationTitle();
            string strQrCodeUrlGR = Common.GenerateQrCode(id, "DownloadRg");
            ViewBag.QrCodeUrlGR = strQrCodeUrlGR;
            TempData["QrCodeUrlGR"] = strQrCodeUrlGR;
            TempData.Peek("QrCodeUrlGR");
            ViewBag.WizardData = Service.EMCS.SvcWizard.GetWizardData("rg", id);
            data.DetailGr = Service.EMCS.DocumentStreamGenerator.GetGrDetailData(id);
            data.YesNo = YesNoList();
            return View("GRFormPreview", data);
        }

        public ActionResult ApprovalGr(long id = 0)
        {
            if (id != 0)
            {
                ViewBag.crudMode = "U";
                PaginatorBoot.Remove("SessionTRN");
                var idReq = Service.EMCS.SvcRequestGr.GetRequestById(id);
                var idGr = Convert.ToInt64(idReq.IdGr);
                GoodReceiveModel data = InitGoodReceive(idGr);
                ViewBag.WizardData = Service.EMCS.SvcWizard.GetWizardData("rg", idGr);
                ApplicationTitle();
                data.DetailGr = Service.EMCS.DocumentStreamGenerator.GetGrDetailData(id);
                if (data.Data.Status != "Draft")
                {
                    data.YesNo = YesNoList();
                    return View("GRFormApproval", data);
                }
                return View("NotAuthorize");
            }
            return View("NotAuthorize");
        }
        #endregion

        #region Update Form GR
        public ActionResult EditGrForm(long id)
        {
            ViewBag.crudMode = "U";
            PaginatorBoot.Remove("SessionTRN");
            GoodReceiveModel data = InitGoodReceive(id);
            data.YesNo = YesNoList();
            return View("GRForm", data);
        }

        [HttpPost, ValidateInput(false)]
        public JsonResult EditGrForm(GoodReceiveModel form)
        {
            var ResultPicName = Service.Master.EmailRecipients.ValidateInputHtmlInjection(form.Data.PicName, "`^<>");
            var ResultPhoneNumber = Service.Master.EmailRecipients.ValidateInputHtmlInjection(form.Data.PhoneNumber, "`^<>");
            var ResultKtpNumber = Service.Master.EmailRecipients.ValidateInputHtmlInjection(form.Data.KtpNumber, "`^<>");
            var ResultSimNumber = Service.Master.EmailRecipients.ValidateInputHtmlInjection(form.Data.SimNumber, "`^<>");
            var ResultStnkNumber = Service.Master.EmailRecipients.ValidateInputHtmlInjection(form.Data.StnkNumber, "`^<>");
            var ResultNopolNumber = Service.Master.EmailRecipients.ValidateInputHtmlInjection(form.Data.NopolNumber, "`^<>");
            var ResultVehicleType = Service.Master.EmailRecipients.ValidateInputHtmlInjection(form.Data.VehicleType, "`^<>");
            var ResultVehicleMerk = Service.Master.EmailRecipients.ValidateInputHtmlInjection(form.Data.VehicleMerk, "`^<>");
            var ResultKirNumber = Service.Master.EmailRecipients.ValidateInputHtmlInjection(form.Data.KirNumber, "`^<>");
            var ResultNotes = Service.Master.EmailRecipients.ValidateInputHtmlInjection(form.Data.Notes, "`^<>");

            if (!ResultPicName)
            {
                return JsonMessage("Please Enter a Valid PIC Name", 1, "i");
            }
            if (!ResultPhoneNumber)
            {
                return JsonMessage("Please Enter a Valid Contact", 1, "i");
            }
            if (!ResultKtpNumber)
            {
                return JsonMessage("Please Enter a Valid ID Card", 1, "i");
            }
            if (!ResultSimNumber)
            {
                return JsonMessage("Please Enter a Valid Driving License", 1, "i");
            }
            if (!ResultStnkNumber)
            {
                return JsonMessage("Please Enter a Valid No STNK", 1, "i");
            }
            if (!ResultNopolNumber)
            {
                return JsonMessage("Please Enter a Valid License Plate", 1, "i");
            }
            if (!ResultVehicleType)
            {
                return JsonMessage("Please Enter a Valid Vehicle Type", 1, "i");
            }
            if (!ResultVehicleMerk)
            {
                return JsonMessage("Please Enter a Valid Vehicle Brand", 1, "i");
            }
            if (!ResultKirNumber)
            {
                return JsonMessage("Please Enter a Valid KIR Number", 1, "i");
            }
            if (!ResultNotes)
            {
                return JsonMessage("Please Enter a Valid Notes", 1, "i");
            }

            try
            {
                var item = new Data.Domain.EMCS.SpGoodReceive();
                item.PicName = form.Data.PicName;
                item.PhoneNumber = form.Data.PhoneNumber;
                item.KtpNumber = form.Data.KtpNumber;
                item.SimNumber = form.Data.SimNumber;
                item.StnkNumber = form.Data.StnkNumber;
                item.NopolNumber = form.Data.NopolNumber;
                item.Notes = form.Data.Notes;
                item.PickupPic = form.Data.PickupPic;
                item.PickupPoint = form.Data.PickupPoint;
                item.EstimationTimePickup = form.Data.EstimationTimePickup;
                Service.EMCS.SvcGoodsReceive.CrudSp(item, form.Data.Status, "U");
                var detail = InitGoodReceive(form.Data.Id);
                return JsonCRUDMessage("U", new { detail });
            }
            catch (Exception)
            {
                return Json(new { success = false });
            }
        }

        [HttpPost]
        public JsonResult GrApprove(GoodReceiveModel form)
        {
            var gr = Service.EMCS.SvcGoodsReceive.GetById(form.Data.Id);
            gr.Notes = form.Data.Notes;

            Service.EMCS.SvcGoodsReceive.CrudSp(gr, form.Data.Status, "U");
            var data = InitGoodReceive(form.Data.Id);
            return JsonCRUDMessage("U", data);
        }
        #endregion

        public ActionResult GrValidate(long id)
        {
            PaginatorBoot.Remove("SessionTRN");
            GoodReceiveModel data = InitGoodReceive(id);
            return View(data);
        }

        [AuthorizeAcces(ActionType = AuthorizeAcces.IsRead, UrlMenu = "GRList")]
        public JsonResult GetGrList(GridListFilterModel filter)
        {
            var dataFilter = new Data.Domain.EMCS.GridListFilter();
            ApplicationTitle();
            dataFilter.Limit = filter.Limit;
            dataFilter.Order = filter.Order;
            dataFilter.Term = filter.Term;
            dataFilter.Sort = filter.Sort;
            dataFilter.Offset = filter.Offset;
            var data = Service.EMCS.SvcGoodsReceive.GetListSp(dataFilter);
            return Json(data, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetGrHistoryList(Data.Domain.EMCS.GridListFilter filter)
        {
            ApplicationTitle();
            var data = Service.EMCS.SvcGoodsReceive.GetListSpGRhistory(filter);
            return Json(data, JsonRequestBehavior.AllowGet);
        }

        public ActionResult ReportGr(long grId, string reportType)
        {
            string fileExcel = Server.MapPath("~\\Content\\EMCS\\Templates\\TemplateRG.xls");
            string fileName = "RGDocument_" + DateTime.Now.ToString("yyyyMMddHHmmss");
            string filePath = Server.MapPath("~\\Content\\EMCS\\Templates\\" + fileName);
            MemoryStream output = Service.EMCS.DocumentStreamGenerator.GetStream(grId, fileExcel, filePath, reportType);
            return File(output.ToArray(), "application/pdf", "RGDocument_" + DateTime.Now.ToString("ddMMyyyyhhhmmss") + ".pdf");    //Suggested file name in the "Save as" dialog which will be displayed to the end user
        }

        public ActionResult PreviewDa(long id)
        {
            var dataItem = Service.EMCS.SvcGoodsReceiveItem.GetById(id);
            ViewBag.data = dataItem;
            return View();
        }

        public JsonResult GetCiplAreaAvailable()
        {
            try
            {
                var data = Service.EMCS.SvcGoodsReceive.GetAreaAvailable();
                return Json(new { data }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                return Json(new { data = "" }, JsonRequestBehavior.AllowGet);
            }
        }

        public JsonResult GetCiplPicAvailable(string area = "")
        {
            try
            {
                var data = Service.EMCS.SvcGoodsReceive.GetPicAvailable(area);
                return Json(new { data }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                return Json(new { data = "" }, JsonRequestBehavior.AllowGet);
            }
        }
    }
}