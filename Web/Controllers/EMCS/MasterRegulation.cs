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
using App.Data.Domain.EMCS;
using App.Web.Models.EMCS;
using System.IO;

namespace App.Web.Controllers.EMCS
{
    public partial class EmcsController
    {
        #region Initilize
        private MasterRegulation InitilizeRegulation(long regulationId)
        {
            MasterRegulation regulation = new MasterRegulation();
            if (regulationId == 0)
            {
                return regulation;
            }

            regulation = Service.EMCS.MasterRegulation.GetDataById(regulationId);
            return regulation;
        }
        #endregion

        [AuthorizeAcces(ActionType = AuthorizeAcces.IsRead)]
        public ActionResult RegulationList()
        {
            ViewBag.AllowRead = AuthorizeAcces.AllowRead;
            ViewBag.AllowCreate = AuthorizeAcces.AllowCreated;
            ViewBag.AllowUpdate = AuthorizeAcces.AllowUpdated;
            ViewBag.AllowDelete = AuthorizeAcces.AllowDeleted;
            PaginatorBoot.Remove("SessionTRN");
            return View();
        }

        [AuthorizeAcces(ActionType = AuthorizeAcces.IsRead, UrlMenu = "RegulationList")]
        public ActionResult RegulationPage()
        {
            PaginatorBoot.Remove("SessionTRN");
            return RegulationPageXt();
        }

        [AuthorizeAcces(ActionType = AuthorizeAcces.IsRead, UrlMenu = "RegulationList")]
        public ActionResult RegulationPageXt()
        {
            Func<MasterSearchForm, IList<MasterRegulation>> func = delegate (MasterSearchForm crit)
            {
                List<MasterRegulation> list = Service.EMCS.MasterRegulation.GetList(crit);
                return list.OrderBy(o => o.UpdateDate).ToList();
            };

            ActionResult paging = PaginatorBoot.Manage("SessionTRN", func).Pagination.ToJsonResult();
            return Json(paging, JsonRequestBehavior.AllowGet);

        }

        [HttpGet]
        public ActionResult PreviewDocument(long id)
        {
            var regulationData = new RegulationModel();
            regulationData.Regulation = Service.EMCS.MasterRegulation.GetDataById(id);
            regulationData.StatusList = YesNoList();

            if (regulationData.Regulation == null)
            {
                return HttpNotFound();
            }
            return PartialView("PreviewRegulation", regulationData);
        }

        [AuthorizeAcces(ActionType = AuthorizeAcces.IsRead)]
        [HttpGet]
        public ActionResult RegulationCreate()
        {
            ViewBag.crudMode = "I";
            var regulationData = new RegulationModel();
            regulationData.CategoryList = Service.EMCS.MasterParameter.GetParamByGroup("RegulationParams");
            regulationData.Regulation = Service.EMCS.MasterRegulation.GetDataById(0);
            regulationData.StatusList = YesNoList();
            return PartialView("Modal.FormRegulation", regulationData);
        }

        [HttpPost]
        public ActionResult RegulationCreate(RegulationModel items)
        {
            ViewBag.crudMode = "I";
            Service.EMCS.MasterRegulation.Crud(items.Regulation, ViewBag.crudMode);
            return JsonCRUDMessage(ViewBag.crudMode);
        }

        [HttpGet]
        public ActionResult RegulationEdit(long id)
        {
            ViewBag.crudMode = "U";
            var regulationData = new RegulationModel();
            regulationData.Regulation = Service.EMCS.MasterRegulation.GetDataById(id);
            regulationData.CategoryList = Service.EMCS.MasterParameter.GetParamByIdList(id);
            regulationData.StatusList = YesNoList();
            if (regulationData.Regulation == null)
            {
                return HttpNotFound();
            }
            return PartialView("Modal.FormRegulation", regulationData);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult RegulationEdit(RegulationModel items)
        {
            ViewBag.crudMode = "U";
            if (ModelState.IsValid)
            {
                MasterRegulation regulation = Service.EMCS.MasterRegulation.GetDataById(items.Regulation.Id);
                regulation.Id = items.Regulation.Id;
                regulation.Instansi = items.Regulation.Instansi;
                regulation.Nomor = items.Regulation.Nomor;
                regulation.RegulationType = items.Regulation.RegulationType;
                regulation.Category = items.Regulation.Category;
                regulation.Reference = items.Regulation.Reference;
                regulation.Description = items.Regulation.Description;
                regulation.RegulationNo = items.Regulation.RegulationNo;
                regulation.TanggalPenetapan = items.Regulation.TanggalPenetapan;
                regulation.TanggalDiUndangkan = items.Regulation.TanggalDiUndangkan;
                regulation.TanggalBerlaku = items.Regulation.TanggalBerlaku;
                regulation.TanggalBerakhir = items.Regulation.TanggalBerakhir;
                regulation.Keterangan = items.Regulation.Keterangan;
                regulation.Files = items.Regulation.Files;

                Service.EMCS.MasterRegulation.Crud(regulation, ViewBag.crudMode);
                return JsonCRUDMessage(ViewBag.crudMode);
            }
            return Json(new { success = false });
        }

        [HttpGet]
        public ActionResult RegulationUpload(long id)
        {
            ViewBag.crudMode = "I";
            var regulationData = new RegulationModel();
            regulationData.Regulation = InitilizeRegulation(id);
            regulationData.StatusList = YesNoList();
            if (regulationData.Regulation == null)
            {
                return HttpNotFound();
            }
            return PartialView("Modal.FormUploadRegulation", regulationData);
        }

        public string UploadFileRegulation(string dir, long id)
        {
            string fileName = "";
            string strFiles = null;

            if (Request.Files.Count > 0)
            {

                var file = Request.Files[0];

                if (file != null && file.ContentLength > 0)
                {
                    fileName = Path.GetFileName(file.FileName);
                    if (fileName != null) strFiles = fileName;
                    fileName = strFiles;

                    // Get Mime Type
                    var ext = Path.GetExtension(fileName);
                    var path = Server.MapPath("~/Upload/EMCS/Regulation/");
                    bool isExists = Directory.Exists(path);
                    fileName = id.ToString() + ext;

                    if (!isExists)
                        Directory.CreateDirectory(path);

                    var fullPath = Path.Combine(path, id.ToString() + ext);

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
        public JsonResult RegulationUpload(MasterRegulation form)
        {
            ViewBag.crudMode = "U";
            string fileResult = UploadFileRegulation("Regulation", form.Id);
            if (fileResult != "")
            {
                var item = Service.EMCS.MasterRegulation.GetDataById(form.Id);
                item.Files = fileResult;
                Service.EMCS.MasterRegulation.Crud(item, ViewBag.crudMode);
            }
            else
            {
                return Json(new { status = false, msg = "Upload File gagal" });
            }
            return JsonCRUDMessage(ViewBag.crudMode);
        }

        [AuthorizeAcces(ActionType = AuthorizeAcces.IsRead, UrlMenu = "RegulationList")]
        [HttpPost]
        //[ValidateAntiForgeryToken]
        public ActionResult RegulationDelete(MasterRegulation items)
        {
            ViewBag.crudMode = "D";
            Service.EMCS.MasterRegulation.Crud(
                          items,
                          ViewBag.crudMode);
            return JsonCRUDMessage(ViewBag.crudMode);
        }

        [HttpPost]
        public ActionResult RegulationDeleteById(long regulationId)
        {
            MasterRegulation item = Service.EMCS.MasterRegulation.GetDataById(regulationId);
            Service.EMCS.MasterRegulation.Crud(
                item,
                "D");
            return JsonCRUDMessage("D");
        }

        public FileResult DownloadRegulation(long id)
        {
            var files = Service.EMCS.SvcRegulation.GetById(id);
            string fullPath = Request.MapPath("~/Upload/EMCS/Dummy/NotFound.txt");
            byte[] fileBytes;

            if (files != null)
            {
                var fileData = files;
                fullPath = Request.MapPath("~/Upload/EMCS/Regulation/" + files.Files);
                fileBytes = System.IO.File.ReadAllBytes(fullPath);
                string fileName = fileData.Files;
                return File(fileBytes, System.Net.Mime.MediaTypeNames.Application.Octet, fileName);
            }

            return File(fullPath, "text/plain", "NotFound.txt");
        }

    }
}