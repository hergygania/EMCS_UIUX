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
using App.Data.Domain.EMCS;
using System.IO;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;

namespace App.Web.Controllers.EMCS
{
    public partial class EmcsController
    {
        // GET: TransactionGoodsReceive
        #region Initialize Data
        public GoodReceiveItemModel InitGoodReceiveItem(long id)
        {
            var item = Service.EMCS.SvcGoodsReceiveItem.GetById(id);
            GoodReceiveItemModel data = new GoodReceiveItemModel();
            if (item != null)
            {
                data.Id = item.Id;
                data.IdGr = item.IdGr;
                data.DaNo = item.DaNo;
                data.IdCipl = item.IdCipl;
                data.DoNo = item.DoNo;
                data.FileName = item.FileName;
                data.CreateDate = item.CreateDate;
                data.CreateBy = item.CreateBy;
                data.UpdateDate = item.UpdateDate;
                data.UpdateBy = item.UpdateBy;
                data.IsDelete = item.IsDelete;
            }
            return data;
        }

        public JsonResult GetDataEdoList(Cipl filter)
        {
            var idGr = Service.EMCS.SvcGoodsReceiveItem.GetGrIdByIdCipl(filter.Id);

            var detailGr = InitGoodReceive(idGr);
            var area = detailGr.Data.PickupPoint;
            var pic = detailGr.Data.PickupPic;

            var data = Service.EMCS.SvcCargo.GetEdoNoList(area, pic);
            return Json(new { data }, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region Insert, Update Data, Delete Data
        [HttpGet]
        public ActionResult CreateGrItem(GoodReceiveItemModel form)
        {
            ViewBag.crudMode = "I";
            var data = InitGoodReceiveItem(0);

            var detailGr = InitGoodReceive(form.IdGr);
            var area = detailGr.Data.PickupPoint;
            var pic = detailGr.Data.PickupPic;
            data.IdGr = form.IdGr;

            data.DoList = Service.EMCS.SvcCargo.GetEdoNoList(area, pic);
            return PartialView("Modal.FormItemGR", data);
        }

        [HttpGet]
        public ActionResult PreviewGrItem(GoodReceiveItemModel form)
        {
            ViewBag.crudMode = "I";
            var id = form.Id;
            var data = InitGoodReceiveItem(id);
            return PartialView("Modal.FormPreviewUpload", data);
        }

        [HttpPost, ValidateInput(false)]
        public JsonResult SaveGrItem(GoodReceiveItemModel form)
        {
            try
            {
                var ResultDaNo = Service.Master.EmailRecipients.ValidateInputHtmlInjection(form.DaNo, "`^<>");
                if (!ResultDaNo)
                {
                    return JsonMessage("Please Enter a Valid DO Reference", 1, "i");
                }

                long id = 0;
                string state = form.Id == 0 ? "I" : "U";

                if (ModelState.IsValid)
                {
                    var data = InitGoodReceiveItem(id);
                    var item = new GoodsReceiveItem();
                    if (state == "U")
                        item = Service.EMCS.SvcGoodsReceiveItem.GetById(form.Id);

                    var itemCipl = Service.EMCS.SvcCipl.CiplGetById(form.IdCipl);

                    item.IdGr = form.IdGr;
                    item.IdCipl = form.IdCipl;
                    item.DoNo = itemCipl.EdoNo;
                    item.DaNo = form.DaNo;

                    Service.EMCS.SvcGoodsReceiveItem.CrudSp(item, form.Status);
                    return JsonCRUDMessage(state, data);
                }
                return Json(new { success = false });
            }
            catch (Exception)
            {
                return Json(new { success = false });
            }
        }

        [HttpPost]
        public JsonResult RemoveGrItem(GoodReceiveItemModel form)
        {
            try
            {
                var item = Service.EMCS.SvcGoodsReceiveItem.GetById(form.Id);
                Service.EMCS.SvcGoodsReceiveItem.Crud(item, "D");
                return JsonCRUDMessage("D", item);
            }
            catch (Exception)
            {
                return Json(new { success = false });
            }
        }

        #endregion

        #region Upload Item
        [HttpGet]
        public ActionResult UploadGrItem(long id)
        {
            ViewBag.crudMode = "U";
            var data = InitGoodReceiveItem(id);
            return PartialView("Modal.FormUploadDa", data);
        }

        public FileResult DownloadGrItem(long id)
        {
            var files = Service.EMCS.SvcGoodsReceiveItem.GetById(id);
            string fullPath = Request.MapPath("~/Upload/EMCS/Dummy/NotFound.txt");

            if (files != null)
            {
                var fileData = files;
                fullPath = Request.MapPath("~/Upload/EMCS/GR/" + files.IdGr + "/" + fileData.FileName);
                var fileBytes = System.IO.File.ReadAllBytes(fullPath);
                string fileName = fileData.FileName;
                return File(fileBytes, System.Net.Mime.MediaTypeNames.Application.Octet, fileName);
            }

            return File(fullPath, "text/plain", "NotFound.txt");
        }

        [HttpPost]
        public JsonResult UploadGrItem(GoodReceiveItemModel form)
        {
            ViewBag.crudMode = "U";
            var data = InitGoodReceiveItem(form.Id);
            string fileResult = UploadGrFile(data);
            if (fileResult != "")
            {
                var item = Service.EMCS.SvcGoodsReceiveItem.GetById(form.Id);
                item.FileName = fileResult;
                Service.EMCS.SvcGoodsReceiveItem.CrudSp(item, ViewBag.crudMode);
            }
            else
            {
                return Json(new { status = false, msg = "Failed to Upload File" });
            }
            return JsonCRUDMessage(ViewBag.crudMode);
        }

        public string UploadGrFile(GoodReceiveItemModel data)
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
                    var path = Server.MapPath("~/Upload/EMCS/GR/" + data.IdGr);
                    bool isExists = Directory.Exists(path);
                    var newFileName = data.DoNo + data.DaNo;
                    fileName = newFileName + ext;

                    if (!isExists)
                        Directory.CreateDirectory(path);

                    var fullPath = Path.Combine(path, fileName);

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

        public ActionResult UpdateGrItem(GoodReceiveItemModel form)
        {
            ViewBag.crudMode = "U";
            var data = InitGoodReceiveItem(form.Id);

            var detailGr = InitGoodReceive(form.IdGr);
            var area = detailGr.Data.PickupPoint;
            data.DoList = Service.EMCS.SvcGoodsReceiveItem.GetEdoNoGrItemList(area, form.IdGr);
            return PartialView("Modal.FormItemGR", data);
        }
        #endregion

        #region Get List Item
        public JsonResult GetItemGr(GridListFilter crit)
        {
            var data = Service.EMCS.SvcGoodsReceiveItem.GetList(crit);
            return Json(new { data }, JsonRequestBehavior.AllowGet);
        }
        #endregion
    }
}