using App.Data;
using System;
using System.Collections.Generic;
using System.Linq;

namespace App.Service.SOVetting
{
    public class SapSoOrderItem
    {
        private static string FormatDateStringSAP = "dd.MM.yyyy";
        private static readonly EfDbContext DbContext = new EfDbContext();
        public static List<Data.Domain.SOVetting.SapSoOrderItem> GetListSearchSd(string salesDocument)
        {
            List<Data.Domain.SOVetting.SapSoOrderItem> result = DbContext.SapSoOrderItem
                .Where(s => s.SalesDocument == salesDocument)
                    .ToList();
            return result;
        }
        public static int GetListSearchSdCount(string salesDocument)
        {
            int result = DbContext.SapSoOrderItem
                .Count(s => s.SalesDocument == salesDocument);
            return result;
        }
        public static List<Data.Domain.SOVetting.SapSoOrderItem> GetListSearchSdPage(string salesDocument, int start, int length)
        {
            List<Data.Domain.SOVetting.SapSoOrderItem> result = DbContext.SapSoOrderItem
                .Where(s => s.SalesDocument == salesDocument)
                .OrderBy(o => o.ItemSD)
                .Skip(start).Take(length)
                .ToList();
            return result;
        }
        public static List<Data.Domain.SOVetting.Json_SapSoOrderItem> GetListSearchSalesDocumentJoinSourceItem(string salesDocument, int start, int length, Data.Domain.SOVetting.Json_SapSoOrderItem getSearch)
        {
            var query = (from sapSoOrderItem in DbContext.SapSoOrderItem
                         join dpsTrkItemSource in DbContext.DpsSoSourceItem on
                            new
                            {
                                sapSoOrderItem.SalesDocument, sapSoOrderItem.ItemSD
                            }
                            equals
                            new
                            {
                                SalesDocument = dpsTrkItemSource.SalesDocNumber1,
                                ItemSD = dpsTrkItemSource.HighLevelItem4
                            }
                            into su
                         from dpsTrkItemSource in su.DefaultIfEmpty()

                         where sapSoOrderItem.SalesDocument == salesDocument


                         orderby sapSoOrderItem.ItemSD

                         select new
                         {
                             sapSoOrderItem.ItemSD,
                             sapSoOrderItem.Material,
                             sapSoOrderItem.Description,
                             sapSoOrderItem.OrderQuantity,
                             OrderQuantity7 = dpsTrkItemSource.OrderQuantity7 ?? "",
                             FulfillmentClass15 = dpsTrkItemSource.FulfillmentClass15 ?? "",
                             SupplyingPlant33 = dpsTrkItemSource.SupplyingPlant33 ?? "",
                             ETA22 = dpsTrkItemSource.ETA22 ?? "",
                             GRDocumentDate54b = dpsTrkItemSource.GRDocumentDate54b ?? "",
                             GoodsIssueSTODate46 = dpsTrkItemSource.GoodsIssueSTODate46 ?? "",
                             ConfirmedQuantity8 = dpsTrkItemSource.ConfirmedQuantity8 ?? "",
                             CustRef31 = dpsTrkItemSource.CustRef31 ?? "",
                             SIEta = dpsTrkItemSource.ETA22 ?? "",
                             ShipmentNumber56 = dpsTrkItemSource.ShipmentNumber56 ?? "",
                             DANumber57 = dpsTrkItemSource.DANumber57 ?? "",
                             CKBda = 0,
                             CKBref = 0,
                             tracking_status_desc = "",
                             city = "",
                             sourceitemsd = dpsTrkItemSource.SalesOrderItem2 ?? "0",
                             supplyplant = dpsTrkItemSource.FulfillmentClass15 != null ? dpsTrkItemSource.FulfillmentClass15 == "PO" ? dpsTrkItemSource.CATFacility36 : dpsTrkItemSource.FulfillmentClass15 == "TRANSFER" ? dpsTrkItemSource.SupplyingPlant33 : "": "",
                             ActualGoodsIssueDate64 = dpsTrkItemSource.ActualGoodsIssueDate64 ?? "",
                             GatePassDate65 = dpsTrkItemSource.GatePassDate65 ?? ""
                         }); 
            if (getSearch.description != "" && !string.IsNullOrEmpty(getSearch.description))
                query = query.Where(c => c.Description.Contains(getSearch.description));
            if (getSearch.action != "" && !string.IsNullOrEmpty(getSearch.action))
                query = query.Where(c => c.ItemSD.Contains(getSearch.action));
            if (getSearch.material != "" && !string.IsNullOrEmpty(getSearch.material))
                query = query.Where(c => c.Material.Contains(getSearch.material));
            if (getSearch.classes != "" && !string.IsNullOrEmpty(getSearch.classes))
                query = query.Where(c => c.FulfillmentClass15.Contains(getSearch.classes));
            if (getSearch.qty != null)
                query = query.Where(c => c.OrderQuantity7.Contains(getSearch.qty.ToString()));
            if (getSearch.suppl_plant != "" && !string.IsNullOrEmpty(getSearch.suppl_plant))
                query = query.Where(c => c.SupplyingPlant33.Contains(getSearch.suppl_plant));
            if (getSearch.eta != "" && !string.IsNullOrEmpty(getSearch.eta))
                query = query.Where(c => c.SIEta.Contains(getSearch.eta));
            if (getSearch.shipment_no != "" && !string.IsNullOrEmpty(getSearch.shipment_no))
                query = query.Where(c => c.DANumber57.Contains(getSearch.shipment_no));
            if (getSearch.gr_date != "" && !string.IsNullOrEmpty(getSearch.gr_date))
                query = query.Where(c => c.GRDocumentDate54b.Contains(getSearch.gr_date));
            if (getSearch.gi_date != "" && !string.IsNullOrEmpty(getSearch.gi_date))
                query = query.Where(c => c.GoodsIssueSTODate46.Contains(getSearch.gi_date));
            if (getSearch.shipment_stat.value != "" && !string.IsNullOrEmpty(getSearch.shipment_stat.value))
                query = query.Where(c => c.tracking_status_desc.Contains(getSearch.shipment_stat.value));
            if (getSearch != null)
            {
                if (getSearch.OrderBy == 1)
                {
                    query = getSearch.OrderAsc ? query.OrderBy(s => s.Material).Skip(start).Take(length) : query.OrderByDescending(s => s.Material).Skip(start).Take(length);
                }
                else if (getSearch.OrderBy == 2)
                {
                    query = getSearch.OrderAsc ? query.OrderBy(s => s.Description).Skip(start).Take(length) : query.OrderByDescending(s => s.Description).Skip(start).Take(length);
                }
                else if (getSearch.OrderBy == 3)
                {
                    query = getSearch.OrderAsc ? query.OrderBy(s => s.OrderQuantity).Skip(start).Take(length) : query.OrderByDescending(s => s.OrderQuantity).Skip(start).Take(length);
                }
                else if (getSearch.OrderBy == 4)
                {
                    query = getSearch.OrderAsc ? query.OrderBy(s => s.ConfirmedQuantity8).Skip(start).Take(length) : query.OrderByDescending(s => s.ConfirmedQuantity8).Skip(start).Take(length);
                }
                else if (getSearch.OrderBy == 5)
                {
                    query = getSearch.OrderAsc ? query.OrderBy(s => s.FulfillmentClass15).Skip(start).Take(length) : query.OrderByDescending(s => s.FulfillmentClass15).Skip(start).Take(length);
                }
                else if (getSearch.OrderBy == 6)
                {
                    query = getSearch.OrderAsc ? query.OrderBy(s => s.SupplyingPlant33).Skip(start).Take(length) : query.OrderByDescending(s => s.SupplyingPlant33).Skip(start).Take(length);
                }
                else if (getSearch.OrderBy == 7)
                {
                    query = getSearch.OrderAsc ? query.OrderBy(s => s.ETA22).Skip(start).Take(length) : query.OrderByDescending(s => s.ETA22).Skip(start).Take(length);
                }
                else if (getSearch.OrderBy == 8)
                {
                    query = getSearch.OrderAsc ? query.OrderBy(s => s.ShipmentNumber56).Skip(start).Take(length) : query.OrderByDescending(s => s.ShipmentNumber56).Skip(start).Take(length);
                }
                else if (getSearch.OrderBy == 9)
                {
                    query = getSearch.OrderAsc ? query.OrderBy(s => s.GRDocumentDate54b).Skip(start).Take(length) : query.OrderByDescending(s => s.GRDocumentDate54b).Skip(start).Take(length);
                }
                else if (getSearch.OrderBy == 10)
                {
                    query = getSearch.OrderAsc ? query.OrderBy(s => s.GoodsIssueSTODate46).Skip(start).Take(length) : query.OrderByDescending(s => s.GoodsIssueSTODate46).Skip(start).Take(length);
                }
                else
                {
                    query = query.Skip(start).Take(length);
                }
            }
            else
            {
                query = query.Skip(start).Take(length);
            }
            List<Data.Domain.SOVetting.Json_SapSoOrderItem> listSapSoOrderItem = new List<Data.Domain.SOVetting.Json_SapSoOrderItem>();
            foreach (var v in query)
            {
                Data.Domain.SOVetting.Json_SapSoOrderItem sapSoOrderItem = new Data.Domain.SOVetting.Json_SapSoOrderItem();
                string tmpConfirmQty;
                if (!string.IsNullOrWhiteSpace(v.ConfirmedQuantity8))
                {
                    string[] splitConfirm = v.ConfirmedQuantity8.Split('.');
                    tmpConfirmQty = splitConfirm[0];
                }
                else
                {
                    tmpConfirmQty = "0";
                }
                var tmpEta = !v.ETA22.Contains("1753") ?  v.ETA22: "";
                var tmpGrDate = !v.GRDocumentDate54b.Contains("1753") ? v.GRDocumentDate54b:"";
                var tmpGiDate = !v.GoodsIssueSTODate46.Contains("1753") ? v.GoodsIssueSTODate46:"";
                sapSoOrderItem.action = v.ItemSD;
                sapSoOrderItem.classes = v.FulfillmentClass15;
                sapSoOrderItem.material = v.Material;
                sapSoOrderItem.description = v.Description;
                sapSoOrderItem.qty = Convert.ToDouble(v.OrderQuantity);
                sapSoOrderItem.confirm_city = tmpConfirmQty;
                sapSoOrderItem.classes = v.FulfillmentClass15;
                sapSoOrderItem.suppl_plant = v.supplyplant;
                sapSoOrderItem.eta = tmpEta;
                sapSoOrderItem.gr_date = tmpGrDate;
                sapSoOrderItem.gi_date = tmpGiDate;
                sapSoOrderItem.shipment_no = v.ShipmentNumber56;
                sapSoOrderItem.confirm_qty = tmpConfirmQty;
                sapSoOrderItem.shipment_stat.row_id = Convert.ToDouble(salesDocument);
                sapSoOrderItem.shipment_stat.refer = v.sourceitemsd ;
                sapSoOrderItem.shipment_stat.value =  "";
                sapSoOrderItem.confirm_city = "";
                sapSoOrderItem.gatepass = GetGatePassItemSource(salesDocument, v.sourceitemsd);
                sapSoOrderItem.giOrdering = !v.ActualGoodsIssueDate64.Contains("1753") ? v.ActualGoodsIssueDate64 : "";
                listSapSoOrderItem.Add(sapSoOrderItem);
            }
            return listSapSoOrderItem;
        }
        public static int GetListSearchSalesDocumentJoinSourceItemCount(string salesDocument, int start, int length, Data.Domain.SOVetting.Json_SapSoOrderItem getSearch)
        {
            var query = (from sapSoOrderItem in DbContext.SapSoOrderItem
                         join dpsTrkItemSource in DbContext.DpsSoSourceItem on
                            new
                            {
                                sapSoOrderItem.SalesDocument, sapSoOrderItem.ItemSD
                            }
                            equals
                            new
                            {
                                SalesDocument = dpsTrkItemSource.SalesDocNumber1,
                                ItemSD = dpsTrkItemSource.SalesOrderItem2
                            }
                            into su
                         from dpsTrkItemSource in su.DefaultIfEmpty()

                         where sapSoOrderItem.SalesDocument == salesDocument 


                         orderby sapSoOrderItem.ItemSD

                         select new
                         {
                             sapSoOrderItem.ItemSD,
                             sapSoOrderItem.Material,
                             sapSoOrderItem.Description,
                             sapSoOrderItem.OrderQuantity,
                             itemSDSource = dpsTrkItemSource.SalesOrderItem2,
                             dpsTrkItemSource.OrderQuantity7,
                             dpsTrkItemSource.FulfillmentClass15,
                             dpsTrkItemSource.SupplyingPlant33,
                             dpsTrkItemSource.LastUpdate118,
                             dpsTrkItemSource.GRDocumentDate54b,
                             dpsTrkItemSource.GoodsIssueSTODate46,
                             dpsTrkItemSource.ConfirmedQuantity8,
                             dpsTrkItemSource.CustRef31,
                             SIEta = dpsTrkItemSource.ETA22,
                             dpsTrkItemSource.DANumber57,
                             CKBda = 0,
                             CKBref = 0,
                             tracking_status_desc = "",
                             city = ""
                         });
            if (getSearch.description != "" && !string.IsNullOrEmpty(getSearch.description))
                query = query.Where(c => c.Description.Contains(getSearch.description));
            if (getSearch.action != "" && !string.IsNullOrEmpty(getSearch.action))
                query = query.Where(c => c.ItemSD.Contains(getSearch.action));
            if (getSearch.material != "" && !string.IsNullOrEmpty(getSearch.material))
                query = query.Where(c => c.Material.Contains(getSearch.material));
            if (getSearch.classes != "" && !string.IsNullOrEmpty(getSearch.classes))
                query = query.Where(c => c.FulfillmentClass15.Contains(getSearch.classes));
            if (getSearch.qty != null)
                query = query.Where(c => c.OrderQuantity7.Contains(getSearch.qty.ToString()));
            if (getSearch.suppl_plant != "" && !string.IsNullOrEmpty(getSearch.suppl_plant))
                query = query.Where(c => c.SupplyingPlant33.Contains(getSearch.suppl_plant));
            if (getSearch.eta != "" && !string.IsNullOrEmpty(getSearch.eta))
                query = query.Where(c => c.SIEta.Contains(getSearch.eta));
            if (getSearch.shipment_no != "" && !string.IsNullOrEmpty(getSearch.shipment_no))
                query = query.Where(c => c.DANumber57.Contains(getSearch.shipment_no));
            if (getSearch.gr_date != "" && !string.IsNullOrEmpty(getSearch.gr_date))
                query = query.Where(c => c.GRDocumentDate54b.Contains(getSearch.gr_date));
            if (getSearch.gi_date != "" && !string.IsNullOrEmpty(getSearch.gi_date))
                query = query.Where(c => c.GoodsIssueSTODate46.Contains(getSearch.gi_date));
            if (getSearch.shipment_stat.value != "" && !string.IsNullOrEmpty(getSearch.shipment_stat.value))
                query = query.Where(c => c.tracking_status_desc.Contains(getSearch.shipment_stat.value));
            var queryCount = query.Count();
            return queryCount;
        }

        public static List<Data.Domain.SOVetting.Json_SapSoOrderItem> GetListSearchExportExcelSalesDocumentJoinSourceItem(string salesDocument)
        {
            var query = (from sapSoOrderItem in DbContext.SapSoOrderItem
                         join dpsTrkItemSource in DbContext.DpsSoSourceItem on
                            new
                            {
                                sapSoOrderItem.SalesDocument, sapSoOrderItem.ItemSD
                            }
                            equals
                            new
                            {
                                SalesDocument = dpsTrkItemSource.SalesDocNumber1,
                                ItemSD = dpsTrkItemSource.HighLevelItem4
                            }
                            into su
                         from dpsTrkItemSource in su.DefaultIfEmpty()

                         where sapSoOrderItem.SalesDocument == salesDocument


                         orderby sapSoOrderItem.ItemSD

                         select new
                         {
                             sapSoOrderItem.ItemSD,
                             sapSoOrderItem.Material,
                             sapSoOrderItem.Description,
                             sapSoOrderItem.OrderQuantity,
                             OrderQuantity7 = dpsTrkItemSource.OrderQuantity7 ?? "",
                             FulfillmentClass15 = dpsTrkItemSource.FulfillmentClass15 ?? "",
                             SupplyingPlant33 = dpsTrkItemSource.SupplyingPlant33 ?? "",
                             ETA22 = dpsTrkItemSource.ETA22 ?? "",
                             GRDocumentDate54b = dpsTrkItemSource.GRDocumentDate54b ?? "",
                             GoodsIssueSTODate46 = dpsTrkItemSource.GoodsIssueSTODate46 ?? "",
                             ConfirmedQuantity8 = dpsTrkItemSource.ConfirmedQuantity8 ?? "",
                             CustRef31 = dpsTrkItemSource.CustRef31 ?? "",
                             SIEta = dpsTrkItemSource.ETA22 ?? "",
                             ShipmentNumber56 = dpsTrkItemSource.ShipmentNumber56 ?? "",
                             DANumber57 = dpsTrkItemSource.DANumber57 ?? "",
                             CKBda = 0,
                             CKBref = 0,
                             tracking_status_desc = "",
                             city = "",
                             sourceitemsd = dpsTrkItemSource.SalesOrderItem2 ?? "0",
                             supplyplant = dpsTrkItemSource.FulfillmentClass15 != null ? dpsTrkItemSource.FulfillmentClass15 == "PO" ? dpsTrkItemSource.CATFacility36 : dpsTrkItemSource.FulfillmentClass15 == "TRANSFER" ? dpsTrkItemSource.SupplyingPlant33 : "" : "",
                             ActualGoodsIssueDate64 = dpsTrkItemSource.ActualGoodsIssueDate64 ?? "",
                             GatePassDate65 = dpsTrkItemSource.GatePassDate65 ?? ""
                         });

            List<Data.Domain.SOVetting.Json_SapSoOrderItem> lisSapSoOrderItem = new List<Data.Domain.SOVetting.Json_SapSoOrderItem>();
            foreach (var v in query)
            {
                Data.Domain.SOVetting.Json_SapSoOrderItem sapSoOrderItem = new Data.Domain.SOVetting.Json_SapSoOrderItem();
                string tmpConfirmQty;
                if (!string.IsNullOrWhiteSpace(v.ConfirmedQuantity8))
                {
                    string[] splitConfirm = v.ConfirmedQuantity8.Split('.');
                    tmpConfirmQty = splitConfirm[0];
                }
                else
                {
                    tmpConfirmQty = "0";
                }
                var tmpEta = !v.ETA22.Contains("1753") ? v.ETA22 : "";
                var tmpGrDate = !v.GRDocumentDate54b.Contains("1753") ? v.GRDocumentDate54b : "";
                var tmpGiDate = !v.GoodsIssueSTODate46.Contains("1753") ? v.GoodsIssueSTODate46 : "";
                sapSoOrderItem.action = v.ItemSD;
                sapSoOrderItem.classes = v.FulfillmentClass15;
                sapSoOrderItem.material = v.Material;
                sapSoOrderItem.description = v.Description;
                sapSoOrderItem.qty = Convert.ToDouble(v.OrderQuantity);
                sapSoOrderItem.confirm_city = tmpConfirmQty;
                sapSoOrderItem.classes = v.FulfillmentClass15;
                sapSoOrderItem.suppl_plant = v.supplyplant;
                sapSoOrderItem.eta = tmpEta;
                sapSoOrderItem.gr_date = tmpGrDate;
                sapSoOrderItem.gi_date = tmpGiDate;
                sapSoOrderItem.shipment_no = v.ShipmentNumber56;
                sapSoOrderItem.confirm_qty = tmpConfirmQty;
                sapSoOrderItem.shipment_stat.row_id = Convert.ToDouble(salesDocument);
                sapSoOrderItem.shipment_stat.refer = v.sourceitemsd;
                sapSoOrderItem.shipment_stat.value = "";
                sapSoOrderItem.confirm_city = "";
                sapSoOrderItem.gatepass = GetGatePassItemSource(salesDocument, v.sourceitemsd);
                sapSoOrderItem.giOrdering = !v.ActualGoodsIssueDate64.Contains("1753") ? v.GatePassDate65 : "";
                lisSapSoOrderItem.Add(sapSoOrderItem);
            }
            return lisSapSoOrderItem;
        }


        public static string GetGatePassItemSource(string salesDocument, string itemSalesDocument)
        {
            var customerPoSummary = CustomerPOSummary.GetCustomerPOSummary(salesDocument, Convert.ToInt32(itemSalesDocument));
            if (customerPoSummary == null) return "";

            return (customerPoSummary.GatePassDate != null) ? (!customerPoSummary.GatePassDate.Value.ToString(FormatDateStringSAP).Contains("1753")) ? customerPoSummary.GatePassDate.Value.ToString(FormatDateStringSAP) : "" : "";

        }

    }
}
