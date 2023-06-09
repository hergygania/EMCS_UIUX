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
using App.Web.Helper;
using App.Data.Domain.EMCS;
using System.ComponentModel;

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
            Data.Domain.EMCS.ShippingFleet datashippingFleet = new Data.Domain.EMCS.ShippingFleet();
            data.EstimationTimePickup = DateTime.Now;

            if (item != null)
            {
                data.Id = item.Id;
                data.GrNo = item.GrNo;

                data.Vendor = item.Vendor;
                data.VendorName = item.VendorName;
                data.VendorCode = item.VendorCode;
                data.VendorTelephone = item.VendorTelephone;
                data.VendorAddress = item.VendorAddress;
                data.VendorCity = item.VendorCity;

                data.VehicleMerk = item.VehicleMerk;
                data.VehicleType = item.VehicleType;
                data.Notes = item.Notes;
                data.CreateDate = item.CreateDate;
                data.CreateBy = item.CreateBy;
                data.UpdateDate = item.UpdateDate;
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

            var shippingFleet = Service.EMCS.SvcGoodsReceive.GetShippingFleetList(id);
            
            var detailGr = result;
            result.ShippingFleet.YesNo = YesNoList();
            result.ShippingFleet.DoList.Add(new SelectListItem() { Text = "Select Do No", Value = "0" });
            result.ShippingFleet.ShippingFleetList = shippingFleet;
            if (item != null)
            {
                result.ShippingFleet.DoList.AddRange(Service.EMCS.SvcCargo.GetEdoNoList(detailGr.Data.PickupPoint, detailGr.Data.PickupPic, item.Id).ConvertAll(a =>
                {
                    return new SelectListItem()
                    {
                        Text = a.EdoNo,
                        Value = a.Id.ToString()
                    };
                }));
            }
            return result;
        }
        #endregion

        #region List GR
        [AuthorizeAcces(ActionType = AuthorizeAcces.IsRead)]
        public ActionResult GrList()
        {
            ApplicationTitle();
            ViewBag.AllowRead = AuthorizeAcces.AllowRead;
            ViewBag.AllowCreate = AuthorizeAcces.AllowCreated;
            ViewBag.AllowUpdate = AuthorizeAcces.AllowUpdated;
            ViewBag.AllowDelete = AuthorizeAcces.AllowDeleted;
            string userRoles = User.Identity.GetUserRoles();
            //if (userRoles.Contains("EMCSImex") || userRoles.Contains("Administrator") || userRoles.Contains("Imex"))
            if (userRoles.Contains("EMCSImex") || userRoles.Contains("Imex"))
                ViewBag.IsImexUser = true;
            else
                ViewBag.IsImexUser = false;
            if (User.Identity.Name == "eko.suhartarto")
                ViewBag.IsCKB = true;
            else
                ViewBag.IsCKB = false;
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

                var ResultVehicleType = Service.Master.EmailRecipients.ValidateInputHtmlInjection(form.Data.VehicleType, "`^<>");
                var ResultVehicleMerk = Service.Master.EmailRecipients.ValidateInputHtmlInjection(form.Data.VehicleMerk, "`^<>");
                var ResultNotes = Service.Master.EmailRecipients.ValidateInputHtmlInjection(form.Data.Notes, "`^<>");

                if (!ResultVehicleType)
                {
                    return JsonMessage("Please Enter a Valid Vehicle Type", 1, "i");
                }
                if (!ResultVehicleMerk)
                {
                    return JsonMessage("Please Enter a Valid Vehicle Brand", 1, "i");
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
                    item.PickupPoint = form.Data.PickupPoint;
                    item.PickupPic = form.Data.PickupPic;

                    item.Vendor = form.Data.Vendor ?? "";
                    item.VehicleType = form.Data.VehicleType ?? "";
                    item.VehicleMerk = form.Data.VehicleMerk ?? "";
                    item.Notes = form.Data.Notes;
                    var userId = User.Identity.GetUserId();
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
            ViewBag.IsOwned = true;
            ViewBag.IsApprover = false;
            ViewBag.IsRFC = false;
            HttpContext.Session["IsApprover"] = false;
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
        public JsonResult GetDataByIdGrForHistory(long Id)
        {
            var data = App.Service.EMCS.SvcGoodsReceiveItem.GetDataByIdGrForHistory(Id);
            return Json(data, JsonRequestBehavior.AllowGet);
        }
        [HttpGet]
        public ActionResult GetHistoryDataById(long Id)
        {
            try
            {
                var data = App.Service.EMCS.SvcGoodsReceiveItem.GetHistoryDataById(Id);
                return Json(data, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }
        [HttpGet]
        public ActionResult GetListArmadaForRFC(long IdGr)
        {
            try
            {
                var data = Service.EMCS.SvcGoodsReceiveItem.GetDataByIdGrForRFC(IdGr);
                return Json(data, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
        [HttpPost]
        public JsonResult SaveHistory(ShippingFleet data)
        {
            try
            {
                ApplicationTitle();
                var IsChange = false;
                ShippingFleet result = new ShippingFleet();
                if (data.Id != 0)
                {
                    var model = App.Service.EMCS.SvcGoodsReceiveItem.GetListArmada(data.Id, 0);
                    string[] _ignnoreParameters = { "Id", "IdGr", "IdCipl", "FileName" };
                    var properties = TypeDescriptor.GetProperties(typeof(ShippingFleet));
                    foreach (PropertyDescriptor property in properties)
                    {
                        if (!_ignnoreParameters.Contains(property.Name))
                        {
                            var currentValue = property.GetValue(data);
                            if (currentValue != null && property.GetValue(model[0]) != null)
                            {
                                if (currentValue.ToString() != property.GetValue(model[0]).ToString())
                                {
                                    IsChange = true;
                                    break;
                                }
                            }

                        }
                    }
                    if (IsChange == true)
                    {
                        var HistoryData = Service.EMCS.SvcGoodsReceiveItem.GetHistoryDataById(data.Id);
                        if (HistoryData == null)
                        {
                            Service.EMCS.SvcGoodsReceiveItem.SaveArmadaHistory(model[0], "Updated");
                            var Id = Service.EMCS.SvcGoodsReceiveItem.SaveArmada(data);
                            Service.EMCS.SvcGoodsReceiveItem.SaveArmadaHistory(data, "Updated");
                            List<string> DoNoList = new List<string>(data.DoNo.Split(','));
                            if (DoNoList.Count > 0)
                            {
                                foreach (var dono in DoNoList)
                                {
                                    data.DoNo = dono;
                                    Service.EMCS.SvcGoodsReceiveItem.SaveArmadaRefrence(data);
                                }
                            }
                        }
                        else
                        {
                            var Id = Service.EMCS.SvcGoodsReceiveItem.SaveArmada(data);
                            Service.EMCS.SvcGoodsReceiveItem.SaveArmadaHistory(data, "Updated");
                            List<string> DoNoList = new List<string>(data.DoNo.Split(','));
                            if (DoNoList.Count > 0)
                            {
                                foreach (var dono in DoNoList)
                                {
                                    data.DoNo = dono;
                                    Service.EMCS.SvcGoodsReceiveItem.SaveArmadaRefrence(data);
                                }
                            }
                        }
                    }
                }
                else
                {
                    var Id = Service.EMCS.SvcGoodsReceiveItem.SaveArmada(data);
                    data.Id = Id;
                    Service.EMCS.SvcGoodsReceiveItem.SaveArmadaHistory(data, "Created");
                    List<string> DoNoList = new List<string>(data.DoNo.Split(','));
                    if (DoNoList.Count > 0)
                    {
                        foreach (var dono in DoNoList)
                        {
                            data.DoNo = dono;
                            Service.EMCS.SvcGoodsReceiveItem.SaveArmadaRefrence(data);
                        }
                    }
                }


                return Json(result.Id, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }
        [HttpPost]
        public ActionResult SaveHistoryAndApproveForGR(RequestForChangeModel form, SpGoodReceive item)
        {
            try
            {
                var NewArmada = new List<ShippingFleet>();
                SpGoodReceive GRData = new SpGoodReceive();
                var IsChange = false;
                _errorHelper.Error("SaveChangeHistory - Method call started");
                var requestForChange = new RequestForChange();

                requestForChange.FormNo = item.GrNo;
                requestForChange.FormType = form.FormType;
                requestForChange.Status = form.Status;
                requestForChange.FormId = form.FormId;
                requestForChange.Reason = form.Reason;
                var id = Service.EMCS.SvcCipl.InsertChangeHistory(requestForChange);

                var model = Service.EMCS.SvcGoodsReceive.GetById(item.Id);
                var listRfcItems = new List<Data.Domain.RFCItem>();
                string[] _ignnoreParameters = { "Id", "GrNo", "PhoneNumber", "PicName", "KtpNumber", "SimNumber", "StnkNumber", "NopolNumber", "EstimationTimePickup", "CreateBy", "CreateDate", "IsDelete", "KirNumber", "KirExpire", "Apar", "Apd", "SimExpiryDate", "ActualTimePickup", "VendorAddress" };
                var properties = TypeDescriptor.GetProperties(typeof(SpGoodReceive));
                foreach (PropertyDescriptor property in properties)
                {
                    if (!_ignnoreParameters.Contains(property.Name))
                    {
                        var currentValue = property.GetValue(item);
                        if (currentValue != null && property.GetValue(model) != null)
                        {
                            if (currentValue.ToString().Trim() != property.GetValue(model).ToString())
                            {
                                IsChange = true;
                                var rfcItem = new Data.Domain.RFCItem();

                                rfcItem.RFCID = id;
                                rfcItem.TableName = "GoodsReceive";
                                rfcItem.LableName = property.Name;
                                rfcItem.FieldName = property.Name;
                                rfcItem.BeforeValue = property.GetValue(model).ToString();
                                rfcItem.AfterValue = currentValue.ToString();
                                listRfcItems.Add(rfcItem);

                            }
                        }
                    }

                }

                if (IsChange == true)
                {
                    Service.EMCS.SvcCipl.InsertRFCItem(listRfcItems);
                    //Service.EMCS.SvcGoodsReceiveItem.SaveGRHistory(model);
                    var result = Service.EMCS.SvcGoodsReceive.UpdateGrByApprover(item);
                }
                else
                {
                    var result = Service.EMCS.SvcGoodsReceive.UpdateGrByApprover(model);
                }

                return Json("");
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        [HttpPost]
        public ActionResult SaveArmadaForRFC(ShippingFleet form)
        {
            try
            {
                ApplicationTitle();
                ViewBag.crudMode = "U";
                SP_ShippingFleetItem data = new SP_ShippingFleetItem();
                ShippingFleet result = new ShippingFleet();
                if (form.Id == 0)
                {
                    result.Id = Service.EMCS.SvcGoodsReceiveItem.SaveArmadaForRFC(form, "Created");

                }
                else
                {
                    var obj = Service.EMCS.SvcGoodsReceiveItem.GetDataByIdShippingFleetById(form.Id);
                    if (obj == null)
                    {
                        result.Id = Service.EMCS.SvcGoodsReceiveItem.SaveArmadaForRFC(form, "Updated");

                    }
                    else if (obj.Status != "Deleted")
                    {
                        result.Id = Service.EMCS.SvcGoodsReceiveItem.SaveArmadaForRFC(form, "Updated");

                    }


                }
                //List<string> DoNoList = new List<string>(form.DoNo.Split(','));
                //if (DoNoList.Count > 0)
                //{
                //    form.Id = result.Id;
                //    foreach (var item in DoNoList)
                //    {
                //        form.DoNo = item;
                //        Service.EMCS.SvcGoodsReceiveItem.SaveArmadaRefrence(form);

                //    }

                //}
                //}



                return Json(result.Id, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        [HttpPost]
        public ActionResult RequestForChangeRG(RequestForChangeModel form, SpGoodReceive formdata)
        {
            try
            {
                var NewArmada = new List<ShippingFleet>();
                SpGoodReceive GRData = new SpGoodReceive();
                var IsChange = false;
                _errorHelper.Error("SaveChangeHistory - Method call started");
                var requestForChange = new RequestForChange();

                requestForChange.FormNo = form.FormNo;
                requestForChange.FormType = form.FormType;
                requestForChange.Status = form.Status;
                requestForChange.FormId = form.FormId;
                requestForChange.Reason = form.Reason;
                var id = Service.EMCS.SvcCipl.InsertRequestChangeHistory(requestForChange);

                var model = Service.EMCS.SvcGoodsReceive.GetById(formdata.Id);
                var listRfcItems = new List<Data.Domain.RFCItem>();
                string[] _ignnoreParameters = { "Id", "GrNo", "PhoneNumber", "PicName", "KtpNumber", "SimNumber", "StnkNumber", "NopolNumber", "EstimationTimePickup", "CreateBy", "CreateDate", "IsDelete", "KirNumber", "KirExpire", "Apar", "Apd", "SimExpiryDate", "ActualTimePickup" };
                var properties = TypeDescriptor.GetProperties(typeof(SpGoodReceive));
                foreach (PropertyDescriptor property in properties)
                {
                    if (!_ignnoreParameters.Contains(property.Name))
                    {
                        var currentValue = property.GetValue(formdata);
                        if (currentValue != null && property.GetValue(model) != null)
                        {
                            if (currentValue.ToString().Trim() != property.GetValue(model).ToString())
                            {
                                IsChange = true;
                                var rfcItem = new Data.Domain.RFCItem();

                                rfcItem.RFCID = id;
                                rfcItem.TableName = "GoodsReceive";
                                rfcItem.LableName = property.Name;
                                rfcItem.FieldName = property.Name;
                                rfcItem.BeforeValue = property.GetValue(model).ToString();
                                rfcItem.AfterValue = currentValue.ToString();
                                listRfcItems.Add(rfcItem);

                            }
                        }
                    }

                }
                if (listRfcItems.Count != 0)
                {
                    Service.EMCS.SvcCipl.InsertRFCItem(listRfcItems);
                }

                return Json("");
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public JsonResult GetDocumentListOfArmada(long Id, long IdShippingFleet)
        {
            var data = Service.EMCS.SvcGoodsReceive.GetDocumentListOfArmada(Id, IdShippingFleet);
            return Json(data, JsonRequestBehavior.AllowGet);
        }
        #region
        [HttpPost]
        public JsonResult RemoveGr(long id)
        {
            try
            {
                var gr = Service.EMCS.SvcGoodsReceive.GetData(id);
                string Action = "U";
                var grRequest = Service.EMCS.SvcRequestGr.GetRequestById(id);
                //var grItem = Service.EMCS.SvcGoodsReceiveItem.GetByGrId(id);
                var grItem = Service.EMCS.SvcGoodsReceive.GetShippingFleetList(id);
                var shippingFleetRefItem = Service.EMCS.SvcGoodsReceive.GetShippingFleetReference(id);
                gr.IsDelete = true;
                grRequest.IsDelete = true;
                DeleteArmada(id);
                Service.EMCS.SvcGoodsReceive.Crud(gr, Action);
                Service.EMCS.SvcRequestGr.Crud(grRequest, Action);

                foreach (var items in grItem)
                {
                    //items.IsDelete = true;
                    Service.EMCS.SvcGoodsReceive.CrudShippingFleet(items, "D");
                }

                foreach (var items2 in shippingFleetRefItem)
                {
                    Service.EMCS.SvcGoodsReceive.CrudShippingFleetReference(items2, "D");
                }

                return JsonCRUDMessage(Action, gr);
            }
            catch (Exception ex)
            {
                return Json(new { success = false });
            }
        }
        #endregion

        #region Preview
        public ActionResult PreviewGr(long id)
        {
            ApplicationTitle();
            ViewBag.crudMode = "U";
            PaginatorBoot.Remove("SessionTRN");
            GoodReceiveModel data = InitGoodReceive(id);
            data.Armada = Service.EMCS.SvcGoodsReceiveItem.GetListArmada(0, id);
            var Idcipl = Service.EMCS.SvcGoodsReceive.GetCiplByGr(id);
            var totalvalue = Service.EMCS.SvcTotalCipl.GetById(Convert.ToInt64(Idcipl[0].Id));
            data.Data.TotalGrossWeight = Convert.ToString(totalvalue.TotalGrossWeight);
            data.Data.TotalNetWeight = Convert.ToString(totalvalue.TotalNetWeight);
            data.Data.TotalPackages = Convert.ToString(totalvalue.TotalPackage);
            data.Data.TotalVolume = Convert.ToString(totalvalue.TotalVolume);
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
                string strQrCodeUrlGR = Common.GenerateQrCode(id, "DownloadRg");
                ViewBag.QrCodeUrlGR = strQrCodeUrlGR;
                TempData["QrCodeUrlGR"] = strQrCodeUrlGR;
                TempData.Peek("QrCodeUrlGR");
                var idReq = Service.EMCS.SvcRequestGr.GetRequestById(id);
                ViewBag.Idgr = idReq.IdGr;
                var idGr = Convert.ToInt64(idReq.IdGr);
                GoodReceiveModel data = InitGoodReceive(idGr);
                data.Armada = Service.EMCS.SvcGoodsReceiveItem.GetListArmada(0, idGr);
                var Idcipl = Service.EMCS.SvcGoodsReceive.GetCiplByGr(idGr);
                var totalvalue = Service.EMCS.SvcTotalCipl.GetById(Convert.ToInt64(Idcipl[0].Id));
                data.Data.TotalGrossWeight = Convert.ToString(totalvalue.TotalGrossWeight);
                data.Data.TotalNetWeight = Convert.ToString(totalvalue.TotalNetWeight);
                data.Data.TotalPackages = Convert.ToString(totalvalue.TotalPackage);
                data.Data.TotalVolume = Convert.ToString(totalvalue.TotalVolume);
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
        public ActionResult EditGrForm(long id, bool rfc = false)
        {
            var userId = User.Identity.GetUserId();
            string userRoles = User.Identity.GetUserRoles();
            ViewBag.IsApprover = false;
            ViewBag.IsRFC = rfc;
            if (Service.EMCS.SvcGoodsReceive.GRHisOwned(id, userId))
            {
                ViewBag.IsOwned = true;
            }
            else
            {
                ViewBag.IsOwned = false;
            }
            var idReq = Service.EMCS.SvcRequestGr.GetRequestById(id);
            GoodReceiveModel data = new GoodReceiveModel();
            if (Request.UrlReferrer != null)
            {
                var url = Request.UrlReferrer.ToString().ToLower();
                if (url.Contains("emcs/grlist"))
                {
                    ViewBag.IsApprover = false;
                    HttpContext.Session["IsApprover"] = false;
                    ViewBag.crudMode = "U";
                    PaginatorBoot.Remove("SessionTRN");
                    data = InitGoodReceive(Convert.ToInt64(id));
                    data.YesNo = YesNoList();

                }
                else if (!Service.EMCS.SvcGoodsReceive.GRHisOwned(id, userId))
                {
                    ViewBag.IsApprover = true;
                    HttpContext.Session["IsApprover"] = true;
                    ViewBag.crudMode = "U";
                    PaginatorBoot.Remove("SessionTRN");
                    data = InitGoodReceive(Convert.ToInt64(idReq.IdGr));
                    data.YesNo = YesNoList();
                }


            }

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
                var userId = User.Identity.GetUserId();
                if (Service.EMCS.SvcGoodsReceive.GRHisOwned(item.Id, userId) || User.Identity.GetUserRoles().Contains("EMCSImex"))
                {
                    Service.EMCS.SvcGoodsReceive.CrudSp(item, form.Data.Status, "U");
                }
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
            return File(output.ToArray(), "application/pdf", "RGDocument_" + DateTime.Now.ToString("ddMMyyyyhhhmmss") + ".pdf");
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
        public string GetdocFileNameForArmada(System.Web.HttpPostedFileBase file, long id)
        {
            var fileName = Path.GetFileName(file.FileName);

            if (fileName != null)
            {
                var ext = Path.GetExtension(fileName);
                var path = Server.MapPath("~/Upload/EMCS/GoodsReceive");
                bool isExists = Directory.Exists(path);
                fileName = "DocArmada-" + Guid.NewGuid() + "-" + id.ToString() + ext;

                if (!isExists)
                    Directory.CreateDirectory(path);

                var fullPath = Path.Combine(path, fileName);

                if (System.IO.File.Exists(fullPath))
                {
                    System.IO.File.Delete(fullPath);
                }

                file.SaveAs(fullPath);

            }

            return fileName;
        }
        public string UploadDocumentArmada(long id)
        {
            string fileName = "";

            if (Request.Files.Count > 0)
            {
                var file = Request.Files[0];

                if (file != null && file.ContentLength > 0)
                {
                    return GetdocFileNameForArmada(file, id);
                }
            }
            return fileName;
        }
        [HttpPost]
        public ActionResult GrDocumentUploadArmada(long id, bool IsRFC = false, bool buttonRFC = false)
        {
            string fileResult = UploadDocumentArmada(id);
            if (fileResult != "")
            {
                if (IsRFC == true)
                {
                    if (buttonRFC == false)
                    {
                        var data1 = Service.EMCS.SvcGoodsReceiveItem.GetDataByIdShippingFleetById(id);
                        if (data1 != null)
                        {
                            if(data1.Status != "Deleted")
                            {
                                Service.EMCS.SvcGoodsReceive.UpdateFileArmadaDocumentForRFC(id, fileResult, buttonRFC);
                            }
                           return Json(new { status = true, msg = "Upload File Successfully" });
                        }
                        else
                        {
                            var data = Service.EMCS.SvcGoodsReceiveItem.GetArmadaById(id);
                            var obj = new ShippingFleet();
                            obj.Id = data.Id;
                            obj.IdCipl = 0;
                            obj.IdGr = data.IdGr;
                            obj.DoNo = data.DoNo ?? "";
                            obj.DaNo = data.DaNo ?? "";
                            obj.PicName = data.PicName ?? "";
                            obj.PhoneNumber = data.PhoneNumber ?? "";
                            obj.KtpNumber = data.KtpNumber ?? "";
                            obj.SimNumber = data.SimNumber ?? "";
                            obj.SimExpiryDate = data.SimExpiryDate;
                            obj.StnkNumber = data.StnkNumber ?? "";
                            obj.KirNumber = data.KirNumber ?? "";
                            obj.KirExpire = data.KirExpire;
                            obj.NopolNumber = data.NopolNumber ?? "";
                            obj.EstimationTimePickup = data.EstimationTimePickup;
                            obj.FileName = fileResult;
                            obj.Apar = data.Apar;
                            obj.Apd = data.Apd;
                            obj.Bast = data.Bast;

                            Service.EMCS.SvcGoodsReceiveItem.SaveArmadaForRFC(obj, "Updated");
                            return Json(new { status = true, msg = "Upload File Successfully" });

                        }
                    }
                    else
                    {
                        Service.EMCS.SvcGoodsReceive.UpdateFileArmadaDocumentForRFC(id, fileResult, buttonRFC);
                        return Json(new { status = true, msg = "Upload File Successfully" });

                    }

                }
                else
                {
                    var result = Service.EMCS.SvcGoodsReceive.UpdateFileArmadaDocument(id, fileResult);
                    if (HttpContext.Session["IsApprover"].ToString() == "True" && HttpContext.Session["IsApprover"] != null)
                    {
                        Service.EMCS.SvcGoodsReceiveItem.UploadHistoryOfDocument(id, fileResult);
                        return Json(new { status = true, msg = "Upload File Successfully" });

                    }
                    else
                    {
                        return Json(new { status = true, msg = "Upload File Successfully" });

                    }
                }
            }
            else
            {
                return Json(new { status = false, msg = "Upload File gagal" });
            }
        }
        public FileResult DownloadArmadaDocument(long id)
        {
            //var list = Service.EMCS.SvcCipl.CiplHistoryGetById(id);
            var files = Service.EMCS.SvcGoodsReceive.ArmadaDocumentListById(id).FirstOrDefault();
            string fullPath = Request.MapPath("~/Upload/EMCS/Dummy/NotFound.txt");

            if (files != null)
            {
                //var fileData = files;
                //fullPath = Request.MapPath("~/Upload/EMCS/GoodsReceive/" + files.Id + "/" + files.FileName);
                fullPath = Request.MapPath("~/Upload/EMCS/GoodsReceive/" + files.FileName);
                var fileBytes = System.IO.File.ReadAllBytes(fullPath);
                string fileName = files.FileName;
                return File(fileBytes, System.Net.Mime.MediaTypeNames.Application.Octet, fileName);
            }

            return File(fullPath, "text/plain", "NotFound.txt");
        }
        public FileResult DownloadArmadaDocumentForRFC(string FileName)
        {
            string fullPath = Request.MapPath("~/Upload/EMCS/Dummy/NotFound.txt");

            if (FileName != null)
            {
                fullPath = Request.MapPath("~/Upload/EMCS/GoodsReceive/" + FileName);
                var fileBytes = System.IO.File.ReadAllBytes(fullPath);
                string fileName = FileName;
                return File(fileBytes, System.Net.Mime.MediaTypeNames.Application.Octet, fileName);
            }

            return File(fullPath, "text/plain", "NotFound.txt");
        }
        public FileResult DownloadArmadaDocumentHistory(string FileName)
        {
            string fullPath = Request.MapPath("~/Upload/EMCS/Dummy/NotFound.txt");

            if (FileName != null || FileName != "")
            {
                fullPath = Request.MapPath("~/Upload/EMCS/GoodsReceive/" + FileName);
                var fileBytes = System.IO.File.ReadAllBytes(fullPath);
                string fileName = FileName;
                return File(fileBytes, System.Net.Mime.MediaTypeNames.Application.Octet, fileName);
            }

            return File(fullPath, "text/plain", "NotFound.txt");

        }

        [HttpPost]
        public long GrDocumentDeleteById(long id)
        {
            id = Service.EMCS.SvcGoodsReceive.DeleteGRDocumentById(id);
            return id;
        }
        public JsonResult GetRFCitemDataById(long id)
        {
            var data = Service.EMCS.SvcGoodsReceiveItem.GetRFCItemSIById(id);
            return Json(data,JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public bool GRDocumentInsert(List<Data.Domain.EMCS.GoodReceiveDocument> data)
        {

            var id = Service.EMCS.SvcGoodsReceive.InsertGRDocument(data);
            return id;
        }
        public string GetdocFileNameForGR(System.Web.HttpPostedFileBase file, long id)
        {
            var fileName = Path.GetFileName(file.FileName);

            if (fileName != null)
            {
                var ext = Path.GetExtension(fileName);
                var path = Server.MapPath("~/Upload/EMCS/GoodsReceive/" + id);
                bool isExists = Directory.Exists(path);
                fileName = "DocGR-" + id.ToString() + ext;

                if (!isExists)
                    Directory.CreateDirectory(path);

                var fullPath = Path.Combine(path, fileName);

                if (System.IO.File.Exists(fullPath))
                {
                    System.IO.File.Delete(fullPath);
                }

                file.SaveAs(fullPath);

            }

            return fileName;
        }
        public string UploadDocumentGr(long id)
        {
            string fileName = "";

            if (Request.Files.Count > 0)
            {
                var file = Request.Files[0];

                if (file != null && file.ContentLength > 0)
                {
                    return GetdocFileNameForGR(file, id);
                }
            }
            return fileName;
        }
        [HttpPost]
        public ActionResult GrDocumentUpload(long id)
        {
            string fileResult = UploadDocumentGr(id);
            if (fileResult != "")
            {
                var result = Service.EMCS.SvcGoodsReceive.UpdateFileGRDocument(id, fileResult);
                return Json(new { status = true, msg = "Upload File Successfully" });
            }
            else
            {
                return Json(new { status = false, msg = "Upload File gagal" });
            }
        }
        public FileResult DownloadGrDocument(long id)
        {
            //var list = Service.EMCS.SvcCipl.CiplHistoryGetById(id);
            var files = Service.EMCS.SvcGoodsReceive.GRDocumentListById(id).FirstOrDefault();
            string fullPath = Request.MapPath("~/Upload/EMCS/Dummy/NotFound.txt");

            if (files != null)
            {
                //var fileData = files;
                fullPath = Request.MapPath("~/Upload/EMCS/GoodsReceive/" + files.Id + "/" + files.Filename);
                var fileBytes = System.IO.File.ReadAllBytes(fullPath);
                string fileName = files.Filename;
                return File(fileBytes, System.Net.Mime.MediaTypeNames.Application.Octet, fileName);
            }

            return File(fullPath, "text/plain", "NotFound.txt");
        }
    }
}