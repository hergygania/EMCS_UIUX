namespace WindowService.Library.Model
{
    using System;
using System.ComponentModel.DataAnnotations.Schema;
    [Table("dbo.TrPO")]
    public class TrPO
    {
        public long Id { get; set; }
        public long IdRequest { get; set; }
        public string PO_Number { get; set; }
        public string PO_Date { get; set; }
        public string DeliveryDate { get; set; }
        public string PO_STATUS { get; set; }
        public string Shipment { get; set; }
        public string InvPostingDate { get; set; }
        public string GRDate { get; set; }
        public string DeliveryDocDate { get; set; }
        public string VendorId { get; set; }
        public string VendorName { get; set; }
        public string VendorNpwp { get; set; }
        public string Currency { get; set; }
        public string TermOfPayment { get; set; }
        public string SubTotal { get; set; }
        public string SubTotalTax10 { get; set; }
        public string PICName { get; set; }
        public string PICEmail { get; set; }
        public Nullable<System.DateTime> PO_ReceiptDate { get; set; }
        public Nullable<System.DateTime> PO_ReleaseDate { get; set; }
        public Nullable<System.DateTime> PO_ConfirmedDate { get; set; }
        public Nullable<System.DateTime> PO_ProcessedDate { get; set; }
        public Nullable<System.DateTime> PO_DeliveryDate { get; set; }
        public string InvoiceNo { get; set; }
        public string InvoiceDate { get; set; }
        public string POLineItem { get; set; }
        public string PurchasingGroup { get; set; }
        public string POType { get; set; }
        public string CreatedBy { get; set; }
        public System.DateTime CreatedOn { get; set; }
        public string UpdatedBy { get; set; }
        public Nullable<System.DateTime> UpdatedOn { get; set; }
        public bool IsDeleted { get; set; }
        public bool IsActive { get; set; }
    }
}