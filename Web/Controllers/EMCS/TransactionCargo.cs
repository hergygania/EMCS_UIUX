using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using App.Web.App_Start;
using App.Web.Models.EMCS;
using System.IO;
using System.Text.RegularExpressions;

namespace App.Web.Controllers.EMCS
{
    public partial class EmcsController
    {
        [AuthorizeAcces(ActionType = AuthorizeAcces.IsRead)]
        public ActionResult CargoList()
        {
            ApplicationTitle();
            ViewBag.AllowRead = AuthorizeAcces.AllowRead;
            ViewBag.AllowCreate = AuthorizeAcces.AllowCreated;
            ViewBag.AllowUpdate = AuthorizeAcces.AllowUpdated;
            ViewBag.AllowDelete = AuthorizeAcces.AllowDeleted;
            PaginatorBoot.Remove("SessionTRN");
            return View();
        }

        public Data.Domain.EMCS.SpCargoDetail InitCargo(long id)
        {
            var detail = Service.EMCS.SvcCargo.GetCargoById(id);
            return detail;
        }

        [AuthorizeAcces(ActionType = AuthorizeAcces.IsRead, UrlMenu = "CargoList")]
        public ActionResult CargoForm()
        {
            ApplicationTitle();
            ViewBag.AllowRead = AuthorizeAcces.AllowRead;
            ViewBag.AllowCreate = AuthorizeAcces.AllowCreated;
            ViewBag.AllowUpdate = AuthorizeAcces.AllowUpdated;
            ViewBag.AllowDelete = AuthorizeAcces.AllowDeleted;
            PaginatorBoot.Remove("SessionTRN");
            return View();
        }

        public ActionResult CreateCargo()
        {
            ApplicationTitle();
            ViewBag.CargoID = 0;
            ViewBag.AllowRead = AuthorizeAcces.AllowRead;
            ViewBag.AllowCreate = AuthorizeAcces.AllowCreated;
            ViewBag.AllowUpdate = AuthorizeAcces.AllowUpdated;
            ViewBag.AllowDelete = AuthorizeAcces.AllowDeleted;
            ViewBag.crudMode = "I";
            PaginatorBoot.Remove("SessionTRN");

            var detail = new CargoModel();
            // ReSharper disable once Mvc.InvalidModelType
            return View("CargoForm", detail);
        }

        [AuthorizeAcces(ActionType = AuthorizeAcces.IsUpdated, UrlMenu = "CargoList")]
        public ActionResult UpdateCargo(long cargoId)
        {
            ApplicationTitle();
            ViewBag.CargoID = cargoId;
            ViewBag.AllowRead = AuthorizeAcces.AllowRead;
            ViewBag.AllowCreate = AuthorizeAcces.AllowCreated;
            ViewBag.AllowUpdate = AuthorizeAcces.AllowUpdated;
            ViewBag.AllowDelete = AuthorizeAcces.AllowDeleted;

            ViewBag.crudMode = "I";
            var detail = new CargoFormModel();
            PaginatorBoot.Remove("SessionTRN");
            return View("CargoForm", detail);
        }

        [AuthorizeAcces(ActionType = AuthorizeAcces.IsRead, UrlMenu = "CargoList")]
        [HttpGet]
        public ActionResult CargoView(Data.Domain.EMCS.GridListFilter filter)
        {
            ApplicationTitle();
            ViewBag.AllowRead = AuthorizeAcces.AllowRead;
            ViewBag.AllowCreate = AuthorizeAcces.AllowCreated;
            ViewBag.AllowUpdate = AuthorizeAcces.AllowUpdated;
            ViewBag.AllowDelete = AuthorizeAcces.AllowDeleted;
            ViewBag.WizardData = Service.EMCS.SvcWizard.GetWizardData("cargo", filter.Cargoid);
            PaginatorBoot.Remove("SessionTRN");

            var detail = new CargoModel();
            detail.Data = Service.EMCS.SvcCargo.GetCargoById(filter.Cargoid);
            detail.DataItem = Service.EMCS.SvcCargo.GetCargoDetailData(filter.Cargoid);
            detail.TemplateHeader = Service.EMCS.DocumentStreamGenerator.GetCargoHeaderData(filter.Cargoid);
            detail.TemplateDetail = Service.EMCS.DocumentStreamGenerator.GetCargoDetailData(filter.Cargoid);
            ViewBag.IdCipl = filter.IdCipl;

            return View(detail);
        }

        public ActionResult CargoApproval(long id)
        {
            if (id != 0)
            {
                ViewBag.crudMode = "U";
                ApplicationTitle();
                ViewBag.AllowRead = AuthorizeAcces.AllowRead;
                ViewBag.AllowCreate = AuthorizeAcces.AllowCreated;
                ViewBag.AllowUpdate = AuthorizeAcces.AllowUpdated;
                ViewBag.AllowDelete = AuthorizeAcces.AllowDeleted;
                ViewBag.WizardData = Service.EMCS.SvcWizard.GetWizardData("cargo", id);
                PaginatorBoot.Remove("SessionTRN");

                var detail = new CargoModel();
                detail.Data = Service.EMCS.SvcCargo.GetCargoById(id);
                detail.DataItem = Service.EMCS.SvcCargo.GetCargoDetailData(id);
                detail.DataRequest = Service.EMCS.SvcRequestCl.GetRequestCl(id);
                detail.TemplateHeader = Service.EMCS.DocumentStreamGenerator.GetCargoHeaderData(id);
                detail.TemplateDetail = Service.EMCS.DocumentStreamGenerator.GetCargoDetailData(id);

                return View(detail);
            }
            return View("NotAuthorize");
        }

        [AuthorizeAcces(ActionType = AuthorizeAcces.IsRead, UrlMenu = "CargoList")]
        public JsonResult GetCargoList(GridListFilterModel filter)
        {
            var dataFilter = new Data.Domain.EMCS.GridListFilter();
            dataFilter.Limit = filter.Limit;
            dataFilter.Order = filter.Order;
            dataFilter.Term = filter.Term;
            dataFilter.Sort = filter.Sort;
            dataFilter.Offset = filter.Offset;

            var data = Service.EMCS.SvcCargo.CargoList(dataFilter);
            return Json(data, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetConsigneeList(Domain.MasterSearchForm crit)
        {
            var data = Service.EMCS.SvcCargo.GetConsignee(crit);
            return Json(new { data }, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetNotifyPartyList(Domain.MasterSearchForm crit)
        {
            var data = Service.EMCS.SvcCargo.GetNotifyParty(crit);
            return Json(new { data }, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetOptionItem(Domain.MasterSearchForm crit)
        {
            var data = Service.EMCS.MasterParameter.GetSelectOption(crit);
            return Json(new { data }, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetParamItems(Domain.MasterSearchForm crit)
        {
            var data = Service.EMCS.MasterParameter.GetParamOptions(crit);
            return Json(new { data }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetSelectItem()
        {
            var consignee = Service.EMCS.SvcCargo.GetCiplColumnList("ConsigneeName");
            var notify = Service.EMCS.SvcCargo.GetCiplColumnList("NotifyName");
            var exporttype = Service.EMCS.MasterParameter.GetSelectList("ExportType");
            var category = Service.EMCS.MasterParameter.GetSelectList("Category");
            var incoterms = Service.EMCS.MasterIncoterms.GetSelectList();

            return Json(new { consignee, notify, exporttype, category, incoterms }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetCargoData(long cargoId)
        {
            try
            {
                var header = Service.EMCS.SvcCargo.GetCargoById(cargoId);
                var detail = Service.EMCS.SvcCargo.GetCargoDetailData(cargoId);
                var ciplno = Service.EMCS.SvcCargo.GetCiplNoByCargo(cargoId);
                var cipllist = Service.EMCS.SvcCargo.GetCargoReferenceById(cargoId);

                return Json(new { header, detail, ciplno, cipllist }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception err)
            {
                Console.WriteLine(err.Message);
                throw;
            }
        }

        [HttpPost]
        public ActionResult GetCiplNoList(CargoFormModel cargoData)
        {
            var data = Service.EMCS.SvcCargo.GetCiplNoList(cargoData.Data.Consignee, cargoData.Data.NotifyParty, cargoData.Data.ExportType, cargoData.Data.Category, cargoData.Data.Incoterms);
            return Json(data, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetCipLforCargo(string ciplno, long cargoid)
        {
            List<long> ciplnoList = new List<long>();
            if (ciplno != String.Empty)
            {
                List<string> ciplnoStr = ciplno.Split(',').ToList();
                foreach (var str in ciplnoStr)
                {
                    var id = Convert.ToInt64(str);
                    ciplnoList.Add(id);
                }
            }

            var data = Service.EMCS.SvcCargo.GetCiplForCargo(ciplnoList, cargoid);

            return Json(data, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult InsertCargo(Data.Domain.EMCS.CargoFormData item)
        {
            try
            {                
                long id = Service.EMCS.SvcCargo.CrudSp(item, "I");
                var cargoData = Service.EMCS.SvcCargo.GetCargoById(id);
                var ss = Service.EMCS.SvcCargo.CiplItemAvailable(id);
                return JsonCRUDMessage("I", new { cargoData });
            }
            catch (Exception ex)
            {
                return JsonMessage("Error", 1, ex);
            }
        }

        [HttpPost]
        public JsonResult InsertCargoAtBottom(Data.Domain.EMCS.CargoFormData item)
        {
            try
            {
                if (Service.EMCS.SvcCargo.CiplItemAvailable(item.Id) || item.Status == "Draft")
                {
                    long id = Service.EMCS.SvcCargo.CrudSp(item, "I");
                    var cargoData = Service.EMCS.SvcCargo.GetCargoById(id);
                    return JsonCRUDMessage("I", new { cargoData });
                }
                else
                {
                    return Json(new { success = false, responseText = "Cargo Item is not complete !" });
                }
            }
            catch (Exception ex)
            {
                return JsonMessage("Error", 1, ex);
            }
        }

        [HttpPost]
        public long InsertCargoItem(List<Data.Domain.EMCS.CargoItem> data, long id)
        {
            Service.EMCS.SvcCargo.InsertCargolItem(data, id);
            return 1;
        }

        [AuthorizeAcces(ActionType = AuthorizeAcces.IsDeleted, UrlMenu = "CargoList")]
        [HttpPost]
        public long RemoveCargo(long cargoid)
        {
            long id = Service.EMCS.SvcCargo.RemoveCargo(cargoid);
            return id;
        }

        [HttpPost]
        public JsonResult RemoveCargoItem(long id)
        {
            var detail = Service.EMCS.SvcCargoItem.GetDataById(id);
            try
            {
                Service.EMCS.SvcCargoItem.Remove(id);
                return JsonCRUDMessage("D", detail);
            }
            catch (Exception err)
            {
                return JsonMessage(err.Message, 1, detail);
            }
        }

        public JsonResult GetCargoListItem(Data.Domain.EMCS.GridListFilter filter)
        {
            var data = Service.EMCS.SvcCargo.GetCargoListItem(filter);
            return Json(data, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetCargoListItemApproval(Data.Domain.EMCS.GridListFilter filter)
        {
            var data = Service.EMCS.SvcCargo.GetCargoListItemApproval(filter);
            return Json(data, JsonRequestBehavior.AllowGet);
        }
        public JsonResult GetCargoHistory(Data.Domain.EMCS.GridListFilter filter)
        {
            var data = Service.EMCS.SvcCargo.CargoHistoryGetById(filter.Id);
            return Json(data, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetProblemDataList(Data.Domain.EMCS.GridListFilter filter)
        {
            var dataFilter = new Data.Domain.EMCS.GridListFilter();
            var data = Service.EMCS.SvcProblemHistory.GetListCargoSp(dataFilter);
            return Json(data, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult CargoApprove(Data.Domain.EMCS.CargoFormData form)
        {
            try
            {
                Service.EMCS.SvcCargo.ApprovalRequest(form, "U");
                var data = InitCargo(form.Id);
                return JsonCRUDMessage("U", data);
            }
            catch (Exception err)
            {
                Console.WriteLine(err.Message);
                throw;
            }
        }

        public ActionResult ReportCl(long clId, string reportType)
        {
            string fileExcel = Server.MapPath("~\\Content\\EMCS\\Templates\\TemplateCL.xls");
            string fileName = "CLDocument_" + DateTime.Now.ToString("yyyyMMddHHmmss");
            string filePath = Server.MapPath("~\\Content\\EMCS\\Templates\\" + fileName);
            MemoryStream output = Service.EMCS.DocumentStreamGenerator.GetStream(clId, fileExcel, filePath, reportType);
            return File(output.ToArray(), "application/pdf", "CLDocument_" + DateTime.Now.ToString("ddMMyyyyhhhmmss") + ".pdf");    //Suggested file name in the "Save as" dialog which will be displayed to the end user
        }
    }
}