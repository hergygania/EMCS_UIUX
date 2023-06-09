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
using System.Text.RegularExpressions;
using System.Data.SqlClient;

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
            //var idGr = Service.EMCS.SvcGoodsReceiveItem.GetGrIdByIdCipl(filter.Id);
            try
            {
                var detailGr = InitGoodReceive(filter.Id);
                var area = detailGr.Data.PickupPoint;
                var pic = detailGr.Data.PickupPic;

                var data = Service.EMCS.SvcCargo.GetEdoNoList(area, pic, detailGr.Data.Id);
                return Json(new { data }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {

                throw ex;
            }

        }
        public JsonResult GetDataDonoListForRFC(Cipl model)
        {
            try
            {
                var data = Service.EMCS.SvcCargo.GetEdoNoList(model.PickUpArea, model.PickUpPic, model.Id);
                return Json(new { data }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {

                throw ex;
            }

        }
        public JsonResult GetDoList(string picuppoint, string picuppic, long Id)
        {
            List<Cipl> data = new List<Cipl>();
            data = Service.EMCS.SvcCargo.GetEdoNoList(picuppoint, picuppic, Id);
            return Json(data, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region Insert, Update Data, Delete Data
        [HttpGet]
        public ActionResult CreateGrItem(GoodReceiveItemModel form)
        {

            try
            {
                ViewBag.crudMode = "I";
                var data = InitGoodReceiveItem(0);

                var detailGr = InitGoodReceive(form.IdGr);
                var area = detailGr.Data.PickupPoint;
                var pic = detailGr.Data.PickupPic;
                data.IdGr = form.IdGr;

                data.DoList = Service.EMCS.SvcCargo.GetEdoNoList(area, pic, data.IdGr);
                GoodReceiveModel getdata = new GoodReceiveModel();
                getdata.ItemModel = data;
                return PartialView("Model.FormItemGR", data);
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }
        public JsonResult GetListArmada(long IdGr, long Id)
        {
            try
            {
                var data = App.Service.EMCS.SvcGoodsReceiveItem.GetListArmada(Id, IdGr);
                return Json(data, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {

                throw ex;
            }

        }

        [HttpPost]
        public ActionResult GetDataForEdiDropDown(GoodReceiveModel objGrModel, string picupoint, string picupppic)
        {

            try
            {
                var PickupPoint = picupoint;
                var PickupPic = picupppic;
                objGrModel.ShippingFleet.DoList.Add(new SelectListItem() { Text = "Select Do No", Value = "0" });
                objGrModel.ShippingFleet.DoList.AddRange(Service.EMCS.SvcCargo.GetEdoNoList(PickupPoint, PickupPic).ConvertAll(a =>
                {
                    return new SelectListItem()
                    {
                        Text = a.EdoNo,
                        Value = a.Id.ToString()
                    };
                }));
                objGrModel.ShippingFleet.YesNo = YesNoList();
                return PartialView("ShippingFleetForm", objGrModel);

            }
            catch (Exception ex)
            {
                throw ex;
            }

        }
        [HttpPost]
        public JsonResult SaveArmada(ShippingFleet form)
        {
            try
            {
                ApplicationTitle();
                ViewBag.crudMode = "U";
                SP_ShippingFleetItem data = new SP_ShippingFleetItem();
                ShippingFleet result = new ShippingFleet();
                if (ModelState.IsValid)
                {
                    result.Id = Service.EMCS.SvcGoodsReceiveItem.SaveArmada(form);
                    List<string> DoNoList = new List<string>(form.DoNo.Split(','));
                    if (DoNoList.Count > 0)
                    {
                        form.Id = result.Id;
                        foreach (var item in DoNoList)
                        {
                            form.DoNo = item;
                            Service.EMCS.SvcGoodsReceiveItem.SaveArmadaRefrence(form);

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
        public List<GoodsReceive> UpdateGr(long Id, string pickuppoint, string pickuppic)
        {
            try
            {
                var item = new Data.Domain.EMCS.SpGoodReceive();
                item.Id = Id;
                item.PickupPoint = pickuppoint;
                item.PickupPic = pickuppic;
                var data = Service.EMCS.SvcGoodsReceiveItem.UpdateGr(item);
                return data;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public JsonResult GetCiplAvailableForShippingFleet(string Method, string IdCipl, long idgr, long idShippingFleet)
        {
            //List<CiplItem> data = new List<CiplItem>();
            if (Method == "Edit")
            {
                var list = App.Service.EMCS.SvcGoodsReceiveItem.GetCiplAvailableForShippingFleet(IdCipl, idgr, idShippingFleet);
                return Json(list, JsonRequestBehavior.AllowGet);
            }
            else if (Method == "View")
            {
                var list = App.Service.EMCS.SvcGoodsReceiveItem.CiplItemAvailableInArmada(IdCipl, idgr, idShippingFleet);
                return Json(list, JsonRequestBehavior.AllowGet);
            }
            else
            {
                var list = App.Service.EMCS.SvcGoodsReceiveItem.GetCiplAvailableForShippingFleet(IdCipl, idgr, idShippingFleet);
                return Json(list, JsonRequestBehavior.AllowGet);
            }

        }
        public JsonResult GetCiplIdFromDoNo(string DoNo)
        {
            //List<CiplItem> data = new List<CiplItem>();

            var list = App.Service.EMCS.SvcGoodsReceiveItem.GetCiplIdFromDoNo(DoNo);
            return Json(list, JsonRequestBehavior.AllowGet);
        }
        public JsonResult GetCiplItemCount(string idcipl, long idgr, long idshippingfleet)
        {
            //List<CiplItem> data = new List<CiplItem>();

            var list = App.Service.EMCS.SvcGoodsReceiveItem.GetCiplItemCount(idcipl, idgr, idshippingfleet);
            return Json(list, JsonRequestBehavior.AllowGet);
        }
        public JsonResult GetCiplItem(long idcipl)
        {
            try
            {
                var list = App.Service.EMCS.SvcGoodsReceiveItem.GetCiplItem(idcipl);
                return Json(list, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                throw ex;
            }


        }
        public JsonResult CiplItemAvailableInArmada(string Method, string IdCipl, long idgr, long idShippingFleet)
        {
            if (Method == "View")
            {
                var list = App.Service.EMCS.SvcGoodsReceiveItem.CiplItemAvailableInArmada(IdCipl, idgr, idShippingFleet);
                return Json(list, JsonRequestBehavior.AllowGet);

            }
            else
            {
                var list = App.Service.EMCS.SvcGoodsReceiveItem.GetCiplAvailableForShippingFleet(IdCipl, idgr, idShippingFleet);
                return Json(list, JsonRequestBehavior.AllowGet);
            }

        }
        public JsonResult GetCiplItemInShippingFleetItem(string idcipl, long idgr)
        {
            var list = App.Service.EMCS.SvcGoodsReceiveItem.GetCiplItemInShippingFleetItem(idcipl, idgr);
            return Json(list, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public long DeleteItem(long id, long idCiplItem)
        {
            App.Service.EMCS.SvcGoodsReceiveItem.DeleteItemShippingFleet(id, idCiplItem);
            return 1;
        }
        [HttpPost]
        public long DeleteArmada(long Id, bool IsRFC = false)
        {
            var data = Service.EMCS.SvcGoodsReceiveItem.GetListArmada(Id, 0);
            if (data.Count != 0)
            {
                if (IsRFC != true)
                {
                    if (HttpContext.Session["IsApprover"].ToString() == "True")
                    {
                        var HistoryData = Service.EMCS.SvcGoodsReceiveItem.GetHistoryDataById(data[0].Id);
                        if (HistoryData == null)
                        {

                            Service.EMCS.SvcGoodsReceiveItem.SaveArmadaHistory(data[0], "Deleted");
                            App.Service.EMCS.SvcGoodsReceiveItem.DeleteArmada(Id);

                        }
                        else
                        {
                            App.Service.EMCS.SvcGoodsReceiveItem.DeleteArmada(Id);
                        }
                        return 0;
                    }
                    else
                    {
                        App.Service.EMCS.SvcGoodsReceiveItem.DeleteArmada(Id);
                        return 1;
                    }
                }
                else
                {
                    Service.EMCS.SvcGoodsReceiveItem.SaveArmadaForRFC(data[0], "Deleted");
                }

            }
            return 1;


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
            var files = Service.EMCS.SvcGoodsReceiveItem.GetDocById(id);
            string fullPath = Request.MapPath("~/Upload/EMCS/Dummy/NotFound.txt");

            if (files != null)
            {
                var fileData = files;
                fullPath = Request.MapPath("~/Upload/EMCS/GoodsReceive/" + files.Id + "/" + fileData.Filename);
                var fileBytes = System.IO.File.ReadAllBytes(fullPath);
                string fileName = fileData.Filename;
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
        [HttpGet]
        public ActionResult UpdateArmada(long Id, long IdGr)
        {

            try
            {

                var data = App.Service.EMCS.SvcGoodsReceiveItem.GetListArmada(Id, IdGr);
                return Json(data, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }
        #endregion

        #region Get List Item
        public JsonResult GetItemGr(GridListFilter crit)
        {
            var data = Service.EMCS.SvcGoodsReceiveItem.GetList(crit);
            return Json(new { data }, JsonRequestBehavior.AllowGet);
        }
        public JsonResult GetItemGrById(GridListFilter crit)
        {
            var data = Service.EMCS.SvcGoodsReceiveItem.GetName(crit.Id);
            return Json(new { data }, JsonRequestBehavior.AllowGet);
        }
        public JsonResult GetCheckarmadaDetail(long Id)
        {
            var itemlistCount = Service.EMCS.SvcGoodsReceiveItem.GetArmadaItemList(Id);
            var FileName = Service.EMCS.SvcGoodsReceiveItem.GetFileName(Id);
            var itemlistingrcount = Service.EMCS.SvcGoodsReceiveItem.GetItemInGr(Id);
            return Json(new { itemlistCount, FileName, itemlistingrcount }, JsonRequestBehavior.AllowGet);
        }


        #endregion
    }
}