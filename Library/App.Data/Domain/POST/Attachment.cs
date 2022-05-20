using System;

namespace App.Data.Domain.POST
{
    #region List All
    public class AttachmentList
    {
        public long ID { get; set; }
        public long RequestID { get; set; }
        public string FileName { get; set; }
        public string FileNameOri { get; set; }
        public string CodeAttachment { get; set; }
        public string Path { get; set; }
        public bool IsActive { get; set; }
        public Nullable<System.DateTime> UploadedDate { get; set; }
        public Nullable<System.DateTime> ApproveDateFinance { get; set; }
        public string UpoadedBy { get; set; }
        public string Name { get; set; }
        public string Progress { get; set; }
        public string DocType { get; set; }
        public string QtyPartial { get; set; }
        public bool IsRejected { get; set; }
        public string Notes { get; set; }
        public bool IsApprove { get; set; }
        public int CountItemHasInvoiceSAP { get; set; }
        public string ItemDescription { get; set; }
        public string ApproveStatus { get; set; }
        public string SAPDocNumber { get; set; }
        public string WHTaxCode { get; set; }
        public Int64? WHTaxAmount { get; set; }
        public string Status { get; set; }
        public string StatusMessage { get; set; }
        public Nullable<System.DateTime> CreateDate { get; set; }
    }
    public class AttachmentListSAP
    {
        public long ID { get; set; }
        public string InvoiceNo { get; set; }
        public Nullable<System.DateTime> InvoiceDate { get; set; }
        public string ItemDescription { get; set; }
    }
    #endregion

}
 