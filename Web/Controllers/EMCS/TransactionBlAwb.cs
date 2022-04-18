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

namespace App.Web.Controllers.EMCS
{
    public partial class EmcsController
    {
<<<<<<< HEAD
<<<<<<< HEAD
=======
>>>>>>> d3e2e7a (Tasks from P1-CIPL , P1-CL , P!-SS , P!-SI , P1-BL/AWB & P1-PEB_NPE)

        [AuthorizeAcces(ActionType = AuthorizeAcces.IsRead)]
        public ActionResult BLAWBList()
        {
            ApplicationTitle();
            ViewBag.AllowRead = AuthorizeAcces.AllowRead;
            ViewBag.AllowCreate = AuthorizeAcces.AllowCreated;
            ViewBag.AllowUpdate = AuthorizeAcces.AllowUpdated;
            ViewBag.AllowDelete = AuthorizeAcces.AllowDeleted;
<<<<<<< HEAD

=======
>>>>>>> d3e2e7a (Tasks from P1-CIPL , P1-CL , P!-SS , P!-SI , P1-BL/AWB & P1-PEB_NPE)
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
<<<<<<< HEAD
=======
>>>>>>> 639d8d0 (Intial commit)
=======
>>>>>>> d3e2e7a (Tasks from P1-CIPL , P1-CL , P!-SS , P!-SI , P1-BL/AWB & P1-PEB_NPE)
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

        public ActionResult CreateBlAwb(Data.Domain.EMCS.GridListFilter filter)
        {
            ApplicationTitle();
            var data = new BlAwbModel();
<<<<<<< HEAD
            if (filter.Rfc)
                ViewBag.CanRequestForChange = true;
            else
                ViewBag.CanRequestForChange = false;
=======
>>>>>>> 639d8d0 (Intial commit)
            data.Data = Service.EMCS.SvcCargo.GetCargoById(filter.Id);
            data.BlAwb = Service.EMCS.SvcBlAwb.GetByIdcl(filter.Id);
            data.Request = Service.EMCS.SvcRequestCl.GetRequestCl(filter.Id);
            return View(data);
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
        public ActionResult UploadDocumentBlAwb(string idCargo, string blAwbNo, string typeDoc)
        {
            ViewBag.crudMode = "I";
            Data.Domain.EMCS.RequestCl step = Service.EMCS.SvcNpePeb.GetRequestClById(idCargo);
            if (step.Id != 0)
            {
                string fileResult = UploadFileBlAwb("BLAWB" + idCargo + blAwbNo + typeDoc);
                if (fileResult != "")
                {
                    Service.EMCS.SvcBlAwb.InsertDocumentBlAwb(step, fileResult, typeDoc);
                }
                else
                {
                    return Json(new { status = false, msg = "Upload File gagal" });
                }
            }

            return JsonCRUDMessage(ViewBag.crudMode);
        }

        [HttpPost]
        public ActionResult DraftBlAwb(Data.Domain.EMCS.BlAwb form, string status)
        {
<<<<<<< HEAD
<<<<<<< HEAD
            if (form.Id > 0 && form.CreateBy != SiteConfiguration.UserName)
            {
                var model = new BlAwbModel();
                model.Data = Service.EMCS.SvcCargo.GetCargoById(form.IdCl);
                model.BlAwb = Service.EMCS.SvcBlAwb.GetByIdcl(form.IdCl);
                var requestForChange = new RequestForChange();

                requestForChange.FormNo = model.Data.ClNo;
                requestForChange.FormType = "BlAwb";
                requestForChange.Status = 0;
                requestForChange.FormId = Convert.ToInt32(form.IdCl);
                requestForChange.Reason = "";
                int id = 0;
                id = Service.EMCS.SvcCipl.InsertChangeHistory(requestForChange);
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
            long data = Service.EMCS.SvcBlAwb.InsertBlAwb(form, status);
            return JsonMessage("This ticket has been " + status, 0, data);

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
=======
            long data = Service.EMCS.SvcBlAwb.InsertBlAwb(form, status);
            return JsonMessage("This ticket has been " + status, 0, data);
>>>>>>> 639d8d0 (Intial commit)
=======
            var userId = User.Identity.GetUserId();
            if (Service.EMCS.SvcBlAwb.BlAwbHisOwned(form.Id, userId) || User.Identity.GetUserRoles().Contains("EMCSImex"))
            {
                long data = Service.EMCS.SvcBlAwb.InsertBlAwb(form, status);
                return JsonMessage("This ticket has been " + status, 0, data);
            }
            else
            {
                return JsonMessage("This ticket has been " + status, 0,null);
            }
            

>>>>>>> d3e2e7a (Tasks from P1-CIPL , P1-CL , P!-SS , P!-SI , P1-BL/AWB & P1-PEB_NPE)
        }

    }
}