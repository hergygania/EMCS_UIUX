using System;

namespace App.Data.Domain.POST
{
    #region List HOME
    public class PODone_LIST
    {
        public long Request_Id { get; set; }
        public long PO_Id { get; set; }
        public string PO_No { get; set; }
        public string PO_Date { get; set; }
        public string Delivery_Date { get; set; }
        public string Status_PO { get; set; }
        public string Confirmation { get; set; }
        public Nullable<System.DateTime> Confirmation_Date { get; set; }
        public string Processing { get; set; }
        public Nullable<System.DateTime> Processing_Date { get; set; }
        public string Delivering { get; set; }
        public Nullable<System.DateTime> Delivering_Date { get; set; }
        public int CountHasUploadedBAST { get; set; }
        public int CountNotUploadedBAST { get; set; }
        public int CountHasUploadedInvoice { get; set; }
        public int CountNotUploadedInvoice { get; set; }
        public int CountPOSTINGSAP { get; set; }
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
        public string POType { get; set; }
        public int TotalNotComplete { get; set; }
        public int TotalComplete { get; set; }
        public int TotalPOD { get; set; }
        public int TotalNotPOD { get; set; }
        public string Progress { get; set; }
        public int TotalNotGR { get; set; }
        public int TotalHasGR { get; set; }
        public string PrePayment { get; set; }
        public string PAID { get; set; }
    }

    public class POIncoming_LIST
    {
        public long Request_Id { get; set; }
        public long PO_Id { get; set; }
        public string PO_No { get; set; }
        public string PO_Date { get; set; }
        public string Delivery_Date { get; set; }
        public string Status_PO { get; set; }
        public string Confirmation { get; set; }
        public string DocumentFinance { get; set; }
        public Nullable<System.DateTime> Confirmation_Date { get; set; }
        public string Processing { get; set; }
        public Nullable<System.DateTime> Processing_Date { get; set; }
        public string Delivering { get; set; }
        public Nullable<System.DateTime> Delivering_Date { get; set; }
        public int? CountHasUploadedBAST { get; set; }
        public int? CountNotUploadedBAST { get; set; }
        public int? CountHasUploadedInvoice { get; set; }
        public int? CountNotUploadedInvoice { get; set; }
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
        public string POType { get; set; }
        public int? TotalNotComplete { get; set; }
        public int? TotalComplete { get; set; }
        public int? TotalPOD { get; set; }
        public int? TotalNotPOD { get; set; }
        public string Progress { get; set; }
        public int? TotalNotGR { get; set; }
        public int? TotalHasGR { get; set; }
        public string PrePayment { get; set; }
        public string PAID { get; set; }
        public string InvoiceDate { get; set; }
    }

    public class POInProgress_LIST
    {
        public long Request_Id { get; set; }
        public long PO_Id { get; set; }
        public string PO_No { get; set; }
        public string PO_Date { get; set; }
        public string Delivery_Date { get; set; }
        public string Status_PO { get; set; }
        public string Confirmation { get; set; }
        public Nullable<System.DateTime> Confirmation_Date { get; set; }
        public string Processing { get; set; }
        public Nullable<System.DateTime> Processing_Date { get; set; }
        public string Delivering { get; set; }
        public Nullable<System.DateTime> Delivering_Date { get; set; }
        public int CountHasUploadedBAST { get; set; }
        public int CountNotUploadedBAST { get; set; }
        public int CountHasUploadedInvoice { get; set; }
        public int CountNotUploadedInvoice { get; set; }
        public int CountPOSTINGSAP { get; set; }
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
        public string POType { get; set; }
        public int TotalNotComplete { get; set; }
        public int TotalComplete { get; set; }
        public int TotalPOD { get; set; }
        public int TotalNotPOD { get; set; }
        public string Progress { get; set; }
        public int TotalNotGR { get; set; }
        public int TotalHasGR { get; set; }
        public string PrePayment { get; set; }
        public string PAID { get; set; }
        public int JmlInvApprove { get; set; }
        public int JmlInv { get; set; }
    }
    #endregion




    #region List Parameter
    public partial class POByRequestId_LIST
    {
        public long Request_Id { get; set; }
        public long PO_Id { get; set; }
        public string PO_No { get; set; }
        public string PO_Date { get; set; }
        public string Delivery_Date { get; set; }
        public string Status_PO { get; set; }
        public string Confirmation { get; set; }
        public Nullable<System.DateTime> Confirmation_Date { get; set; }
        public string Processing { get; set; }
        public Nullable<System.DateTime> Processing_Date { get; set; }
        public string Delivering { get; set; }
        public Nullable<System.DateTime> Delivering_Date { get; set; }
        public int? CountHasUploadedBAST { get; set; }
        public int? CountNotUploadedBAST { get; set; }
        public int? CountHasUploadedInvoice { get; set; }
        public int? CountNotUploadedInvoice { get; set; }
        public int? CountReviewInvoice { get; set; }
        public int? CountPOSTINGSAP { get; set; }
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
        public string POType { get; set; }
        public int? TotalNotComplete { get; set; }
        public int? TotalComplete { get; set; }
        public int? TotalPOD { get; set; }
        public int? TotalNotPOD { get; set; }
        public string Progress { get; set; }
        public string GRSA { get; set; }
        public string INVOICING { get; set; }
        public string PAID { get; set; }
        public int? TotalNotGR { get; set; }
        public int? TotalHasGR { get; set; }
        public bool PrePayment { get; set; }
    }

    public partial class POItemCKBByPO_LIST
    {
        public string PONo { get; set; }
        public string DANo { get; set; }
        public string origin { get; set; }
        public string destination { get; set; }
        public string service_id { get; set; }
        public string tracking_station_desc { get; set; }
        public string description { get; set; }
        public Nullable<System.DateTime> tracking_date { get; set; }
        public Nullable<System.DateTime> etd { get; set; }
        public Nullable<System.DateTime> atd { get; set; }
        public Nullable<System.DateTime> eta { get; set; }
        public Nullable<System.DateTime> ata { get; set; }
    }
    public class ItemByRequestId_LIST
    {
        public long Request_Id { get; set; }
        public long PO_Id { get; set; }
        public long PR_Id { get; set; }
        public long Item_Id { get; set; }
        public string Item_Description { get; set; }
        public string Item_Status { get; set; }
        public string Delivery_Type { get; set; }
        public string ATA { get; set; }
        public string ETA { get; set; }
        public string ETD { get; set; }
        public string ATD { get; set; }
        public decimal Qty { get; set; }
        public string UOM { get; set; }
        public string Price_Item { get; set; }
        public string Total { get; set; }
        public string Destination { get; set; }
        public string Currency { get; set; }
        public string PR_Number { get; set; }
        public string DeliveryDate { get; set; }
        public string USER { get; set; }
        public string POSITION { get; set; }
        public string Notes { get; set; }
        public string GRNo { get; set; }
        public string GRDate { get; set; }
        public string GRPostingDate { get; set; }        
        public int TotalBAST { get; set; }
        public int TotalINV { get; set; }
        public string POType { get; set; }
        public string Shipment { get; set; }
        public int isPartial { get; set; }
        public int CountPartialPOD { get; set; }
        public int CountPartialNotPOD { get; set; }
        public string VendorId { get; set; }
    }
    
    public class ItemById_LIST
    {
        public long Request_Id { get; set; }
        public long PO_Id { get; set; }
        public long PR_Id { get; set; }
        public long Item_Id { get; set; }
        public string Item_Description { get; set; }
        public string Item_Status { get; set; }
        public string Delivery_Type { get; set; }
        public string ATA { get; set; }
        public string ETA { get; set; }
        public string ETD { get; set; }
        public string ATD { get; set; }
        public string Qty { get; set; }
        public string UOM { get; set; }
        public string Price_Item { get; set; }
        public string Total { get; set; }
        public string Destination { get; set; }
        public string USER { get; set; }
        public string POSITION { get; set; }
        public string Notes { get; set; }
        public Nullable<long> QtyPartial_Id { get; set; }
        public Nullable<long> QtyPartial_IdMax { get; set; }
        public string QtyPartial { get; set; }
        public DateTime TimeStamp { get; set; }
        public string Step { get; set; }
        public string Status { get; set; }
        public string Po_Number { get; set; }
        public string BusinessArea { get; set; }
    }

    public class HistoryPOById_List
    {
        public string PO_Number { get; set; }
        public string ProcessName { get; set; }
        public string StatusName { get; set; }
        public DateTime SubmitDate { get; set; }
        public string UpdatedBy { get; set; }
    }
    #endregion


    #region

    public class CountPOList
    {
        public int po_incoming { get; set; }
        public int po_progress { get; set; }
        public int po_done { get; set; }
        public int po_reject { get; set; }
    }


    public class CountPODashboard
    {
        public int po_incoming { get; set; }
        public int po_progress { get; set; }
        public int po_done { get; set; }
        public int po_complete { get; set; }
        public int po_bast { get; set; }
        public int po_invoice { get; set; }
        public int CountPODNotBAST { get; set; }
        public int CountBASTNotGR { get; set; }
        public int CountGRNotInvoice { get; set; }
        public int CountInvoiceNotPayment { get; set; }
    }

    #endregion

    #region

    public class ItemHistoryById_LIST
    {
        public long Id { get; set; }
        public Nullable<System.DateTime> TimeStamp { get; set; }
        public string Step { get; set; }
        public string Status { get; set; }
        public string Notes { get; set; }

        public long Request_Id { get; set; }
        public long PO_Id { get; set; }
        public long PR_Id { get; set; }
        public long Item_Id { get; set; }
        public string Item_Description { get; set; }
        public string Item_Status { get; set; }
        public string Delivery_Type { get; set; }
        public string ATA { get; set; }
        public string ETA { get; set; }
        public string ETD { get; set; }
        public string ATD { get; set; }
        public string Qty { get; set; }
        public string UOM { get; set; }
        public string Price_Item { get; set; }
        public string Total { get; set; }
        public string Destination { get; set; }
        public string USER { get; set; }
        public string POSITION { get; set; }
        public Nullable<long> QtyPartial_Id { get; set; }
        public Nullable<long> QtyPartial_IdMax { get; set; }
        public string QtyPartial { get; set; }

    }
    #endregion
}
