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

namespace App.Web.Controllers.EMCS
{
    public partial class EmcsController
    {
        public CargoItemModel InitCargoItem(long id)
        {
            var detail = new CargoItemModel();
            var data = Service.EMCS.SvcCargoItem.GetDataById(id);
            if (data != null)
            {
                detail.Id = data.Id;
                detail.CargoDescription = data.CargoDescription;
                detail.ContainerNumber = data.ContainerNumber;
                detail.ContainerType = data.ContainerType;
                detail.ContainerSealNumber = data.ContainerSealNumber;
                detail.IdCargo = data.IdCargo;
                detail.IdCipl = data.IdCipl;
                detail.IdCiplItem = data.IdCargo;
                detail.InBoundDa = data.InboundDa;
                detail.Length = data.Length;
                detail.Width = data.Width;
                detail.Height = data.Height;
                detail.Net = data.NetWeight;
                detail.Gross = data.GrossWeight;
                detail.CaseNumber = data.CaseNumber;
                detail.ItemName = data.ItemName;
                detail.Ea = data.EdoNo;
            }
            return detail;
        }

        public ContainerModel InitItemContainer(long id)
        {
            var data = new ContainerModel();
            data.Cargo = Service.EMCS.SvcCargo.GetCargoById(id);
            data.Container = Service.EMCS.SvcContainer.GetDataById(id);
            return data;
        }

        public ActionResult EditCargoItem(long id = 0)
        {
            ApplicationTitle();
            ViewBag.crudMode = "U";
            var detail = InitCargoItem(id);
            ViewBag.listContainerType = Service.EMCS.MasterParameter.GetParamByGroup("ContainerType");
            return PartialView("Modal.FormEditCargoItem", detail);
        }

        [HttpPost]
        public JsonResult EditCargoItem(CargoItemModel form)
        {
            try
            {
                var obj = Service.EMCS.SvcCargoItem.GetDataById(form.Id);
                var dataUpdate = new CargoItem();
                dataUpdate.Id = form.Id;
                dataUpdate.IdCargo = form.IdCargo;
                dataUpdate.ContainerNumber = form.ContainerNumber;
                dataUpdate.ContainerType = form.ContainerType;
                dataUpdate.ContainerSealNumber = form.ContainerSealNumber;
                dataUpdate.Length = form.Length;
                dataUpdate.Width = form.Width;
                dataUpdate.Height = form.Height;
                dataUpdate.Gross = form.Gross;
                dataUpdate.Net = form.Net;

                var hasChange = Service.EMCS.SvcCargoItem.GetChangesData(dataUpdate, obj.IdCiplItem);
                if (hasChange)
                {
                    #region cek apakah perubahan untuk item yg sama sudah dilakukan
                    var oldUpdate = Service.EMCS.SvcCargoItem.IsAlreadyUpdate(obj.IdCargo, obj.IdCipl, obj.IdCiplItem);
                    var dml = "I";
                    if (oldUpdate != null)
                    {
                        dml = "U";
                    }

                    Service.EMCS.SvcCargoItem.SaveChangeHistory(dml, obj, dataUpdate);
                    #endregion
                }

                Service.EMCS.SvcCargoItem.Update(dataUpdate, "U");
                return JsonCRUDMessage("U", obj);
            }
            catch (Exception err)
            {
                return JsonMessage("Update Cargo Item Failed", 1, err);
            }
        }

        public ActionResult CreateCargoItem(long id = 0)
        {
            var detailCargo = InitCargo(id);
            ApplicationTitle();
            ViewBag.CargoId = id;
            ViewBag.DetailCargo = detailCargo;
            return PartialView("Modal.FormCargoItem");
        }

        [HttpPost]
        public JsonResult CreateCargoItem(ContainerFormModel form)
        {
            try
            {
                var cargo = Service.EMCS.SvcCargo.GetCargoById(form.CargoId);
                if (cargo != null)
                {
                    if (cargo.CargoType == "FCL")
                    {
                        var itemsData = form.Items;
                        InsertItems(form.CargoId, 0, itemsData);

                        return JsonCRUDMessage("I");
                    }
                    else
                    {
                        var itemsData = form.Items;
                        InsertItems(form.CargoId, null, itemsData);
                        return JsonCRUDMessage("I");
                    }
                }
                return JsonMessage("Cannot find cargo data", 1, null);
            }
            catch (Exception err)
            {
                return JsonMessage(err.Message, 1, err);
            }
        }

        [HttpPost]
        public JsonResult InsertNoContainerItems(long cargoId, Nullable<long> containerId, string items)
        {
            // ReSharper disable once JoinDeclarationAndInitializer
            // ReSharper disable once CollectionNeverQueried.Local
            List<long> itemIds;
            itemIds = new List<long>();
            try
            {

                var itemList = items.Split(',');
                if (itemList.Any())
                {
                    foreach (var item in itemList)
                    {
                        var itemsId = Convert.ToInt64(item);
                        var itm = new CargoItem();
                        itm.Id = 0;
                        itm.IdCargo = cargoId;
                        var itemId = Service.EMCS.SvcCargoItem.Insert(itm, itemsId, "I");
                        itemIds.Add(itemId);
                    }
                }
                return JsonCRUDMessage("I");
            }
            catch (Exception err)
            {
                return JsonMessage(err.Message, 1, err);
            }
        }

        public bool InsertItems(long cargoId, Nullable<long> containerId, string itemsData)
        {
            var items = itemsData.Split(',');
            // ReSharper disable once CollectionNeverQueried.Local
            var itemIds = new List<long>();
            foreach (var item in items)
            {
                var itemsId = Convert.ToInt64(item);
                Service.EMCS.SvcCipl.GetCiplItemById(itemsId);

                var itm = new CargoItem();
                itm.Id = 0;
                var itemId = Service.EMCS.SvcCargoItem.Insert(itm, itemsId, "I");
                itemIds.Add(itemId);
            }
            return false;
        }

        public ActionResult UpdateContainerItem(long cargoId)
        {
            ApplicationTitle();
            ViewBag.CargoID = cargoId;
            ViewBag.AllowRead = AuthorizeAcces.AllowRead;
            ViewBag.AllowCreate = AuthorizeAcces.AllowCreated;
            ViewBag.AllowUpdate = AuthorizeAcces.AllowUpdated;
            ViewBag.AllowDelete = AuthorizeAcces.AllowDeleted;

            ViewBag.crudMode = "I";
            PaginatorBoot.Remove("SessionTRN");
            return View("CargoForm");
        }

        public JsonResult GetContainerItemList(GridListFilter filter)
        {
            var data = Service.EMCS.SvcCargoItem.GetList(filter);
            return Json(data, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetTotalDataCargo(long id)
        {
            var data = Service.EMCS.SvcCargoItem.GetTotalDataCargo(id);
            return Json(data, JsonRequestBehavior.AllowGet);
        }

        public JsonResult DeleteCargoItem(long id)
        {
            try
            {
                var data = Service.EMCS.SvcCargoItem.GetDataById(id);
                Service.EMCS.SvcCargoItem.Remove(id);
                return JsonCRUDMessage("U", data);
            }
            catch (Exception err)
            {
                return JsonMessage(err.Message, 1, err);
            }
        }
    }
}