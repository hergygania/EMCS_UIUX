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
using App.Data.Domain.EMCS;
using App.Web.Models.EMCS;
using System.IO;
using System.Text.RegularExpressions;

namespace App.Web.Controllers.EMCS
{
    public partial class EmcsController
    {

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
        {   
            //if (!ModelState.IsValid)
            //{
            //    return View(form);
            //} 

            Data.Domain.EMCS.ReturnSpInsert data = Service.EMCS.SvcNpePeb.InsertNpePeb(form, status);
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