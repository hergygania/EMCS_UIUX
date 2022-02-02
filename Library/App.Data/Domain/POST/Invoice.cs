using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace App.Data.Domain.POST
{
    public class ItemInvoice
    {
        public string InvoiveNo { get; set; }
        public Nullable<System.DateTime> InvDate { get; set; }
        public Nullable<System.DateTime> InvPostingDate { get; set; }
        public Nullable<System.DateTime> InvPaymentDate { get; set; }
        public int POItem { get; set; }
        public string PRNumber { get; set; }
        public string PONumber { get; set; }
        public string ItemDescription { get; set; }
        public string InvProgressPercent { get; set; }
    }
    public class PopUp
    {
        public string VendorId { get; set; }
        public Boolean IsAgreeInvoice { get; set; }
        public Boolean IsAgreeHomePage { get; set; }
    }
    public class Itemmapping
    {
        public long AttachmentId { get; set; }
        public long itemId { get; set; }
    }
}
