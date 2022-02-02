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

namespace App.Web.Controllers.EMCS
{
    public partial class EmcsController
    {
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
            long data = Service.EMCS.SvcBlAwb.InsertBlAwb(form, status);
            return JsonMessage("This ticket has been " + status, 0, data);
        }

    }
}