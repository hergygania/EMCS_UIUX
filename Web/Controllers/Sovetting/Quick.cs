using App.Web.App_Start;
using System;
using System.Collections.Generic;
using System.Text;
using System.Web.Mvc;

namespace App.Web.Controllers.SoVetting
{
    public partial class SoVettingController
    {

        [AuthorizeAcces(ActionType = AuthorizeAcces.IsRead, UrlMenu = "quick")]
        [Route("quick")]
        public ActionResult Quick()
        {
            Session["SoVettingQuickId"] = null;
            Session["SoVettingQuickCustomer"] = null;
            Session["SoVettingQuickStart"] = null;
            Session["SoVettingQuickEnd"] = null;
            Session["QuickDuaId"] = null;
            Session["totalData"] = null;
            return View();
        }

        //[AuthorizeAcces(ActionType = AuthorizeAcces.IsRead)]
        [HttpPost]      
        public ActionResult QuickFirst(Data.Domain.SOVetting.Req_SapSoHeader collection,string getDataOnly, int? draw, int? start, int? length)
        {
            string id, customer, startDate, endDate;

            var startPick = start ?? 0;
            var lengthPick = length ?? 10 ;

            if (getDataOnly == null)
            {
                id = collection.quickSearch;
                customer = collection.quickSearchCustName;
                startDate = collection.quickSearchDateStart;
                endDate = collection.quickSearchDateEnd;
                Session["SoVettingQuickId"] = id;
                Session["SoVettingQuickCustomer"] = customer;
                Session["SoVettingQuickStartDate"] = startDate;
                Session["SoVettingQuickEndDate"] = endDate;            
            }
            else
            {
                id = Session["SoVettingQuickId"] != null ? Session["SoVettingQuickId"].ToString() : "";
                customer = Session["SoVettingQuickCustomer"] != null ? Session["SoVettingQuickCustomer"].ToString() : "";
                startDate = Session["SoVettingQuickStartDate"] != null ? Session["SoVettingQuickStartDate"].ToString() : "";
                endDate = Session["SoVettingQuickEndDate"] != null ? Session["SoVettingQuickEndDate"].ToString() : ""; 
            }

            if (!ModelState.IsValid) return Json(new {succes = false});
            var temp = new List<Data.Domain.SOVetting.SapSoHeader>();
            int  totalData;
            int  totalDataFilter;
            if (getDataOnly == null)
            {
                totalData = Service.SOVetting.SapSoHeader.GetListSearchCount(id, customer, startDate, endDate);
                totalDataFilter = totalData;
                Session["totalData"] = totalData;
            }
            else
            {
                var keys = Request.Form.AllKeys;
                var getSearchSsh = SearchPick(keys);
                temp = Service.SOVetting.SapSoHeader.GetListSearchPick(id, customer, startDate, endDate, startPick, lengthPick, getSearchSsh);
                totalData = Session["totalData"] != null ? Convert.ToInt32(Session["totalData"]) : 0;
                totalDataFilter = Service.SOVetting.SapSoHeader.GetListSearchPickCount(id, customer, startDate, endDate, startPick, lengthPick, getSearchSsh);
            }
            var listSearch = Service.SOVetting.SapSoHeader.MappingDataQuickFirst(temp,startPick);
            var  etl = Service.SOVetting.SapSoHeader.MappingDataQuickFirstGetEtl(temp,startPick);
            return Json(new { draw , recordsTotal = totalData, recordsFiltered = totalDataFilter, latest_update = etl, data = listSearch }, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public ActionResult QuickDua(Data.Domain.SOVetting.Req2_SapSoHeader collection, string getDataOnly, int? draw, int? start, int? length)
        {
            string id;
            var getSearchSapSoOrderItem = new Data.Domain.SOVetting.Json_SapSoOrderItem();
            if (getDataOnly == null)
            {
                id = collection.order_item;
                Session["QuickDuaId"] = id;    
            }
            else
            {
                 id = Session["QuickDuaId"].ToString();
                var keys = Request.Form.AllKeys;
                getSearchSapSoOrderItem = SearchPickOrderItem(keys);
            }

            if (!ModelState.IsValid) return Json(new {succes = false});
            int totalDataFilter;
            var startPick = start ?? 0;
            var lengthPick = length ?? 10;
            var listSapSoOrderItem = new List<Data.Domain.SOVetting.Json_SapSoOrderItem>();
            var totalData = Service.SOVetting.SapSoOrderItem.GetListSearchSdCount(id);
            if (getDataOnly == null)
            {
                totalDataFilter = totalData;
            }
            else
            {
                listSapSoOrderItem = Service.SOVetting.SapSoOrderItem.GetListSearchSalesDocumentJoinSourceItem(id, startPick, lengthPick, getSearchSapSoOrderItem);
                totalDataFilter = Service.SOVetting.SapSoOrderItem.GetListSearchSalesDocumentJoinSourceItemCount(id, startPick, lengthPick, getSearchSapSoOrderItem);
            }

            const string etlDate = "1970-01-01 00:00";
            return Json(new { draw, recordsTotal = totalData, recordsFiltered = totalDataFilter, latest_update = etlDate, salesdocument = id, data = listSapSoOrderItem });
        }
        [HttpPost]
        public ActionResult QuickThird(Data.Domain.SOVetting.Req_CkbDeliv collection)
        {
           
            var postDataFromCollection =
                new Data.Domain.SOVetting.CKBDeliveryStatusTrack {dano = collection.da};
            var customerOrderSummary = Service.SOVetting.CustomerPOSummary.GetCustomerPOSummary(collection.da, Convert.ToInt32(collection.reference), collection.shipmentNo);
            if (customerOrderSummary == null) customerOrderSummary = Service.SOVetting.CustomerPOSummary.GetCustomerPOSummary(collection.da, Convert.ToInt32(collection.reference));
            if (customerOrderSummary == null) return Json(new {is_valid = false});
            if (customerOrderSummary.HUID2 != "" && !string.IsNullOrWhiteSpace(customerOrderSummary.HUID2))
            {
                var tempCkb = Service.SOVetting.CkbDeliveryStatus.GetListDaTrackWithCaseNo(Convert
                    .ToInt64(customerOrderSummary.HUID2).ToString());
                var listJsonCkb = Service.SOVetting.CkbDeliveryStatus.MappingQuickThirdListTracking(tempCkb);
                return Json(new { is_valid = true, data = listJsonCkb });
            }
            else
            {
                if (customerOrderSummary.CaseNo == "" && string.IsNullOrWhiteSpace(customerOrderSummary.CaseNo) &&
                string.IsNullOrEmpty(customerOrderSummary.CaseNo)) return Json(new { is_valid = false });
                if (postDataFromCollection.dano == "0") return Json(new { is_valid = false });
                var tempCkb = Service.SOVetting.CkbDeliveryStatus.GetListDaTrackWithCaseNo(Convert.ToInt64(customerOrderSummary.CaseNo).ToString());
                var listJsonCkb = Service.SOVetting.CkbDeliveryStatus.MappingQuickThirdListTracking(tempCkb);
                return Json(new { is_valid = true, data = listJsonCkb });
            }
            
        }
        [HttpPost]
        public ActionResult QuickLastTrackStatus(Data.Domain.SOVetting.Req_CkbDeliv collection)
        {
            var customerOrderSummary = Service.SOVetting.CustomerPOSummary.GetCustomerPOSummary(collection.da, Convert.ToInt32(collection.reference), collection.shipmentNo);
            if (customerOrderSummary == null) customerOrderSummary = Service.SOVetting.CustomerPOSummary.GetCustomerPOSummary(collection.da, Convert.ToInt32(collection.reference));
            if (customerOrderSummary == null) return Json(new {is_valid = false});

            if (customerOrderSummary.HUID2 != "" && !string.IsNullOrWhiteSpace(customerOrderSummary.HUID2))
            {
                var dataCkb = Service.SOVetting.CkbDeliveryStatus.GetLastTrackingWithCaseNo(Convert.ToInt64(customerOrderSummary.HUID2).ToString());
                if (dataCkb == null) return Json(new { is_valid = false });
                var lastStatusCkb = Service.SOVetting.CkbDeliveryStatus.MappingQuickSearchGetLastStatus(dataCkb);
                return Json(new { is_valid = true, data = lastStatusCkb });
            }
            else
            {
                if (customerOrderSummary.CaseNo == "" && string.IsNullOrWhiteSpace(customerOrderSummary.CaseNo) &&
                    string.IsNullOrEmpty(customerOrderSummary.CaseNo)) return Json(new { is_valid = false });
                var dataCkb = Service.SOVetting.CkbDeliveryStatus.GetLastTrackingWithCaseNo(Convert.ToInt64(customerOrderSummary.CaseNo).ToString());
                if (dataCkb == null) return Json(new { is_valid = false });
                var lastStatusCkb = Service.SOVetting.CkbDeliveryStatus.MappingQuickSearchGetLastStatus(dataCkb);
                return Json(new { is_valid = true, data = lastStatusCkb });
            }
            
        }
        private Data.Domain.SOVetting.Json_SapSoHeader SearchPick(IReadOnlyList<string> value)
            {
            var searchSapSoHeader = new Data.Domain.SOVetting.Json_SapSoHeader();
            for (var i = 0; i < value.Count; i++)
            {
                if (Request.Form[value[i]] == "so_number")
                    searchSapSoHeader.so_number = Request.Form[value[i + 4]];
                if (Request.Form[value[i]] == "so_date")
                    searchSapSoHeader.so_date = Request.Form[value[i + 4]];
                if (Request.Form[value[i]] == "po_number")
                    searchSapSoHeader.po_number = Request.Form[value[i + 4]];
                if (Request.Form[value[i]] == "po_date")
                    searchSapSoHeader.po_date = Request.Form[value[i + 4]];
                if (Request.Form[value[i]] == "ship_to_party")
                    searchSapSoHeader.ship_to_party = Request.Form[value[i + 4]];
                if (Request.Form[value[i]] == "order_item" && Request.Form[value[i + 4]] != "")
                    searchSapSoHeader.order_item.value = Convert.ToDouble(Request.Form[value[i + 4]]);
                if (Request.Form[value[i]] == "bo" && Request.Form[value[i + 4]] != "")
                    searchSapSoHeader.bo = Convert.ToDouble(Request.Form[value[i + 4]]);
                if (Request.Form[value[i]] == "sub_con" && Request.Form[value[i + 4]] != "")
                    searchSapSoHeader.sub_con = Convert.ToDouble(Request.Form[value[i + 4]]);
                if (Request.Form[value[i]] == "eta")
                    searchSapSoHeader.eta = Request.Form[value[i + 4]];
                if (Request.Form[value[i]] == "nbd")
                    searchSapSoHeader.nbd = Request.Form[value[i + 4]];
                if (Request.Form[value[i]] == "completion_date")
                    searchSapSoHeader.completion_date = Request.Form[value[i + 4]];
                if (Request.Form[value[i]] == "completion")
                    searchSapSoHeader.completion = Request.Form[value[i + 4]];
                if (value[i] == "order[0][column]")
                    searchSapSoHeader.OrderBy = Convert.ToInt32(Request.Form[value[i]]);
                if(value[i] == "order[0][dir]")
                {
                    searchSapSoHeader.OrderAsc = Request.Form[value[i]] == "asc";
                } 
            }
            return searchSapSoHeader;
        }
        private Data.Domain.SOVetting.Json_SapSoOrderItem SearchPickOrderItem(IReadOnlyList<string> value)
        {
            var searchSapSoOrderItem = new Data.Domain.SOVetting.Json_SapSoOrderItem();
            for (var i = 0; i < value.Count; i++)
            {
                if (Request.Form[value[i]] == "action")
                    searchSapSoOrderItem.action = Request.Form[value[i + 4]];
                if (Request.Form[value[i]] == "material")
                    searchSapSoOrderItem.material = Request.Form[value[i + 4]];
                if (Request.Form[value[i]] == "description")
                    searchSapSoOrderItem.description = Request.Form[value[i + 4]];
                if (Request.Form[value[i]] == "qty" && Request.Form[value[i + 4]] != "")
                    searchSapSoOrderItem.qty = Convert.ToDouble(Request.Form[value[i + 4]]);
                if (Request.Form[value[i]] == "confirm_qty")
                    searchSapSoOrderItem.confirm_qty = Request.Form[value[i + 4]];
                if (Request.Form[value[i]] == "classes")
                    searchSapSoOrderItem.classes = Request.Form[value[i + 4]];
                if (Request.Form[value[i]] == "suppl_plant" && Request.Form[value[i + 4]] != "")
                    searchSapSoOrderItem.suppl_plant = Request.Form[value[i + 4]];
                if (Request.Form[value[i]] == "eta")
                    searchSapSoOrderItem.eta = Request.Form[value[i + 4]];
                if (Request.Form[value[i]] == "shipment_no" && Request.Form[value[i + 4]] != "")
                    searchSapSoOrderItem.shipment_no = Request.Form[value[i + 4]];
                if (Request.Form[value[i]] == "gr_date" && Request.Form[value[i + 4]] != "")
                    searchSapSoOrderItem.gr_date = Request.Form[value[i + 4]];
                if (Request.Form[value[i]] == "gi_date")
                    searchSapSoOrderItem.gi_date = Request.Form[value[i + 4]];
                if (Request.Form[value[i]] == "shipment_stat")
                    searchSapSoOrderItem.shipment_stat.value = Request.Form[value[i + 4]];
                if (value[i] == "order[0][column]")
                    searchSapSoOrderItem.OrderBy = Convert.ToInt32(Request.Form[value[i]]);
                if (value[i] == "order[0][dir]")
                {
                    searchSapSoOrderItem.OrderAsc = Request.Form[value[i]] == "asc";
                }
            }
            return searchSapSoOrderItem;
        }

        //[AuthorizeAcces(ActionType = AuthorizeAcces.IsRead, UrlMenu = "quick")]
        public ActionResult QuickSummaryExportExcel(Data.Domain.SOVetting.Req_SapSoHeader collection)
        {
            var salesDocument = Session["SoVettingQuickId"] != null ? Session["SoVettingQuickId"].ToString() : "";
            var customer = Session["SoVettingQuickCustomer"] != null ? Session["SoVettingQuickCustomer"].ToString() : "";
            var startDate = Session["SoVettingQuickStartDate"] != null ? Session["SoVettingQuickStartDate"].ToString() : "";
            var endDate = Session["SoVettingQuickEndDate"] != null ? Session["SoVettingQuickEndDate"].ToString() : "";
            var sb = new StringBuilder();
            var temp = Service.SOVetting.SapSoHeader.GetListSearchPickExport(salesDocument, customer, startDate, endDate);
            var mappingData = Service.SOVetting.SapSoHeader.MappingDataQuickFirst(temp, 0);

            for (var i = 0; i <= mappingData.Count - 1; i++)
            {
                if (i != 0) continue;
                var propertyData = mappingData[i].GetType().GetProperties();
                for (var j = 0; j < propertyData.Length-2; j++)
                {
                    sb.Append(propertyData[j].Name);
                    if (j != propertyData.Length-2)
                    {
                        sb.Append(";");
                    }
                }
            }
            sb.AppendLine();


            for (var i = 0; i <= mappingData.Count - 1; i++)
            {
                var propertyData = mappingData[i].GetType().GetProperties();
                for (var j = 0; j < propertyData.Length-2; j++)
                {
                    sb.Append(propertyData[j].Name == "order_item"
                        ? mappingData[i].order_item.value
                        : propertyData[j].GetValue(mappingData[i], null));

                    if (j != propertyData.Length-2)
                    {
                        sb.Append(";");
                    }
                }
                sb.AppendLine();
            }
            Response.ClearContent();
            Response.Buffer = true;
            Response.AddHeader("content-disposition", "attachment; filename=QuickSearchSummary.csv");
            Response.ContentType = "application/text";
            Response.Charset = "";
            Response.Output.Write(sb.ToString());
            Response.Flush();
            Response.End();
            return new EmptyResult();
        }

        public ActionResult QuickItemExportExcel(Data.Domain.SOVetting.Req_SapSoHeader collection)
        {
            var salesDocument = Session["QuickDuaId"] != null ? Session["QuickDuaId"].ToString() : "";
            var sb = new StringBuilder();
            var temp = Service.SOVetting.SapSoOrderItem.GetListSearchExportExcelSalesDocumentJoinSourceItem(salesDocument);

            for (var i = 0; i <= temp.Count - 1; i++)
            {
                if (i != 0) continue;
                var propertyData = temp[i].GetType().GetProperties();
                for (var j = 0; j < propertyData.Length - 2; j++)
                {
                    if (j == 0) continue;
                    if (propertyData[j].Name == "confirm_city ")
                    {

                    }
                    else
                    {
                        switch (propertyData[j].Name)
                        {
                            case "action":
                                sb.Append("itemSD");
                                break;
                            case " suppl_plant ":
                                sb.Append("source");
                                break;
                            default:
                                sb.Append(propertyData[j].Name);
                                break;
                        }

                        if (j != propertyData.Length - 2)
                        {
                            sb.Append(";");
                        }
                    }

                }
            }
            sb.AppendLine();


            for (var i = 0; i <= temp.Count - 1; i++)
            {
                var propertyData = temp[i].GetType().GetProperties();
                for (var j = 0; j < propertyData.Length - 2; j++)
                {
                    if (j == 0) continue;
                    if (propertyData[j].Name == "confirm_city ")
                    {

                    }
                    else
                    {
                        if (propertyData[j].Name == "shipment_stat")
                        {
                            var getLastStatus = GetLastStatusTrackingForExportExcel(salesDocument, temp[i].shipment_stat.refer);
                            sb.Append(getLastStatus != null ? getLastStatus.status : "");
                        }
                        else
                        {
                            sb.Append(propertyData[j].GetValue(temp[i], null));
                        }

                        if (j != propertyData.Length - 2)
                        {
                            sb.Append(";");
                        }
                    }

                }
                sb.AppendLine();
            }
            Response.ClearContent();
            Response.Buffer = true;
            Response.AddHeader("content-disposition", "attachment; filename=QuickSearchItem-"+ salesDocument+".csv");
            Response.ContentType = "application/text";
            Response.Charset = "";
            Response.Output.Write(sb.ToString());
            Response.Flush();
            Response.End();
            return new EmptyResult();
        }

        public Data.Domain.SOVetting.Res_CkbDeliv GetLastStatusTrackingForExportExcel(string salesDocument, string itemSalesDocument)
        {
            var customerOrderSummary = Service.SOVetting.CustomerPOSummary.GetCustomerPOSummary(salesDocument, Convert.ToInt32(itemSalesDocument));
            if (customerOrderSummary == null) return null;
            if (customerOrderSummary.HUID2 != "" && !string.IsNullOrWhiteSpace(customerOrderSummary.HUID2))
            {
                var dataCkb = Service.SOVetting.CkbDeliveryStatus.GetLastTrackingWithCaseNo(Convert.ToInt64(customerOrderSummary.HUID2).ToString());
                if (dataCkb == null) return null;
                var lastStatusCkb = Service.SOVetting.CkbDeliveryStatus.MappingQuickSearchGetLastStatus(dataCkb);
                return lastStatusCkb;
            }
            else
            {
                if (customerOrderSummary.CaseNo == "" && string.IsNullOrWhiteSpace(customerOrderSummary.CaseNo) &&
                    string.IsNullOrEmpty(customerOrderSummary.CaseNo)) return null;
                var dataCkb = Service.SOVetting.CkbDeliveryStatus.GetLastTrackingWithCaseNo(Convert.ToInt64(customerOrderSummary.CaseNo).ToString());
                if (dataCkb == null) return null;
                var lastStatusCkb = Service.SOVetting.CkbDeliveryStatus.MappingQuickSearchGetLastStatus(dataCkb);
                return lastStatusCkb;
            }


           

        }

    }
}