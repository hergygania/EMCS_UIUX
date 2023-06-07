using App.Data.Caching;
using App.Data.Domain;
using App.Domain;
using App.Web.App_Start;
using System.Globalization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web; 
using System.Web.Mvc;
using Newtonsoft.Json;
using System.Web.Script.Serialization;

namespace App.Web.Controllers.DTS
{
    public partial class DtsController
    {
        // GET: Outbound
        [AuthorizeAcces(ActionType = AuthorizeAcces.IsRead)]
        public ActionResult OutboundV1()
        {
            ViewBag.AllowRead = AuthorizeAcces.AllowRead;
            ViewBag.AllowCreate = AuthorizeAcces.AllowCreated;
            ViewBag.AllowUpdate = AuthorizeAcces.AllowUpdated;
            ViewBag.AllowDelete = AuthorizeAcces.AllowDeleted;

            Session["AllowRead_Outbound"] = AuthorizeAcces.AllowRead;
            Session["AllowCreated_Outbound"] = AuthorizeAcces.AllowCreated;
            Session["AllowUpdated_Outbound"] = AuthorizeAcces.AllowUpdated;
            Session["AllowDeleted_Outbound"] = AuthorizeAcces.AllowDeleted;
            return View();
        }

        [AuthorizeAcces(ActionType = AuthorizeAcces.IsRead)]
        public ActionResult Outbound()
        {
            ViewBag.AllowRead = AuthorizeAcces.AllowRead;
            ViewBag.AllowCreate = AuthorizeAcces.AllowCreated;
            ViewBag.AllowUpdate = AuthorizeAcces.AllowUpdated;
            ViewBag.AllowDelete = AuthorizeAcces.AllowDeleted;

            ViewBag.RoleName = Service.DTS.DeliveryRequisition.GetRoleName(SiteConfiguration.UserName);

            Session["AllowRead_Outbound"] = AuthorizeAcces.AllowRead;
            Session["AllowCreated_Outbound"] = AuthorizeAcces.AllowCreated;
            Session["AllowUpdated_Outbound"] = AuthorizeAcces.AllowUpdated;
            Session["AllowDeleted_Outbound"] = AuthorizeAcces.AllowDeleted;
            return View("v2/Outbound");
        }

        [AuthorizeAcces(ActionType = AuthorizeAcces.IsRead)]
        [Route("DTS/V2/DetailItem/{HeaderID}/{RefItemId}")]
        public ActionResult OutboundDetailItem(long HeaderID, long RefItemId)
        {
            ViewBag.AllowRead = AuthorizeAcces.AllowRead;
            ViewBag.AllowCreate = AuthorizeAcces.AllowCreated;
            ViewBag.AllowUpdate = AuthorizeAcces.AllowUpdated;
            ViewBag.AllowDelete = AuthorizeAcces.AllowDeleted;

            ViewBag.HeaderId = HeaderID;
            ViewBag.RefItemId = RefItemId;

            Session["AllowRead_Outbound"] = AuthorizeAcces.AllowRead;
            Session["AllowCreated_Outbound"] = AuthorizeAcces.AllowCreated;
            Session["AllowUpdated_Outbound"] = AuthorizeAcces.AllowUpdated;
            Session["AllowDeleted_Outbound"] = AuthorizeAcces.AllowDeleted;
            return View("v2/FormOutboundHistoryTracking");
        }

        [HttpGet]
        [AuthorizeAcces(ActionType = AuthorizeAcces.IsRead,UrlMenu = "OutboundOld")]
        public ActionResult FormOutbound()
        {
            ViewBag.formType = "insert";
            var DANo = Request.QueryString["ID"];
            if (DANo != null)
            {
                ViewBag.formType = "edit";
                ViewBag.detail = Service.DTS.ShipmentOutbound.GetDetailForNonCKBbyID(Convert.ToInt32(DANo));
            }
            return View();
        }

        [HttpPost]
        public JsonResult FormOutbound(ShipmentOutbound formColl)
        {
            var formType = Request.Form["formType"].ToString();
            var DA = Request.Form["ID"].ToString();
            if (formType == "edit")
            {
                var ob = Service.DTS.ShipmentOutbound.GetDetailForNonCKBbyID(Convert.ToInt32(DA));
                formColl.ID = ob.ID;
                ob = formColl;
                Service.DTS.ShipmentOutbound.crud(ob, "U");
                return JsonCRUDMessage("U");
            }
            else
            {
                var ob = new ShipmentOutbound();
                ob = formColl;

                Service.DTS.ShipmentOutbound.crud(ob, "I");
                return JsonCRUDMessage("I");
            }
        }

        [HttpPost]
        public JsonResult FormLogProcess(DeliveryRequisitionUnit formColl)
        {
            try
            {
                bool IsApplyToAll = Boolean.Parse( Request.Form["IsApplyToAll"].ToString() );
                string Attachment1 = null;
                if (Request.Files["Attachment1"] !=null)
                {
                    if (Request.ContentType.Contains("multipart/form-data"))
                    {
                        if (App.Web.Helper.Extensions.FileExtention.isValidLogProcessFile(Request.Files["Attachment1"].FileName))
                        {
                            Tuple<bool, Object> result = DoUpload(Request.Files["Attachment1"], null, formColl.HeaderID.ToString());
                            if (result.Item1 == true)
                            {
                                Attachment1 = result.Item2.ToString();
                            }
                        }

                    }
                    else
                    {
                        Attachment1 = "";
                    }
                }
               
                if (IsApplyToAll)
                {
                    var items = Service.DTS.DeliveryRequisitionUnit.GetListByHeaderID(formColl.HeaderID);
                    if (items == null)
                    {
                        return Json(new { status = 1, Msg = "Item Not Found" }, JsonRequestBehavior.DenyGet);
                    }
                    foreach (DeliveryRequisitionUnit itemOld in items)
                    {
                        SaveLogUnit(itemOld, formColl, Attachment1);
                    }
                }
                else
                {
                    var itemOld = Service.DTS.DeliveryRequisitionUnit.GetByHeaderID(formColl.HeaderID, formColl.RefItemId);
                    if (itemOld == null)
                    {
                        return Json(new { status = 1, Msg = "Item Not Found" }, JsonRequestBehavior.DenyGet);
                    }
                    SaveLogUnit(itemOld, formColl, Attachment1);
                }
                return JsonCRUDMessage("U");
            }
            catch (Exception ex)
            {
                return Json(new { status = 1, Msg = ex.Message }, JsonRequestBehavior.DenyGet);
            }
        }

        public JsonResult FormDestinationUnit(DeliveryRequisitionUnit formColl)
        {
            try
            {
                var itemOld = Service.DTS.DeliveryRequisitionUnit.GetByHeaderID(formColl.HeaderID, formColl.RefItemId);
                itemOld.CustID = formColl.CustID;
                itemOld.CustName = formColl.CustName;
                itemOld.CustAddress = formColl.CustAddress;
                itemOld.PICName = formColl.PICName;
                itemOld.PICHp = formColl.PICHp;
                itemOld.Kecamatan = formColl.Kecamatan;
                itemOld.Kabupaten = formColl.Kabupaten;
                itemOld.Province = formColl.Province;

                var res = Service.DTS.DeliveryRequisitionUnit.crud(itemOld, "U");

                return JsonCRUDMessage("U");
            }
            catch (Exception ex)
            {
                return Json(new { status = 1, Msg = ex.Message }, JsonRequestBehavior.DenyGet);
            }
        }

        private void SaveLogUnit(DeliveryRequisitionUnit itemOld, DeliveryRequisitionUnit formColl, string Attachment1)
        {
            if (formColl.Action != null && formColl.Action.Length > 4)
            {
                formColl.Action = "";
            }
            string logDescription = GenerrateLogDesc(itemOld, formColl);
            itemOld.Attachment1 = Attachment1;
            itemOld.RefItemId = itemOld.RefItemId;
            itemOld.EstTimeDeparture = (formColl.EstTimeDeparture == null) ? itemOld.EstTimeDeparture : formColl.EstTimeDeparture;
            itemOld.EstTimeArrival = (formColl.EstTimeArrival == null) ? itemOld.EstTimeArrival : formColl.EstTimeArrival;
            itemOld.ActTimeArrival = (formColl.ActTimeArrival == null) ? itemOld.ActTimeArrival : formColl.ActTimeArrival;
            itemOld.ActTimeDeparture = (formColl.ActTimeDeparture == null) ? itemOld.ActTimeDeparture : formColl.ActTimeDeparture;
            itemOld.Checked = true;

            itemOld.Action = (formColl.Action == null) ? itemOld.Action : formColl.Action;
            itemOld.Status = (formColl.Status == null) ? itemOld.Status : formColl.Status;
            itemOld.Position = (formColl.Position == null) ? itemOld.Position : formColl.Position;

            itemOld.VeselName = (formColl.VeselName == null) ? itemOld.VeselName : formColl.VeselName;
            itemOld.PICName = (formColl.PICName == null) ? itemOld.PICName : formColl.PICName;
            itemOld.PICHp = (formColl.PICHp == null) ? itemOld.PICHp : formColl.PICHp;
            itemOld.DANo = (formColl.DANo == null) ? itemOld.DANo : formColl.DANo;
            itemOld.Notes = (formColl.Notes == null) ? itemOld.Notes : formColl.Notes;

            itemOld.VeselNoPolice = (formColl.VeselNoPolice == null) ? itemOld.VeselNoPolice : formColl.VeselNoPolice;
            itemOld.DriverName = (formColl.DriverName == null) ? itemOld.DriverName : formColl.DriverName;
            itemOld.DriverHp = (formColl.DriverHp == null) ? itemOld.DriverHp : formColl.DriverHp;

            var res = Service.DTS.DeliveryRequisitionUnit.crud(itemOld, "U");

            if (res > 0 && logDescription != "")
            {
                var logStatus = new DeliveryRequisitionStatus
                {
                    HeaderID = itemOld.HeaderID,
                    RefNo = itemOld.RefNo,
                    RefItemId = itemOld.RefItemId,
                    Action = itemOld.Action,
                    Status = itemOld.Status,
                    Position = itemOld.Position,
                    EstTimeDeparture = itemOld.EstTimeDeparture,
                    EstTimeArrival = itemOld.EstTimeArrival,
                    ActTimeDeparture = itemOld.ActTimeDeparture,
                    ActTimeArrival = itemOld.ActTimeArrival,
                    Notes = itemOld.Notes,
                    LogDescription = logDescription,
                    Attachment1 = itemOld.Attachment1,
                };
                Service.DTS.DeliveryRequisitionStatus.crud(logStatus, "I");
                sendingEmailDR("complete", formColl.HeaderID, "NOTIFICATION_DR", logStatus.ID);
                //sendingEmail(item.Status, item.KeyCustom, item.Origin, item.Kabupaten, item.Model, expectedTime, item.ReqName, item.ReqID, item.Unit, item.DINo, item.CustName);
            }
        }

        [HttpPost]
        public JsonResult UploadTrackingHistory(List<DeliveryRequisitionStatusUpload> formColl)
        {
            try
            {
                if (formColl != null && formColl.Count() > 0)
                {
                    List<DeliveryRequisitionStatusUpload> upSuccess = new List<DeliveryRequisitionStatusUpload>();
                    List<DeliveryRequisitionStatusUpload> upFailed = new List<DeliveryRequisitionStatusUpload>();
                    char[] separator = { '-' };
                    foreach (DeliveryRequisitionStatusUpload upload in formColl)
                    {
                        var header = Service.DTS.DeliveryRequisition.GetByKeyCustom(upload.KeyCustom);
                        if (header == null)
                        {
                            upFailed.Add(upload);
                            continue;
                        }
                        DeliveryRequisitionUnit itemOld = Service.DTS.DeliveryRequisitionUnit.GetByHeaderID(
                                header.ID, upload.Model, upload.SerilaNumber, upload.Batch, Convert.ToInt64( upload.RefItemId )
                            );
                        if (itemOld == null)
                        {
                            upFailed.Add(upload);
                            continue;
                        }
                        DeliveryRequisitionUnit itemNew = new DeliveryRequisitionUnit {
                            HeaderID = itemOld.HeaderID,
                            RefNo = itemOld.RefNo,
                            RefItemId = itemOld.RefItemId,
                            Model = itemOld.Model,
                            SerialNumber = itemOld.SerialNumber,
                            Batch = itemOld.Batch,
                            VeselName = itemOld.VeselName,
                            PICName = itemOld.PICName,
                            PICHp = itemOld.PICHp,
                            VeselNoPolice = itemOld.VeselNoPolice,
                            DriverName = itemOld.DriverName,
                            DriverHp = itemOld.DriverHp,
                            DANo = itemOld.DANo,
                            PickUpPlan = itemOld.PickUpPlan,
                            EstTimeDeparture = itemOld.EstTimeDeparture,
                            EstTimeArrival = itemOld.EstTimeArrival,
                            Attachment1 = itemOld.Attachment1,
                            Attachment2 = itemOld.Attachment2,
                            ActTimeDeparture = itemOld.ActTimeDeparture,
                            ActTimeArrival = itemOld.ActTimeArrival,
                            Action = itemOld.Action,
                            Status = itemOld.Status,
                            Position = itemOld.Position,
                            Notes = itemOld.Notes,
                            Checked = itemOld.Checked,
                            CreateBy = itemOld.CreateBy,
                            CreateDate = itemOld.CreateDate,
                            UpdateBy = itemOld.UpdateBy,
                            UpdateDate = itemOld.UpdateDate
                        };
                        bool isUpdated = false;
                        if (upload.Action != null && upload.Action != "")
                        {
                            string[] actionArr = upload.Action.Split(separator, 2);
                            itemNew.Action = actionArr[0].Trim();
                            isUpdated = true;
                        }
                        if (upload.Status != null && upload.Status != "")
                        {
                            string[] statusArr = upload.Status.Split(separator, 2);
                            itemNew.Status = statusArr[0].Trim();
                            isUpdated = true;
                        }
                        if (upload.Position != null && upload.Position != "")
                        {
                            itemNew.Position = upload.Position;
                            isUpdated = true;
                        }
                        if (upload.ETD != null)
                        {
                            itemNew.EstTimeDeparture = upload.ETD;
                            isUpdated = true;
                        }
                        if (upload.ETA != null)
                        {
                            itemNew.EstTimeArrival = upload.ETA;
                            isUpdated = true;
                        }
                        if (upload.ATD != null)
                        {
                            itemNew.ActTimeDeparture = upload.ATD;
                            isUpdated = true;
                        }
                        if (upload.ATA != null)
                        {
                            itemNew.ActTimeArrival = upload.ATA;
                            isUpdated = true;
                        }
                        if (upload.Notes!=null)
                        {
                            itemNew.Notes = upload.Notes;
                            isUpdated = true;
                        }
                        if (isUpdated)
                        {
                            var res = Service.DTS.DeliveryRequisitionUnit.crud(itemNew, "U");
                            string logDescription = GenerrateLogDesc(itemOld, itemNew);
                            // if (res > 0 && logDescription != "")
                            if (res > 0)
                            {
                                var logStatus = new DeliveryRequisitionStatus
                                {
                                    HeaderID = itemNew.HeaderID,
                                    RefNo = itemNew.RefNo,
                                    RefItemId = itemNew.RefItemId,
                                    Action = (upload.Action != null && upload.Action != "") ? itemNew.Action : null,
                                    Status = (upload.Status != null && upload.Status != "") ? itemNew.Status : null,
                                    Position = upload.Position,
                                    EstTimeDeparture = upload.ETD,
                                    EstTimeArrival = upload.ETA,
                                    ActTimeDeparture = upload.ATD,
                                    ActTimeArrival = upload.ATA,
                                    Notes = upload.Notes,
                                    LogDescription = logDescription,
                                };
                                Service.DTS.DeliveryRequisitionStatus.crud(logStatus, "I");
                                upSuccess.Add(upload);
                                //sendingEmail(item.Status, item.KeyCustom, item.Origin, item.Kabupaten, item.Model, expectedTime, item.ReqName, item.ReqID, item.Unit, item.DINo, item.CustName);
                            }
                        }
                    }
                    //return JsonCRUDMessage("U");
                    return Json(new {
                        Status = 0,
                        Msg = upSuccess.Count() + " of " + formColl.Count() + " Data Success!",
                        Success = upSuccess,
                        Failed = upFailed
                    }, JsonRequestBehavior.DenyGet);
                }
                else
                {
                    return Json(new { Status = 1, Msg = "Upload data should not be empty" }, JsonRequestBehavior.DenyGet);
                }
            }
            catch (Exception ex)
            {
                return Json(new { Status = 1, Msg = ex.Message }, JsonRequestBehavior.DenyGet);
            }
        }

        [HttpGet]
        public FileResult DownloadTemplateUploadDR()
        {
            Helper.Service.DTS.DownloadDRController data = new Helper.Service.DTS.DownloadDRController();
            string path = Server.MapPath("~/Temp/template-mass-upload-dr-new.xlsx"); // @"c:\temp\template-mass-upload-dr.xls"
            var tbl = App.Service.DTS.DeliveryRequisition.GetListForTemplate();
            tbl = tbl.OrderBy(a => a.ID).ToList();
            var fileResult = data.DownloadTemplateUpload2(path, tbl);
            return fileResult as FileResult;
        }

        private string GenerrateLogDesc(DeliveryRequisitionUnit _old, DeliveryRequisitionUnit _new)
        {
            string logDescription = "";
            //if (_old.EstTimeDeparture != _new.EstTimeDeparture)
            if (IsValueChange(_old.EstTimeDeparture, _new.EstTimeDeparture))
            {
                logDescription += "ETD Changed to " + _new.EstTimeDeparture?.ToString("dd.MM.yyyy");
            }
            if (IsValueChange(_old.EstTimeArrival , _new.EstTimeArrival))
            {
                logDescription += (logDescription.Equals("") ? "" : ", ") + "ETA Changed to " + _new.EstTimeArrival?.ToString("dd.MM.yyyy");
            }
            if (IsValueChange(_old.ActTimeDeparture , _new.ActTimeDeparture))
            {
                logDescription += (logDescription.Equals("") ? "" : ", ") + "ATD Changed to " + _new.ActTimeDeparture?.ToString("dd.MM.yyyy");
            }
            if (IsValueChange(_old.ActTimeArrival , _new.ActTimeArrival))
            {
                logDescription += (logDescription.Equals("") ? "" : ", ") + "ATA Changed to " + _new.ActTimeArrival?.ToString("dd.MM.yyyy");
            }
            if (_old.Action != _new.Action)
            {
                var ccAction = Service.DTS.CategoryCode.GetAction(_new.Action);
                string actionDesc = (ccAction != null) ? ccAction.Description1 : "";
                logDescription += (logDescription.Equals("") ? "" : ", ") + "Activity Changed to " + actionDesc;
            }
            if (_old.Status != _new.Status)
            {
                var ccStatus = Service.DTS.CategoryCode.GetStatus(_new.Status);
                string statusDesc = (ccStatus != null) ? ccStatus.Description1 : "";
                logDescription += (logDescription.Equals("") ? "" : ", ") + "Status Changed to " + statusDesc;
            }
            if (_old.Position != _new.Position)
            {
                logDescription += (logDescription.Equals("") ? "" : ", ") + "Position on " + _new.Position;
            }

            if (IsValueChange(_old.VeselName, _new.VeselName))
            {
                logDescription += (logDescription.Equals("") ? "" : ", ") + "Vessel Name Changed to " + _new.VeselName;
            }
            if (IsValueChange(_old.PICName, _new.PICName))
            {
                logDescription += (logDescription.Equals("") ? "" : ", ") + "PIC Name Changed to " + _new.PICName;
            }
            if (IsValueChange(_old.PICHp, _new.PICHp))
            {
                logDescription += (logDescription.Equals("") ? "" : ", ") + "PIC HP Changed to " + _new.PICHp;
            }
            if (IsValueChange(_old.DANo, _new.DANo))
            {
                logDescription += (logDescription.Equals("") ? "" : ", ") + "DA No Changed to " + _new.DANo;
            }
            if (IsValueChange(_old.VeselNoPolice, _new.VeselNoPolice))
            {
                logDescription += (logDescription.Equals("") ? "" : ", ") + "No Police Changed to " + _new.VeselNoPolice;
            }
            if (IsValueChange(_old.DriverName, _new.DriverName))
            {
                logDescription += (logDescription.Equals("") ? "" : ", ") + "Driver Name Changed to " + _new.DriverName;
            }
            if (IsValueChange(_old.DriverHp, _new.DriverHp))
            {
                logDescription += (logDescription.Equals("") ? "" : ", ") + "Driver HP Changed to " + _new.DriverHp;
            }

            if (_old.Notes != _new.Notes)
            {
                logDescription += (logDescription.Equals("") ? "" : ", ") + _new.Notes;
            }
            return logDescription;
        }

        private bool IsValueChange(string _old, string _new)
        {
            if (_new == null)
            {
                return false;
            }

            return _old != _new;
        }

        private bool IsValueChange(DateTime? _old, DateTime? _new)
        {
            if (_new == null)
            {
                return false;
            }

            return _old != _new;
        }

        public ActionResult OutboundPage()
        {
            this.PaginatorBoot.Remove("SessionTRN");
            return OutboundPageXt();
        }

        public ActionResult OutboundPageXt()
        {
            Func<App.Data.Domain.DTS.OutboundFilter, List<Data.Domain.ShipmentOutboundListData>> func = delegate (App.Data.Domain.DTS.OutboundFilter filter)
            {
                var param = Request["params"];
                if (!string.IsNullOrEmpty(param))
                {
                    JavaScriptSerializer ser = new JavaScriptSerializer();
                    filter = ser.Deserialize<App.Data.Domain.DTS.OutboundFilter>(param);
                }

                var list = Service.DTS.ShipmentOutbound.GetListFilter(filter);
                return list.ToList();
            };

            var paging = PaginatorBoot.Manage("SessionTRN", func).Pagination.ToJsonResult();
            return Json(paging, JsonRequestBehavior.AllowGet);
        }
        [AuthorizeAcces(ActionType = AuthorizeAcces.IsRead,UrlMenu = "Outbound")]
        public ActionResult OutboundReportPage()
        {
            this.PaginatorBoot.Remove("SessionTRN");
            return OutboundReportPageXt();
        }
        [AuthorizeAcces(ActionType = AuthorizeAcces.IsRead, UrlMenu = "Outbound")]
        public ActionResult OutboundReportPageXt()
        {
            Func<App.Data.Domain.DTS.OutboundFilter, List<Data.Domain.ShipmentOutbound>> func = delegate (App.Data.Domain.DTS.OutboundFilter filter)
            {
                var param = Request["params"];
                if (!string.IsNullOrEmpty(param))
                {
                    JavaScriptSerializer ser = new JavaScriptSerializer();
                    filter = ser.Deserialize<App.Data.Domain.DTS.OutboundFilter>(param);
                }

                var list = Service.DTS.ShipmentOutbound.GetListFilterReport(filter);
                return list.ToList();
            };

            var paging = PaginatorBoot.Manage("SessionTRN", func).Pagination.ToJsonResult();
            return Json(paging, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult isDaExists()
        {
            var key = Request.Form["DA"];
            var isExists = Service.DTS.ShipmentOutbound.isDaExists(key);
            return Json(isExists, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult OutboundDelete()
        {
            try
            {
                var DA = Request.Form["DA"].ToString();
                ShipmentOutbound item = Service.DTS.ShipmentOutbound.GetDetailForNonCKBbyID(Convert.ToInt32(DA));
                Service.DTS.ShipmentOutbound.crud(item, "D");
                return JsonCRUDMessage("D");
            }
            catch (Exception ex)
            {
                return Json(new { result = "failed", message = ex.Message }, JsonRequestBehavior.DenyGet);
            }
        }

        [HttpPost]
        public JsonResult submitRemarkOutbound(ShipmentOutbound formColl)
        {
            var ob = Service.DTS.ShipmentOutbound.GetDetailByID(formColl.ID);
            ob.Remarks = formColl.Remarks;
            ob.Position = formColl.Position;
            ob.Status = formColl.Status;
            ob.NoPol = formColl.NoPol;
            ob.DriverName = formColl.DriverName;
            ob.HPInlandFreight = formColl.HPInlandFreight;
            ob.VesselName = formColl.VesselName;
            ob.PIC = formColl.PIC;
            ob.HPSealandFreight = formColl.HPSealandFreight;
            try
            {
                Service.DTS.ShipmentOutbound.crudupdateOutbound(ob, "U");
                return JsonCRUDMessage("U");
            }
            catch (Exception ex)
            {
                return Json(new { result = "failed", message = ex.Message }, JsonRequestBehavior.DenyGet);
            }
        }

        [HttpPost]
        public JsonResult getDetailDataOutbound()
        {
            var key = Request.Form["Key"].ToString();
            if (key != "")
            {
                var detail = Service.DTS.ShipmentOutbound.GetDetailSPview(key);
                return Json(detail, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json("", JsonRequestBehavior.AllowGet);
            }
        }

        [Route("DTS/v2/getDetailDataOutbound")]
        public JsonResult getDetailDataOutboundV2(string key)
        {
            try
            {
                string _key = key;
                if (!key.Contains("DR-") && !key.Contains("DR0"))
                {
                    _key = "DR-" + key.Trim();
                }
                var _header = Service.DTS.DeliveryRequisition.GetDetailOutbound(_key);
                if (_header == null)
                {
                    _header = Service.DTS.DeliveryRequisition.GetDetailOutbound("DR" + key.Trim().PadLeft(9, '0'));
                }

                if (_header != null)
                {
                    var _units = Service.DTS.DeliveryRequisitionUnit.GetListRefByHeaderID(_header.ID);
                    var _log = Service.DTS.DeliveryRequisitionStatus.GetListByHeaderID(_header.ID);
                    return Json(new
                    {
                        result = true,
                        data = new { header = _header, details = _units, log = _log }
                    },
                        JsonRequestBehavior.AllowGet);
                }
                return Json(new { result = false, message = "data not fund" }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { result = false, message = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        [Route("DTS/v2/getDetailDataOutboundUnitV2")]
        public JsonResult getDetailDataOutboundUnitV2(long HeaderID)
        {
            try
            {
                var data = Service.DTS.DeliveryRequisitionUnit.GetListByHeaderID(HeaderID);
                return Json(data, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { result = false, message = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        public JsonResult GetDetailDataOutboundStatus(long HeaderID, long RefItemId)
        {
            try
            {
                var header = Service.DTS.DeliveryRequisition.GetId(HeaderID);
                var unit = Service.DTS.DeliveryRequisitionUnit.GetListRefByHeaderID(HeaderID, RefItemId);
                var dataStatus = Service.DTS.DeliveryRequisitionStatus.GetListByHeaderID(HeaderID, RefItemId);
                return Json( new { result = true, data = new { header = header , detail=unit, detailStatus = dataStatus } }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { result = false, message = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult DownloadOutbound(App.Data.Domain.DTS.OutboundFilter filter)
        {
            Guid guid = Guid.NewGuid();
            Helper.Service.DTS.DownloadOutboundController data = new Helper.Service.DTS.DownloadOutboundController();

            Session[guid.ToString()] = data.DownloadToExcel(filter);

            return Json(guid.ToString(), JsonRequestBehavior.AllowGet);
        }

        public ActionResult DownloadOutboundReport(App.Data.Domain.DTS.OutboundFilter filter)
        {
            Guid guid = Guid.NewGuid();
            Helper.Service.DTS.DownloadOutboundReportController data = new Helper.Service.DTS.DownloadOutboundReportController();
            Session[guid.ToString()] = data.DownloadToExcel(filter);
            return Json(guid.ToString(), JsonRequestBehavior.AllowGet);
        }

        public FileResult DownloadToExcelOutbound(string guid)
        {
            return Session[guid] as FileResult;
        }

        public JsonResult GetMasterAction(string key)
        {
            var item = Service.DTS.CategoryCode.GetListAction(key);
            return Json(item, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetMasterStatus(string key)
        {
            var item = Service.DTS.CategoryCode.GetListStatus(key);
            return Json(item, JsonRequestBehavior.AllowGet);
        }
        public JsonResult GetSNVisionLink(string SerialNumber)
        {
            var details = Service.DTS.ShipmentOutbound.GetSNVisionLinkSN(SerialNumber);
            return Json(details, JsonRequestBehavior.AllowGet);
        }
    }
}