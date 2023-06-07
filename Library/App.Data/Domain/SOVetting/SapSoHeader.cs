using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace App.Data.Domain.SOVetting
{
    [Table("SAP.SO_Summary")]
    public class SapSoHeader
    {
        public string Area { get; set; }
        public string SalesOffice { get; set; }
        public string SoldToPartyNo { get; set; }
        public string CustomerName { get; set; }
        public string ShipToPartyNo { get; set; }
        public string ShipToPartyName { get; set; }
        public string PayerNo { get; set; }
        public string PayerName { get; set; }
        public string GracePeriod { get; set; }
        public DateTime? GracePeriodDate { get; set; }
        public string GracePeriodNotes { get; set; }
        [Key]
        public string SalesDocument { get; set; } 
        public string SalesDocumentType { get; set; }
        [Column("Purchaseorderno")]
        public string PurchaseOrderNo { get; set; }
        public DateTime? DocumentDate { get; set; }
        [Column("TermsofPayment")]
        public string TermsOfPayment { get; set; }
        public string ConsolidateIndicator { get; set; }
        [Column("Requesteddelivdate")]
        public DateTime? RequestedDeliveryDate { get; set; }
        public string SerialNumber { get; set; }
        public string CustomerEquipmentNo { get; set; }
        public string UnitModel { get; set; }
        [Column("ETAofParts")]
        public DateTime? ETAOfParts { get; set; }
        public string Remarks { get; set; }
        [Column("TotalPartsIteminSalesDoc")]
        public double? TotalPartsItemInSalesDoc { get; set; }
        public double? TotalPartsBackOrderedItem { get; set; }
        public double? TotalPartsSubcontracting { get; set; }
        public double? PartsCompletion { get; set; }
        public string DocumentCurrency { get; set; }
        public double? PartsSalesValue { get; set; }
        public DateTime? LastGIDate { get; set; }
        public string DeliveryStatus { get; set; }
        public string InvoiceStatus { get; set; }
        public string SalesOrderStatus { get; set; }
        public string CreditStatus { get; set; }
        public string SalesmanPersonalNo { get; set; }
        public string SalesmanName { get; set; }
        public DateTime? PaymentDate { get; set; }
        public double? SOAging { get; set; }
        [Column("Createdby")]
        public string CreatedBy { get; set; }
        [Column("Createdon")]
        public DateTime? CreatedOn { get; set; }
        public DateTime? Time { get; set; }
        [Column("Purchaseorderdate")]
        public DateTime? PurchaseOrderDate { get; set; }
        public string Plant { get; set; }
        [Column("NeedbyDate")]
        public DateTime? NeedByDate { get; set; }
        [Column("CompletionDateofSO")]
        public DateTime? CompletionDateOfSO { get; set; }
    }
    public class Json_SapSoHeader
    {
        public int row_number { get; set; }
        public string so_number { get; set; }
        public string so_date { get; set; }
        public string po_number { get; set; }
        public string po_date { get; set; }
        public string sold_to_party { get; set; }
        public string ship_to_party { get; set; }
        public double? bo { get; set; }
       
        public double? sub_con { get; set; }
        public string eta { get; set; }
        public string nbd { get; set; }
        public string completion { get; set; }
        public string completion_date { get; set; }
        public string SalesDocument { get; set; }
        public Json_SapSoHeader()
        {
            order_item = new Json_SapSoHeaderOrderitem();
        }
        public Json_SapSoHeaderOrderitem order_item { get; set; }
        public int OrderBy { get; set; }
        public bool OrderAsc { get; set; }
    }
    
    public class Json_SapSoHeaderOrderitem
    {
        public string row_id { get; set; }
        public double? value { get; set; }      
    }

    public class Json_SummarySapSoHeader_soNumber
    {
        public string number { get; set; }
        public string url { get; set; }      
    }

    public class Json_SummarySapSoHeader
    {
        [Key]
        public int id { get; set; }
        public string action { get; set; }
        public Json_SummarySapSoHeader()
        {
            soNumber = new Json_SummarySapSoHeader_soNumber();
        }
        public Json_SummarySapSoHeader_soNumber soNumber { get;set; }
        public string Area { get; set; }
        public string SalesOffice { get; set; }
        public string SoldToPartyNo { get; set; }
        public string CustomerName { get; set; }
        public string ShipToPartyNo { get; set; }
        public string ShipToPartyName { get; set; }
        public string PayerNo { get; set; }
        public string PayerName { get; set; }
        public string GracePeriod { get; set; }
        public string GracePeriodDate { get; set; }
        public string GracePeriodNotes { get; set; }
        public string SalesDocument { get; set; }
        public string SalesDocumentType { get; set; }
        public string PurchaseOrderNo { get; set; }
        public string DocumentDate { get; set; }
        public string TermsOfPayment { get; set; }
        public string ConsolidateIndicator { get; set; }
        public string RequestedDeliveryDate { get; set; }
        public string SerialNumber { get; set; }
        public string CustomerEquipmentNo { get; set; }
        public string UnitModel { get; set; }
        public string ETAOfParts { get; set; }
        public string Remarks { get; set; }
        public double? TotalPartsItemInSalesDoc { get; set; }
        public double? TotalPartsBackOrderedItem { get; set; }
        public double? TotalPartsSubcontracting { get; set; }
        public double? PartsCompletion { get; set; }
        public string DocumentCurrency { get; set; }
        public double? PartsSalesValue { get; set; }
        public string LastGIDate { get; set; }
        public string DeliveryStatus { get; set; }
        public string InvoiceStatus { get; set; }
        public string SalesOrderStatus { get; set; }
        public string CreditStatus { get; set; }
        public string SalesmanPersonalNo { get; set; }
        public string SalesmanName { get; set; }
        public string PaymentDate { get; set; }
        public double? SOAging { get; set; }
        public string CreatedBy { get; set; }
        public string CreatedOn { get; set; }
        public string Time { get; set; }
        public string PurchaseOrderDate { get; set; }
        public string Plant { get; set; }
        public string NeedByDate { get; set; }
        public string CompletionDateOfSO { get; set; }
    }
    public class Req_SapSoHeader
    {
        public string quickSearch { get; set; }
        public string quickSearchCustName { get; set; }
        public string quickSearchDateStart { get; set; }
        public string quickSearchDateEnd { get; set; }
    }
    public class Req2_SapSoHeader
    {
        public string order_item { get; set; }
    }
    public class ReqAdv_SapSoHeader
    {
        public string salesOffice { get; set; }
        public string soldToParty { get; set; }
        public string poNumber { get; set; }
        public string deliveryStatus { get; set; }
        public string salesDocType { get; set; }
        public string invoiceStatus { get; set; }
        public string soNumber { get; set; }
        public string creditStatus { get; set; }
        public string paramDateCreate { get; set; }
        public string paramDateEnd { get; set; }
        public string filterSoItem { get; set; }
    }

    public class ReqAdvDefCol_SapSoHeader
    {
        public bool visible { get; set; }
        public int[] targets { get; set; }
    }

    public class ResSelect
    {
        public string option { get; set; }
        public string value { get; set; }
    }
}
