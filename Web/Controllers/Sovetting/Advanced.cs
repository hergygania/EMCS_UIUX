using App.Web.App_Start;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;

namespace App.Web.Controllers.SoVetting
{
    public partial class SoVettingController
    {
        private static string FormatDateStringTracking = "yyyy-MM-dd HH:mm:ss";
        private static string FormatDateStringSAP = "dd.MM.yyyy";

        [AuthorizeAcces(ActionType = AuthorizeAcces.IsRead)]
        [Route("advanced")]
        public ActionResult Advanced() 
        {
            this.Session["SoVettingAdvTotalData"] = null;
            this.Session["SoVettingAdv-salesOffice"] = null;
            this.Session["SoVettingAdv-deliveryStatus"] = null;
            this.Session["SoVettingAdv-invoiceStatus"] = null;
            this.Session["SoVettingAdv-paramDateCreate"] = null;
            this.Session["SoVettingAdv-paramDateEnd"] = null;
            this.Session["SoVettingAdv-poNumber"] = null;
            this.Session["SoVettingAdv-salesDocType"] = null;
            this.Session["SoVettingAdv-soNumber"] = null;
            this.Session["SoVettingAdv-soldToParty"] = null;
            this.Session["SoVettingAdvanceSalesDocument"] = null;
            this.Session["SoVettingAdvanceSalesDocumentItem"] = null;
            ViewBag.SapHeader = typeof(Data.Domain.SOVetting.SapSoHeader).GetProperties()
                        .Select(property => property.Name)
                        .ToArray();
            ViewBag.SapOrder = typeof(Data.Domain.SOVetting.SapSoOrderItem).GetProperties()
                        .Select(property => property.Name)
                        .ToArray();
            ViewBag.SapSource = typeof(Data.Domain.SOVetting.DpsSoSourceItem).GetProperties()
                        .Select(property => property.Name)
                        .ToArray();
            ViewBag.SapTracking = typeof(Data.Domain.SOVetting.CKBDeliveryStatusTrackWithoutDa).GetProperties()
                        .Select(property => property.Name)
                        .ToArray();
            var sapSoSearch = Service.SOVetting.SapSoSearch.GetListByUser(User.Identity.GetUserId());
            if (sapSoSearch == null)
            {
                ViewBag.CheckListHeader = null;
                ViewBag.CheckListOrder = null;
                ViewBag.CheckListSource = null;
            }
            else
            {
                string[] idHeader = sapSoSearch.so_header.Split(',').Select(i => i).ToArray();
                ViewBag.CheckListHeader = idHeader;
                string[] idOrder = sapSoSearch.so_order_item.Split(',').Select(i => i).ToArray();
                ViewBag.CheckListOrder = idOrder;
                string[] idSource = sapSoSearch.so_source_item.Split(',').Select(i => i).ToArray();
                ViewBag.CheckListSource = idSource;
            }
            return View();
        }

        [HttpPost]
        public ActionResult AdvancedSetDefault(string[] chkSummary, string[] chkItemOrder, string[] chkItemSource)
        {
            var requestSapSoSearch = new Data.Domain.SOVetting.SapSoSearch
            {
                so_header = string.Join(",", chkSummary),
                so_order_item = string.Join(",", chkItemOrder),
                so_source_item = string.Join(",", chkItemSource),
                user_id = User.Identity.GetUserId()
            };
            if (Service.SOVetting.SapSoSearch.CheckSearchUser(requestSapSoSearch.user_id))
            {
                var sapSoSearch = Service.SOVetting.SapSoSearch.SearchUser(requestSapSoSearch.user_id);
                sapSoSearch.so_header = requestSapSoSearch.so_header;
                sapSoSearch.so_order_item = requestSapSoSearch.so_order_item;
                sapSoSearch.so_source_item = requestSapSoSearch.so_source_item;
                Service.SOVetting.SapSoSearch.UpdateDb(sapSoSearch);
                return Json(new { is_valid = true });
            }

            Service.SOVetting.SapSoSearch.InsertDb(requestSapSoSearch);
            return Json(new { is_valid = true });
        }
        [HttpPost]
        public ActionResult AdvancedSummary(Data.Domain.SOVetting.ReqAdv_SapSoHeader requestAdvanceSapSoHeader, string[] soldToParty, string getDataOnly, int? draw, int? start, int? length) 
        {
            if (soldToParty != null)
            {
                requestAdvanceSapSoHeader.soldToParty = string.Join(",", soldToParty);                
            }

            if (!ModelState.IsValid) return Json(new {is_valid = false});
            int totalData;
            int totalDataFilter;
            var startPick = start ?? 0;
            var lengthPick = length ?? 10;
            List<Data.Domain.SOVetting.SapSoHeader> temp;
            if (getDataOnly == null)
            {
                this.Session["SoVettingAdv-salesOffice"] = requestAdvanceSapSoHeader.salesOffice;
                this.Session["SoVettingAdv-deliveryStatus"] = requestAdvanceSapSoHeader.deliveryStatus;
                this.Session["SoVettingAdv-invoiceStatus"] = requestAdvanceSapSoHeader.invoiceStatus;
                this.Session["SoVettingAdv-paramDateCreate"] = requestAdvanceSapSoHeader.paramDateCreate;
                this.Session["SoVettingAdv-paramDateEnd"] = requestAdvanceSapSoHeader.paramDateEnd;
                this.Session["SoVettingAdv-poNumber"] = requestAdvanceSapSoHeader.poNumber;
                this.Session["SoVettingAdv-salesDocType"] = requestAdvanceSapSoHeader.salesDocType;
                this.Session["SoVettingAdv-soNumber"] = requestAdvanceSapSoHeader.soNumber;
                this.Session["SoVettingAdv-soldToParty"] = requestAdvanceSapSoHeader.soldToParty;
                totalData = Service.SOVetting.SapSoHeader.GetListSearchAdvCount(requestAdvanceSapSoHeader);
                totalDataFilter = totalData;
                this.Session["SoVettingAdvTotalData"] = totalData;
                temp = Service.SOVetting.SapSoHeader.GetListSearchAdv(requestAdvanceSapSoHeader, startPick, lengthPick);
            }
            else
            {
                totalData = this.Session["SoVettingAdvTotalData"] != null ? Convert.ToInt32(this.Session["SoVettingAdvTotalData"]) : 0;
                requestAdvanceSapSoHeader.salesOffice = Session["SoVettingAdv-salesOffice"]?.ToString();
                requestAdvanceSapSoHeader.deliveryStatus = Session["SoVettingAdv-deliveryStatus"]?.ToString();
                requestAdvanceSapSoHeader.invoiceStatus = Session["SoVettingAdv-invoiceStatus"]?.ToString();
                requestAdvanceSapSoHeader.paramDateCreate = Session["SoVettingAdv-paramDateCreate"]?.ToString();
                requestAdvanceSapSoHeader.paramDateEnd = Session["SoVettingAdv-paramDateEnd"]?.ToString();
                requestAdvanceSapSoHeader.poNumber = Session["SoVettingAdv-poNumber"]?.ToString();
                requestAdvanceSapSoHeader.salesDocType = Session["SoVettingAdv-salesDocType"]?.ToString();
                requestAdvanceSapSoHeader.soNumber = Session["SoVettingAdv-soNumber"]?.ToString();
                requestAdvanceSapSoHeader.soldToParty = Session["SoVettingAdv-soldToParty"]?.ToString();
                string[] keys = Request.Form.AllKeys;
                var advSearch = this.SearchHeaderPick(keys);
                temp = Service.SOVetting.SapSoHeader.GetListSearchAdvPick(requestAdvanceSapSoHeader, startPick, lengthPick, advSearch);
                totalDataFilter = Service.SOVetting.SapSoHeader.GetListSearchAdvPickCount(requestAdvanceSapSoHeader, startPick, lengthPick, advSearch);
            }
            var listResponse = ParsingSummary(temp);
            var columnDifferent = new Data.Domain.SOVetting.ReqAdvDefCol_SapSoHeader();
            List<Data.Domain.SOVetting.ReqAdvDefCol_SapSoHeader> listColumnDifferent = new List<Data.Domain.SOVetting.ReqAdvDefCol_SapSoHeader>();
            columnDifferent.visible = false;
            var array = ArraySoHeader();
            columnDifferent.targets = array;
            listColumnDifferent.Add(columnDifferent);
            return Json(new { draw, recordsTotal = totalData, recordsFiltered = totalDataFilter, latest_update = DateTime.Now.ToString(FormatDateStringTracking), columnDefs = listColumnDifferent.ToArray(), data = listResponse });
        }
        [HttpPost]
        public ActionResult AdvanceItemSourcePost(string salesDocumentNumber, string salesDocumentItem,string getDataOnly, int? draw, int? start, int? length)
        {
            int totalData;
            var startPick = start ?? 0;
            var lengthPick = length ?? 10;
            if (getDataOnly == null)
            {
                this.Session["SoVettingAdvanceSalesDocument"] = salesDocumentNumber;
                this.Session["SoVettingAdvanceSalesDocumentItem"] = salesDocumentItem;
            }
            else
            {
                salesDocumentNumber = this.Session["SoVettingAdvanceSalesDocument"]?.ToString();
                salesDocumentItem = this.Session["SoVettingAdvanceSalesDocumentItem"]?.ToString();
            }
            
            Data.Domain.SOVetting.ReqAdvDefCol_SapSoHeader columnDifferent = new Data.Domain.SOVetting.ReqAdvDefCol_SapSoHeader();
            List<Data.Domain.SOVetting.ReqAdvDefCol_SapSoHeader> listColumnDifferent = new List<Data.Domain.SOVetting.ReqAdvDefCol_SapSoHeader>();
            columnDifferent.visible = false;
            var array = ArraySoSource();
            columnDifferent.targets = array;
            listColumnDifferent.Add(columnDifferent);
            var tempSource = Service.SOVetting.DpsSoSourceItem.GetListSearchSdAndItemPage(salesDocumentNumber, salesDocumentItem, startPick, lengthPick);
            var totalDataFilter = totalData = Service.SOVetting.DpsSoSourceItem.GetListSearchSdAndItemCount(salesDocumentNumber, salesDocumentItem);
            return Json(new { draw, recordsTotal = totalData, recordsFiltered = totalDataFilter, latest_update = DateTime.Now.ToString(FormatDateStringTracking), columnDefs = listColumnDifferent.ToArray(), data = tempSource });
        }
        [HttpPost]
        public ActionResult AdvanceItemOrderPost(string soNumber, string getDataOnly, int? draw, int? start, int? length)
        {
           
            int totalData;
            var startPick = start ?? 0;
            var lengthPick = length ?? 10;
            if (getDataOnly == null)
            {
                this.Session["SoVettingAdvanceSalesDocument"] = soNumber;
            }
            else
            {
                soNumber =  this.Session["SoVettingAdvanceSalesDocument"]?.ToString();
            }

            Data.Domain.SOVetting.ReqAdvDefCol_SapSoHeader columnDifferent = new Data.Domain.SOVetting.ReqAdvDefCol_SapSoHeader();
            List<Data.Domain.SOVetting.ReqAdvDefCol_SapSoHeader> listColumnDifferent = new List<Data.Domain.SOVetting.ReqAdvDefCol_SapSoHeader>();
            columnDifferent.visible = false;
            var array = ArraySoOrder();
            columnDifferent.targets = array;
            listColumnDifferent.Add(columnDifferent);
            var tempSource = Service.SOVetting.SapSoOrderItem.GetListSearchSdPage(soNumber, startPick, lengthPick);
            var totalDataFilter = totalData = Service.SOVetting.SapSoOrderItem.GetListSearchSdCount(soNumber);
            var listResOdr = ParsingOrder(tempSource);
            return Json(new { draw, recordsTotal = totalData, recordsFiltered = totalDataFilter, latest_update = DateTime.Now.ToString(FormatDateStringTracking), columnDefs = listColumnDifferent.ToArray(), data = listResOdr });
        }
        [HttpPost]
        public ActionResult AdvanceTrackingDaBySalesDocumentPost(string salesDocumentNumber)
        {
            var tempSource = Service.SOVetting.CustomerPOSummary.GetListDaCustomerPOSummary(salesDocumentNumber);
            var daList = tempSource.Where(itm => itm.DaNo != "").Select(a => a.DaNo).Distinct().ToArray();
            return Json(new { soNumber = salesDocumentNumber, data = AdvanceTrackingDaBySalesDocumentPostGetByDa(daList) });
           
        }

        #region GetAdvanceTrackingByDa
        private static List<Data.Domain.SOVetting.DpsSoSourceItemTrakingOnly> AdvanceTrackingDaBySalesDocumentPostGetByDa(string[] daList)
        {
            List<Data.Domain.SOVetting.DpsSoSourceItemTrakingOnly> tmpDataTracking = new List<Data.Domain.SOVetting.DpsSoSourceItemTrakingOnly>();

            foreach (var da in daList)
            {
                Data.Domain.SOVetting.CKBDeliveryStatus dataCkb = Service.SOVetting.CkbDeliveryStatus.GetLastTracking(da);
                if (dataCkb == null) continue;
                var tempCkbWithoutDa = new Data.Domain.SOVetting.CKBDeliveryStatusTrackWithoutDa();
                var itemDa = new Data.Domain.SOVetting.DpsSoSourceItemTrakingOnly();
                tempCkbWithoutDa.origin = dataCkb.origin;
                tempCkbWithoutDa.destination = dataCkb.destination;
                tempCkbWithoutDa.customer = dataCkb.customer;
                tempCkbWithoutDa.receiver = dataCkb.receiver;
                tempCkbWithoutDa.no_sequence = dataCkb.no_sequence;
                tempCkbWithoutDa.tracking_station = dataCkb.tracking_station;
                tempCkbWithoutDa.tracking_status_id = dataCkb.tracking_status_id;
                tempCkbWithoutDa.tracking_date = (dataCkb.tracking_date.HasValue) ? dataCkb.tracking_date.Value.ToString(FormatDateStringTracking) : "";
                tempCkbWithoutDa.tracking_status_desc = dataCkb.tracking_status_desc;
                tempCkbWithoutDa.city = dataCkb.city;
                tempCkbWithoutDa.datetime_updated = (dataCkb.tracking_date.HasValue) ? dataCkb.tracking_date.Value.ToString(FormatDateStringTracking) : "";
                itemDa.DANumber57 = da;
                itemDa.Trakingdata = tempCkbWithoutDa;
                tmpDataTracking.Add(itemDa);
            }
            return tmpDataTracking;
        }
        #endregion

        #region  GetAdvanceTrackingByCaseNo

        //private static List<Data.Domain.SOVetting.DpsSoSourceItemTrakingOnly> AdvanceTrackingDaBySalesDocumentPostGetByCaseNo(string salesDocumentNumber,string[] itemOrder)
        //{
        //    List<Data.Domain.SOVetting.DpsSoSourceItemTrakingOnly> tmpDataTracking = new  List<Data.Domain.SOVetting.DpsSoSourceItemTrakingOnly>();

        //    foreach (string salesOrderItem in itemOrder)
        //    {
        //        Data.Domain.SOVetting.CustomerPOSummary customerOrderSummary = Service.SOVetting.CustomerPOSummary.GetCustomerPOSummary(salesDocumentNumber, Convert.ToInt32(salesOrderItem));
        //        if (customerOrderSummary == null) continue;
        //        if (customerOrderSummary.HUID2 != "" && !string.IsNullOrWhiteSpace(customerOrderSummary.HUID2))
        //        {
        //            Data.Domain.SOVetting.CKBDeliveryStatus dataCkb = Service.SOVetting.CkbDeliveryStatus.GetLastTrackingWithCaseNo(Convert.ToInt64(customerOrderSummary.HUID2).ToString());
        //            if (dataCkb == null) continue;
        //            var tempCkbWithoutDa = new Data.Domain.SOVetting.CKBDeliveryStatusTrackWithoutDa();
        //            var itemDa = new Data.Domain.SOVetting.DpsSoSourceItemTrakingOnly();
        //            tempCkbWithoutDa.origin = dataCkb.origin;
        //            tempCkbWithoutDa.destination = dataCkb.destination;
        //            tempCkbWithoutDa.customer = dataCkb.customer;
        //            tempCkbWithoutDa.receiver = dataCkb.receiver;
        //            tempCkbWithoutDa.no_sequence = dataCkb.no_sequence;
        //            tempCkbWithoutDa.tracking_station = dataCkb.tracking_station;
        //            tempCkbWithoutDa.tracking_status_id = dataCkb.tracking_status_id;
        //            tempCkbWithoutDa.tracking_date = (dataCkb.tracking_date.HasValue) ? dataCkb.tracking_date.Value.ToString(FormatDateStringTracking) : "";
        //            tempCkbWithoutDa.tracking_status_desc = dataCkb.tracking_status_desc;
        //            tempCkbWithoutDa.city = dataCkb.city;
        //            tempCkbWithoutDa.datetime_updated = (dataCkb.tracking_date.HasValue) ? dataCkb.tracking_date.Value.ToString(FormatDateStringTracking) : "";
        //            itemDa.DANumber57 = Convert.ToInt64(customerOrderSummary.CaseNo).ToString();
        //            itemDa.Trakingdata = tempCkbWithoutDa;
        //            tmpDataTracking.Add(itemDa);
        //        }
        //        else
        //        {
        //            if (customerOrderSummary.CaseNo == "" && string.IsNullOrWhiteSpace(customerOrderSummary.CaseNo) &&
        //                string.IsNullOrEmpty(customerOrderSummary.CaseNo)) continue;
        //            Data.Domain.SOVetting.CKBDeliveryStatus dataCkb = Service.SOVetting.CkbDeliveryStatus.GetLastTrackingWithCaseNo(Convert.ToInt64(customerOrderSummary.CaseNo).ToString());
        //            if (dataCkb == null) continue;
        //            var tempCkbWithoutDa = new Data.Domain.SOVetting.CKBDeliveryStatusTrackWithoutDa();
        //            var itemDa = new Data.Domain.SOVetting.DpsSoSourceItemTrakingOnly();
        //            tempCkbWithoutDa.origin = dataCkb.origin;
        //            tempCkbWithoutDa.destination = dataCkb.destination;
        //            tempCkbWithoutDa.customer = dataCkb.customer;
        //            tempCkbWithoutDa.receiver = dataCkb.receiver;
        //            tempCkbWithoutDa.no_sequence = dataCkb.no_sequence;
        //            tempCkbWithoutDa.tracking_station = dataCkb.tracking_station;
        //            tempCkbWithoutDa.tracking_status_id = dataCkb.tracking_status_id;
        //            tempCkbWithoutDa.tracking_date = (dataCkb.tracking_date.HasValue) ? dataCkb.tracking_date.Value.ToString(FormatDateStringTracking) : "";
        //            tempCkbWithoutDa.tracking_status_desc = dataCkb.tracking_status_desc;
        //            tempCkbWithoutDa.city = dataCkb.city;
        //            tempCkbWithoutDa.datetime_updated = (dataCkb.tracking_date.HasValue) ? dataCkb.tracking_date.Value.ToString(FormatDateStringTracking) : "";
        //            itemDa.DANumber57 = Convert.ToInt64(customerOrderSummary.CaseNo).ToString();
        //            itemDa.Trakingdata = tempCkbWithoutDa;
        //            tmpDataTracking.Add(itemDa);
        //        }
        //    }
        //    return tmpDataTracking;
        //}
        #endregion

        [HttpPost]
        public ActionResult AdvanceSelect(string id, string q)
        {
            Data.Domain.SOVetting.SapSoHeader selectedHeader = new Data.Domain.SOVetting.SapSoHeader();
            switch (id)
            {
                case "sales-off":
                    selectedHeader.SalesOffice = q;
                    break;
                case "sold-to-part":
                    selectedHeader.SoldToPartyNo = q;
                    break;
                case "delivery":
                    selectedHeader.DeliveryStatus = q;
                    break;
                case "sales-doc-type":
                    selectedHeader.SalesDocumentType = q;
                    break;
                case "invoice":
                    selectedHeader.InvoiceStatus = q;
                    break;
                case "so-number":
                    selectedHeader.SalesDocument = q;
                    break;
                case "credit-sts":
                    selectedHeader.CreditStatus = q;
                    break;
            }

            var tempHeader = Service.SOVetting.SapSoHeader.GetListSelect(selectedHeader);
            List<Data.Domain.SOVetting.ResSelect> listOption = new List<Data.Domain.SOVetting.ResSelect>();
            foreach (var item in tempHeader)
            {
                Data.Domain.SOVetting.ResSelect opt = new Data.Domain.SOVetting.ResSelect();
                switch (id)
                {
                    case "sales-off":
                        opt.option = item.SalesOffice;
                        opt.value = item.SalesOffice;
                        break;
                    case "sold-to-part":
                        opt.option = item.SoldToPartyNo;
                        opt.value = item.SoldToPartyNo;
                        break;
                    case "delivery":
                        opt.option = item.DeliveryStatus;
                        opt.value = item.DeliveryStatus;
                        break;
                    case "sales-doc-type":
                        opt.option = item.SalesDocumentType;
                        opt.value = item.SalesDocumentType;
                        break;
                    case "invoice":
                        opt.option = item.InvoiceStatus;
                        opt.value = item.InvoiceStatus;
                        break;
                    case "so-number":
                        opt.option = item.SalesDocument;
                        opt.value = item.SalesDocument;
                        break;
                    case "credit-sts":
                        opt.option = item.CreditStatus;
                        opt.value = item.CreditStatus;
                        break;
                }

                listOption.Add(opt);
            }
            return Json(new { is_valid = id, data = listOption });
        }
        private static List<Data.Domain.SOVetting.Json_SummarySapSoHeader> ParsingSummary(List<Data.Domain.SOVetting.SapSoHeader> soHeaders)
        {
            List<Data.Domain.SOVetting.Json_SummarySapSoHeader> listResponse = new List<Data.Domain.SOVetting.Json_SummarySapSoHeader>();
            int i = 0;
            foreach (Data.Domain.SOVetting.SapSoHeader item in soHeaders)
            {
                Data.Domain.SOVetting.Json_SummarySapSoHeader res = new Data.Domain.SOVetting.Json_SummarySapSoHeader
                {
                    id = ++i,
                    action = item.SalesDocument,
                    soNumber = {number = item.SalesDocument, url = ""},
                    Area = item.Area,
                    SalesOffice = item.SalesOffice,
                    SoldToPartyNo = item.SoldToPartyNo,
                    CustomerName = item.CustomerName,
                    ShipToPartyNo = item.ShipToPartyNo,
                    ShipToPartyName = item.ShipToPartyName,
                    PayerNo = item.PayerNo,
                    PayerName = item.PayerName,
                    GracePeriod = item.GracePeriod,
                    GracePeriodDate = item.GracePeriodDate.HasValue
                        ? !item.GracePeriodDate.Value.ToString(FormatDateStringSAP).Contains("1753")
                            ? item.GracePeriodDate.Value.ToString(FormatDateStringSAP)
                            : ""
                        : "",
                    GracePeriodNotes = item.GracePeriodNotes,
                    SalesDocument = item.SalesDocument,
                    SalesDocumentType = item.SalesDocumentType,
                    PurchaseOrderNo = item.PurchaseOrderNo,
                    DocumentDate = item.DocumentDate.HasValue
                        ? !item.DocumentDate.Value.ToString(FormatDateStringSAP).Contains("1753")
                            ? item.DocumentDate.Value.ToString(FormatDateStringSAP)
                            : ""
                        : "",
                    TermsOfPayment = item.TermsOfPayment,
                    ConsolidateIndicator = item.ConsolidateIndicator,
                    RequestedDeliveryDate = item.RequestedDeliveryDate.HasValue
                        ? !item.RequestedDeliveryDate.Value.ToString(FormatDateStringSAP).Contains("1753")
                            ? item.RequestedDeliveryDate.Value.ToString(FormatDateStringSAP)
                            : ""
                        : "",
                    SerialNumber = item.SerialNumber,
                    CustomerEquipmentNo = item.CustomerEquipmentNo,
                    UnitModel = item.UnitModel,
                    ETAOfParts = item.ETAOfParts.HasValue
                        ? !item.ETAOfParts.Value.ToString(FormatDateStringSAP).Contains("1753")
                            ? item.ETAOfParts.Value.ToString(FormatDateStringSAP)
                            : ""
                        : "",
                    Remarks = item.Remarks,
                    TotalPartsItemInSalesDoc =
                        item.TotalPartsItemInSalesDoc ?? 0,
                    TotalPartsBackOrderedItem = item.TotalPartsBackOrderedItem ?? 0,
                    TotalPartsSubcontracting =
                        item.TotalPartsSubcontracting ?? 0
                };

                if (item.PartsCompletion != null)
                {
                    res.PartsCompletion = item.PartsCompletion.Value;
                }
                else
                {
                    res.PartsCompletion = 0;
                }
                res.DocumentCurrency = item.DocumentCurrency;
                res.PartsSalesValue = item.PartsSalesValue ?? 0;
                res.LastGIDate = item.LastGIDate.HasValue ? !item.LastGIDate.Value.ToString(FormatDateStringSAP).Contains("1753") ? item.LastGIDate.Value.ToString(FormatDateStringSAP) :"": "";
                res.DeliveryStatus = item.DeliveryStatus;
                res.InvoiceStatus = item.InvoiceStatus;
                res.SalesOrderStatus = item.SalesOrderStatus;
                res.CreditStatus = item.CreditStatus;
                res.SalesmanPersonalNo = item.SalesmanPersonalNo;
                res.SalesmanName = item.SalesmanName;
                res.PaymentDate = item.PaymentDate.HasValue ? !item.PaymentDate.Value.ToString(FormatDateStringSAP).Contains("1753") ? item.PaymentDate.Value.ToString(FormatDateStringSAP) :"" : "";
                res.SOAging = item.SOAging ?? 0;
                res.CreatedBy = item.CreatedBy;
                res.CreatedOn = item.CreatedOn.HasValue ? !item.CreatedOn.Value.ToString(FormatDateStringSAP).Contains("1753") ? item.CreatedOn.Value.ToString(FormatDateStringSAP) : "" : "";
                res.Time = item.Time.HasValue ? !item.Time.Value.ToString(FormatDateStringSAP).Contains("1753") ? item.Time.Value.ToString(FormatDateStringSAP) : "" : "";
                res.PurchaseOrderDate = item.PurchaseOrderDate.HasValue ? !item.PurchaseOrderDate.Value.ToString(FormatDateStringSAP).Contains("1753") ? item.PurchaseOrderDate.Value.ToString(FormatDateStringSAP) :"" : "";
                res.Plant = item.Plant;
                res.NeedByDate = item.NeedByDate.HasValue ? !item.NeedByDate.Value.ToString(FormatDateStringSAP).Contains("1753") ? item.NeedByDate.Value.ToString(FormatDateStringSAP) :"" : "";
                res.CompletionDateOfSO = item.CompletionDateOfSO.HasValue ? !item.CompletionDateOfSO.Value.ToString(FormatDateStringSAP).Contains("1753") ? item.CompletionDateOfSO.Value.ToString(FormatDateStringSAP) : "" : "";
                listResponse.Add(res);
            }
            return listResponse;
        }
        private static List<Data.Domain.SOVetting.Json_OrderSapSoOrderItem> ParsingOrder(List<Data.Domain.SOVetting.SapSoOrderItem> tempOdr)
        {
            int i = 0;
            return tempOdr.Select(itmO => new Data.Domain.SOVetting.Json_OrderSapSoOrderItem
                {
                    id = ++i,
                    product_name = {row_id = itmO.SalesDocument, itemsd = itmO.ItemSD, name = itmO.Description},
                    Area = itmO.Area,
                    SalesOffice = itmO.SalesOffice,
                    SoldToPartyNo = itmO.SoldToPartyNo,
                    SoldToPartyName = itmO.SoldToPartyName,
                    ShiptoPartyNo = itmO.ShiptoPartyNo,
                    ShipToPartyName = itmO.ShipToPartyName,
                    SalesDocumentType = itmO.SalesDocumentType,
                    Purchaseorderno = itmO.Purchaseorderno,
                    SalesDocument = itmO.SalesDocument,
                    ItemSD = itmO.ItemSD,
                    Material = itmO.Material,
                    Description = itmO.Description,
                    OrderQuantity = itmO.OrderQuantity ?? 0,
                    Salesunit = itmO.Salesunit,
                    Plant = itmO.Plant,
                    ValuationType = itmO.ValuationType,
                    ETAofParts = itmO.ETAofParts.HasValue
                        ? !itmO.ETAofParts.Value.ToString(FormatDateStringSAP).Contains("1753")
                            ? itmO.ETAofParts.Value.ToString(FormatDateStringSAP)
                            : ""
                        : "",
                    DocumentCurrency = itmO.DocumentCurrency,
                    PartsSellingPrice = itmO.PartsSellingPrice ?? 0,
                    PajakPertambahanNilaiPPN = itmO.PajakPertambahanNilaiPPN ?? 0,
                    PPh22 = itmO.PPh22 ?? 0,
                    PPh23 = itmO.PPh23 ?? 0,
                    IREQNumber = itmO.IREQNumber,
                    IREQItem = itmO.IREQItem,
                    FulfilmentStatus = itmO.FulfilmentStatus,
                    ItemReturnIndicator = itmO.ItemReturnIndicator,
                    PEXCoreLife = itmO.PEXCoreLife,
                    ReplacementParts = itmO.ReplacementParts,
                    AlternativeParts = itmO.AlternativeParts,
                    BusinessEconomicCode = itmO.BusinessEconomicCode,
                    CommodityCode = itmO.CommodityCode,
                    UniqueID = itmO.UniqueID,
                    UniqueID1 = itmO.UniqueID1,
                    UniqueID2 = itmO.UniqueID2,
                    OrderMethod = itmO.OrderMethod,
                    Commimpcodeno = itmO.Commimpcodeno,
                    Costing = itmO.Costing,
                    MadeasOrder = itmO.MadeasOrder,
                    Remarks = itmO.Remarks,
                    Itemcategory = itmO.Itemcategory,
                    DiscountTotal = itmO.DiscountTotal ?? 0,
                    SurchargeTotal = itmO.SurchargeTotal ?? 0,
                    PartsSellingAfterDisc = itmO.PartsSellingAfterDisc ?? 0,
                    CorePriceBeforeDiscount = itmO.CorePriceBeforeDiscount ?? 0,
                    StatusofFulfillmentItem = itmO.StatusofFulfillmentItem
                })
                .ToList();
        }
        private static int[] ArraySoHeader()
        {
            List<int> tempRes = new List<int>();
            string user = Domain.SiteConfiguration.UserName;
            var aSearch = Service.SOVetting.SapSoSearch.GetListByUser(user);
             string[] idHeader = aSearch.so_header.Split(',').Select(i => i).ToArray();
            var tempHead = typeof(Data.Domain.SOVetting.SapSoHeader).GetProperties()
                        .Select(property => property.Name)
                        .ToArray();
            int j = 1;
            foreach (string item in tempHead)
            {
                ++j;
                if (!idHeader.Contains(item))
                {
                    tempRes.Add(j);
                }
            }
            var result = tempRes.ToArray();
            return result;
        }
        private static int[] ArraySoOrder()
        {
            List<int> tempRes = new List<int>();
            string user = Domain.SiteConfiguration.UserName;
            var aSearch = Service.SOVetting.SapSoSearch.GetListByUser(user);
            string[] idHeader = aSearch.so_order_item.Split(',').Select(i => i).ToArray();
            var tempHead = typeof(Data.Domain.SOVetting.SapSoOrderItem).GetProperties()
                        .Select(property => property.Name)
                        .ToArray();
            int j = 0;
            foreach (string item in tempHead)
            {
                ++j;
                if (!idHeader.Contains(item))
                {
                    tempRes.Add(j);
                }
            }
            var result = tempRes.ToArray();
            return result;
        }
        private static int[] ArraySoSource()
        {
            List<int> tempRes = new List<int>();
            string user = Domain.SiteConfiguration.UserName;
            var aSearch = Service.SOVetting.SapSoSearch.GetListByUser(user);
            string[] idHeader = aSearch.so_source_item.Split(',').Select(i => i).ToArray();
            var tempHead = typeof(Data.Domain.SOVetting.DpsSoSourceItem).GetProperties()
                        .Select(property => property.Name)
                        .ToArray();
            int j = -1;
            foreach (string item in tempHead)
            {
                ++j;
                if (!idHeader.Contains(item))
                {
                    tempRes.Add(j);
                }
            }
            var result = tempRes.ToArray();
            return result;
        }

        #region TrackByDa
        [HttpPost]
        public ActionResult Track(Data.Domain.SOVetting.Req_CkbDeliv collection)
        {
            Data.Domain.SOVetting.CKBDeliveryStatusTrack ckbDev = new Data.Domain.SOVetting.CKBDeliveryStatusTrack
            {
                dano = collection.da
            };
            var tempCkb = Service.SOVetting.CkbDeliveryStatus.GetListDaTrack(Convert.ToInt64(ckbDev.dano).ToString());
            if (tempCkb == null) return Json(new {is_valid = false});
            int i = 0;
            List<Data.Domain.SOVetting.Res_CkbDeliv> listJsonCkb = tempCkb.Select(item => new Data.Domain.SOVetting.Res_CkbDeliv
                {
                    id = ++i,
                    city = item.city,
                    status = item.tracking_status_desc,
                    date = (item.tracking_date.HasValue)
                        ? item.tracking_date.Value.ToString(FormatDateStringTracking)
                        : ""
                })
                .ToList();
            return Json(new { is_valid = true, data = listJsonCkb });
        }
        #endregion

        #region TrackByCaseNo
        //[HttpPost]
        //public ActionResult Track(Data.Domain.SOVetting.Req_CkbDeliv collection)
        //{
        //    Data.Domain.SOVetting.CKBDeliveryStatusTrack ckbDev = new Data.Domain.SOVetting.CKBDeliveryStatusTrack
        //    {
        //        dano = collection.da
        //    };
        //    var tempCkb = Service.SOVetting.CkbDeliveryStatus.GetListDaTrackWithCaseNo(Convert.ToInt64(ckbDev.dano).ToString());
        //    if (tempCkb == null) return Json(new { is_valid = false });
        //    int i = 0;
        //    List<Data.Domain.SOVetting.Res_CkbDeliv> listJsonCkb = tempCkb.Select(item => new Data.Domain.SOVetting.Res_CkbDeliv
        //        {
        //            id = ++i,
        //            city = item.city,
        //            status = item.tracking_status_desc,
        //            date = (item.tracking_date.HasValue)
        //                ? item.tracking_date.Value.ToString(FormatDateStringTracking)
        //                : ""
        //        })
        //        .ToList();
        //    return Json(new { is_valid = true, data = listJsonCkb });
        //}
        #endregion
        private Data.Domain.SOVetting.Json_SummarySapSoHeader SearchHeaderPick(string[] value)
        {
            Data.Domain.SOVetting.Json_SummarySapSoHeader ssh = new Data.Domain.SOVetting.Json_SummarySapSoHeader();
            for (int i = 0; i < value.Length; i++)
            {
                if (Request.Form[value[i]] == "soNumber")
                    ssh.SalesDocument = Request.Form[value[i + 4]];
                if (Request.Form[value[i]] == "Area")
                    ssh.Area = Request.Form[value[i + 4]];
                if (Request.Form[value[i]] == "SalesOffice")
                    ssh.SalesOffice = Request.Form[value[i + 4]];
                if (Request.Form[value[i]] == "SoldToPartyNo")
                    ssh.SoldToPartyNo = Request.Form[value[i + 4]];
                if (Request.Form[value[i]] == "CustomerName")
                    ssh.CustomerName = Request.Form[value[i + 4]];
                if (Request.Form[value[i]] == "ShipToPartyNo")
                    ssh.ShipToPartyNo = Request.Form[value[i + 4]];
                if (Request.Form[value[i]] == "ShipToPartyName")
                    ssh.ShipToPartyName = Request.Form[value[i + 4]];
                if (Request.Form[value[i]] == "PayerNo")
                    ssh.PayerNo = Request.Form[value[i + 4]];
                if (Request.Form[value[i]] == "PayerName")
                    ssh.PayerName = Request.Form[value[i + 4]];
                if (Request.Form[value[i]] == "GracePeriod")
                    ssh.GracePeriod = Request.Form[value[i + 4]];
                if (Request.Form[value[i]] == "GracePeriodDate")
                    ssh.GracePeriodDate = Request.Form[value[i + 4]];
                if (Request.Form[value[i]] == "GracePeriodNotes")
                    ssh.GracePeriodNotes = Request.Form[value[i + 4]];
                if (Request.Form[value[i]] == "SalesDocument")
                    ssh.SalesDocument = Request.Form[value[i + 4]];
                if (Request.Form[value[i]] == "SalesDocumentType")
                    ssh.SalesDocumentType = Request.Form[value[i + 4]];
                if (Request.Form[value[i]] == "PurchaseOrderNo")
                    ssh.PurchaseOrderNo = Request.Form[value[i + 4]];
                if (Request.Form[value[i]] == "DocumentDate")
                    ssh.DocumentDate = Request.Form[value[i + 4]];
                if (Request.Form[value[i]] == "TermsOfPayment")
                    ssh.TermsOfPayment = Request.Form[value[i + 4]];
                if (Request.Form[value[i]] == "ConsolidateIndicator")
                    ssh.ConsolidateIndicator = Request.Form[value[i + 4]];
                if (Request.Form[value[i]] == "RequestedDeliveryDate")
                    ssh.RequestedDeliveryDate = Request.Form[value[i + 4]];
                if (Request.Form[value[i]] == "SerialNumber")
                    ssh.SerialNumber = Request.Form[value[i + 4]];
                if (Request.Form[value[i]] == "CustomerEquipmentNo")
                    ssh.CustomerEquipmentNo = Request.Form[value[i + 4]];
                if (Request.Form[value[i]] == "UnitModel")
                    ssh.UnitModel = Request.Form[value[i + 4]];
                if (Request.Form[value[i]] == "ETAOfParts")
                    ssh.ETAOfParts = Request.Form[value[i + 4]];
                if (Request.Form[value[i]] == "Remarks")
                    ssh.Remarks = Request.Form[value[i + 4]];
                if (Request.Form[value[i]] == "TotalPartsItemInSalesDoc" && Request.Form[value[i + 4]] != "")
                    ssh.TotalPartsItemInSalesDoc = Convert.ToDouble(Request.Form[value[i + 4]]);
                if (Request.Form[value[i]] == "TotalPartsBackOrderedItem" && Request.Form[value[i + 4]] != "")
                    ssh.TotalPartsBackOrderedItem = Convert.ToDouble(Request.Form[value[i + 4]]);
                if (Request.Form[value[i]] == "TotalPartsSubcontracting" && Request.Form[value[i + 4]] != "")
                    ssh.TotalPartsSubcontracting = Convert.ToDouble(Request.Form[value[i + 4]]);
                if (Request.Form[value[i]] == "PartsCompletion" && Request.Form[value[i + 4]] != "")
                    ssh.PartsCompletion = Convert.ToDouble(Request.Form[value[i + 4]]);
                if (Request.Form[value[i]] == "DocumentCurrency")
                    ssh.DocumentCurrency = Request.Form[value[i + 4]];
                if (Request.Form[value[i]] == "PartsSalesValue" && Request.Form[value[i + 4]] != "")
                    ssh.PartsSalesValue = Convert.ToDouble(Request.Form[value[i + 4]]);
                if (Request.Form[value[i]] == "LastGIDate")
                    ssh.LastGIDate = Request.Form[value[i + 4]]; 
                if (Request.Form[value[i]] == "DeliveryStatus")
                    ssh.DeliveryStatus = Request.Form[value[i + 4]]; 
                if (Request.Form[value[i]] == "InvoiceStatus")
                    ssh.InvoiceStatus = Request.Form[value[i + 4]]; 
                if (Request.Form[value[i]] == "SalesOrderStatus")
                    ssh.SalesOrderStatus = Request.Form[value[i + 4]]; 
                if (Request.Form[value[i]] == "CreditStatus")
                    ssh.CreditStatus = Request.Form[value[i + 4]]; 
                if (Request.Form[value[i]] == "SalesmanPersonalNo")
                    ssh.SalesmanPersonalNo = Request.Form[value[i + 4]]; 
                if (Request.Form[value[i]] == "SalesmanName")
                    ssh.SalesmanName = Request.Form[value[i + 4]];
                if (Request.Form[value[i]] == "PaymentDate")
                    ssh.PaymentDate = Request.Form[value[i + 4]]; 
                if (Request.Form[value[i]] == "SOAging" && Request.Form[value[i + 4]] != "")
                    ssh.SOAging = Convert.ToDouble(Request.Form[value[i + 4]]);
                if (Request.Form[value[i]] == "CreatedBy")
                    ssh.CreatedBy = Request.Form[value[i + 4]]; 
                if (Request.Form[value[i]] == "CreatedOn")
                    ssh.CreatedOn = Request.Form[value[i + 4]]; 
                if (Request.Form[value[i]] == "Time")
                    ssh.Time = Request.Form[value[i + 4]]; 
                if (Request.Form[value[i]] == "PurchaseOrderDate")
                    ssh.PurchaseOrderDate = Request.Form[value[i + 4]]; 
                if (Request.Form[value[i]] == "Plant")
                    ssh.Plant = Request.Form[value[i + 4]]; 
                if (Request.Form[value[i]] == "NeedByDate")
                    ssh.NeedByDate = Request.Form[value[i + 4]]; 
                if (Request.Form[value[i]] == "CompletionDateOfSO")
                    ssh.CompletionDateOfSO = Request.Form[value[i + 4]]; 
            }
            return ssh;
        }

        public ActionResult AdvanceSummaryExportExcel()
        {
            string[] idHeader = null;
            var requestAdvanceSummary = new Data.Domain.SOVetting.ReqAdv_SapSoHeader
            {
                salesOffice = Session["SoVettingAdv-salesOffice"]?.ToString(),
                deliveryStatus = Session["SoVettingAdv-deliveryStatus"]?.ToString(),
                invoiceStatus = Session["SoVettingAdv-invoiceStatus"]?.ToString(),
                paramDateCreate = Session["SoVettingAdv-paramDateCreate"]?.ToString(),
                paramDateEnd = Session["SoVettingAdv-paramDateEnd"]?.ToString(),
                poNumber = Session["SoVettingAdv-poNumber"]?.ToString(),
                salesDocType = Session["SoVettingAdv-salesDocType"]?.ToString(),
                soNumber = Session["SoVettingAdv-soNumber"]?.ToString(),
                soldToParty = Session["SoVettingAdv-soldToParty"]?.ToString()
            };


            var aSearch = Service.SOVetting.SapSoSearch.GetListByUser(User.Identity.GetUserId());
            if (aSearch == null)
            {
                ViewBag.CheckListHeader = null;
            }
            else
            {
                 idHeader = aSearch.so_header.Split(',').Select(i => i).ToArray();
            }

            var temp = Service.SOVetting.SapSoHeader.GetListSearchAdvPickForExport(requestAdvanceSummary);
            var mappingSummary = ParsingSummary(temp);
            var header = typeof(Data.Domain.SOVetting.SapSoHeader).GetProperties().Select(property => property.Name).ToArray();
            StringBuilder sb = new StringBuilder();

            if (idHeader != null)
            {

                for (int i = 0; i <= mappingSummary.Count - 1; i++)
                {
                    if (i != 0) continue;
                    var propertyData = mappingSummary[i].GetType().GetProperties();
                    for (int j = 0; j <= idHeader.Length - 1; j++)
                    {
                        for (int k = 0; k <= propertyData.Length - 1; k++)
                        {
                            if (propertyData[k].Name != idHeader[j]) continue;
                            if(j == 0)
                            {
                                sb.Append("SalesDocument");
                                sb.Append(";");
                                sb.Append(idHeader[j]);
                                sb.Append(";");
                            }
                            else
                            {
                                sb.Append(idHeader[j]);
                                sb.Append(";");
                            }
                        }
                    }
                }
                sb.AppendLine();
            }
            else
            {
                for (int i = 0; i <= header.Length - 1; i++)
                {
                    sb.Append(header[i]);
                    sb.Append(";");
                }
                sb.AppendLine();
            }


            if (idHeader != null)
            {
                for (int i = 0; i <= mappingSummary.Count-1; i++)
                {
                    var propertyData = mappingSummary[i].GetType().GetProperties();
                    for (int j = 0; j <= idHeader.Length - 1; j++)
                    {
                        for (int k = 0; k <= propertyData.Length - 1; k++)
                        {
                            if (propertyData[k].Name != idHeader[j]) continue;
                            if (j == 0)
                            {
                                sb.Append(mappingSummary[i].soNumber.number);
                                sb.Append(";");
                                sb.Append(propertyData[k].GetValue(mappingSummary[i], null));
                                sb.Append(";");
                            }
                            else
                            {
                                sb.Append(propertyData[k].GetValue(mappingSummary[i], null));
                                sb.Append(";");
                            }
                        }
                    }
                    sb.AppendLine();
                }
              
            }
            else
            {
                for (int i = 0; i <= mappingSummary.Count - 1; i++)
                {
                    var propertyData = mappingSummary[i].GetType().GetProperties();
                    for (int j = 0; j <= header.Length - 1; j++)
                    {
                        for (int k = 0; k <= propertyData.Length - 1; k++)
                        {
                            if (propertyData[k].Name != header[j]) continue;
                            sb.Append(propertyData[k].GetValue(mappingSummary[i], null));
                            sb.Append(";");
                        }
                    }
                    sb.AppendLine();
                }
               
            }


            Response.ClearContent();
            Response.Buffer = true;
            Response.AddHeader("content-disposition", "attachment; filename=AdvanceSearchSummary.csv");
            Response.ContentType = "application/text";
            Response.Charset = "";
            Response.Output.Write(sb.ToString());
            Response.Flush();
            Response.End();
            return new EmptyResult();
        }

        public ActionResult AdvanceOrderItemExportExcel()
        {
            string[] idOrder = null;

            var aSearch = Service.SOVetting.SapSoSearch.GetListByUser(User.Identity.GetUserId());
            if (aSearch == null)
            {
            }
            else
            {
                idOrder = aSearch.so_order_item.Split(',').Select(i => i).ToArray();
            }

            var salesDocumentNumber = Session["SoVettingAdvanceSalesDocument"]?.ToString();  
            var temp = Service.SOVetting.SapSoOrderItem.GetListSearchSd(salesDocumentNumber);
            var mappingSummary = ParsingOrder(temp);
            var header = typeof(Data.Domain.SOVetting.SapSoOrderItem).GetProperties().Select(property => property.Name).ToArray();
            StringBuilder sb = new StringBuilder();
            if (idOrder != null)
            {

                for (int i = 0; i <= mappingSummary.Count - 1; i++)
                {
                    if (i != 0) continue;
                    var propertyData = mappingSummary[i].GetType().GetProperties();
                    for (int j = 0; j <= idOrder.Length - 1; j++)
                    {
                        for (int k = 0; k <= propertyData.Length - 1; k++)
                        {
                            if (propertyData[k].Name != idOrder[j]) continue;
                            if (j == 0)
                            {
                                sb.Append("productName");
                                sb.Append(";");
                                sb.Append(idOrder[j]);
                                sb.Append(";");
                            }
                            else
                            {
                                sb.Append(idOrder[j]);
                                sb.Append(";");
                            }
                        }
                    }
                }
                sb.AppendLine();
            }
            else
            {
                for (int i = 0; i <= header.Length - 1; i++)
                {
                    sb.Append(header[i]);
                    sb.Append(";");
                }
                sb.AppendLine();
            }


            if (idOrder != null)
            {
                for (int i = 0; i <= mappingSummary.Count - 1; i++)
                {
                    var propertyData = mappingSummary[i].GetType().GetProperties();
                    for (int j = 0; j <= idOrder.Length - 1; j++)
                    {
                        for (int k = 0; k <= propertyData.Length - 1; k++)
                        {
                            if (propertyData[k].Name != idOrder[j]) continue;
                            if (j == 0)
                            {
                                sb.Append(mappingSummary[i].product_name.name);
                                sb.Append(";");
                                sb.Append(propertyData[k].GetValue(mappingSummary[i], null));
                                sb.Append(";");
                            }
                            else
                            {
                                sb.Append(propertyData[k].GetValue(mappingSummary[i], null));
                                sb.Append(";");
                            }
                        }
                    }
                    sb.AppendLine();
                }

            }
            else
            {
                for (int i = 0; i <= mappingSummary.Count - 1; i++)
                {
                    var propertyData = mappingSummary[i].GetType().GetProperties();
                    for (int j = 0; j <= header.Length - 1; j++)
                    {
                        for (int k = 0; k <= propertyData.Length - 1; k++)
                        {
                            if (propertyData[k].Name != header[j]) continue;
                            sb.Append(propertyData[k].GetValue(mappingSummary[i], null));
                            sb.Append(";");
                        }
                    }
                    sb.AppendLine();
                }

            }


            Response.ClearContent();
            Response.Buffer = true;
            Response.AddHeader("content-disposition", "attachment; filename=AdvanceSearchOrderItem-"+salesDocumentNumber+".csv");
            Response.ContentType = "application/text";
            Response.Charset = "";
            Response.Output.Write(sb.ToString());
            Response.Flush();
            Response.End();
            return new EmptyResult();
        }

        public ActionResult AdvanceItemSourceExportExcel()
        {
            string[] idSource = null;

            var aSearch = Service.SOVetting.SapSoSearch.GetListByUser(User.Identity.GetUserId());
            if (aSearch == null)
            {
            }
            else
            {
                idSource = aSearch.so_source_item.Split(',').Select(i => i).ToArray();
            }
            

            var salesDocumentNumber = Session["SoVettingAdvanceSalesDocument"]?.ToString();
            var salesDocumentItem = Session["SoVettingAdvanceSalesDocumentItem"]?.ToString();
            var tempSource = Service.SOVetting.DpsSoSourceItem.GetListSearchSdAndItem(salesDocumentNumber, salesDocumentItem);
            var header = typeof(Data.Domain.SOVetting.DpsSoSourceItem).GetProperties().Select(property => property.Name).ToArray();



            StringBuilder sb = new StringBuilder();

            if (idSource != null)
            {

                for (int i = 0; i <= tempSource.Count - 1; i++)
                {
                    if (i != 0) continue;
                    var propertyData = tempSource[i].GetType().GetProperties();
                    for (int j = 0; j <= idSource.Length - 1; j++)
                    {
                        for (int k = 0; k <= propertyData.Length - 1; k++)
                        {
                            if (propertyData[k].Name != idSource[j]) continue;
                            sb.Append(idSource[j]);
                            sb.Append(";");
                        }
                    }
                }
                sb.AppendLine();
            }
            else
            {
                for (int i = 0; i <= header.Length - 1; i++)
                {
                    sb.Append(header[i]);
                    sb.Append(";");
                }
                sb.AppendLine();
            }


            if (idSource != null)
            {
                for (int i = 0; i <= tempSource.Count - 1; i++)
                {
                    var propertyData = tempSource[i].GetType().GetProperties();
                    for (int j = 0; j <= idSource.Length - 1; j++)
                    {
                        for (int k = 0; k <= propertyData.Length - 1; k++)
                        {
                            if (propertyData[k].Name != idSource[j]) continue;
                            sb.Append(propertyData[k].GetValue(tempSource[i], null));
                            sb.Append(";");
                        }
                    }
                    sb.AppendLine();
                }

            }
            else
            {
                for (int i = 0; i <= tempSource.Count - 1; i++)
                {
                    var propertyData = tempSource[i].GetType().GetProperties();
                    for (int j = 0; j <= header.Length - 1; j++)
                    {
                        for (int k = 0; k <= propertyData.Length - 1; k++)
                        {
                            if (propertyData[k].Name != header[j]) continue;
                            sb.Append(propertyData[k].GetValue(tempSource[i], null));
                            sb.Append(";");
                        }
                    }
                    sb.AppendLine();
                }

            }


            Response.ClearContent();
            Response.Buffer = true;
            Response.AddHeader("content-disposition", "attachment; filename=AdvanceSearchSourceItem-" + salesDocumentNumber + "-"+ salesDocumentItem + ".csv");
            Response.ContentType = "application/text";
            Response.Charset = "";
            Response.Output.Write(sb.ToString());
            Response.Flush();
            Response.End();
            return new EmptyResult();
        }
    }
}