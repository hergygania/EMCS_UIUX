using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using App.Web.App_Start;
using App.Web.Models.EMCS;
using System.IO;
using System.Text.RegularExpressions;
using App.Data.Domain.EMCS;
using App.Web.Models;
using App.Data.Domain;
using System.ComponentModel;

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
<<<<<<< HEAD
            ViewBag.IsOwned = true;
=======
>>>>>>> 639d8d0 (Intial commit)
            ViewBag.AllowRead = AuthorizeAcces.AllowRead;
            ViewBag.AllowCreate = AuthorizeAcces.AllowCreated;
            ViewBag.AllowUpdate = AuthorizeAcces.AllowUpdated;
            ViewBag.AllowDelete = AuthorizeAcces.AllowDeleted;
<<<<<<< HEAD
            ViewBag.IsImexUser = false;
            ViewBag.CanRequestForChange = false;
            ViewBag.IsApprover = false;
=======
>>>>>>> 639d8d0 (Intial commit)
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

        public ActionResult UpdateCargo(long cargoId, bool rfc = false)
        {
            var userId = User.Identity.GetUserId();
            ApplicationTitle();
            ViewBag.CargoID = cargoId;
            string userRoles = User.Identity.GetUserRoles();
            HttpContext.Session["Cargoid"] = cargoId;

        public ActionResult UpdateCargo(long cargoId)
        {

            ApplicationTitle();
            ViewBag.CargoID = cargoId;
            string userRoles = User.Identity.GetUserRoles();
            HttpContext.Session["Cargoid"] = cargoId;
            ViewBag.AllowRead = AuthorizeAcces.AllowRead;
            ViewBag.AllowCreate = AuthorizeAcces.AllowCreated;
            ViewBag.AllowUpdate = AuthorizeAcces.AllowUpdated;
            ViewBag.AllowDelete = AuthorizeAcces.AllowDeleted;
            ViewBag.IsApprover = false;
            ViewBag.CanRequestForChange = false;
            if (Service.EMCS.SvcCargo.CargoHisOwned(cargoId, userId))
			ViewBag.IsOwned = true;
            else
            ViewBag.IsOwned = false;

            if (Request.UrlReferrer != null)
            {
                if (Request.UrlReferrer.ToString().Contains("EMCS/CargoList"))
                    ViewBag.IsApprover = false;
                else if (!Service.EMCS.SvcCargo.CargoHisOwned(cargoId, userId) && (userRoles.Contains("EMCSImex") || userRoles.Contains("Administrator") || userRoles.Contains("Imex")))
                    ViewBag.IsApprover = true;
            }
            if (userRoles.Contains("EMCSImex") || userRoles.Contains("Administrator") || userRoles.Contains("Imex"))
                ViewBag.IsImexUser = true;
            else
                ViewBag.IsImexUser = false;
<<<<<<< HEAD

            if (Service.EMCS.SvcCipl.CheckRequestExists(Convert.ToInt32(cargoId), "CL") > 0)
                ViewBag.CanRequestForChange = false;
            else if(rfc)
                ViewBag.CanRequestForChange = true;
=======
>>>>>>> 639d8d0 (Intial commit)
=======
            if (Service.EMCS.SvcCipl.CheckRequestExists(Convert.ToInt32(cargoId), "CL") > 0)
                ViewBag.CanRequestForChange = false;
            else
                ViewBag.CanRequestForChange = true;
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
            HttpContext.Session["Cargoid"] = filter.Cargoid;
            GetCargoDocumentList(filter);
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
                //ViewBag.IdCipl = filter.IdCipl;

                GridListFilter filter = new GridListFilter();
                filter.Cargoid = id;
                GetCargoDocumentList(filter);

<<<<<<< HEAD
=======
>>>>>>> 639d8d0 (Intial commit)

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
<<<<<<< HEAD
<<<<<<< HEAD
            {

                var Id = Service.EMCS.SvcCargo.CrudSp(item, "I");
                if (item.Id == 0)
                    item.Id = Id;
                var cargoData = Service.EMCS.SvcCargo.GetCargoById(item.Id);
                var ss = Service.EMCS.SvcCargo.CiplItemAvailable(item.Id);
=======
            {                
=======
            {

>>>>>>> d3e2e7a (Tasks from P1-CIPL , P1-CL , P!-SS , P!-SI , P1-BL/AWB & P1-PEB_NPE)
                long id = Service.EMCS.SvcCargo.CrudSp(item, "I");
                CargoItem a = new CargoItem();
                a.IdCargo = id;
                CheckCNNo(a);
                var cargoData = Service.EMCS.SvcCargo.GetCargoById(id);
                var ss = Service.EMCS.SvcCargo.CiplItemAvailable(id);
>>>>>>> 639d8d0 (Intial commit)
                return JsonCRUDMessage("I", new { cargoData });
            }
            catch (Exception ex)
            {
                return JsonMessage("Error", 1, ex);
            }
        }
<<<<<<< HEAD
<<<<<<< HEAD
=======
>>>>>>> d3e2e7a (Tasks from P1-CIPL , P1-CL , P!-SS , P!-SI , P1-BL/AWB & P1-PEB_NPE)
        [HttpPost]
        public ActionResult SaveChangeHistoryCL(RequestForChangeModel form, Data.Domain.EMCS.CargoFormData item)
        {
            var requestForChange = new RequestForChange();
            var model = Service.EMCS.SvcCargo.GetCargoFormDataById(item.Id);
            var spCargoDetail = Service.EMCS.SvcCargo.GetCargoById(item.Id);
            requestForChange.FormNo = spCargoDetail.ClNo;
            requestForChange.FormType = form.FormType;
            requestForChange.Status = form.Status;
            requestForChange.FormId = form.FormId;
            requestForChange.Reason = form.Reason;

            var id = Service.EMCS.SvcCipl.InsertRequestChangeHistory(requestForChange);

            var listRfcItems = new List<Data.Domain.RFCItem>();
<<<<<<< HEAD
            string[] _ignoreParameters = { "Id", "CiNo", "Consignee", "NotifyParty", "ExportType", "Category", "Incoterms", "ShippingMethod", "Status", "CreateDate" };
=======
            string[] _ignoreParameters = { "Id", "CiNo", "Consignee", "NotifyParty", "ExportType", "Category", "Incoterms", "ShippingMethod","Status", "CreateDate" };
>>>>>>> d3e2e7a (Tasks from P1-CIPL , P1-CL , P!-SS , P!-SI , P1-BL/AWB & P1-PEB_NPE)
            var properties = TypeDescriptor.GetProperties(typeof(Data.Domain.EMCS.CargoFormData));
            foreach (PropertyDescriptor property in properties)
            {
                if (!_ignoreParameters.Contains(property.Name))
                {
                    var currentValue = property.GetValue(item);
                    if (currentValue != null && property.GetValue(model) != null)
                    {
                        if (currentValue.ToString() != property.GetValue(model).ToString())
                        {
                            var rfcItem = new Data.Domain.RFCItem();

                            rfcItem.RFCID = id;
<<<<<<< HEAD
                            rfcItem.TableName = "Cargo";
=======
                            rfcItem.TableName = "CL";
>>>>>>> d3e2e7a (Tasks from P1-CIPL , P1-CL , P!-SS , P!-SI , P1-BL/AWB & P1-PEB_NPE)
                            rfcItem.LableName = property.Name;
                            rfcItem.FieldName = property.Name;
                            rfcItem.BeforeValue = property.GetValue(model).ToString();
                            rfcItem.AfterValue = currentValue.ToString();
                            listRfcItems.Add(rfcItem);
                        }
                    }
                }
            }

            Service.EMCS.SvcCipl.InsertRFCItem(listRfcItems);

            return Json("");
        }
        [HttpPost]
        public JsonResult ApproveChangeHistoryCl(string idTerm, string formId, string formtype)
        {
            try
            {
<<<<<<< HEAD
                var data = Service.EMCS.SvcCipl.GetRequestForChangeDataList(idTerm);
                if (formtype == "Cargo") {

                    var cargo = Service.EMCS.SvcCargo.GetCargoFormDataById(Convert.ToInt32(formId));

                    var properties = TypeDescriptor.GetProperties(typeof(Data.Domain.EMCS.CargoFormData));

                    string[] _ignoreParameters = { "Id", "CiNo", "Consignee", "NotifyParty", "ExportType", "Category", "Incoterms", "ShippingMethod", "Status", "CreateDate" };

                    foreach (PropertyDescriptor property in properties)
                    {
                        if (!_ignoreParameters.Contains(property.Name))
                        {
                            var historyProp = data.Where(x => x.FieldName == property.Name).FirstOrDefault();
                            if (historyProp != null)
                            {
                                System.TypeCode typeCode = System.Type.GetTypeCode(property.PropertyType);
                                switch (typeCode)
                                {
                                    case TypeCode.Boolean:
                                        property.SetValue(cargo, Convert.ToBoolean(historyProp.AfterValue));
                                        break;
                                    case TypeCode.String:
                                        property.SetValue(cargo, Convert.ToString(historyProp.AfterValue));
                                        break;
                                    case TypeCode.Char:
                                        property.SetValue(cargo, Convert.ToChar(historyProp.AfterValue));
                                        break;
                                    case TypeCode.Double:
                                        property.SetValue(cargo, Convert.ToDouble(historyProp.AfterValue));
                                        break;
                                    case TypeCode.Single:
                                        property.SetValue(cargo, Convert.ToSingle(historyProp.AfterValue));
                                        break;
                                    case TypeCode.Int32:
                                        property.SetValue(cargo, Convert.ToInt32(historyProp.AfterValue));
                                        break;
                                    case TypeCode.Int16:
                                        property.SetValue(cargo, Convert.ToInt16(historyProp.AfterValue));
                                        break;
                                    case TypeCode.DateTime:
                                        property.SetValue(cargo, Convert.ToDateTime(historyProp.AfterValue));
                                        break;
                                    case TypeCode.Decimal:
                                        property.SetValue(cargo, Convert.ToDecimal(historyProp.AfterValue));
                                        break;
                                    case TypeCode.Object:
                                        //property.SetValue(cipl, Convert.toobj(historyProp.AfterValue));
                                        break;
                                    default:
                                        property.SetValue(cargo, historyProp.AfterValue);
                                        break;
                                }

                            }
                        }
                    }

                    Service.EMCS.SvcCargo.UpdateCargoByApprover(cargo, "I");
                }
                else if (formtype == "NpePeb") {

                    var model = new PebNpeModel();
                    model.NpePeb = Service.EMCS.SvcNpePeb.GetById(Convert.ToInt32(formId));

                    string[] _ignnoreParametersNpePeb = { "Id", "IdCl", "CreateDate" };
                    var properties = TypeDescriptor.GetProperties(typeof(Data.Domain.EMCS.NpePeb));
                    foreach (PropertyDescriptor propertyNpePeb in properties)
                    {
                        if (!_ignnoreParametersNpePeb.Contains(propertyNpePeb.Name))
                        {
                            var historyProp = data.Where(x => x.FieldName == propertyNpePeb.Name).FirstOrDefault();
                            if (historyProp != null)
                            {
                                System.TypeCode typeCode = System.Type.GetTypeCode(propertyNpePeb.PropertyType);
                                switch (typeCode)
                                {
                                    case TypeCode.Boolean:
                                        propertyNpePeb.SetValue(model.NpePeb, Convert.ToBoolean(historyProp.AfterValue));
                                        break;
                                    case TypeCode.String:
                                        propertyNpePeb.SetValue(model.NpePeb, Convert.ToString(historyProp.AfterValue));
                                        break;
                                    case TypeCode.Char:
                                        propertyNpePeb.SetValue(model.NpePeb, Convert.ToChar(historyProp.AfterValue));
                                        break;
                                    case TypeCode.Double:
                                        propertyNpePeb.SetValue(model.NpePeb, Convert.ToDouble(historyProp.AfterValue));
                                        break;
                                    case TypeCode.Single:
                                        propertyNpePeb.SetValue(model.NpePeb, Convert.ToSingle(historyProp.AfterValue));
                                        break;
                                    case TypeCode.Int32:
                                        propertyNpePeb.SetValue(model.NpePeb, Convert.ToInt32(historyProp.AfterValue));
                                        break;
                                    case TypeCode.Int16:
                                        propertyNpePeb.SetValue(model.NpePeb, Convert.ToInt16(historyProp.AfterValue));
                                        break;
                                    case TypeCode.DateTime:
                                        propertyNpePeb.SetValue(model.NpePeb, Convert.ToDateTime(historyProp.AfterValue));
                                        break;
                                    case TypeCode.Decimal:
                                        propertyNpePeb.SetValue(model.NpePeb, Convert.ToDecimal(historyProp.AfterValue));
                                        break;
                                    case TypeCode.Object:
                                        //property.SetValue(cipl, Convert.toobj(historyProp.AfterValue));
                                        break;
                                    default:
                                        propertyNpePeb.SetValue(model.NpePeb, historyProp.AfterValue);
                                        break;
                                }
                            }
                        }
                    }
                   Service.EMCS.SvcNpePeb.UpdateNpePeb(model.NpePeb);
                }
                else if (formtype == "BlAwb")
                {
                    var model = new BlAwbModel();
                    model.BlAwb = Service.EMCS.SvcBlAwb.GetByIdcl(Convert.ToInt32(formId));

                    string[] _ignnoreParametersBlAwb = { "Id", "IdCl", "CreateDate" };

                    var properties = TypeDescriptor.GetProperties(typeof(Data.Domain.EMCS.BlAwb));
                    foreach (PropertyDescriptor propertyNpePeb in properties)
                    {
                        if (!_ignnoreParametersBlAwb.Contains(propertyNpePeb.Name))
                        {
                            var historyProp = data.Where(x => x.FieldName == propertyNpePeb.Name).FirstOrDefault();
                            if (historyProp != null)
                            {
                                System.TypeCode typeCode = System.Type.GetTypeCode(propertyNpePeb.PropertyType);
                                switch (typeCode)
                                {
                                    case TypeCode.Boolean:
                                        propertyNpePeb.SetValue(model.BlAwb, Convert.ToBoolean(historyProp.AfterValue));
                                        break;
                                    case TypeCode.String:
                                        propertyNpePeb.SetValue(model.BlAwb, Convert.ToString(historyProp.AfterValue));
                                        break;
                                    case TypeCode.Char:
                                        propertyNpePeb.SetValue(model.BlAwb, Convert.ToChar(historyProp.AfterValue));
                                        break;
                                    case TypeCode.Double:
                                        propertyNpePeb.SetValue(model.BlAwb, Convert.ToDouble(historyProp.AfterValue));
                                        break;
                                    case TypeCode.Single:
                                        propertyNpePeb.SetValue(model.BlAwb, Convert.ToSingle(historyProp.AfterValue));
                                        break;
                                    case TypeCode.Int32:
                                        propertyNpePeb.SetValue(model.BlAwb, Convert.ToInt32(historyProp.AfterValue));
                                        break;
                                    case TypeCode.Int16:
                                        propertyNpePeb.SetValue(model.BlAwb, Convert.ToInt16(historyProp.AfterValue));
                                        break;
                                    case TypeCode.DateTime:
                                        propertyNpePeb.SetValue(model.BlAwb, Convert.ToDateTime(historyProp.AfterValue));
                                        break;
                                    case TypeCode.Decimal:
                                        propertyNpePeb.SetValue(model.BlAwb, Convert.ToDecimal(historyProp.AfterValue));
                                        break;
                                    case TypeCode.Object:
                                        //property.SetValue(cipl, Convert.toobj(historyProp.AfterValue));
                                        break;
                                    default:
                                        propertyNpePeb.SetValue(model.BlAwb, historyProp.AfterValue);
                                        break;
                                }
                            }
                        }
                    }
                    Service.EMCS.SvcBlAwb.UpdateBlAwb(model.BlAwb);
                }
                Service.EMCS.SvcCipl.ApproveRequestForChangeHistory(Convert.ToInt32(idTerm));

                return Json("", JsonRequestBehavior.AllowGet);
=======


                var data = Service.EMCS.SvcCipl.GetRequestForChangeDataList(idTerm);
                var cargo = Service.EMCS.SvcCargo.GetCargoFormDataById(Convert.ToInt32(formId));

                var cargoHistory = data.Where(x => x.TableName == typeof(Data.Domain.EMCS.CargoFormData).Name).ToList();

                var properties = TypeDescriptor.GetProperties(typeof(Data.Domain.EMCS.CargoFormData));

                string[] _ignoreParameters = { "Id", "CiNo", "Consignee", "NotifyParty", "ExportType", "Category", "Incoterms", "ShippingMethod", "Status", "CreateDate" };

                foreach (PropertyDescriptor property in properties)
                {
                    if (!_ignoreParameters.Contains(property.Name))
                    {
                        var historyProp = cargoHistory.Where(x => x.FieldName == property.Name).FirstOrDefault();
                        if (historyProp != null)
                        {
                            System.TypeCode typeCode = System.Type.GetTypeCode(property.PropertyType);
                            switch (typeCode)
                            {
                                case TypeCode.Boolean:
                                    property.SetValue(cargo, Convert.ToBoolean(historyProp.AfterValue));
                                    break;
                                case TypeCode.String:
                                    property.SetValue(cargo, Convert.ToString(historyProp.AfterValue));
                                    break;
                                case TypeCode.Char:
                                    property.SetValue(cargo, Convert.ToChar(historyProp.AfterValue));
                                    break;
                                case TypeCode.Double:
                                    property.SetValue(cargo, Convert.ToDouble(historyProp.AfterValue));
                                    break;
                                case TypeCode.Single:
                                    property.SetValue(cargo, Convert.ToSingle(historyProp.AfterValue));
                                    break;
                                case TypeCode.Int32:
                                    property.SetValue(cargo, Convert.ToInt32(historyProp.AfterValue));
                                    break;
                                case TypeCode.Int16:
                                    property.SetValue(cargo, Convert.ToInt16(historyProp.AfterValue));
                                    break;
                                case TypeCode.DateTime:
                                    property.SetValue(cargo, Convert.ToDateTime(historyProp.AfterValue));
                                    break;
                                case TypeCode.Decimal:
                                    property.SetValue(cargo, Convert.ToDecimal(historyProp.AfterValue));
                                    break;
                                case TypeCode.Object:
                                    //property.SetValue(cipl, Convert.toobj(historyProp.AfterValue));
                                    break;
                                default:
                                    property.SetValue(cargo, historyProp.AfterValue);
                                    break;
                            }
                            //Convert.ChangeType(historyProp.AfterValue, cipl.GetType());

                        }
                    }
                }

                Service.EMCS.SvcCipl.ApproveRequestForChangeHistory(Convert.ToInt32(idTerm));
                Service.EMCS.SvcCargo.CrudSp(cargo, "I");
                var userId = User.Identity.GetUserId();
                
                return Json(data, JsonRequestBehavior.AllowGet);
>>>>>>> d3e2e7a (Tasks from P1-CIPL , P1-CL , P!-SS , P!-SI , P1-BL/AWB & P1-PEB_NPE)
            }
            catch (Exception ex)
            {

                _errorHelper.Error(ex.ToString());
                throw ex;
            }
        }
<<<<<<< HEAD
=======

>>>>>>> 639d8d0 (Intial commit)
=======
>>>>>>> d3e2e7a (Tasks from P1-CIPL , P1-CL , P!-SS , P!-SI , P1-BL/AWB & P1-PEB_NPE)
        [HttpPost]
        public JsonResult InsertCargoAtBottom(Data.Domain.EMCS.CargoFormData item)
        {
            try
            {
<<<<<<< HEAD
<<<<<<< HEAD
                var userId = User.Identity.GetUserId();
                //if (Service.EMCS.SvcCargo.CargoHisOwned(item.Id, userId) || User.Identity.GetUserRoles().Contains("EMCSImex"))
                //{
                    long id = Service.EMCS.SvcCargo.CrudSp(item, "I");
                    var cargoData = Service.EMCS.SvcCargo.GetCargoById(id);
                    return JsonCRUDMessage("I", new { cargoData });
                //}
                //else
                //{
                //    return Json(new { success = false, responseText = "Cargo Item is not complete !" });
                //}
=======
                if (Service.EMCS.SvcCargo.CiplItemAvailable(item.Id) || item.Status == "Draft")
=======
                var userId = User.Identity.GetUserId();
                if (Service.EMCS.SvcCargo.CargoHisOwned(item.Id, userId) || User.Identity.GetUserRoles().Contains("EMCSImex"))
>>>>>>> d3e2e7a (Tasks from P1-CIPL , P1-CL , P!-SS , P!-SI , P1-BL/AWB & P1-PEB_NPE)
                {
                    long id = Service.EMCS.SvcCargo.CrudSp(item, "I");
                    var cargoData = Service.EMCS.SvcCargo.GetCargoById(id);
                    return JsonCRUDMessage("I", new { cargoData });
                }
                else
                {
                    return Json(new { success = false, responseText = "Cargo Item is not complete !" });
                }
>>>>>>> 639d8d0 (Intial commit)
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

<<<<<<< HEAD
        [HttpPost]
        public JsonResult CargoSaveAndApprove(RequestForChangeModel form, Data.Domain.EMCS.CargoFormData cargoform, Data.Domain.EMCS.CargoFormData approvalform)
        {
            try
            {
                var requestForChange = new RequestForChange();
                var model = Service.EMCS.SvcCargo.GetCargoFormDataById(cargoform.Id);
                var spCargoDetail = Service.EMCS.SvcCargo.GetCargoById(cargoform.Id);
                requestForChange.FormNo = spCargoDetail.ClNo;
                requestForChange.FormType = form.FormType;
                requestForChange.Status = 1;
                requestForChange.FormId = form.FormId;
                requestForChange.Reason = form.Reason;

                var id = Service.EMCS.SvcCipl.InsertChangeHistory(requestForChange);

                var listRfcItems = new List<Data.Domain.RFCItem>();
                string[] _ignoreParameters = { "Id", "CiNo", "Consignee", "NotifyParty", "ExportType", "Category", "Incoterms", "ShippingMethod", "Status", "CreateDate" };
                var properties = TypeDescriptor.GetProperties(typeof(Data.Domain.EMCS.CargoFormData));
                foreach (PropertyDescriptor property in properties)
                {
                    if (!_ignoreParameters.Contains(property.Name))
                    {
                        var currentValue = property.GetValue(cargoform);
                        if (currentValue != null && property.GetValue(model) != null)
                        {
                            if (currentValue.ToString() != property.GetValue(model).ToString())
                            {
                                var rfcItem = new Data.Domain.RFCItem();

                                rfcItem.RFCID = id;
                                rfcItem.TableName = "Cargo";
                                rfcItem.LableName = property.Name;
                                rfcItem.FieldName = property.Name;
                                rfcItem.BeforeValue = property.GetValue(model).ToString();
                                rfcItem.AfterValue = currentValue.ToString();
                                listRfcItems.Add(rfcItem);
                            }
                        }
                    }
                }

                Service.EMCS.SvcCipl.InsertRFCItem(listRfcItems);
                Service.EMCS.SvcCargo.UpdateCargoByApprover(cargoform, "I");
                Service.EMCS.SvcCargo.ApprovalRequest(approvalform, "U");

                var data = InitCargo(cargoform.Id);
                return JsonCRUDMessage("U", data);
            }
            catch (Exception err)
            {
                Console.WriteLine(err.Message);
                throw;
            }
        }


=======
>>>>>>> 639d8d0 (Intial commit)
        public ActionResult ReportCl(long clId, string reportType)
        {
            string fileExcel = Server.MapPath("~\\Content\\EMCS\\Templates\\TemplateCL.xls");
            string fileName = "CLDocument_" + DateTime.Now.ToString("yyyyMMddHHmmss");
            string filePath = Server.MapPath("~\\Content\\EMCS\\Templates\\" + fileName);
            MemoryStream output = Service.EMCS.DocumentStreamGenerator.GetStream(clId, fileExcel, filePath, reportType);
            return File(output.ToArray(), "application/pdf", "CLDocument_" + DateTime.Now.ToString("ddMMyyyyhhhmmss") + ".pdf");    //Suggested file name in the "Save as" dialog which will be displayed to the end user
        }
<<<<<<< HEAD
<<<<<<< HEAD
=======
>>>>>>> d3e2e7a (Tasks from P1-CIPL , P1-CL , P!-SS , P!-SI , P1-BL/AWB & P1-PEB_NPE)
        [HttpPost]
        public bool CargoDocumentInsert(List<Data.Domain.EMCS.CargoDocument> data)
        {

            var id = Service.EMCS.SvcCargo.InsertCargoDocument(data);
            return id;
        }
        public string GetdocFileName(System.Web.HttpPostedFileBase file, long id)
        {
            var fileName = Path.GetFileName(file.FileName);

            if (fileName != null)
            {
                var ext = Path.GetExtension(fileName);
                var path = Server.MapPath("~/Upload/EMCS/Cargo/" + id);
                bool isExists = Directory.Exists(path);
                fileName = "DocCargo-" + id.ToString() + ext;

                if (!isExists)
                    Directory.CreateDirectory(path);

                var fullPath = Path.Combine(path, fileName);

                if (System.IO.File.Exists(fullPath))
                {
                    System.IO.File.Delete(fullPath);
                }

                file.SaveAs(fullPath);

            }

            return fileName;
        }
        public string UploadDocumentCargo(long id)
        {
            string fileName = "";

            if (Request.Files.Count > 0)
            {
                var file = Request.Files[0];

                if (file != null && file.ContentLength > 0)
                {
                    return GetdocFileName(file, id);
                }
            }
            return fileName;
        }
        [HttpPost]
        public ActionResult CargoDocumentUpload(long id)
        {
            string fileResult = UploadDocumentCargo(id);
            if (fileResult != "")
            {
                var result = Service.EMCS.SvcCargo.UpdateFileCargoDocument(id, fileResult);
                return Json(new { status = true, msg = "Upload File Successfully" });
            }
            else
            {
                return Json(new { status = false, msg = "Upload File gagal" });
            }
        }


        [HttpPost]
        public long CargoDocumentDeleteById(long id)
        {
            id = Service.EMCS.SvcCargo.DeleteCargoDocumentById(id);
            return id;
        }
        public FileResult DownloadCargoDocument(long id)
        {
            //var list = Service.EMCS.SvcCipl.CiplHistoryGetById(id);
            var files = Service.EMCS.SvcCargo.CargoDocumentListById(id).FirstOrDefault();
            string fullPath = Request.MapPath("~/Upload/EMCS/Dummy/NotFound.txt");

            if (files != null)
            {
                //var fileData = files;
                fullPath = Request.MapPath("~/Upload/EMCS/Cargo/" + files.Id + "/" + files.Filename);
                var fileBytes = System.IO.File.ReadAllBytes(fullPath);
                string fileName = files.Filename;
                return File(fileBytes, System.Net.Mime.MediaTypeNames.Application.Octet, fileName);
            }

            return File(fullPath, "text/plain", "NotFound.txt");
        }
        [HttpPost]
        public JsonResult CheckCNNo(CargoItem cargoItem)
        {

            try
            {
                if (cargoItem.ContainerSealNumber == null)
                {
                    string gettype = "";
                    List<CargoItem> data = new List<CargoItem>();
                    
                    if (cargoItem.ContainerNumber == null)
                    {
                        cargoItem.ContainerNumber = "";
                        data = Service.EMCS.SvcCargo.SearchContainNumber(cargoItem);
                    }
                    else
                    {
                        data = Service.EMCS.SvcCargo.SearchContainNumber(cargoItem);
                    }
                    
                    //if (data.Count == 0)
                    //{
                    //    //List<CargoItem> listcargo = new List<CargoItem>();
                    //    //for (int i = 0; i == listcargo.Count; i++)
                    //    //{
                    //        data[0].ContainerType = "";
                    //    //}
                    //}
                        foreach (var i in data)
                    {
                        gettype = i.ContainerType;
                        
                    }
                
                    ContainerFormModel a = new ContainerFormModel();
                    App.Data.Domain.EMCS.MasterParameter m = new App.Data.Domain.EMCS.MasterParameter();
                    if (data.Count > 0)
                    {
                        m = Service.EMCS.SvcCargo.GetCargoType(gettype);
                        if (m.Description != null)
                        {
                            data[0].ContainerType = m.Description;
                        }
                    }
                    //for(int i=0;i == data.Count; i++)
                    //{   
                        
                    //    data = null;
                       
                    //}

                    return Json(data);
                }
                else
                {
                    string gettype = "";
                    var data = Service.EMCS.SvcCargo.SearchContainNumber(cargoItem);
                    foreach (var i in data)
                    {
                        gettype = i.ContainerType;
                    }
                    return Json(gettype);
                }

            }
            catch (Exception ex)
            {
                throw ex;
            }

        }
<<<<<<< HEAD
        [HttpPost]
        public JsonResult CheckCNNoAndGetId(CargoItem cargoItem)
        {

            try
            {
                if (cargoItem.ContainerSealNumber == null)
                {
                    //string gettype = "";
                    List<CargoItem> data = new List<CargoItem>();

                    if (cargoItem.ContainerNumber == null)
                    {
                        cargoItem.ContainerNumber = "";
                        data = Service.EMCS.SvcCargo.SearchContainNumber(cargoItem);
                    }
                    else
                    {
                        data = Service.EMCS.SvcCargo.SearchContainNumber(cargoItem);
                    }

                    //if (data.Count == 0)
                    //{
                    //    //List<CargoItem> listcargo = new List<CargoItem>();
                    //    //for (int i = 0; i == listcargo.Count; i++)
                    //    //{
                    //        data[0].ContainerType = "";
                    //    //}
                    //}
                    //foreach (var i in data)
                    //{
                    //    gettype = i.ContainerType;
                        

                    //}
                    //data[0].ContainerType = gettype;

                    //ContainerFormModel a = new ContainerFormModel();
                    //App.Data.Domain.EMCS.MasterParameter m = new App.Data.Domain.EMCS.MasterParameter();
                    //if (data.Count > 0)
                    //{
                    //    m = Service.EMCS.SvcCargo.GetCargoType(gettype);
                    //    if (m.Description != null)
                    //    {
                    //        data[0].ContainerType = m.Description;
                    //    }
                    //}
                    //for(int i=0;i == data.Count; i++)
                    //{   

                    //    data = null;

                    //}

                    return Json(data);
                }
                else
                {
                    string gettype = "";
                    var data = Service.EMCS.SvcCargo.SearchContainNumber(cargoItem);
                    foreach (var i in data)
                    {
                        gettype = i.ContainerType;
                    }
                    return Json(gettype);
                }

            }
            catch (Exception ex)
            {
                throw ex;
            }

        }
=======
>>>>>>> d3e2e7a (Tasks from P1-CIPL , P1-CL , P!-SS , P!-SI , P1-BL/AWB & P1-PEB_NPE)
        //public JsonResult GetCargotype(int  cargotype)
        //{
        //    string data = "";
        //    App.Data.Domain.EMCS.MasterParameter m = new App.Data.Domain.EMCS.MasterParameter();
        //    try
        //    {


        //        m = Service.EMCS.SvcCargo.GetCargoType(cargotype);
        //        data = m.Description;
        //    }
        //    catch(Exception ex)
        //    {
        //         data = ex.Message;
        //    }

        //    return Json(data);




        //}
<<<<<<< HEAD
=======
>>>>>>> 639d8d0 (Intial commit)
=======
>>>>>>> d3e2e7a (Tasks from P1-CIPL , P1-CL , P!-SS , P!-SI , P1-BL/AWB & P1-PEB_NPE)
    }
}