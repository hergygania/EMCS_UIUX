using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Data.Domain.POST
{
    #region List HOME

    public class InvoiceIncomingList
    {
        public long Request_Id { get; set; }
        public long InvoiceId { get; set; }
        public string VendorName { get; set; }
        public string PO_Number { get; set; }     
        public string Item { get; set; }
        public Int64 ItemId { get; set; }
        public string FileNameOri { get; set; }
        public string InvoiceUploadedDate { get; set; }
        public string TypeTab { get; set; } = "Incoming";       
        public string BASTSubmitDate { get; set; }       
        public string PlantCodeKOFAX { get; set; }
    }
    public class InvoiceInProgressList
    {
        public long Request_Id { get; set; }
        public long InvoiceId { get; set; }
        public string VendorName { get; set; }
        public string PO_Number { get; set; }
        public string Item { get; set; }
        public Int64 ItemId { get; set; }
        public string FileNameOri { get; set; }
        public string InvoiceUploadedDate { get; set; }
        public string TypeTab { get; set; } = "Progress";       
        public string BASTSubmitDate { get; set; }
        public string StatusInvoice { get; set; }
        public string MessageInvoice { get; set; }
    }
    public class InvoiceInCompleteList
    {
        public long Request_Id { get; set; }
        public long InvoiceId { get; set; }
        public string VendorName { get; set; }
        public string PO_Number { get; set; }
        public string Item { get; set; }     
        public string FileNameOri { get; set; }
        public string InvoiceUploadedDate { get; set; }
        public string TypeTab { get; set; } = "Complete";       
        public string BASTSubmitDate { get; set; }
        public string SAPDocNo { get; set; }
        public string StatusInvoice { get; set; }
        public string MessageInvoice { get; set; }
    }
    public class InvoiceInRejectList
    {
        public long Request_Id { get; set; }
        public long InvoiceId { get; set; }
        public string VendorName { get; set; }
        public string PO_Number { get; set; }
        public string Item { get; set; }
        public Int64 ItemId { get; set; }
        public string FileNameOri { get; set; }
        public string InvoiceUploadedDate { get; set; }
        public string TypeTab { get; set; } = "Reject";        
        public string BASTSubmitDate { get; set; }
        public string StatusInvoice { get; set; }
        public string MessageInvoice { get; set; }
    }
    #endregion
}
