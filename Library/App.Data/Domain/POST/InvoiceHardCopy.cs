using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace App.Data.Domain.POST
{
    [Table("dbo.InvoiceHardCopy")]
    public class InvoiceHardCopy
    {
        [Key]
        public long Id { get;set;}
        public long requestId { get; set; }
        public long AttachmentId { get; set; }
        public string SubmissionType { get; set; }
        public System.DateTime SubmissionDate { get; set; }
        public bool IsActive { get; set; }
        public string CreateBy { get; set; }
        public System.DateTime CreateDate { get; set; }
        public string ReceiptNameOrNumber { get; set; }

    }
}
