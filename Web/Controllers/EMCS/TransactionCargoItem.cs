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
        public List<CargoItemModel> InitCargoItem(List<long> ids)
        {
            try
            {
                var detail = new List<CargoItemModel>();
                var dataList = Service.EMCS.SvcCargoItem.GetDataByCargoId(ids);
                if (dataList.Count > 0)
                {
                    foreach (var data in dataList)
                    {
                        var cargoModel = new CargoItemModel();
                        cargoModel.Id = data.Id;
                        cargoModel.CargoDescription = data.CargoDescription;
                        cargoModel.ContainerNumber = data.ContainerNumber;
                        cargoModel.ContainerType = data.ContainerType;
                        cargoModel.ContainerSealNumber = data.ContainerSealNumber;
                        cargoModel.IdCargo = data.IdCargo;
                        cargoModel.IdCipl = data.IdCipl;
                        cargoModel.IdCiplItem = data.IdCargo;
                        cargoModel.InBoundDa = data.InboundDa;
                        cargoModel.Length = data.Length;
                        cargoModel.Width = data.Width;
                        cargoModel.Height = data.Height;
                        cargoModel.Net = data.NetWeight;
                        cargoModel.Gross = data.GrossWeight;
                        cargoModel.CaseNumber = data.CaseNumber;
                        cargoModel.ItemName = data.ItemName;
                        cargoModel.Ea = data.EdoNo;
                        detail.Add(cargoModel);

                        cargoModel.listContainerType = Service.EMCS.MasterParameter.GetParamByGroup("ContainerType").ConvertAll(a =>
                        {
                            return new SelectListItem()
                            {
                                Text = a.Description,
                                Value = a.Value.ToString(),
                                Selected = data.ContainerType != null && a.Description.ToString() == data.ContainerType.ToString() ? true : false
                            };
                        });
                    }

                }
                return detail;
            }
            catch (Exception ex)
            {
                throw ex;

            }

        }

        public ContainerModel InitItemContainer(long id)
        {
            var data = new ContainerModel();
            data.Cargo = Service.EMCS.SvcCargo.GetCargoById(id);
            data.Container = Service.EMCS.SvcContainer.GetDataById(id);
            return data;
        }
        public CargoItem InitItemCargoFor(long id)
        {
            var data = new CargoItem();
            data = Service.EMCS.SvcContainer.GetDataItemById(id);
            return data;
        }


        public ActionResult EditCargoitem(string cargoids, bool IsRFC = false)
        {
            try
            {
                var model = JsonConvert.DeserializeObject<cargoItem>(cargoids);
                ApplicationTitle();
                ViewBag.crudMode = "U";
                ViewBag.IsRFC = IsRFC;
                var detail = InitCargoItem(model.cargoIds);
                if (detail.Count == 1) // Single item
                    ViewBag.SingleItem = true;
                else
                    ViewBag.SingleItem = false;
                ViewBag.listContainerType = Service.EMCS.MasterParameter.GetParamByGroup("ContainerType");

                var containerTypeList = Service.EMCS.MasterParameter.GetParamByGroup("ContainerType").Select(a => new SelectListItem
                {
                    Text = a.Description,
                    Value = a.Value.ToString(),
                });
                return PartialView("Modal.FormEditCargoItem", detail);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public class cargoItem
        {
            public List<long> cargoIds { get; set; }

        }
        [HttpPost]
        public JsonResult EditCargoItem(List<CargoItemModel> modeldata, bool IsRFC = false)
        {
            try
            {
                var updateModel = new List<SpCargoItemDetail>();
                if (modeldata != null)
                {
                    for (int i = 0; i < modeldata.Count; i++)
                    {
                        modeldata[i].ContainerNumber = modeldata[0].ContainerNumber;
                        modeldata[i].ContainerSealNumber = modeldata[0].ContainerSealNumber;
                        modeldata[i].ContainerType = modeldata[0].ContainerType;
                    }
                    foreach (var item in modeldata)
                    {
                        var obj = Service.EMCS.SvcCargoItem.GetDataById(item.Id);
                        var dataUpdate = new CargoItem();

                        dataUpdate.IdCiplItem = obj.IdCiplItem;
                        dataUpdate.Id = item.Id;
                        dataUpdate.IdCargo = item.IdCargo;
                        dataUpdate.ContainerNumber = item.ContainerNumber;
                        dataUpdate.ContainerType = item.ContainerType;
                        dataUpdate.ContainerSealNumber = item.ContainerSealNumber;
                        dataUpdate.Length = item.Length;
                        dataUpdate.Width = item.Width;
                        dataUpdate.Height = item.Height;
                        dataUpdate.Gross = item.Gross;
                        dataUpdate.Net = item.Net;


                        if (modeldata.Count == 1)
                        {
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
                        }
                        else
                        {
                            dataUpdate.Length = obj.Length;
                            dataUpdate.Width = obj.Width;
                            dataUpdate.Height = obj.Height;
                            dataUpdate.Gross = obj.GrossWeight;
                            dataUpdate.Net = obj.NetWeight;

                        }

                        if (IsRFC == true)
                        {
                            var id = Service.EMCS.SvcCargoItem.GetCargoItemChangeByIdCiplItem(dataUpdate.IdCiplItem);

                            if (id == null)
                            {
                                id = 0;
                                Service.EMCS.SvcCargoItem.UpdateItemChange(dataUpdate, "U", id);

                            }
                            else if (id.Status != "Deleted")
                            {
                                dataUpdate.Id = id.Id;
                                Service.EMCS.SvcCargoItem.UpdateItemChange(dataUpdate, "U", id.Id);

                            }
                        }
                        else
                        {
                            Service.EMCS.SvcCargoItem.Update(dataUpdate, "U");

                        }
                        updateModel.Add(obj);
                    }
                }


                return JsonCRUDMessage("U", updateModel);
            }
            catch (Exception err)
            {
                return JsonMessage("Update Cargo Item Failed", 1, err);
            }
        }
        //public ActionResult EditCargoItemById(long id)
        //{
        //    try
        //    {
        //        ApplicationTitle();
        //        ViewBag.crudMode = "U";
        //        var detail = InitItemCargoFor(id);
        //        ViewBag.listContainerType = Service.EMCS.MasterParameter.GetParamByGroup("ContainerType");

        //        var containerTypeList = Service.EMCS.MasterParameter.GetParamByGroup("ContainerType").Select(a => new SelectListItem
        //        {
        //            Text = a.Description,
        //            Value = a.Value.ToString(),
        //        });
        //        return PartialView("Modal.FormEditCargoItem", detail);
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //}
        //[HttpPost]
        //public JsonResult EditCargoItemById(CargoItemModel form)
        //{
        //    try
        //    {
        //        var updateModel = new List<SpCargoItemDetail>();
        //        //foreach (var item in form)
        //        //{
        //        var obj = Service.EMCS.SvcCargoItem.GetDataById(form.Id);
        //        var dataUpdate = new CargoItem();
        //        dataUpdate.Id = form.Id;
        //        dataUpdate.IdCargo = form.IdCargo;
        //        dataUpdate.ContainerNumber = form.ContainerNumber;
        //        dataUpdate.ContainerType = form.ContainerType;
        //        dataUpdate.ContainerSealNumber = form.ContainerSealNumber;
        //        dataUpdate.Length = form.Length;
        //        dataUpdate.Width = form.Width;
        //        dataUpdate.Height = form.Height;
        //        dataUpdate.Gross = form.Gross;
        //        dataUpdate.Net = form.Net;

        //        var hasChange = Service.EMCS.SvcCargoItem.GetChangesData(dataUpdate, obj.IdCiplItem);
        //        if (hasChange)
        //        {
        //            #region cek apakah perubahan untuk item yg sama sudah dilakukan
        //            var oldUpdate = Service.EMCS.SvcCargoItem.IsAlreadyUpdate(obj.IdCargo, obj.IdCipl, obj.IdCiplItem);
        //            var dml = "I";
        //            if (oldUpdate != null)
        //            {
        //                dml = "U";
        //            }

        //            Service.EMCS.SvcCargoItem.SaveChangeHistory(dml, obj, dataUpdate);
        //            #endregion
        //        }

        //        Service.EMCS.SvcCargoItem.Update(dataUpdate, "U");
        //        updateModel.Add(obj);
        //    //}

        //        return JsonCRUDMessage("U", updateModel);
        //    }
        //    catch (Exception err)
        //    {
        //        return JsonMessage("Update Cargo Item Failed", 1, err);
        //    }
        //}

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
        public JsonResult InsertNoContainerItems(long cargoId, Nullable<long> containerId, string items, bool IsRFC = false,List<CargoItemModel> input = null)
        {
            // ReSharper disable once JoinDeclarationAndInitializer
            // ReSharper disable once CollectionNeverQueried.Local
            List<long> itemIds;
            itemIds = new List<long>();
            try
            {
                if (IsRFC == true)
                {
                    if (items == null)
                    {
                        foreach (var item in input)
                        {
                            var obj = Service.EMCS.SvcCargoItem.GetDataById(item.Id);
                            var Status = Service.EMCS.SvcCargoItem.GetCargoItemChangeByIdCiplItem(obj.IdCiplItem);
                            var dataUpdate = new CargoItem();

                            dataUpdate.IdCiplItem = obj.IdCiplItem;
                            dataUpdate.Id = item.Id;
                            dataUpdate.IdCargo = item.IdCargo;
                            dataUpdate.ContainerNumber = item.ContainerNumber;
                            dataUpdate.ContainerType = item.ContainerType;
                            dataUpdate.ContainerSealNumber = item.ContainerSealNumber;
                            dataUpdate.Length = item.Length;
                            dataUpdate.Width = item.Width;
                            dataUpdate.Height = item.Height;
                            dataUpdate.Gross = item.Gross;
                            dataUpdate.Net = item.Net;
                            if (Status != null)
                            {
                                if (Status.Status != "Deleted")
                                {
                                    Service.EMCS.SvcCargoItem.UpdateItemChange(dataUpdate, "U", 0);
                                }
                                else
                                {
                                    return JsonMessage("This Record Is Deleted", 1, "");
                                }
                            }
                            else
                            {
                                Service.EMCS.SvcCargoItem.UpdateItemChange(dataUpdate, "U", 0);

                            }
                        }
                    }
                    else
                    {
                        if(input != null)
                        {
                            if (input.Count > 1)
                            {
                                for (int i = 0; i < input.Count; i++)
                                {
                                    input[i].ContainerNumber = input[0].ContainerNumber;
                                    input[i].ContainerSealNumber = input[0].ContainerSealNumber;
                                    input[i].ContainerType = input[0].ContainerType;

                                }
                                foreach (var item in input)
                                {
                                    var obj = Service.EMCS.SvcCargoItem.GetDataById(item.Id);
                                    var Status = Service.EMCS.SvcCargoItem.GetCargoItemChangeByIdCiplItem(obj.IdCiplItem);
                                    var dataUpdate = new CargoItem();

                                    dataUpdate.IdCiplItem = obj.IdCiplItem;
                                    dataUpdate.Id = item.Id;
                                    dataUpdate.IdCargo = item.IdCargo;
                                    dataUpdate.ContainerNumber = item.ContainerNumber;
                                    dataUpdate.ContainerType = item.ContainerType;
                                    dataUpdate.ContainerSealNumber = item.ContainerSealNumber;
                                    dataUpdate.Length = obj.Length;
                                    dataUpdate.Width = obj.Width;
                                    dataUpdate.Height = obj.Height;
                                    dataUpdate.Gross = obj.Gross;
                                    dataUpdate.Net = obj.Net;
                                    if (Status != null)
                                    {
                                        if (Status.Status != "Deleted")
                                        {
                                            Service.EMCS.SvcCargoItem.UpdateItemChange(dataUpdate, "U", 0);
                                        }
                                        else
                                        {
                                            return JsonMessage("This Record Is Deleted", 1, "");
                                        }
                                    }
                                    else
                                    {
                                        Service.EMCS.SvcCargoItem.UpdateItemChange(dataUpdate, "U", 0);

                                    }
                                }
                            }
                            else
                            {
                                foreach (var item in input)
                                {
                                    var obj = Service.EMCS.SvcCargoItem.GetDataById(item.Id);
                                    var Status = Service.EMCS.SvcCargoItem.GetCargoItemChangeByIdCiplItem(obj.IdCiplItem);
                                    var dataUpdate = new CargoItem();

                                    dataUpdate.IdCiplItem = obj.IdCiplItem;
                                    dataUpdate.Id = item.Id;
                                    dataUpdate.IdCargo = item.IdCargo;
                                    dataUpdate.ContainerNumber = item.ContainerNumber;
                                    dataUpdate.ContainerType = item.ContainerType;
                                    dataUpdate.ContainerSealNumber = item.ContainerSealNumber;
                                    dataUpdate.Length = item.Length;
                                    dataUpdate.Width = item.Width;
                                    dataUpdate.Height = item.Height;
                                    dataUpdate.Gross = item.Gross;
                                    dataUpdate.Net = item.Net;
                                    if (Status != null)
                                    {
                                        if (Status.Status != "Deleted")
                                        {
                                            Service.EMCS.SvcCargoItem.UpdateItemChange(dataUpdate, "U", 0);
                                        }
                                        else
                                        {
                                            return JsonMessage("This Record Is Deleted", 1, "");
                                        }
                                    }
                                    else
                                    {
                                        Service.EMCS.SvcCargoItem.UpdateItemChange(dataUpdate, "U", 0);

                                    }
                                }
                            }
                        }
                        else
                        {
                            var itemList = items.Split(',');
                            if (itemList.Any())
                            {
                                foreach (var item in itemList)
                                {
                                    var itemsId = Convert.ToInt64(item);
                                    var data = Service.EMCS.SvcCipl.GetCiplItemById(itemsId);
                                    var itm = new CargoItem();
                                    itm.Id = 0;
                                    itm.IdCargo = cargoId;
                                    var dataUpdate = new CargoItem();

                                    dataUpdate.IdCiplItem = data.Id;
                                    dataUpdate.Id = 0;
                                    dataUpdate.IdCargo = cargoId;
                                    dataUpdate.Length = data.Length;
                                    dataUpdate.Width = data.Width;
                                    dataUpdate.Height = data.Height;
                                    dataUpdate.Gross = data.GrossWeight;
                                    dataUpdate.Net = data.NetWeight;
                                    Service.EMCS.SvcCargoItem.UpdateItemChange(dataUpdate, "U", 0);
                                    //var itemId = Service.EMCS.SvcCargoItem.Insert(itm, itemsId, "I", false);
                                }
                            }
                        }
                        
                    }
                   
                }
                else
                {
                    if (items == null)
                    {
                        foreach (var item in input)
                        {
                            var obj = Service.EMCS.SvcCargoItem.GetDataById(item.Id);
                            var dataUpdate = new CargoItem();

                            dataUpdate.IdCiplItem = obj.IdCiplItem;
                            dataUpdate.Id = item.Id;
                            dataUpdate.IdCargo = item.IdCargo;
                            dataUpdate.ContainerNumber = item.ContainerNumber;
                            dataUpdate.ContainerType = item.ContainerType;
                            dataUpdate.ContainerSealNumber = item.ContainerSealNumber;
                            dataUpdate.Length = item.Length;
                            dataUpdate.Width = item.Width;
                            dataUpdate.Height = item.Height;
                            dataUpdate.Gross = item.Gross;
                            dataUpdate.Net = item.Net;
                            Service.EMCS.SvcCargoItem.Insert(dataUpdate,dataUpdate.Id, "I", false);
                        }
                    }
                    else
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
                                var itemId = Service.EMCS.SvcCargoItem.Insert(itm, itemsId, "I", false);
                                itemIds.Add(itemId);
                            }
                        }
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
                var itemId = Service.EMCS.SvcCargoItem.Insert(itm, itemsId, "I", false);
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

        //public JsonResult GetTotalDataCargo(long id)
        //{
        //    var data = Service.EMCS.SvcCargoItem.GetTotalDataCargo(id);
        //    return Json(data, JsonRequestBehavior.AllowGet);
        //}
        public JsonResult GetTotalDataCargo(long id, string selectvalue)
        {
            if (selectvalue == "CaseNo")
            {
                var data = Service.EMCS.SvcCargoItem.GetTotalDataCargo(id, "CaseNo");
                return Json(new { data = data }, JsonRequestBehavior.AllowGet);

            }
            else
            {
                var data = Service.EMCS.SvcCargoItem.GetTotalDataCargo(id, "NoItem");
                return Json(new { data = data }, JsonRequestBehavior.AllowGet);


            }
        }
        //public JsonResult GetTotalDataCargoByItem(long id)
        //{
        //    var data = Service.EMCS.SvcCargoItem.GetTotalDataCargo(id, "NoItem");
        //    return Json(data, JsonRequestBehavior.AllowGet);
        //}
        public JsonResult DeleteCargoItem(long id, bool IsRFC)
        {
            try
            {
                if (IsRFC == true)
                {
                    var data = Service.EMCS.SvcCargoItem.GetItemById(id);
                    Service.EMCS.SvcCargoItem.DeleteItemChange(data, "U", id);
                    return JsonCRUDMessage("U", data);
                }
                else
                {
                    var data = Service.EMCS.SvcCargoItem.GetDataById(id);
                    Service.EMCS.SvcCargoItem.Remove(id);
                    return JsonCRUDMessage("U", data);
                }

            }
            catch (Exception err)
            {
                return JsonMessage(err.Message, 1, err);
            }
        }
    }
}