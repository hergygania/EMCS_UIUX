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
using System.IO;
using System.Text.RegularExpressions;
using System.ComponentModel;
using System;
using App.Data.Domain.EMCS;

namespace App.Web.Controllers.EMCS
{
    public partial class EmcsController
    {

        [AuthorizeAcces(ActionType = AuthorizeAcces.IsRead)]
        public ActionResult BLAWBList()
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
        [AuthorizeAcces(ActionType = AuthorizeAcces.IsRead, UrlMenu = "BLAWBList")]
        public JsonResult GetBLAWBList(GridListFilterModel filter)
        {
            var dataFilter = new Data.Domain.EMCS.GridListFilter();
            dataFilter.Limit = filter.Limit;
            dataFilter.Order = filter.Order;
            dataFilter.Term = filter.Term;
            dataFilter.Sort = filter.Sort;
            dataFilter.Offset = filter.Offset;

            var data = Service.EMCS.SvcBlAwb.BLAWBList(dataFilter);
            return Json(data, JsonRequestBehavior.AllowGet);
        }
        public ActionResult BlAwbRevise(Data.Domain.EMCS.GridListFilter filter)
        {
            ApplicationTitle();
            var data = new BlAwbModel();
            data.Data = Service.EMCS.SvcCargo.GetCargoById(filter.Id);
            data.BlAwb = Service.EMCS.SvcBlAwb.GetByIdcl(filter.Id);
            data.Request = Service.EMCS.SvcRequestCl.GetRequestCl(filter.Id);
            return View(data);
        }
        public ActionResult BlAwbView(Data.Domain.EMCS.GridListFilter filter)
        {
            ApplicationTitle();
            var data = new BlAwbModel();
            data.Data = Service.EMCS.SvcCargo.GetCargoById(filter.Id);
            data.BlAwb = Service.EMCS.SvcBlAwb.GetByIdcl(filter.Id);
            data.Request = Service.EMCS.SvcRequestCl.GetRequestCl(filter.Id);
            //data.Request = Service.EMCS.SvcRequestCl.GetRequestCl(filter.Id);
            ViewBag.WizardData = Service.EMCS.SvcWizard.GetWizardData("cargo", filter.Id);
            data.Request = Service.EMCS.SvcRequestCl.GetRequestCl(filter.Id);
            return View(data);
        }
        public ActionResult CreateBlAwb(Data.Domain.EMCS.GridListFilter filter,string Type = "")
        {
            ApplicationTitle();
            var data = new BlAwbModel();
            ViewBag.IsApprover = false;
            if (filter.Rfc)
                ViewBag.CanRequestForChange = true;
            else
                ViewBag.CanRequestForChange = false;
            data.Data = Service.EMCS.SvcCargo.GetCargoById(filter.Id);
            data.Request = Service.EMCS.SvcRequestCl.GetRequestCl(filter.Id);
            data.documentsList = Service.EMCS.SvcBlAwb.GetDocumentsBlAwb(filter.Id);
            var blAwbs = Service.EMCS.SvcBlAwb.ListGetByIdcl(filter.Id);
            if (Request.UrlReferrer != null)
            {
                var url = Request.UrlReferrer.ToString().ToLower();
                if (url.Contains("emcs/blawblist"))
                {
                    ViewBag.IsApprover = false;
                }
                else if (!Service.EMCS.SvcBlAwb.BlAwbHisOwned(filter.Id, SiteConfiguration.UserName))
                {
                    if (Type != "New")
                    {
                        ViewBag.IsApprover = true;
                    }
                }
            }
            if (blAwbs.Count > 0)
            {
                foreach (Data.Domain.EMCS.BlAwb blAwb in blAwbs)
                {
                    var item = new BlAwbViewModel();

                    item.Id = Convert.ToInt32(blAwb.Id);
                    item.Number = blAwb.Number;
                    item.MasterBlDate = Convert.ToDateTime(blAwb.MasterBlDate);
                    item.HouseBlNumber = blAwb.HouseBlNumber;
                    item.HouseBlDate = Convert.ToDateTime(blAwb.HouseBlDate);
                    item.Description = blAwb.Description;
                    item.FileName = blAwb.FileName;
                    item.Publisher = blAwb.Publisher;
                    item.CreateBy = blAwb.CreateBy;
                    item.CreateDate = Convert.ToDateTime(blAwb.CreateDate);
                    item.UpdateBy = blAwb.UpdateBy;
                    item.UpdateDate = blAwb.UpdateDate;
                    item.IsDelete = blAwb.IsDelete;
                    item.IdCl = Convert.ToInt32(blAwb.IdCl);
                    data.BlAwbList.Add(item);
                }
            }
            else
            {
                data.BlAwbList.Add(new BlAwbViewModel());
            }
            return View(data);
        }
        public ActionResult RFCBlAwb(Data.Domain.EMCS.GridListFilter filter)
        {
            ApplicationTitle();
            var data = new BlAwbModel();
            if (filter.Rfc)
                ViewBag.CanRequestForChange = true;
            else
                ViewBag.CanRequestForChange = false;
            data.Data = Service.EMCS.SvcCargo.GetCargoById(filter.Id);
            data.Request = Service.EMCS.SvcRequestCl.GetRequestCl(filter.Id);
            data.documentsList = Service.EMCS.SvcBlAwb.GetDocumentsBlAwb(filter.Id);
            var blAwbs = Service.EMCS.SvcBlAwb.ListGetByIdcl(filter.Id);
            if (blAwbs.Count > 0)
            {
                foreach (Data.Domain.EMCS.BlAwb blAwb in blAwbs)
                {
                    var item = new BlAwbViewModel();

                    item.Id = Convert.ToInt32(blAwb.Id);
                    item.Number = blAwb.Number;
                    item.MasterBlDate = Convert.ToDateTime(blAwb.MasterBlDate);
                    item.HouseBlNumber = blAwb.HouseBlNumber;
                    item.HouseBlDate = Convert.ToDateTime(blAwb.HouseBlDate);
                    item.Description = blAwb.Description;
                    item.FileName = blAwb.FileName;
                    item.Publisher = blAwb.Publisher;
                    item.CreateBy = blAwb.CreateBy;
                    item.CreateDate = Convert.ToDateTime(blAwb.CreateDate);
                    item.UpdateBy = blAwb.UpdateBy;
                    item.UpdateDate = blAwb.UpdateDate;
                    item.IsDelete = blAwb.IsDelete;
                    item.IdCl = Convert.ToInt32(blAwb.IdCl);
                    data.BlAwbList.Add(item);
                }
            }
            else
            {
                data.BlAwbList.Add(new BlAwbViewModel());
            }
            return View(data);
        }

        [HttpPost]
        public ActionResult DraftRFCBlAwb(Data.Domain.EMCS.BlAwb_Change form)
        {
            long data = 0;

            if (form.Status == "Updated")
            {
                //if (form.Id != 0)
                //{
                //    var obj = Service.EMCS.SvcBlAwb.BlAwbRFCChangeListById(form.Id);
                //    if(obj != null)
                //    {
                //        form = obj;
                //        string fileResult = UploadFileBlAwb("BLAWB" + form.IdCl + form.Number + "Doc");
                //        form.FileName = fileResult;
                //        if (form.Status != "Deleted")
                //        {
                //            data = Service.EMCS.SvcBlAwb.InsertRFCBlAwb(form);
                //            return Json(new { status = true, msg = "Upload sucess" });
                //        }
                //    }
                //}
                //else
                //{
                var data1 = Service.EMCS.SvcBlAwb.BlAwbRFCChangeListByIdBlAwb(form.IdBlAwb);
                if (data1 == null)
                {
                    var getblawb = Service.EMCS.SvcBlAwb.GetBlAwbById(form.IdBlAwb);
                    form.FileName = getblawb.FileName;
                    data = Service.EMCS.SvcBlAwb.InsertRFCBlAwb(form);
                    return Json(new { status = true, msg = "Upload sucess" });
                }
                else
                {
                    if (data1.Status != "Deleted")
                    {
                        var getblawb = Service.EMCS.SvcBlAwb.GetBlAwbById(form.IdBlAwb);
                        form.FileName = getblawb.FileName;
                        data = Service.EMCS.SvcBlAwb.InsertRFCBlAwb(form);
                    }

                }

            }
            else if (form.Status == "UploadFile")
            {
                if (form.Id != 0)
                {
                    var obj = Service.EMCS.SvcBlAwb.BlAwbRFCChangeListById(form.Id);
                    if (obj != null)
                    {
                        if (obj.Status != "Deleted")
                        {
                            string fileResult = UploadFileBlAwb("BLAWB" + obj.IdCl + Guid.NewGuid() + obj.Number + "Doc");
                            obj.FileName = fileResult;
                            //if (obj.Status != "Deleted")
                            //{
                            Service.EMCS.SvcBlAwb.InsertRFCBlAwb(obj);
                            return Json(new { status = true, msg = "Upload sucess" });
                            //}
                        }
                        else
                        {
                            return Json(new { status = false, msg = "This Record Is Deleted" });
                        }

                    }
                }
                if (form.IdBlAwb != 0)
                {
                    var data1 = Service.EMCS.SvcBlAwb.BlAwbRFCChangeListByIdBlAwb(form.IdBlAwb);
                    if (data1 != null)
                    {
                        if (data1.Status != "Deleted")
                        {
                            string fileResult = UploadFileBlAwb("BLAWB" + form.IdCl + Guid.NewGuid() + form.Number + "Doc");
                            data1.FileName = fileResult;
                            data = Service.EMCS.SvcBlAwb.InsertRFCBlAwb(data1);
                            return Json(new { status = true, msg = "Upload sucess" });

                        }
                        else
                        {

                            return Json(new { status = false, msg = "This Record Is Deleted" });
                        }
                    }
                    else
                    {
                        string fileResult = UploadFileBlAwb("BLAWB" + form.IdCl + Guid.NewGuid() + form.Number + "Doc");
                        form.FileName = fileResult;
                        form.Status = "Updated";
                        data = Service.EMCS.SvcBlAwb.InsertRFCBlAwb(form);
                        return Json(new { status = true, msg = "Upload sucess" });
                    }
                }
            }
            else
            {
                data = Service.EMCS.SvcBlAwb.InsertRFCBlAwb(form);
            }

            //if (status == "Submit")
            //    Service.EMCS.SvcBlAwb.UpdateRequestCargolist(form.IdCl, status);

            return Json(data);
        }
        [HttpPost]
        public ActionResult RequestForChangeBl(RequestForChangeModel form)
        {

            try
            {
                var NewArmada = new List<ShippingFleet>();
                SpGoodReceive GRData = new SpGoodReceive();
                _errorHelper.Error("SaveChangeHistory - Method call started");
                var requestForChange = new RequestForChange();
                var cargodata = Service.EMCS.SvcCargo.GetCargoById(Convert.ToInt64(form.FormId));
                requestForChange.FormNo = cargodata.ClNo;
                requestForChange.FormType = form.FormType;
                requestForChange.Status = form.Status;
                requestForChange.FormId = form.FormId;
                requestForChange.Reason = form.Reason;
                var id = Service.EMCS.SvcCipl.InsertRequestChangeHistory(requestForChange);
                return Json("");
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public ActionResult BlAwbApproval(Data.Domain.EMCS.GridListFilter filter)
        {
            ApplicationTitle();
            var data = new BlAwbModel();
            data.Data = Service.EMCS.SvcCargo.GetCargoById(filter.Id);
            data.BlAwb = Service.EMCS.SvcBlAwb.GetByIdcl(filter.Id);
            data.Request = Service.EMCS.SvcRequestCl.GetRequestCl(filter.Id);
            ViewBag.WizardData = Service.EMCS.SvcWizard.GetWizardData("cargo", filter.Id);
            return View(data);
        }

        [HttpPost]
        public JsonResult ApprovalBlAwb(Data.Domain.EMCS.CiplApprove form)
        {
            var blAwb = Service.EMCS.SvcBlAwb.GetByIdcl(form.Id);
            ViewBag.WizardData = Service.EMCS.SvcWizard.GetWizardData("cargo", form.Id);
            Service.EMCS.SvcBlAwb.ApprovalBlAwb(blAwb, form, "U");
            return JsonMessage("This ticket has been " + form.Status, 0, blAwb);
        }

        [HttpPost]
        public ActionResult AddBlAwb(BlAwbModel blAwbModel)
        {
            blAwbModel.BlAwbList.Add(new BlAwbViewModel());
            return PartialView("_FormTemplateBlAwb", blAwbModel);
        }


        [HttpPost]
        public ActionResult AddBlAwbDocuments(BlAwbModel blAwbModel)
        {
            return PartialView("_FormTemplateBlAwbDocument", blAwbModel);
        }


        [HttpPost]
        public ActionResult GetDocumentBlAwb(long idRequest)
        {
            List<Data.Domain.EMCS.Documents> documents = Service.EMCS.SvcBlAwb.GetDocumentsBlAwb(idRequest);
            return Json(documents, JsonRequestBehavior.AllowGet);
        }

        public string UploadFileBlAwb(string dir)
        {
            string fileName = "";

            if (Request.Files.Count > 0)
            {

                var file = Request.Files[0];

                if (file != null && file.ContentLength > 0)
                {
                    fileName = Path.GetFileName(file.FileName);
                    var strFiles = fileName;
                    fileName = strFiles;

                    // Get Mime Type
                    var ext = Path.GetExtension(fileName);
                    var path = Server.MapPath("~/Upload/EMCS/BLAWB/");
                    bool isExists = Directory.Exists(path);

                    Regex reg = new Regex("[*/'\",_&#^@]");
                    fileName = reg.Replace(dir + ext, "");

                    if (!isExists)
                        Directory.CreateDirectory(path);

                    var fullPath = Path.Combine(path, reg.Replace(dir + ext, ""));

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

        [HttpPost]
        public ActionResult UploadDocumentBlAwb(string idCargo, string blawbid, string blAwbNo, string typeDoc,bool IsApprover = false)
        {
            bool status = false;
            string msg = "";
            Data.Domain.EMCS.RequestCl step = Service.EMCS.SvcNpePeb.GetRequestClById(idCargo);
            if (step.Id != 0)
            {
                string fileResult = UploadFileBlAwb("BLAWB" + idCargo + Guid.NewGuid() + blAwbNo + typeDoc);
                if (fileResult != "")
                {
                    if (IsApprover == true)
                    {
                        var olddata = Service.EMCS.SvcBlAwb.GetBlAwb_HistoryById(Convert.ToInt64(blawbid));
                        if (olddata != null)
                        {
                        Service.EMCS.SvcBlAwb.InsertDocumentBlAwb(step, blawbid, fileResult, typeDoc);
                            Service.EMCS.SvcBlAwb.UploadHistoryOfDocumentBlAwb(Convert.ToInt64(blawbid), fileResult);
                        }
                        else
                        {
                            var data = Service.EMCS.SvcBlAwb.GetBlAwbById(Convert.ToInt64(blawbid));
                            data.FileName = fileResult;
                            Service.EMCS.SvcBlAwb.SaveHistoryBlAwb(data, "Updated");
                            Service.EMCS.SvcBlAwb.InsertDocumentBlAwb(step, blawbid, fileResult, typeDoc);
                            Service.EMCS.SvcBlAwb.UploadHistoryOfDocumentBlAwb(Convert.ToInt64(blawbid), fileResult);

                        }

                    }
                    else
                    {
                        Service.EMCS.SvcBlAwb.InsertDocumentBlAwb(step, blawbid, fileResult, typeDoc);
                    }
                    
                    status = true;
                    msg = "Upload sucess";
                }
                else
                {
                    status = false;
                    msg = "Upload File gagal";
                }
            }

            return Json(new { status = status, msg = msg });
        }


        [HttpPost]
        public ActionResult DraftBlAwb(Data.Domain.EMCS.BlAwb form, string status)
        {
            long data = 0;

            if (status == "Draft")
                data = Service.EMCS.SvcBlAwb.InsertBlAwb(form, status);

            if (status == "Submit")
                Service.EMCS.SvcBlAwb.UpdateRequestCargolist(form.IdCl, status);

            return JsonMessage("This ticket has been " + status, 0, data);
        }

        [HttpPost]
        public ActionResult SaveHistoryBlAwb(Data.Domain.EMCS.BlAwb form, string status)
        {
            long data = 0;

            if (status == "Created")
            {
                data = Service.EMCS.SvcBlAwb.InsertBlAwb(form, "Draft");
                form.Id = data;
                data = Service.EMCS.SvcBlAwb.SaveHistoryBlAwb(form, status);

            }
            else if (status == "Updated")
            {
                var olddata = Service.EMCS.SvcBlAwb.GetBlAwbById(form.Id);
                if (olddata != null)
                {
                    data = Service.EMCS.SvcBlAwb.SaveHistoryBlAwb(olddata, status);
                    data = Service.EMCS.SvcBlAwb.InsertBlAwb(form, "Draft");
                    data = Service.EMCS.SvcBlAwb.SaveHistoryBlAwb(form, status);
                }
            }
            else
            {
                var olddata = Service.EMCS.SvcBlAwb.GetBlAwbById(form.Id);
                olddata.IsDelete = true;
                data = Service.EMCS.SvcBlAwb.SaveHistoryBlAwb(olddata, status);
                data = Service.EMCS.SvcBlAwb.Remove(form.Id);

            }
            return JsonMessage("This ticket has been " + status, 0, data);
        }
        [HttpPost]
        public ActionResult SaveAndApproveBlAwb(Data.Domain.EMCS.BlAwb form, string status, Data.Domain.EMCS.CiplApprove approvalForm)
        {
            long data = 0;
            var Changedata = Service.EMCS.SvcBlAwb.ListGetByIdclOnHistory(approvalForm.Id);
            //foreach (var item in Changedata)
            //{
            //    form.Number = item.Number;
            //    form.MasterBlDate= item.MasterBlDate;
            //    form.HouseBlNumber = item.HouseBlNumber;
            //    form.HouseBlDate = item.HouseBlDate;
            //    form.Description = item.Description;
            //    form.FileName = item.FileName;
            //    form.Publisher = item.Publisher;
            //    form.CreateBy = item.CreateBy;
            //    form.IsDelete = item.IsDelete;
            //    form.IdCl = item.IdCl;
            //    form.Id = item.IdBlAwb;
            //    if(item.Status == "Created")
            //    {
            //        Service.EMCS.SvcBlAwb.InsertBlAwb(form, "Draft");
            //    }
            //    else if(item.Status == "Updated")
            //    {
            //        Service.EMCS.SvcBlAwb.InsertBlAwb(form, "Draft");
            //    }
            //    else
            //    {
            //        Service.EMCS.SvcBlAwb.Remove(form.Id);
            //    }
            //}
            //Service.EMCS.SvcBlAwb.RemoveItemFromHistory(form.IdCl);
            Service.EMCS.SvcBlAwb.ApprovalBlAwb(null, approvalForm, "U");
            return JsonMessage("This ticket has been " + status, 0, data);
        }

        [HttpPost]
        public ActionResult RemoveBlAwb(long Id)
        {
            long data = Service.EMCS.SvcBlAwb.Remove(Id);
            return JsonMessage("This ticket has been " + "Draft", 0, data);
        }
        [HttpPost]
        public ActionResult RemoveBlAwbRFC(long Id)
        {
            long data = Service.EMCS.SvcBlAwb.RemoveRFC(Id);
            return JsonMessage("This ticket has been " + "Draft", 0, data);
        }


        public JsonResult GetBlAwbListByCargo(GridListFilter filter)
        {
            try
            {
                if (filter.Cargoid == 0)
                {
                    var a = HttpContext.Session["Cargoid"];
                    if (a == null)
                    {
                        a = 0;
                    }
                    filter.Cargoid = Convert.ToInt32(a);
                    HttpContext.Session.Remove("Cargoid");
                }
            }
            catch (Exception ex)
            {
                var a = ex.Message;
            }

            var data = Service.EMCS.SvcBlAwb.BlAwbListByCargo(filter);
            return Json(data, JsonRequestBehavior.AllowGet);
        }
        public JsonResult GetBlAwbListByCargoForSaveAndApprove(GridListFilter filter)
        {
            
            var data = Service.EMCS.SvcBlAwb.BlAwbListByCargoSaveApprove(filter);
            return Json(new { row = data }, JsonRequestBehavior.AllowGet);
        }
        public FileResult BlAWBDocument(long id)
        {
            try
            {
                var files = Service.EMCS.SvcBlAwb.GetBlAwbById2(id).FirstOrDefault();
                string fullPath = Request.MapPath("~/Upload/EMCS/Dummy/NotFound.txt");

                if (files != null)
                {
                    fullPath = Request.MapPath("~/Upload/EMCS/BLAWB/" + files.FileName);
                    var fileBytes = System.IO.File.ReadAllBytes(fullPath);
                    string fileName = files.FileName;
                    return File(fileBytes, System.Net.Mime.MediaTypeNames.Application.Octet, fileName);
                }

                return File(fullPath, "text/plain", "NotFound.txt");
            } 
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public FileResult BlAWBHistoryDocument(long id)
        {
            var files = Service.EMCS.SvcBlAwb.GetBlAwb_HistoryById(id);
            string fullPath = Request.MapPath("~/Upload/EMCS/Dummy/NotFound.txt");

            if (files != null)
            {
                fullPath = Request.MapPath("~/Upload/EMCS/BLAWB/" + files.FileName);
                var fileBytes = System.IO.File.ReadAllBytes(fullPath);
                string fileName = files.FileName;
                return File(fileBytes, System.Net.Mime.MediaTypeNames.Application.Octet, fileName);
            }

            return File(fullPath, "text/plain", "NotFound.txt");
        }
        public FileResult BlAWBRFCDocument(long id)
        {
            var files = Service.EMCS.SvcBlAwb.BlAwbRFCChangeListById(id);
            string fullPath = Request.MapPath("~/Upload/EMCS/Dummy/NotFound.txt");

            if (files != null)
            {
                fullPath = Request.MapPath("~/Upload/EMCS/BLAWB/" + files.FileName);
                var fileBytes = System.IO.File.ReadAllBytes(fullPath);
                string fileName = files.FileName;
                return File(fileBytes, System.Net.Mime.MediaTypeNames.Application.Octet, fileName);
            }

            return File(fullPath, "text/plain", "NotFound.txt");
        }
        [HttpPost]
        public ActionResult RequestForChangeBlAwb(Data.Domain.EMCS.BlAwb form, string reason)
        {
            Data.Domain.EMCS.ReturnSpInsert data = new Data.Domain.EMCS.ReturnSpInsert();
            if (form.Id > 0)
            {
                var model = new BlAwbModel();
                model.Data = Service.EMCS.SvcCargo.GetCargoById(form.IdCl);
                model.BlAwb = Service.EMCS.SvcBlAwb.GetByIdcl(form.IdCl);
                var requestForChange = new RequestForChange();

                requestForChange.FormNo = model.Data.ClNo;
                requestForChange.FormType = "BlAwb";
                requestForChange.Status = 0;
                requestForChange.FormId = Convert.ToInt32(form.IdCl);
                requestForChange.Reason = reason;
                int id = 0;
                id = Service.EMCS.SvcCipl.InsertRequestChangeHistory(requestForChange);
                string[] _ignnoreParameters = { "Id", "IdCl", "CreateDate" };

                var properties = TypeDescriptor.GetProperties(typeof(Data.Domain.EMCS.BlAwb));
                var listRfcItems = new List<Data.Domain.RFCItem>();
                foreach (PropertyDescriptor property in properties)
                {
                    if (!_ignnoreParameters.Contains(property.Name))
                    {
                        var currentValue = property.GetValue(form);
                        if (currentValue != null && property.GetValue(model.BlAwb) != null)
                        {
                            if (currentValue.ToString() != property.GetValue(model.BlAwb).ToString())
                            {
                                var rfcItem = new Data.Domain.RFCItem();

                                rfcItem.RFCID = id;
                                rfcItem.TableName = "BlAwb";
                                rfcItem.LableName = property.Name;
                                rfcItem.FieldName = property.Name;
                                rfcItem.BeforeValue = property.GetValue(model.BlAwb).ToString();
                                rfcItem.AfterValue = currentValue.ToString();
                                listRfcItems.Add(rfcItem);
                            }
                        }
                    }
                }
                Service.EMCS.SvcCipl.InsertRFCItem(listRfcItems);
            }
            return Json(new { data }, JsonRequestBehavior.AllowGet);
        }
        public JsonResult GetBlAwbRFCChangeList(long id)
        {
            var data = Service.EMCS.SvcBlAwb.BlAwbRFCChangeList(id);
            return Json(data, JsonRequestBehavior.AllowGet);
        }
        public JsonResult GetBlAwbRFCChangeListById(long id)
        {
            var data = Service.EMCS.SvcBlAwb.BlAwbRFCChangeListById(id);
            return Json(data, JsonRequestBehavior.AllowGet);
        }
    }


}