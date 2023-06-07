using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace App.Data.Domain.SOVetting
{
    [Table("SAP.SO_OrderItem")]
    public class SapSoOrderItem
    {
        [StringLength(255)]
        public string Area { get; set; }
        [StringLength(255)]
        public string SalesOffice { get; set; }
        [StringLength(255)]
        public string SoldToPartyNo { get; set; }
        [StringLength(255)]
        public string SoldToPartyName { get; set; }
        [StringLength(255)]
        public string ShiptoPartyNo { get; set; }
        [StringLength(255)]
        public string ShipToPartyName { get; set; }
        [StringLength(255)]
        public string SalesDocumentType { get; set; }
        [StringLength(255)]
        public string Purchaseorderno { get; set; }
        [Key, Column(Order = 1)]
        [StringLength(255)]
        public string SalesDocument { get; set; }
        [Key, Column(Order = 2)]
        [StringLength(255)]
        public string ItemSD { get; set; }
        [StringLength(255)]
        public string Material { get; set; }
        [StringLength(255)]
        public string Description { get; set; }
        public double? OrderQuantity { get; set; }
        [StringLength(255)]
        public string Salesunit { get; set; }
        [StringLength(255)]
        public string Plant { get; set; }
        [StringLength(255)]
        public string ValuationType { get; set; }
        public DateTime? ETAofParts { get; set; }
        [StringLength(255)]
        public string DocumentCurrency { get; set; }
        public double? PartsSellingPrice { get; set; }
        public double? PajakPertambahanNilaiPPN { get; set; }
        public double? PPh22 { get; set; }
        public double? PPh23 { get; set; }
        [StringLength(255)]
        public string IREQNumber { get; set; }
        [StringLength(255)]
        public string IREQItem { get; set; }
        [StringLength(255)]
        public string FulfilmentStatus { get; set; }
        public string ItemReturnIndicator { get; set; }
        [StringLength(255)]
        public string PEXCoreLife { get; set; }
        [StringLength(255)]
        public string ReplacementParts { get; set; }
        [StringLength(255)]
        public string AlternativeParts { get; set; }
        [StringLength(255)]
        public string BusinessEconomicCode { get; set; }
        [StringLength(255)]
        public string CommodityCode { get; set; }
        [StringLength(255)]
        public string UniqueID { get; set; }
        [StringLength(255)]
        public string UniqueID1 { get; set; }
        [StringLength(255)]
        public string UniqueID2 { get; set; }
        [StringLength(255)]
        public string OrderMethod { get; set; }
        [StringLength(255)]
        public string Commimpcodeno { get; set; }
        [StringLength(255)]
        public string Costing { get; set; }
        [StringLength(255)]
        public string MadeasOrder { get; set; }
        [StringLength(255)]
        public string Remarks { get; set; }
        [StringLength(255)]
        public string Itemcategory { get; set; }
        public double? DiscountTotal { get; set; }
        public double? SurchargeTotal { get; set; }
        public double? PartsSellingAfterDisc { get; set; }
        public double? CorePriceBeforeDiscount { get; set; }
        [StringLength(255)]
        public string StatusofFulfillmentItem { get; set; }
    }
    public class Json_SapSoOrderItem
    {
        public int id {get;set;}
        public string action {get;set;} 
        public string material {get;set;}
        public string description {get;set;}
        public double? qty {get;set;} 
        public string confirm_qty { get; set; } 
        public string gr_date { get; set; } 
        public string gi_date { get; set; } 
        public string confirm_city { get; set; }
        public string classes {get;set;} 
        public string suppl_plant { get; set; }
        public string eta { get; set; }
        public string shipment_no { get; set; }
        public Json_SapSoOrderItem()
        {
            shipment_stat = new Json_SapSoOrderItemShipment();
        }
        public Json_SapSoOrderItemShipment shipment_stat { get; set; }
        public string gatepass { get; set; }
        public string giOrdering { get; set; }

        public int OrderBy { get; set; }
        public bool OrderAsc { get; set; }
    }
    public class Json_SapSoOrderItemShipment
    {
        public double? row_id { get; set; }
        public string refer { get; set; }
        public string value { get; set; }
    }

    public class Json_OrderSapSoOrderItemPName
    {
        public string row_id { get; set; }
        public string itemsd { get; set; }
        public string name { get; set; }
    }
    public class Json_OrderSapSoOrderItem
    {
        public int id { get; set; }
        public Json_OrderSapSoOrderItem()
        {
            product_name = new Json_OrderSapSoOrderItemPName();
        }
        public Json_OrderSapSoOrderItemPName product_name { get; set; }
        public string Area { get; set; }
        public string SalesOffice { get; set; }
        public string SoldToPartyNo { get; set; }
        public string SoldToPartyName { get; set; }
        public string ShiptoPartyNo { get; set; }
        public string ShipToPartyName { get; set; }
        public string SalesDocumentType { get; set; }
        public string Purchaseorderno { get; set; }
        public string SalesDocument { get; set; }
        public string ItemSD { get; set; }
        public string Material { get; set; }
        public string Description { get; set; }
        public double OrderQuantity { get; set; }
        public string Salesunit { get; set; }
        public string Plant { get; set; }
        public string ValuationType { get; set; }
        public string ETAofParts { get; set; }
        public string DocumentCurrency { get; set; }
        public double PartsSellingPrice { get; set; }
        public double PajakPertambahanNilaiPPN { get; set; }
        public double PPh22 { get; set; }
        public double PPh23 { get; set; }
        public string IREQNumber { get; set; }
        public string IREQItem { get; set; }
        public string FulfilmentStatus { get; set; }
        public string ItemReturnIndicator { get; set; }
        public string PEXCoreLife { get; set; }
        public string ReplacementParts { get; set; }
        public string AlternativeParts { get; set; }
        public string BusinessEconomicCode { get; set; }
        public string CommodityCode { get; set; }
        public string UniqueID { get; set; }
        public string UniqueID1 { get; set; }
        public string UniqueID2 { get; set; }
        public string OrderMethod { get; set; }
        public string Commimpcodeno { get; set; }
        public string Costing { get; set; }
        public string MadeasOrder { get; set; }
        public string Remarks { get; set; }
        public string Itemcategory { get; set; }
        public double DiscountTotal { get; set; }
        public double SurchargeTotal { get; set; }
        public double PartsSellingAfterDisc { get; set; }
        public double CorePriceBeforeDiscount { get; set; }
        public string StatusofFulfillmentItem { get; set; }
    }
}
