using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using App.Domain;
using App.Web.App_Start;
using System.Web.Script.Serialization;
using App.Web.Models.EMCS;
using System.IO;
using System.Text.RegularExpressions;
using App.Web.Helper;

namespace App.Web.Controllers.EMCS
{
    public partial class EmcsController
    {


        // ================================ CIPL LAYOUT ================================
        [AuthorizeAcces(ActionType = AuthorizeAcces.IsRead)]
        public ActionResult CiplList()
        {
            ApplicationTitle();
            ViewBag.AllowRead = AuthorizeAcces.AllowRead;
            ViewBag.AllowCreate = AuthorizeAcces.AllowCreated;
            ViewBag.AllowUpdate = AuthorizeAcces.AllowUpdated;
            ViewBag.AllowDelete = AuthorizeAcces.AllowDeleted;
            PaginatorBoot.Remove("SessionTRN");
            return View();
        }

        [AuthorizeAcces(ActionType = AuthorizeAcces.IsRead, UrlMenu = "CiplList")]
        public ActionResult CiplForm()
        {
            ApplicationTitle();
            ViewBag.AllowRead = AuthorizeAcces.AllowRead;
            ViewBag.AllowCreate = AuthorizeAcces.AllowCreated;
            ViewBag.AllowUpdate = AuthorizeAcces.AllowUpdated;
            ViewBag.AllowDelete = AuthorizeAcces.AllowDeleted;
            PaginatorBoot.Remove("SessionTRN");
            return View();
        }

        [HttpGet]
        public ActionResult CiplEdit(long id)
        {
            var userId = User.Identity.GetUserId();
            string userRoles = User.Identity.GetUserRoles();
           if (Service.EMCS.SvcCipl.CiplHisOwned(id, userId) || userRoles.Contains("EMCSImex"))
            {
                ApplicationTitle();
                ViewBag.AllowRead = AuthorizeAcces.AllowRead;
                ViewBag.AllowCreate = AuthorizeAcces.AllowCreated;
                ViewBag.AllowUpdate = AuthorizeAcces.AllowUpdated;
                ViewBag.AllowDelete = AuthorizeAcces.AllowDeleted;
                PaginatorBoot.Remove("SessionTRN");
                return View();
            }
            else
            {
                return RedirectToAction("Unauthorised", "Shared");
            }


        }

        [HttpGet]
        public ActionResult CiplView(long id)
        {
            ApplicationTitle();
            string strQrCodeUrlEDI = Common.GenerateQrCode(id, "downloadedi");
            ViewBag.QrCodeUrlEDI = strQrCodeUrlEDI;
            TempData["QrCodeUrlEDI"] = strQrCodeUrlEDI;
            TempData.Peek("QrCodeUrlEDI");
            string strQrCodeUrlInvoice = Common.GenerateQrCode(id, "downloadInvoice");
            ViewBag.QrCodeUrlInvoice = strQrCodeUrlInvoice;
            TempData["QrCodeUrlInvoice"] = strQrCodeUrlInvoice;
            TempData.Peek("QrCodeUrlInvoice");
            string strQrCodeUrlPL = Common.GenerateQrCode(id, "DownloadPl");
            ViewBag.QrCodeUrlPL = strQrCodeUrlPL;
            TempData["QrCodeUrlPL"] = strQrCodeUrlPL;
            TempData.Peek("QrCodeUrlPL");
            ViewBag.AllowRead = AuthorizeAcces.AllowRead;
            ViewBag.AllowCreate = AuthorizeAcces.AllowCreated;
            ViewBag.AllowUpdate = AuthorizeAcces.AllowUpdated;
            ViewBag.AllowDelete = AuthorizeAcces.AllowDeleted;
            ViewBag.WizardData = Service.EMCS.SvcWizard.GetWizardData("cipl", id);
            ViewBag.GroupName = Service.EMCS.SvcUserLog.GetUserDetail().Group == null ? "" : Service.EMCS.SvcUserLog.GetUserDetail().Group;
            ViewBag.StepId = Service.EMCS.SvcCipl.GetCiplNextStepGetById(id);
            ViewBag.StepCargoId = Service.EMCS.SvcCipl.GetCiplCargoStepGetById(id);
            ViewBag.FlowCargoId = Service.EMCS.SvcCipl.GetCiplCargoFlowGetById(id);
            var detail = new CiplModel();
            detail.Data = Service.EMCS.SvcCipl.CiplGetById(id);
            detail.Forwarder = Service.EMCS.SvcCipl.CiplForwaderGetById(id);
            detail.DataItem = Service.EMCS.SvcCipl.CiplItemGetById(id);
            detail.TemplateHeader = Service.EMCS.DocumentStreamGenerator.GetCiplInvoicePlHeaderData(id);
            detail.TemplateDetail = Service.EMCS.DocumentStreamGenerator.GetCiplInvoicePlDetailData(id);
            detail.DataRequest = Service.EMCS.SvcRequestCipl.GetRequestById(id);

            List<string> items = new List<string>();
            foreach (var item in detail.DataItem.GroupBy(a => a.ReferenceNo))
            {
                items.Add(item.Key);
            }

            var detailTotalData = Service.EMCS.SvcTotalCipl.GetById(id);
            ViewBag.Quantity = detailTotalData.TotalPackage;
            ViewBag.Collies = detailTotalData.TotalPackage;
            ViewBag.grossWeight = detailTotalData.TotalGrossWeight;
            ViewBag.netWeight = detailTotalData.TotalNetWeight;
            ViewBag.volume = detailTotalData.TotalVolume;
            ViewBag.refs = string.Join(",", items);
            ViewBag.Currency = detail.Data.Currency;
            ViewBag.IdCust = detail.DataItem.Count != 0 ? detail.DataItem.FirstOrDefault()?.IdCustomer : "";

            return View(detail);
        }

        public ActionResult CiplApprove(long id)
        {
            ApplicationTitle();
            ViewBag.AllowRead = AuthorizeAcces.AllowRead;
            ViewBag.AllowCreate = AuthorizeAcces.AllowCreated;
            ViewBag.AllowUpdate = AuthorizeAcces.AllowUpdated;
            ViewBag.AllowDelete = AuthorizeAcces.AllowDeleted;
            ViewBag.WizardData = Service.EMCS.SvcWizard.GetWizardData("cipl", id);
            PaginatorBoot.Remove("SessionTRN");

            var detail = new CiplModel();
            detail.Data = Service.EMCS.SvcCipl.CiplGetById(id);
            detail.Forwarder = Service.EMCS.SvcCipl.CiplForwaderGetById(id);
            detail.DataItem = Service.EMCS.SvcCipl.CiplItemGetById(id);
            detail.TemplateHeader = Service.EMCS.DocumentStreamGenerator.GetCiplInvoicePlHeaderData(id);
            detail.TemplateDetail = Service.EMCS.DocumentStreamGenerator.GetCiplInvoicePlDetailData(id);
            detail.DataRequest = Service.EMCS.SvcRequestCipl.GetRequestById(id);

            List<string> items = new List<string>();
            foreach (var item in detail.DataItem.GroupBy(a => a.ReferenceNo))
            {
                items.Add(item.Key);
            }

            var detailTotalData = Service.EMCS.SvcTotalCipl.GetById(id);
            ViewBag.Quantity = detailTotalData.TotalPackage;
            ViewBag.Collies = detailTotalData.TotalPackage;
            ViewBag.grossWeight = detailTotalData.TotalGrossWeight;
            ViewBag.netWeight = detailTotalData.TotalNetWeight;
            ViewBag.volume = detailTotalData.TotalVolume;
            ViewBag.refs = string.Join(",", items);
            ViewBag.Currency = detail.Data.Currency;
            ViewBag.IdCust = detail.DataItem.Count != 0 ? detail.DataItem.FirstOrDefault()?.IdCustomer : "";

            return View(detail);
        }

        public ActionResult CiplCancel(long id)
        {
            ApplicationTitle();
            ViewBag.AllowRead = AuthorizeAcces.AllowRead;
            ViewBag.AllowCreate = AuthorizeAcces.AllowCreated;
            ViewBag.AllowUpdate = AuthorizeAcces.AllowUpdated;
            ViewBag.AllowDelete = AuthorizeAcces.AllowDeleted;
            ViewBag.WizardData = Service.EMCS.SvcWizard.GetWizardData("cipl", id);
            PaginatorBoot.Remove("SessionTRN");

            var detail = new CiplModel();
            detail.Data = Service.EMCS.SvcCipl.CiplGetById(id);
            detail.Forwarder = Service.EMCS.SvcCipl.CiplForwaderGetById(id);
            detail.DataItem = Service.EMCS.SvcCipl.CiplItemGetById(id);
            detail.TemplateHeader = Service.EMCS.DocumentStreamGenerator.GetCiplInvoicePlHeaderData(id);
            detail.TemplateDetail = Service.EMCS.DocumentStreamGenerator.GetCiplInvoicePlDetailData(id);
            detail.DataRequest = Service.EMCS.SvcRequestCipl.GetRequestById(id);

            var items = new List<string>();
            foreach (var item in detail.DataItem.GroupBy(a => a.ReferenceNo))
            {
                items.Add(item.Key);
            }

            decimal volume = 0;
            decimal netWeight = 0;
            decimal grossWeight = 0;

            foreach (var itm in detail.DataItem)
            {
                decimal subtotal;
                decimal length = itm.Length.HasValue ? itm.Length.Value : 0;
                decimal width = itm.Width.HasValue ? itm.Width.Value : 0;
                decimal height = itm.Height.HasValue ? itm.Height.Value : 0;
                subtotal = length * width * height;
                volume = +subtotal;

                var net = itm.NetWeight.HasValue ? itm.NetWeight.Value : 0;
                var gross = itm.GrossWeight.HasValue ? itm.GrossWeight.Value : 0;
                netWeight = +net;
                grossWeight = +gross;
            }

            ViewBag.Quantity = detail.DataItem.Sum(a => a.Quantity);
            ViewBag.Collies = detail.DataItem.GroupBy(a => a.CaseNumber).Count();
            ViewBag.grossWeight = grossWeight;
            ViewBag.netWeight = netWeight;
            ViewBag.volume = volume;
            ViewBag.refs = string.Join(",", items);
            ViewBag.Currency = detail.DataItem.Count != 0 ? detail.DataItem.FirstOrDefault()?.Currency : "";
            ViewBag.IdCust = detail.DataItem.Count != 0 ? detail.DataItem.FirstOrDefault()?.IdCustomer : "";

            return View(detail);
        }

        [AuthorizeAcces(ActionType = AuthorizeAcces.IsRead, UrlMenu = "CiplList")]
        public ActionResult CiplListPage()
        {
            this.PaginatorBoot.Remove("SessionTRN");
            return CiplListPageXt();
        }

        [AuthorizeAcces(ActionType = AuthorizeAcces.IsRead, UrlMenu = "CiplList")]
        public ActionResult CiplListPageXt()
        {
            Func<App.Data.Domain.EMCS.CiplListFilter, List<Data.Domain.EMCS.SpCiplList>> func = delegate (App.Data.Domain.EMCS.CiplListFilter filter)
            {
                var param = Request.Params["SearchName"];

                filter.ConsigneeName = param;
                var list = Service.EMCS.SvcCipl.CiplList(filter);
                return list.ToList();
            };

            var paging = PaginatorBoot.Manage("SessionTRN", func).Pagination.ToJsonResult();
            return Json(paging, JsonRequestBehavior.AllowGet);
        }

        public ActionResult CiplHistoryPage(long id)
        {
            this.PaginatorBoot.Remove("SessionTRN");
            return CiplHistoryPageXt(id);
        }

        public JsonResult GetProblemHistoryList(string idTerm, string search, int limit, int offset, string sort, string order)
        {
            var dataFilter = new Data.Domain.EMCS.GridListFilter();
            dataFilter.Sort = sort;
            dataFilter.Order = order;
            dataFilter.Offset = offset;
            dataFilter.Limit = limit;
            dataFilter.Term = idTerm;
            var data = Service.EMCS.SvcProblemHistory.GetListSp(dataFilter);
            return Json(data, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetHistoryList(string idTerm, string search, int limit, int offset, string sort, string order)
        {
            var dataFilter = new Data.Domain.EMCS.GridListFilter();
            dataFilter.Sort = sort;
            dataFilter.Order = order;
            dataFilter.Offset = offset;
            dataFilter.Limit = limit;
            dataFilter.Term = idTerm;
            var data = Service.EMCS.SvcCipl.GetListSp(dataFilter);
            return Json(data, JsonRequestBehavior.AllowGet);
        }

        public ActionResult CiplHistoryPageXt(long id)
        {
            Func<App.Data.Domain.EMCS.CiplListFilter, List<Data.Domain.EMCS.SpGetCiplHistory>> func = delegate (App.Data.Domain.EMCS.CiplListFilter filter)
            {
                if (filter == null) throw new ArgumentNullException(nameof(filter));
                var param = Request["params"];
                if (!string.IsNullOrEmpty(param))
                {
                    JavaScriptSerializer ser = new JavaScriptSerializer();
                    ser.Deserialize<App.Data.Domain.EMCS.CiplListFilter>(param);
                }
                var list = Service.EMCS.SvcCipl.CiplHistoryGetById(id);
                return list.ToList();
            };

            var paging = PaginatorBoot.Manage("SessionTRN", func).Pagination.ToJsonResult();
            return Json(paging, JsonRequestBehavior.AllowGet);
        }

        public ActionResult CiplDocumentsPage(long id)
        {
            this.PaginatorBoot.Remove("SessionTRN");
            return CiplDocumentsPageXt(id);
        }

        public ActionResult CiplDocumentsPageXt(long id)
        {
            Func<App.Data.Domain.EMCS.CiplListFilter, List<Data.Domain.EMCS.Documents>> func = delegate (App.Data.Domain.EMCS.CiplListFilter filter)
            {
                if (filter == null) throw new ArgumentNullException(nameof(filter));
                var param = Request["params"];
                if (!string.IsNullOrEmpty(param))
                {
                    JavaScriptSerializer ser = new JavaScriptSerializer();
                    ser.Deserialize<App.Data.Domain.EMCS.CiplListFilter>(param);
                }
                var list = Service.EMCS.SvcCipl.CiplDocumentsGetById(id);
                return list.ToList();
            };

            var paging = PaginatorBoot.Manage("SessionTRN", func).Pagination.ToJsonResult();
            return Json(paging, JsonRequestBehavior.AllowGet);
        }

        public ActionResult CiplProblemHistoryPage(long id)
        {
            this.PaginatorBoot.Remove("SessionTRN");
            return CiplProblemHistoryPageXt(id);
        }

        public ActionResult CiplProblemHistoryPageXt(long id)
        {
            Func<App.Data.Domain.EMCS.CiplListFilter, List<Data.Domain.EMCS.SpCiplProblemHistory>> func = delegate (App.Data.Domain.EMCS.CiplListFilter filter)
            {
                if (filter == null) throw new ArgumentNullException(nameof(filter));
                var param = Request["params"];
                if (!string.IsNullOrEmpty(param))
                {
                    JavaScriptSerializer ser = new JavaScriptSerializer();
                    ser.Deserialize<App.Data.Domain.EMCS.CiplListFilter>(param);
                }
                var list = Service.EMCS.SvcCipl.SP_CiplProblemHistory(id);
                return list.ToList();
            };

            var paging = PaginatorBoot.Manage("SessionTRN", func).Pagination.ToJsonResult();
            return Json(paging, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult GetSelectItemCipl(MasterSearchForm crit)
        {
            var category = Service.EMCS.MasterParameter.GetSelectList("Category");
            var categoryunit = Service.EMCS.MasterParameter.GetSelectList("CategoryUnit");
            var categoryspareparts = Service.EMCS.MasterParameter.GetSelectList("CategorySpareparts");
            var soldconsignee = Service.EMCS.MasterParameter.GetParamByGroup("SoldConsignee");
            var shipdelivery = Service.EMCS.MasterParameter.GetParamByGroup("ShipDelivery");
            var exporttype = Service.EMCS.MasterParameter.GetParamByGroup("ExportType");
            var exportremarks = Service.EMCS.MasterParameter.GetParamByGroup("ExportRemarks");
            var paymentterms = Service.EMCS.MasterParameter.GetSelectList2("PaymentTerms");
            var uomtypes = Service.EMCS.MasterParameter.GetParamByGroup("UOMType");
            var incoterms = Service.EMCS.MasterIncoterms.GetList(crit);
            var packagingtype = Service.EMCS.MasterParameter.GetSelectList("PackagingType");

            var shippingmethod = Service.EMCS.MasterParameter.GetSelectList("ShippingMethod");
            var freightpayment = Service.EMCS.MasterParameter.GetSelectList("FreightPayment");
            var forwader = Service.EMCS.MasterParameter.GetSelectList("Forwader");
            var type = Service.EMCS.SvcCipl.GetTypeSelectList();
            var country = Service.EMCS.SvcCipl.GetCountryList();
            var branch = Service.EMCS.SvcCipl.GetBranchList();
            var kurs = Service.EMCS.SvcCipl.GetLastestKurs();
            var currency = Service.EMCS.SvcCipl.GetCurrencyCipl();
            var categoryreference = Service.EMCS.SvcCipl.GetCategoryReferencet("CategoryReference");

            return Json(new { exporttype, category, categoryunit, categoryspareparts, soldconsignee, shipdelivery, incoterms, packagingtype, exportremarks, paymentterms, uomtypes, shippingmethod, freightpayment, forwader, country, branch, kurs, currency, categoryreference,type }, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetPickUpPic(string user, string area)
        {
            var data = Service.EMCS.SvcCipl.GetPickUpPic(user, area);
            return Json(data, JsonRequestBehavior.AllowGet);
        }

        public JsonResult UpdateQuantityReference(List<Data.Domain.EMCS.CiplItemInsert> item, string status)
        {
            try
            {
                var data = Service.EMCS.SvcCipl.UpdateQuantityReference(item, status);
                return JsonCRUDMessage("U", data);
            }
            catch (Exception err)
            {
                return JsonMessage(err.Message, 1, err);
            }
        }

        [HttpGet]
        public JsonResult GetDataMaster()
        {
            var country = Service.EMCS.SvcCipl.GetCountryList();
            var branch = Service.EMCS.SvcCipl.GetBranchList();
            return Json(new { country, branch }, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult GetCustomerForSelect2(MasterSearchForm crit)
        {
            List<App.Data.Domain.EMCS.MasterCustomers> list = Service.EMCS.MasterCustomer.GetList(crit);
            return Json(new { data = list.ToList() }, JsonRequestBehavior.AllowGet);

        }

        public ActionResult GetCategoryItem(int id)
        {
            string item;
            switch (id)
            {
                case 1:
                    item = "PP";
                    break;
                case 2:
                    item = "SP";
                    break;
                default:
                    item = "UE";
                    break;
            }

            var data = Service.EMCS.SvcCipl.GetCategoryItem(item);
            return Json(data, JsonRequestBehavior.AllowGet);
        }

        public ActionResult CiplGetById(long id)
        {
            try
            {
                var model = Service.EMCS.SvcCipl.CiplGetById(id);
                var forwader = Service.EMCS.SvcCipl.CiplForwaderGetById(id);
                var document = Service.EMCS.SvcCipl.CiplDocumentsGetById(id);
                var problem = Service.EMCS.SvcCipl.SP_CiplProblemHistory(id);
                return Json(new { model, forwader, document, problem }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return JsonMessage("Error", 1, ex);
            }
        }

        public JsonResult CiplItemGetById(long id)
        {
            var list = Service.EMCS.SvcCipl.CiplItemGetById(id);

            return Json(list, JsonRequestBehavior.AllowGet);
        }

        public JsonResult CiplItemGetByIdReference(long idReference)
        {
            var list = Service.EMCS.SvcCipl.CiplItemGetByIdReference(idReference);

            return Json(list, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult CiplInsert(CiplFormModel item, string dml, string status)
        {
            try
            {
                var data = Service.EMCS.SvcCipl.InsertCipl(item.Forwader, item.Data, "I", status);
                return Json(data, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return JsonMessage("Error", 1, ex);
            }
        }

        [HttpPost]
        public bool CiplItemInsert(List<Data.Domain.EMCS.CiplItemInsert> data)
        {
            var id = Service.EMCS.SvcCipl.InsertCiplItem(data);
            return id;
        }

        [HttpPost]
        public bool CiplDocumentInsert(List<Data.Domain.EMCS.CiplDocumentInsert> data)
        {
            var id = Service.EMCS.SvcCipl.InsertCiplDocument(data);
            return id;
        }

        [HttpPost]
        public ActionResult CiplDocumentUpload(long id)
        {
            string fileResult = UploadDocumentCipl(id);
            if (fileResult != "")
            {
                var result = Service.EMCS.SvcCipl.UpdateFileCiplDocument(id, fileResult);
                return Json(new { status = true, msg = "Upload File Successfully" });
            }
            else
            {
                return Json(new { status = false, msg = "Upload File gagal" });
            }
        }

        [HttpPost]
        public long CiplDocumentDeleteById(long id)
        {
            id = Service.EMCS.SvcCipl.DeleteCiplDocumentById(id);
            return id;
        }

        [HttpPost]
        public long CiplItemDeleteById(long id)
        {
            id = Service.EMCS.SvcCipl.DeleteCiplById(id, "CIPLITEMID");
            return id;
        }

        [HttpPost]
        public bool CiplItemSplitById(List<Data.Domain.EMCS.CiplItemInsert> data)
        {
            bool id = Service.EMCS.SvcCipl.InsertCiplItem(data);
            return id;
        }

        [HttpPost]
        public long CiplUpdate(CiplFormModel item, string status)
        {
            long result = 0;
            var userId = User.Identity.GetUserId();
            if (Service.EMCS.SvcCipl.CiplHisOwned(item.Data.Id, userId))
            {
                result = Service.EMCS.SvcCipl.UpdateCipl(item.Forwader, item.Data, status);
            }
            return result;
        }

        [HttpPost]
        public long ReferenceToCiplItem(List<Data.Domain.EMCS.ReferenceToCiplItem> data, long idCipl)
        {
            long referenceToCiplItem = Service.EMCS.SvcCipl.ReferenceToCiplItem(data, idCipl);
            return referenceToCiplItem;
        }

        [HttpPost]
        public JsonResult CiplItemCancel(long idCipl)
        {
            try
            {
                Service.EMCS.SvcCipl.DeleteCiplById(idCipl, "CIPLITEM");
                return JsonCRUDMessage("Delete Success");
            }
            catch (Exception)
            {
                return JsonCRUDMessage("Delete Error");
            }
        }

        [HttpPost]
        public long DeleteCiplById(long id)
        {
            long result = 0;
            var userId = User.Identity.GetUserId();
            if (Service.EMCS.SvcCipl.CiplHisOwned(id, userId))
            {
                result = Service.EMCS.SvcCipl.DeleteCiplById(id, "ALL");
            }
            return result;

        }

        [HttpPost]
        public ActionResult GetReference(Data.Domain.EMCS.SpGetReference item, string categoryReference)
        {
            var list = Service.EMCS.SvcCipl.GetReference(item, categoryReference);
            return Json(list, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetReferenceItem(GridListFilterModel filter, string column, string columnValue, string category)
        {
            var dataFilter = new Data.Domain.EMCS.GridListFilter();
            dataFilter.Limit = filter.Limit;
            dataFilter.Order = filter.Order;
            dataFilter.Term = filter.Term;
            dataFilter.Sort = filter.Sort;
            dataFilter.Offset = filter.Offset;

            var data = Service.EMCS.SvcCipl.GetReferenceItem(dataFilter, column, columnValue, category);
            return Json(data, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult GetConsigneeName(string id, string category, string categoryReference)
        {
            List<App.Data.Domain.EMCS.SpGetConsigneeName> list = Service.EMCS.SvcCipl.GetConsigneeName(id, category, categoryReference);
            return Json(list, JsonRequestBehavior.AllowGet);

        }

        // ================================ APPROVE ================================
        [HttpPost]
        public JsonResult CiplApproval(Data.Domain.EMCS.CiplApprove form)
        {
            var cipl = Service.EMCS.SvcCipl.CiplGetById(form.Id);
            Service.EMCS.SvcCipl.CrudSp(cipl, form, "U");
            return JsonMessage("This ticket has been " + form.Status, 0, cipl);
        }

        public JsonResult GetCiplAvailable(Data.Domain.EMCS.SpGetCiplAvailable filter)
        {
            var data = Service.EMCS.SvcCipl.GetCiplAvailable(filter);
            return Json(new { data }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult UploadDocument(string idCipl, string noCipl, string typeDoc)
        {
            ViewBag.crudMode = "I";
            Data.Domain.EMCS.RequestCipl step = Service.EMCS.SvcCipl.GetRequestCiplById(idCipl, "CIPL");
            if (step.IdCipl != null)
            {
                string fileResult = DocumentCipl(noCipl, typeDoc);
                if (fileResult != "")
                {
                    Service.EMCS.SvcCipl.InsertDocument(step, fileResult, typeDoc);
                }
                else
                {
                    return Json(new { status = false, msg = "Upload File gagal" });
                }
            }

            return JsonCRUDMessage(ViewBag.crudMode);
        }
        

        public JsonResult DocumentList()
        {
            var data = Service.EMCS.SvcCipl.DocumentList();
            return Json(new { data }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult GetDocument(Data.Domain.EMCS.Documents crit)
        {
            App.Data.Domain.EMCS.Documents list = Service.EMCS.SvcCipl.GetDocument(crit);
            return Json(list, JsonRequestBehavior.AllowGet);

        }

        /// <summary>
        /// Menampilkan List Cipl View dari Cargo.
        /// </summary>
        /// <returns></returns>
        public ActionResult CiplViewList(string page, long id)
        {
            if (page == "rg")
            {
                ViewBag.ListCipl = Service.EMCS.SvcGoodsReceive.GetCiplByGr(id);
            }
            else
            {
                ViewBag.ListCipl = Service.EMCS.SvcCargo.GetCiplbyCargo(id);
            }
            return View();
        }

        public JsonResult GetConsigneeHistory(string term)
        {
            var data = Service.EMCS.SvcCipl.GetConsigneeHistory(term);
            return Json(new { data }, JsonRequestBehavior.AllowGet);
        }
        //public JsonResult GetUOMHistory(string term)
        //{
        //    var data = Service.EMCS.SvcCipl.GetUOMHistory(term);
        //    return Json(new { data }, JsonRequestBehavior.AllowGet);
        //}

        public ActionResult CiplApproveReviseDimension(long id)
        {
            ApplicationTitle();
            ViewBag.AllowRead = AuthorizeAcces.AllowRead;
            ViewBag.AllowCreate = AuthorizeAcces.AllowCreated;
            ViewBag.AllowUpdate = AuthorizeAcces.AllowUpdated;
            ViewBag.AllowDelete = AuthorizeAcces.AllowDeleted;
            ViewBag.WizardData = Service.EMCS.SvcWizard.GetWizardData("cipl", id);
            PaginatorBoot.Remove("SessionTRN");

            var detail = new CiplModel();
            detail.Data = Service.EMCS.SvcCipl.CiplGetById(id);
            detail.Forwarder = Service.EMCS.SvcCipl.CiplForwaderGetById(id);
            detail.DataItem = Service.EMCS.SvcCipl.CiplItemGetById(id);
            detail.TemplateHeader = Service.EMCS.DocumentStreamGenerator.GetCiplInvoicePlHeaderData(id);
            detail.TemplateDetail = Service.EMCS.DocumentStreamGenerator.GetCiplInvoicePlDetailData(id);
            detail.DataRequest = Service.EMCS.SvcRequestCipl.GetRequestById(id);

            List<string> items = new List<string>();
            foreach (var item in detail.DataItem.GroupBy(a => a.ReferenceNo))
            {
                items.Add(item.Key);
            }

            decimal volume = 0;
            decimal netWeight = 0;
            decimal grossWeight = 0;

            foreach (var itm in detail.DataItem)
            {
                decimal subtotal;
                decimal length = itm.Length.HasValue ? itm.Length.Value : 0;
                decimal width = itm.Width.HasValue ? itm.Width.Value : 0;
                decimal height = itm.Height.HasValue ? itm.Height.Value : 0;
                subtotal = length * width * height;
                volume = +subtotal;

                var net = itm.NetWeight.HasValue ? itm.NetWeight.Value : 0;
                var gross = itm.GrossWeight.HasValue ? itm.GrossWeight.Value : 0;
                netWeight = +net;
                grossWeight = +gross;
            }

            ViewBag.Quantity = detail.DataItem.Sum(a => a.Quantity);
            ViewBag.Collies = detail.DataItem.GroupBy(a => a.CaseNumber).Count();
            ViewBag.grossWeight = grossWeight;
            ViewBag.netWeight = netWeight;
            ViewBag.volume = volume;
            ViewBag.refs = string.Join(",", items);
            ViewBag.Currency = detail.DataItem.Count != 0 ? detail.DataItem.FirstOrDefault()?.Currency : "";

            return View(detail);
        }

        public JsonResult CiplItemReviseGetById(long id)
        {
            var data = Service.EMCS.SvcCipl.GetCiplItemReviseGetById(id);
            var total = Service.EMCS.SvcCipl.GetCiplItemReviseTotalGetById(id);
            return Json(new { rows = data, total }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult CiplDelegation(long id, string delegationTo)
        {
            SvcDelegation.DelegationCipl(id, delegationTo);
            return Json(new { status = true }, JsonRequestBehavior.AllowGet);
        }

        public JsonResult SearchCargoByCipl(long IdCipl)
        {
            var data = Service.EMCS.SvcCargo.SearchCargoByCipl(IdCipl);
            return Json(data, JsonRequestBehavior.AllowGet);
        }

        public string UploadDocumentCipl(long id)
        {
            string fileName = "";

            if (Request.Files.Count > 0)
            {
                var file = Request.Files[0];

                if (file != null && file.ContentLength > 0)
                {
                    return GetDocFileName(file, id);
                }
            }
            return fileName;
        }

        public string GetDocFileName(System.Web.HttpPostedFileBase file, long id)
        {
            var fileName = Path.GetFileName(file.FileName);

            if (fileName != null)
            {
                var ext = Path.GetExtension(fileName);
                var path = Server.MapPath("~/Upload/EMCS/Cipl/" + id);
                bool isExists = Directory.Exists(path);
                fileName = "DocCipl-" + id.ToString() + ext;

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

        public string DocumentCipl(string noCipl, string typeDoc)
        {
            string fileName = "";

            if (Request.Files.Count > 0)
            {

                var file = Request.Files[0];

                if (file != null && file.ContentLength > 0)
                {
                    fileName = Path.GetFileName(file.FileName);
                    var strFiles = fileName;
                    fileName = strFiles;

                    // Get Mime Type
                    var ext = Path.GetExtension(fileName);
                    var path = Server.MapPath("~/Upload/EMCS/CIPL/");
                    bool isExists = Directory.Exists(path);
                    fileName = noCipl + " " + typeDoc + ext;

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

        public FileResult DownloadCiplDocument(long id)
        {
            //var list = Service.EMCS.SvcCipl.CiplHistoryGetById(id);
            var files = Service.EMCS.SvcCipl.CiplDocumentListById(id).FirstOrDefault();
            string fullPath = Request.MapPath("~/Upload/EMCS/Dummy/NotFound.txt");

            if (files != null)
            {
                //var fileData = files;
                fullPath = Request.MapPath("~/Upload/EMCS/Cipl/" + files.Id + "/" + files.Filename);
                var fileBytes = System.IO.File.ReadAllBytes(fullPath);
                string fileName = files.Filename;
                return File(fileBytes, System.Net.Mime.MediaTypeNames.Application.Octet, fileName);
            }

            return File(fullPath, "text/plain", "NotFound.txt");
        }
    }
}