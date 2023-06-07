using System;

namespace App.Data.Domain.POST
{
    public partial class QuickSearchPOByPO
    {
        public long Request_Id { get; set; }
        public long PO_Id { get; set; }
        public string PO_Date { get; set; }
        public string PO_Number { get; set; }
        public string StatusPO { get; set; }
        public string VendorName { get; set; }
        public string VendorAddress { get; set; }
        public string Destination { get; set; }
        public string DeliveryDate { get; set; }
        public string DestinationAddress { get; set; }
        public Nullable<System.DateTime> RequiredDate { get; set; }
        public Nullable<System.DateTime> PrReleaseDate { get; set; }
        public Nullable<System.DateTime> PoReleaseDate { get; set; }
        public Nullable<System.DateTime> VendorReceivePoDate { get; set; }
        public string Confirmation { get; set; }
        public Nullable<System.DateTime> OrderConfirmDate { get; set; }
        public string Processing { get; set; }
        public Nullable<System.DateTime> ProcessingDate { get; set; }
        public string Delivering { get; set; }
        public Nullable<System.DateTime> DeliveringDate { get; set; }
        public string BAST { get; set; }
        public Nullable<System.DateTime> BastDate { get; set; }
        public string Invoicing { get; set; }
        public Nullable<System.DateTime> InvoiceDate { get; set; }
        public string Complete { get; set; }
        public Nullable<System.DateTime> CloseDate { get; set; }
        public int CountHasGR { get; set; }
        public int CountNotGR { get; set; }
        public int CountItemPOD { get; set; }
        public int CountItemNotPOD { get; set; }
        public int CountItemhasInvoice { get; set; }
        public int CountItemNotInvoice { get; set; }
        public int CountItemHasInvoiceFinance { get; set; }
        public int CountItemHasInvoiceSAP { get; set; }
        public int CountItemHasbast { get; set; }
        public int CountItemNotbast { get; set; }
        public string POType { get; set; }
        public string ProgressBAST { get; set; }
        public string ProgressDelivering { get; set; }
        public string ProgressInvoice { get; set; }
        public int TotalNotComplete { get; set; }
        public int TotalComplete { get; set; }
        public int TotalPOD { get; set; }
        public int TotalNotPOD { get; set; }
        public int CloseData { get; set; }
    }


    public partial class QuickSearchPOByPR
    {
        public long Request_Id { get; set; }
        public long PO_Id { get; set; }
        public string PO_Date { get; set; }
        public string StatusPO { get; set; }
        public string PO_Number { get; set; }
        public string VendorName { get; set; }
        public string VendorAddress { get; set; }
        public string Destination { get; set; }
        public string DeliveryDate { get; set; }
        public string DestinationAddress { get; set; }
        public Nullable<System.DateTime> RequiredDate { get; set; }
        public Nullable<System.DateTime> PrReleaseDate { get; set; }
        public Nullable<System.DateTime> PoReleaseDate { get; set; }
        public Nullable<System.DateTime> VendorReceivePoDate { get; set; }
        public string Confirmation { get; set; }
        public Nullable<System.DateTime> OrderConfirmDate { get; set; }
        public string Processing { get; set; }
        public Nullable<System.DateTime> ProcessingDate { get; set; }
        public string Delivering { get; set; }
        public Nullable<System.DateTime> DeliveringDate { get; set; }
        public string BAST { get; set; }
        public Nullable<System.DateTime> BastDate { get; set; }
        public string Invoicing { get; set; }
        public Nullable<System.DateTime> InvoiceDate { get; set; }
        public string Complete { get; set; }
        public Nullable<System.DateTime> CloseDate { get; set; }
        public int CountHasGR { get; set; }
        public int CountNotGR { get; set; }
        public int CountItemPOD { get; set; }
        public int CountItemNotPOD { get; set; }
        public int CountItemhasInvoice { get; set; }
        public int CountItemHasInvoiceFinance { get; set; }
        public int CountItemHasInvoiceSAP { get; set; }
        public int CountItemNotInvoice { get; set; }
        public int CountItemHasbast { get; set; }
        public int CountItemNotbast { get; set; }
        public string POType { get; set; }
        public string ProgressBAST { get; set; }
        public string ProgressDelivering { get; set; }
        public string ProgressInvoice { get; set; }
        public int TotalNotComplete { get; set; }
        public int TotalComplete { get; set; }
        public int TotalPOD { get; set; }
        public int TotalNotPOD { get; set; }
    }


    public partial class POQuickSearchByVendor
    {
        public long Request_Id { get; set; }
        public long PO_Id { get; set; }
        public string PO_No { get; set; }
        public string PO_Date { get; set; }
        public string Delivery_Date { get; set; }
        public string Status_PO { get; set; }
        public string PO_Number { get; set; }
        public string Confirmation { get; set; }
        public Nullable<System.DateTime> Confirmation_Date { get; set; }
        public string Processing { get; set; }
        public Nullable<System.DateTime> Processing_Date { get; set; }
        public string Delivering { get; set; }
        public Nullable<System.DateTime> Delivering_Date { get; set; }
        public string BAST { get; set; }
        public Nullable<System.DateTime> BAST_Date { get; set; }
        public string Invoicing { get; set; }
        public Nullable<System.DateTime> Invoicing_Date { get; set; }
        public string Inv_Posting_Date { get; set; }
        public string GR_Posting_Date { get; set; }
        public string Delivery_Doc_Date { get; set; }
        public string PICName { get; set; }
        public string PICEmail { get; set; }
        public string SubTotal { get; set; }
        public string SubTotalTax10 { get; set; }
        public string CountLineNumber { get; set; }
        public Nullable<System.DateTime> PO_Receipt_Date { get; set; }
        public string Invoice_No { get; set; }
        public string Invoice_Date { get; set; }
        public string Vendor_Name { get; set; }
        public string Shipment { get; set; }
        public string NextProcessName { get; set; }
        public int CountHasGR { get; set; }
        public int CountNotGR { get; set; }
        public int CountItemPOD { get; set; }
        public int CountItemNotPOD { get; set; }
        public int CountItemhasInvoice { get; set; }
        public int CountItemNotInvoice { get; set; }
        public int CountItemHasbast { get; set; }
        public int CountItemNotbast { get; set; }
        public string POType { get; set; }
        public string ProgressBAST { get; set; }
        public string ProgressDelivering { get; set; }
        public string ProgressInvoice { get; set; }
        public int TotalNotComplete { get; set; }
        public int TotalComplete { get; set; }
        public int TotalPOD { get; set; }
        public int TotalNotPOD { get; set; }
        public int TotalHasGR { get; set; }
        public int TotalNotGR { get; set; }
    }
    
    public partial class POQuickSearchByDate
    {
        public long Request_Id { get; set; }
        public long PO_Id { get; set; }
        public string PO_No { get; set; }
        public string PO_Date { get; set; }
        public string Delivery_Date { get; set; }
        public string PO_Number { get; set; }
        public string Status_PO { get; set; }
        public string Confirmation { get; set; }
        public Nullable<System.DateTime> Confirmation_Date { get; set; }
        public string Processing { get; set; }
        public Nullable<System.DateTime> Processing_Date { get; set; }
        public string Delivering { get; set; }
        public Nullable<System.DateTime> Delivering_Date { get; set; }
        public string BAST { get; set; }
        public Nullable<System.DateTime> BAST_Date { get; set; }
        public string Invoicing { get; set; }
        public Nullable<System.DateTime> Invoicing_Date { get; set; }
        public string Inv_Posting_Date { get; set; }
        public string GR_Posting_Date { get; set; }
        public string Delivery_Doc_Date { get; set; }
        public string PICName { get; set; }
        public string PICEmail { get; set; }
        public string SubTotal { get; set; }
        public string SubTotalTax10 { get; set; }
        public string CountLineNumber { get; set; }
        public Nullable<System.DateTime> PO_Receipt_Date { get; set; }
        public string Invoice_No { get; set; }
        public string Invoice_Date { get; set; }
        public string Vendor_Name { get; set; }
        public string Shipment { get; set; }
        public string NextProcessName { get; set; }
        public int CountHasGR { get; set; }
        public int CountNotGR { get; set; }
        public int CountItemPOD { get; set; }
        public int CountItemNotPOD { get; set; }
        public int CountItemhasInvoice { get; set; }
        public int CountItemNotInvoice { get; set; }
        public int CountItemHasbast { get; set; }
        public int CountItemNotbast { get; set; }
        public string POType { get; set; }
        public string ProgressBAST { get; set; }
        public string ProgressDelivering { get; set; }
        public string ProgressInvoice { get; set; }
        public int TotalNotComplete { get; set; }
        public int TotalComplete { get; set; }
        public int TotalPOD { get; set; }
        public int TotalNotPOD { get; set; }

    }
}
