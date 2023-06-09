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
        public ContainerModel InitContainer(long id)
        {
            var data = new ContainerModel();
            data.Cargo = Service.EMCS.SvcCargo.GetCargoById(id);
            data.Container = Service.EMCS.SvcContainer.GetDataById(id);
            return data;
        }

        public ActionResult CreateCargoContainer(long id)
        {
            ViewBag.CargoID = id;
            ViewBag.crudMode = "I";

            var detail = InitContainer(id);
            return PartialView("Modal.FormCargoContainer", detail);
        }


        [HttpPost]
        public JsonResult CreateCargoContainer(ContainerFormModel form,bool IsRFC = false)
        {

            if(form.ContainerType == null)
            {
                CargoItem a = new CargoItem();
                a.IdCargo = form.CargoId;
                a.ContainerNumber = form.ContainerNumber;
                a.ContainerSealNumber = form.ContainerSealNumber;
                var containertype2 = CheckCNNo(a);
                form.ContainerType = Convert.ToString(containertype2.Data);
            }
            var cargo = Service.EMCS.SvcCargo.GetCargoById(form.CargoId);

            if (cargo != null)
            {
                var itemsData = form.Items;
                var items = itemsData.Split(',');
                foreach (var item in items)
                {
                    var itemIdStr = item;
                    var idx = Convert.ToInt64(itemIdStr);

                    var itm = new CargoItem();
                    itm.Id = 0;
                    itm.ContainerNumber = form.ContainerNumber;
                    itm.ContainerType = form.ContainerType;
                    itm.ContainerSealNumber = form.ContainerSealNumber;
                    itm.IdCargo = form.CargoId;
                    //if (IsRFC == true)
                    //{
                    //    Service.EMCS.SvcCargoItem.UpdateItemChange(itm, "U", 0);
                    //}
                    //else
                    //{
                        Service.EMCS.SvcCargoItem.Insert(itm, idx, "I", IsRFC);

                    //}
                }
                return JsonCRUDMessage("I");
            }
            return JsonMessage("Cannot find cargo data", 1, null);
        }

        public ActionResult UpdateContainer(int cargoId)
        {
            ViewBag.AppTitle = "Export Monitoring & Control System";
            ViewBag.CargoID = cargoId;
            ViewBag.AllowRead = AuthorizeAcces.AllowRead;
            ViewBag.AllowCreate = AuthorizeAcces.AllowCreated;
            ViewBag.AllowUpdate = AuthorizeAcces.AllowUpdated;
            ViewBag.AllowDelete = AuthorizeAcces.AllowDeleted;

            ViewBag.crudMode = "I";
            PaginatorBoot.Remove("SessionTRN");
            return View("CargoForm");
        }

        public JsonResult GetContainerList(GridListFilterModel filter)
        {
            var dataFilter = new GridListFilter();
            dataFilter.Limit = filter.Limit;
            dataFilter.Order = filter.Order;
            dataFilter.Term = filter.Term;
            dataFilter.Sort = filter.Sort;
            dataFilter.Offset = filter.Offset;

            var data = Service.EMCS.SvcCargo.CargoList(dataFilter);
            return Json(data, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetCipLforContainer(string ciplNo, long cargoId)
        {
            List<long> ciplnoList = new List<long>();
            if (ciplNo != String.Empty)
            {
                List<string> ciplnoStr = ciplNo.Split(',').ToList();
                foreach (var str in ciplnoStr)
                {
                    var id = Convert.ToInt64(str);
                    ciplnoList.Add(id);
                }
            }

            var data = Service.EMCS.SvcCargo.GetCiplForCargo(ciplnoList, cargoId);
            return Json(data, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult InsertContainer(CargoFormData item, string dml)
        {
            long id = Service.EMCS.SvcCargo.CrudSp(item, "I");
            var cargoData = Service.EMCS.SvcCargo.GetCargoById(id);
            return JsonCRUDMessage("I", new { cargoData });
        }

        [HttpPost]
        public long InsertContainerItem(List<CargoItem> data, long id)
        {
            id = Service.EMCS.SvcCargo.InsertCargolItem(data, id);
            return (id != 0) ? 1 : 0;
        }

        public JsonResult GetCiplItemByContainer(GridListFilterModel crit)
        {
            var id = Convert.ToInt64(crit.SearchName);
            var data = Service.EMCS.SvcCargo.GetCiplItemByContainer(id);
            return Json(new { data }, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetCiplItemAvailable(long idCargo)
        {
            var ciplList = Service.EMCS.SvcCargo.GetCargoReferenceById(idCargo);
            var newData = new List<long>();
            foreach (var item in ciplList)
            {
                newData.Add(item.IdCipl);
            }
            var referrenceList = string.Join(",", newData);
            var data = Service.EMCS.SvcCargo.GetCiplItemAvailable(referrenceList, idCargo);
            return Json(new { data }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult EditContainerCargo(long id)
        {
            ViewBag.CargoID = id;
            ViewBag.crudMode = "I";

            var detail = InitContainer(id);
            return PartialView("Modal.FormCargoContainer", detail);
        }

        [HttpPost]
        public JsonResult UpdateContainer(CargoFormData item, string dml)
        {
            var id = Service.EMCS.SvcCargo.CrudSp(item, "I");
            var cargoData = Service.EMCS.SvcCargo.GetCargoById(id);
            return JsonCRUDMessage("I", new { cargoData });
        }
    }
}