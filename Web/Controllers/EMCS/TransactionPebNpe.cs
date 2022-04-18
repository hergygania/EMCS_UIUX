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

namespace App.Web.Controllers.EMCS
{
    public partial class EmcsController
    {

<<<<<<< HEAD
<<<<<<< HEAD
=======
>>>>>>> d3e2e7a (Tasks from P1-CIPL , P1-CL , P!-SS , P!-SI , P1-BL/AWB & P1-PEB_NPE)
        [AuthorizeAcces(ActionType = AuthorizeAcces.IsRead)]
        public ActionResult NpePebList()
        {
            ApplicationTitle();
            ViewBag.AllowRead = AuthorizeAcces.AllowRead;
            ViewBag.AllowCreate = AuthorizeAcces.AllowCreated;
            ViewBag.AllowUpdate = AuthorizeAcces.AllowUpdated;
            ViewBag.AllowDelete = AuthorizeAcces.AllowDeleted;
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
<<<<<<< HEAD
=======
>>>>>>> 639d8d0 (Intial commit)
=======
>>>>>>> d3e2e7a (Tasks from P1-CIPL , P1-CL , P!-SS , P!-SI , P1-BL/AWB & P1-PEB_NPE)
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

        public ActionResult CreatePebNpe(Data.Domain.EMCS.GridListFilter filter)
        {
            ApplicationTitle();
            var data = new PebNpeModel();
<<<<<<< HEAD
            if (filter.Rfc)
                ViewBag.CanRequestForChange = true;
            else
                ViewBag.CanRequestForChange = false;
=======
>>>>>>> 639d8d0 (Intial commit)
            data.Data = Service.EMCS.SvcCargo.GetCargoById(filter.Id);
            data.NpePeb = Service.EMCS.SvcNpePeb.GetById(filter.Id);
            data.Request = Service.EMCS.SvcRequestCl.GetRequestCl(filter.Id);
            return View(data);
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
            var data = new PebNpeModel();
            data.Data = Service.EMCS.SvcCargo.GetCargoById(filter.Id);
            data.NpePeb = Service.EMCS.SvcNpePeb.GetById(filter.Id);
            data.Request = Service.EMCS.SvcRequestCl.GetRequestCl(filter.Id);
            ViewBag.WizardData = Service.EMCS.SvcWizard.GetWizardData("cargo", filter.Id);
            ViewBag.IdCipl = filter.IdCipl;
            return View(data);
        }

        [HttpPost]
        public ActionResult DraftNpePeb(Data.Domain.EMCS.NpePeb form, string status)
<<<<<<< HEAD
<<<<<<< HEAD
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
            model.Data = Service.EMCS.SvcCargo.GetCargoById(form.Id);
            model.NpePeb = Service.EMCS.SvcNpePeb.GetById(form.Id);

            var requestForChange = new RequestForChange();

            requestForChange.FormNo = model.NpePeb.NpeNumber;
            requestForChange.FormType = "NpePeb";
            requestForChange.Status = 1;
            requestForChange.FormId = Convert.ToInt32(model.NpePeb.Id);
            requestForChange.Reason = "";
            int id = 0;
            id = Service.EMCS.SvcCipl.InsertChangeHistory(requestForChange);

            string[] _ignnoreParameters = { "Id", "IdCl" };

            var properties = TypeDescriptor.GetProperties(typeof(Data.Domain.EMCS.NpePeb));
            var listRfcItems = new List<Data.Domain.RFCItem>();
            foreach (PropertyDescriptor property in properties)
            {
                if (!_ignnoreParameters.Contains(property.Name))
                {
                    var currentValue = property.GetValue(form);
                    if (currentValue != null && property.GetValue(model.NpePeb) != null)
                    {
                        if (currentValue.ToString() != property.GetValue(model).ToString())
                        {
                            var rfcItem = new Data.Domain.RFCItem();

                            rfcItem.RFCID = id;
                            rfcItem.TableName = "NpePeb";
                            rfcItem.LableName = property.Name;
                            rfcItem.FieldName = property.Name;
                            rfcItem.BeforeValue = property.GetValue(model).ToString();
                            rfcItem.AfterValue = currentValue.ToString();
                            listRfcItems.Add(rfcItem);
                        }
                    }
                }
            }
            Service.EMCS.SvcCipl.InsertRFCItem(listRfcItems);

            data = Service.EMCS.SvcNpePeb.UpdateNpePeb(form);
            Service.EMCS.SvcNpePeb.ApprovalNpePeb(form, approvalForm, "U");

=======
        {   
=======
        {
>>>>>>> d3e2e7a (Tasks from P1-CIPL , P1-CL , P!-SS , P!-SI , P1-BL/AWB & P1-PEB_NPE)
            //if (!ModelState.IsValid)
            //{
            //    return View(form);
            //} 
<<<<<<< HEAD

            Data.Domain.EMCS.ReturnSpInsert data = Service.EMCS.SvcNpePeb.InsertNpePeb(form, status);
>>>>>>> 639d8d0 (Intial commit)
            return JsonMessage("This ticket has been " + status, 0, data);
=======
            Data.Domain.EMCS.ReturnSpInsert data = new Data.Domain.EMCS.ReturnSpInsert();
            var userId = User.Identity.GetUserId();
            if (Service.EMCS.SvcNpePeb.NpePebHisOwned(form.Id, userId) || User.Identity.GetUserRoles().Contains("EMCSImex"))
            {
               data = Service.EMCS.SvcNpePeb.InsertNpePeb(form, status);
            }
                return JsonMessage("This ticket has been " + status, 0, data);
>>>>>>> d3e2e7a (Tasks from P1-CIPL , P1-CL , P!-SS , P!-SI , P1-BL/AWB & P1-PEB_NPE)
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


    }
}