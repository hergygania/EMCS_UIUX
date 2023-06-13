using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using App.Data.Caching;
using App.Data.Domain;
using App.Web.App_Start;
using App.Web.Models.EMCS;
using System.IO;
using System.Text.RegularExpressions;
using System;
using System.ComponentModel;
using App.Domain;

namespace App.Web.Controllers.EMCS
{
    public partial class EmcsController
    {

        [AuthorizeAcces(ActionType = AuthorizeAcces.IsRead)]
        public ActionResult NpePebList()
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
        [AuthorizeAcces(ActionType = AuthorizeAcces.IsRead, UrlMenu = "NpePebList")]
        public JsonResult GetNpePebList(GridListFilterModel filter)
        {
            var dataFilter = new Data.Domain.EMCS.GridListFilter();
            dataFilter.Limit = filter.Limit;
            dataFilter.Order = filter.Order;
            dataFilter.Term = filter.Term;
            dataFilter.Sort = filter.Sort;
            dataFilter.Offset = filter.Offset;

            var data = Service.EMCS.SvcNpePeb.NpePebList(dataFilter);
            return Json(data, JsonRequestBehavior.AllowGet);
        }
        public ActionResult NpePebApprove()
        {
            ApplicationTitle();
            ViewBag.AllowRead = AuthorizeAcces.AllowRead;
            ViewBag.AllowCreate = AuthorizeAcces.AllowCreated;
            ViewBag.AllowUpdate = AuthorizeAcces.AllowUpdated;
            ViewBag.AllowDelete = AuthorizeAcces.AllowDeleted;
            PaginatorBoot.Remove("SessionTRN");
            return View();
        }
        public ActionResult NpeView()
        {
            ApplicationTitle();
            ViewBag.AllowRead = AuthorizeAcces.AllowRead;
            ViewBag.AllowCreate = AuthorizeAcces.AllowCreated;
            ViewBag.AllowUpdate = AuthorizeAcces.AllowUpdated;
            ViewBag.AllowDelete = AuthorizeAcces.AllowDeleted;
            PaginatorBoot.Remove("SessionTRN");
            return View();
        }
        public ActionResult CreatePebNpe(Data.Domain.EMCS.GridListFilter filter,string Type)
        {
            ApplicationTitle();
            ViewBag.IsApprover = false;
            var data = new PebNpeModel();
            if (filter.Rfc)
                ViewBag.CanRequestForChange = true;
            else
                ViewBag.CanRequestForChange = false;

            if (Request.UrlReferrer != null)
            {
                var url = Request.UrlReferrer.ToString().ToLower();
                if (url.Contains("emcs/npepeblist"))
                {
                    ViewBag.IsApprover = false;
                }
                else if (!Service.EMCS.SvcNpePeb.NpePebHisOwned(filter.Id, SiteConfiguration.UserName))
                {
                    ViewBag.Type = Type;
                    if (ViewBag.Type != "New" && ViewBag.Type != "Revise")
                    {
                        ViewBag.IsApprover = true;
                    }
                    
                }


            }
            data.Data = Service.EMCS.SvcCargo.GetCargoById(filter.Id);
            data.NpePeb = Service.EMCS.SvcNpePeb.GetById(filter.Id);
            data.Request = Service.EMCS.SvcRequestCl.GetRequestCl(filter.Id);
            return View(data);
        }
        public ActionResult CreateNewPebNpe(Data.Domain.EMCS.GridListFilter filter)
        {
            ApplicationTitle();
            ViewBag.CanRequestForChange = false;
            var data = new PebNpeModel();
            data.Data = Service.EMCS.SvcCargo.GetCargoById(filter.Id);
            data.NpePeb = null;
            data.Request = Service.EMCS.SvcRequestCl.GetRequestCl(filter.Id);
            return View("CreatePebNpe",data);
        }
        public ActionResult RevisePebNpe(Data.Domain.EMCS.GridListFilter filter)
        {
            ApplicationTitle();
            var data = new PebNpeModel();
            data.Data = Service.EMCS.SvcCargo.GetCargoById(filter.Id);
            data.NpePeb = Service.EMCS.SvcNpePeb.GetById(filter.Id);
            data.Request = Service.EMCS.SvcRequestCl.GetRequestCl(filter.Id);
            return View(data);
        }
        public ActionResult PebNpeApproval(Data.Domain.EMCS.GridListFilter filter)
        {
            ApplicationTitle();
            var data = new PebNpeModel();
            data.Data = Service.EMCS.SvcCargo.GetCargoById(filter.Id);
            data.NpePeb = Service.EMCS.SvcNpePeb.GetById(filter.Id);
            data.Request = Service.EMCS.SvcRequestCl.GetRequestCl(filter.Id);
            return View(data);
        }
        public ActionResult PebNpeCancellation(Data.Domain.EMCS.GridListFilter filter)
        {
            ApplicationTitle();
            var data = new PebNpeModel();
            data.Data = Service.EMCS.SvcCargo.GetCargoById(filter.Id);
            data.NpePeb = Service.EMCS.SvcNpePeb.GetById(filter.Id);
            data.Document = Service.EMCS.SvcNpePeb.GetDocumentsPebNpe(filter.Id);
            data.Request = Service.EMCS.SvcRequestCl.GetRequestCl(filter.Id);
            return View(data);
        }
        public ActionResult ViewPebNpe(Data.Domain.EMCS.GridListFilter filter)
        {
            ApplicationTitle();
            var url = Request.UrlReferrer.LocalPath.ToString();
            var userrole = Service.EMCS.SvcNpePeb.GetListUserRoles(SiteConfiguration.UserName);
            for (int i = 0; i < userrole.Count; i++)
            {
                if (userrole[i].RoleID == 8)
                {
                    ViewBag.RoleId = 8;
                }
                else
                {
                    ViewBag.RoleId = userrole[i].RoleID;
                }

            }
            var data = new PebNpeModel();
            data.Data = Service.EMCS.SvcCargo.GetCargoById(filter.Id);
            if (data.Data != null)
            {
                if (data.Data.Id != 0)
                {
                    data.NpePeb = Service.EMCS.SvcNpePeb.GetByIdNpePeb(filter.IdNpePeb);
                    if (data.NpePeb != null)
                    {
                        //var roleid = 
                        var RequestCL = Service.EMCS.SvcNpePeb.GetStatusById(data.NpePeb.IdCl);
                        ViewBag.Status = RequestCL.Status;
                        if (url.ToLower() == "/emcs/npepeblist")
                        {
                            ViewBag.CountDays = 3;
                        }
                        else
                        {
                            if (data.NpePeb.NpeDateSubmitToCustomOffice != null)
                            {
                                if (data.NpePeb.NpeDateSubmitToCustomOffice.Value.Year == DateTime.Now.Year && data.NpePeb.NpeDateSubmitToCustomOffice.Value.Month == DateTime.Now.Month)
                                {
                                    if (data.NpePeb.NpeDateSubmitToCustomOffice != null)
                                    {
                                        ViewBag.CountDays = DateTime.Now.Day - data.NpePeb.NpeDateSubmitToCustomOffice.Value.Day;

                                    }
                                }
                            }

                        }

                    }


                }

            }
            data.Request = Service.EMCS.SvcRequestCl.GetRequestCl(filter.Id);
            ViewBag.WizardData = Service.EMCS.SvcWizard.GetWizardData("cargo", filter.Id);
            ViewBag.IdCipl = filter.IdCipl;
            return View(data);
        }
        [HttpPost]
        public ActionResult DraftNpePeb(Data.Domain.EMCS.NpePeb form, string status)
        {
            Data.Domain.EMCS.ReturnSpInsert data = new Data.Domain.EMCS.ReturnSpInsert();
            if (form.Id > 0)
            {
                var model = new PebNpeModel();
                model.Data = Service.EMCS.SvcCargo.GetCargoById(form.IdCl);
                model.NpePeb = Service.EMCS.SvcNpePeb.GetById(form.IdCl);

                var requestForChange = new RequestForChange();

                requestForChange.FormNo = model.Data.ClNo;
                requestForChange.FormType = "NpePeb";
                requestForChange.Status = 1;
                requestForChange.FormId = Convert.ToInt32(form.IdCl);
                requestForChange.Reason = "";
                int id = 0;
                id = Service.EMCS.SvcCipl.InsertChangeHistory(requestForChange);
                string[] _ignnoreParameters = { "Id" };

                var properties = TypeDescriptor.GetProperties(typeof(Data.Domain.EMCS.NpePeb));
                var listRfcItems = new List<Data.Domain.RFCItem>();
                foreach (PropertyDescriptor property in properties)
                {
                    if (!_ignnoreParameters.Contains(property.Name))
                    {
                        var currentValue = property.GetValue(form);
                        if (currentValue != null && property.GetValue(model.NpePeb) != null)
                        {
                            if (currentValue.ToString() != property.GetValue(model.NpePeb).ToString())
                            {
                                var rfcItem = new Data.Domain.RFCItem();

                                rfcItem.RFCID = id;
                                rfcItem.TableName = "NpePeb";
                                rfcItem.LableName = property.Name;
                                rfcItem.FieldName = property.Name;
                                rfcItem.BeforeValue = property.GetValue(model.NpePeb).ToString();
                                rfcItem.AfterValue = currentValue.ToString();
                                listRfcItems.Add(rfcItem);
                            }
                        }
                    }
                }
                Service.EMCS.SvcCipl.InsertRFCItem(listRfcItems);
            }
            data = Service.EMCS.SvcNpePeb.InsertNpePeb(form, status);
            
            return JsonMessage("This ticket has been " + status, 0, data);
        }
        [HttpPost]
        public ActionResult RequestForChangeNpePeb(Data.Domain.EMCS.NpePeb form, string reason)
        {
            Data.Domain.EMCS.ReturnSpInsert data = new Data.Domain.EMCS.ReturnSpInsert();
            if (form.Id > 0)
            {
                var model = new PebNpeModel();
                model.Data = Service.EMCS.SvcCargo.GetCargoById(form.IdCl);
                model.NpePeb = Service.EMCS.SvcNpePeb.GetById(form.IdCl);

                var requestForChange = new RequestForChange();

                requestForChange.FormNo = model.Data.ClNo;
                requestForChange.FormType = "NpePeb";
                requestForChange.Status = 0;
                requestForChange.FormId = Convert.ToInt32(form.IdCl);
                requestForChange.Reason = reason;
                int id = 0;
                id = Service.EMCS.SvcCipl.InsertRequestChangeHistory(requestForChange);
                string[] _ignnoreParameters = { "Id", "IdCl", "CreateDate" };

                var properties = TypeDescriptor.GetProperties(typeof(Data.Domain.EMCS.NpePeb));
                var listRfcItems = new List<Data.Domain.RFCItem>();
                foreach (PropertyDescriptor property in properties)
                {
                    if (!_ignnoreParameters.Contains(property.Name))
                    {
                        var currentValue = property.GetValue(form);
                        if (currentValue != null && property.GetValue(model.NpePeb) != null)
                        {
                            if (currentValue.ToString() != property.GetValue(model.NpePeb).ToString())
                            {
                                var rfcItem = new Data.Domain.RFCItem();

                                rfcItem.RFCID = id;
                                rfcItem.TableName = "NpePeb";
                                rfcItem.LableName = property.Name;
                                rfcItem.FieldName = property.Name;
                                rfcItem.BeforeValue = property.GetValue(model.NpePeb).ToString();
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
        [HttpPost]
        public ActionResult SaveAndApproveNpePeb(Data.Domain.EMCS.NpePeb form, string status, Data.Domain.EMCS.CiplApprove approvalForm)
        {
            Data.Domain.EMCS.ReturnSpInsert data = new Data.Domain.EMCS.ReturnSpInsert();
            
            var model = new PebNpeModel();
            model.Data = Service.EMCS.SvcCargo.GetCargoById(form.IdCl);
            model.NpePeb = Service.EMCS.SvcNpePeb.GetById(form.IdCl);

            var requestForChange = new RequestForChange();

            requestForChange.FormNo = model.NpePeb.NpeNumber;
            requestForChange.FormType = "NpePeb";
            requestForChange.Status = 1;
            requestForChange.FormId = Convert.ToInt32(model.NpePeb.Id);
            requestForChange.Reason = "";
            int id = 0;
            id = Service.EMCS.SvcCipl.InsertChangeHistory(requestForChange);

            string[] _ignnoreParameters = { "Id", "IdCl","CreateBy", "CreateDate", "UpdateBy","UpdateDate","IsDelete", "IsCancelled", "CancelledDocument", "DraftPeb", "Npwp" , "ReceiverName"};

            var properties = TypeDescriptor.GetProperties(typeof(Data.Domain.EMCS.NpePeb));
            var listRfcItems = new List<Data.Domain.RFCItem>();
            foreach (PropertyDescriptor property in properties)
            {
                if (!_ignnoreParameters.Contains(property.Name))
                {
                    var currentValue = property.GetValue(form);
                    if (currentValue != null && property.GetValue(model.NpePeb) != null)
                    {
                        if (currentValue.ToString() != property.GetValue(model.NpePeb).ToString())
                        {
                            var rfcItem = new Data.Domain.RFCItem();

                            rfcItem.RFCID = id;
                            rfcItem.TableName = "NpePeb";
                            rfcItem.LableName = property.Name;
                            rfcItem.FieldName = property.Name;
                            rfcItem.BeforeValue = property.GetValue(model.NpePeb).ToString();
                            rfcItem.AfterValue = currentValue.ToString();
                            listRfcItems.Add(rfcItem);
                        }
                    }
                }
            }
            Service.EMCS.SvcCipl.InsertRFCItem(listRfcItems);

            data = Service.EMCS.SvcNpePeb.UpdateNpePeb(form);
            Service.EMCS.SvcNpePeb.ApprovalNpePeb(form, approvalForm, "U");

            return JsonMessage("This ticket has been " + status, 0, data);
        }
        [HttpPost]
        public ActionResult GetDocumentPebNpe(long idRequest)
        {
            List<Data.Domain.EMCS.Documents> documents = Service.EMCS.SvcNpePeb.GetDocumentsPebNpe(idRequest);
            return Json(documents, JsonRequestBehavior.AllowGet);

        }
        public string UploadFile(string dir)
        {
            string fileName = "";
            if (Request.Files.Count > 0)
            {

                var file = Request.Files[0];

                if (file != null && file.ContentLength > 0)
                {
                    fileName = Path.GetFileName(file.FileName);
                    if (fileName != null)
                    {
                        var strFiles = fileName;
                        fileName = strFiles;
                    }

                    // Get Mime Type
                    var ext = Path.GetExtension(fileName);
                    var path = Server.MapPath("~/Upload/EMCS/NPEPEB/");
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
        public ActionResult UploadDocumentNpePeb(string idCargo, string ajuNumber, string typeDoc)
        {
            ViewBag.crudMode = "I";
            Data.Domain.EMCS.RequestCl step = Service.EMCS.SvcNpePeb.GetRequestClById(idCargo);
            if (step.Id != 0)
            {
                string fileResult = UploadFile("NPEPEB" + idCargo + "" + step.IdStep + "" + ajuNumber + typeDoc);
                if (fileResult != "")
                {
                    Service.EMCS.SvcNpePeb.InsertDocument(step, fileResult, typeDoc);
                }
                else
                {
                    return Json(new { status = false, msg = "Upload File gagal" });
                }
            }

            return JsonCRUDMessage(ViewBag.crudMode);
        }
        [HttpPost]
        public JsonResult NpePebApproval(Data.Domain.EMCS.CiplApprove form)
        {
            var npePeb = Service.EMCS.SvcNpePeb.GetById(form.Id);
            Service.EMCS.SvcNpePeb.ApprovalNpePeb(npePeb, form, "U");
            return JsonMessage("This ticket has been " + form.Status, 0, npePeb);
        }
        [HttpPost]
        public JsonResult NpePebCancelAlldocument(Data.Domain.EMCS.CiplApprove form)
        {
            try
            {
                var npePeb = Service.EMCS.SvcNpePeb.GetById(form.Id);
                Service.EMCS.SvcNpePeb.CancelAllNpePeb(npePeb, form, "U");
                return JsonMessage("This ticket has been " + form.Status, 0, npePeb);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        [HttpPost]
        public JsonResult NpePebCancel(Data.Domain.EMCS.CiplApprove form)
        {
            try
            {
                var npePeb = Service.EMCS.SvcNpePeb.GetById(form.Id);
                Service.EMCS.SvcNpePeb.CancelNpePeb(npePeb, form, "U");
                return JsonMessage("This ticket has been " + form.Status, 0, npePeb);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public JsonResult GetLastestCurrency()
        {
            var data = Service.EMCS.SvcCipl.GetCurrencyCipl();
            return Json(new { data }, JsonRequestBehavior.AllowGet);
        }
        public ActionResult GetLastestKurs()
        {
            var kurs = Service.EMCS.SvcCipl.GetLastestKurs();
            return Json(kurs, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult GetDataById(long Id)
        {
            var NpePeb = Service.EMCS.SvcNpePeb.GetById(Id);
            return Json(NpePeb);
        }
        [HttpPost]
        public JsonResult GetDataByIdForList(long Id)
        {
            var NpePeb = Service.EMCS.SvcNpePeb.GetDataByIdForList(Id);
            return Json(NpePeb);
        }
        //Document Process
        #region
        public string GetdocCancelFileName(System.Web.HttpPostedFileBase file, long id)
        {
            var fileName = Path.GetFileName(file.FileName);

            if (fileName != null)
            {
                var ext = Path.GetExtension(fileName);
                var path = Server.MapPath("~/Upload/EMCS/NPEPEB/CancelDocument");
                bool isExists = Directory.Exists(path);
                fileName = "DocCancelNpePeb-" + id.ToString() + ext;
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
        public string UploadDocumentNpePebDoc(long id)
        {
            string fileName = "";

            if (Request.Files.Count > 0)
            {
                var file = Request.Files[0];

                if (file != null && file.ContentLength > 0)
                {
                    return GetdocCancelFileName(file, id);
                }
            }
            return fileName;
        }
        [HttpPost]
        public ActionResult CancelDocumentUploadForNpePeb(long id)
        {
            var fileName = UploadDocumentNpePebDoc(id);
            if (fileName != null && fileName != "")
            {
                Service.EMCS.SvcNpePeb.InsertCancelledDocument(id, fileName);
            }
            return Json(new { status = true, msg = "Upload File Successfully" });

        }
        public FileResult DownloadCancelDocument(long id)
        {
            try
            {
                var files = Service.EMCS.SvcNpePeb.GetById(id);
                string fullPath = Request.MapPath("~/Upload/EMCS/Dummy/NotFound.txt");

                if (files != null)
                {
                    fullPath = Request.MapPath("~/Upload/EMCS/NPEPEB/CancelDocument/" + files.CancelledDocument);
                    var fileBytes = System.IO.File.ReadAllBytes(fullPath);
                    string fileName = files.CancelledDocument;
                    return File(fileBytes, System.Net.Mime.MediaTypeNames.Application.Octet, fileName);
                }

                return File(fullPath, "text/plain", "NotFound.txt");
            }
            catch (Exception ex)
            {

                throw ex;
            }

        }
        #endregion
    }
}