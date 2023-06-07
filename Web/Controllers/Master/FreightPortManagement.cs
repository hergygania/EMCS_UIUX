using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using App.Data.Domain;
using App.Domain;
using App.Framework.Mvc;
using App.Service.Master;
using App.Web.Models;
using App.Web.App_Start;
using App.Web.Helper;

namespace App.Web.Controllers
{
    public partial class MasterController
    {
        #region Initilize

        private Data.Domain.FreightPort InitilizeFreightPort(int id)
        {
            var model = new Data.Domain.FreightPort();
            if (id == 0)
            {
                return model;
            }
            model = Service.Master.FreightPort.GetId(id);
            return model;
        }

        #endregion

        [AuthorizeAcces(ActionType = AuthorizeAcces.IsRead, UrlMenu = "AirPortManagement")]
        public ActionResult AirPortManagement()
        {
            ViewBag.AllowRead = AuthorizeAcces.AllowRead;
            ViewBag.AllowCreate = AuthorizeAcces.AllowCreated;
            ViewBag.AllowUpdate = AuthorizeAcces.AllowUpdated;
            ViewBag.AllowDelete = AuthorizeAcces.AllowDeleted;
            ViewBag.PortModel = "Air";
            PaginatorBoot.Remove("SessionTRN");
            return View("FreightPort");
        }

        [AuthorizeAcces(ActionType = AuthorizeAcces.IsRead, UrlMenu = "SeaPortManagement")]
        public ActionResult SeaPortManagement()
        {
            ViewBag.AllowRead = AuthorizeAcces.AllowRead;
            ViewBag.AllowCreate = AuthorizeAcces.AllowCreated;
            ViewBag.AllowUpdate = AuthorizeAcces.AllowUpdated;
            ViewBag.AllowDelete = AuthorizeAcces.AllowDeleted;
            ViewBag.PortModel = "Sea";
            PaginatorBoot.Remove("SessionTRN");
            return View("FreightPort");
        }
        [AuthorizeAcces(ActionType = AuthorizeAcces.IsRead, UrlMenu = "ImportMasterFreightCost")]
        public ActionResult FreightPortPage()
        {
            PaginatorBoot.Remove("SessionTRN");
            return FreightPortPageXt();
        }
        [AuthorizeAcces(ActionType = AuthorizeAcces.IsRead, UrlMenu = "ImportMasterFreightCost")]
        public ActionResult FreightPortPageXt()
        {
            Func<MasterSearchForm, IList<Data.Domain.FreightPort>> func = delegate(MasterSearchForm crit)
                {
                    var param = Request["params"];
                    if (!string.IsNullOrEmpty(param))
                    {
                        JavaScriptSerializer ser = new JavaScriptSerializer();
                        crit = ser.Deserialize<MasterSearchForm>(param);
                    }

                    var list = Service.Master.FreightPort.GetList(crit);
                    return list.OrderByDescending(o => o.ModifiedDate).ToList();
                };
            ActionResult paging = PaginatorBoot.Manage("SessionTRN", func).Pagination.ToJsonResult();
            return Json(paging, JsonRequestBehavior.AllowGet);
        }

        [AuthorizeAcces(ActionType = AuthorizeAcces.IsRead, UrlMenu = "ImportMasterFreightCost")]
        [HttpGet]
        public ActionResult FreightPortCreate(bool isSeaPort)
        {
            ViewBag.crudMode = "I";
            var model = InitilizeFreightPort(0);
            model.PortID = 0;
            model.IsSeaFreight = isSeaPort;
            return PartialView("FreightPort.iud", model);
        }

        [AuthorizeAcces(ActionType = AuthorizeAcces.IsCreated, UrlMenu = "ImportMasterFreightCost")]
        [HttpPost]
        public ActionResult FreightPortCreate(Data.Domain.FreightPort item)
        {
            ViewBag.crudMode = "I";
            var isExist = Service.Master.FreightPort.GetList().Any(a => (a.PortCode == item.PortCode) && (a.IsSeaFreight == item.IsSeaFreight));
            if (isExist)
            {
                //ModelState.AddModelError("PortCode",new Exception("Port Code Already Exists"));
                return Json(new JsonObject { Status = 1, Msg = "Data already exists" });
            }
            if (ModelState.IsValid)
            {
                item.PortCode = Common.Sanitize(item.PortCode);
                item.PortName = Common.Sanitize(item.PortName);
                item.Description = Common.Sanitize(item.Description);

                Service.Master.FreightPort.Update(
                        item,
                        ViewBag.crudMode);
                return JsonCRUDMessage(ViewBag.crudMode);
            }
            else
            {
                var nsg = Helper.Error.ModelStateErrors(ModelState);
                return Json(new { success = false, Msg = nsg });
            }

            //return Json(new { succes = false });
        }

        [AuthorizeAcces(ActionType = AuthorizeAcces.IsRead, UrlMenu = "ImportMasterFreightCost")]
        [HttpGet]
        public ActionResult FreightPortedit(int id)
        {
            ViewBag.crudMode = "U";
            var model = InitilizeFreightPort(id);
            if (model == null)
            {
                return HttpNotFound();
            }
            return PartialView("FreightPort.iud", model);
        }

        [AuthorizeAcces(ActionType = AuthorizeAcces.IsUpdated, UrlMenu = "ImportMasterFreightCost")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult FreightPortEdit(Data.Domain.FreightPort item)
        {
            ViewBag.crudMode = "U";
            var isExist = Service.Master.FreightPort.GetList().Any(a => (a.PortCode == item.PortCode) && (a.IsSeaFreight == item.IsSeaFreight));
            if (isExist)
            {
                //ModelState.AddModelError("PortCode",new Exception("Port Code Already Exists"));
                return Json(new JsonObject { Status = 1, Msg = "Data already exists" });
            }
            if (ModelState.IsValid)
            {
                Service.Master.FreightPort.Update(
                        item,
                        ViewBag.crudMode);

                return JsonCRUDMessage(ViewBag.crudMode);
            }
            else
            {
                var nsg = Helper.Error.ModelStateErrors(ModelState);
                return Json(new { success = false, Msg = nsg });
            }

        }

        [AuthorizeAcces(ActionType = AuthorizeAcces.IsRead, UrlMenu = "ImportMasterFreightCost")]
        [HttpGet]
        public ActionResult FreightPortDelete(string id)
        {
            ViewBag.crudMode = "D";
            UserViewModel model = InitilizeData(id);
            if (model == null)
            {
                return HttpNotFound();
            }

            return PartialView("FreightPort.iud", model);
        }

        [AuthorizeAcces(ActionType = AuthorizeAcces.IsDeleted, UrlMenu = "ImportMasterFreightCost")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult FreightPortDelete(Data.Domain.FreightPort items)
        {
            ViewBag.crudMode = "D";
            if (ModelState.IsValid)
            {
                Service.Master.FreightPort.Update(
                        items,
                        ViewBag.crudMode);
                return JsonCRUDMessage(ViewBag.crudMode);
            }
            return PartialView("FreightPort.iud", items);
        }

        [AuthorizeAcces(ActionType = AuthorizeAcces.IsDeleted, UrlMenu = "ImportMasterFreightCost")]
        [HttpPost]
        public ActionResult FreightPortDeleteById(int id)
        {
            var item = Service.Master.FreightPort.GetId(id);
            Service.Master.FreightPort.Update(
                    item,
                    "D");
            return JsonCRUDMessage("D");
        }
    }
}