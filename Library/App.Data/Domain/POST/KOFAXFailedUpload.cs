using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace App.Data.Domain.POST
{
    [Table("dbo.KOFAXUploadLog")]
    public class KOFAXUploadLog
    {
        [Key]
        public long LogId { get; set; }
        public long AttachmentId { get; set; }
        public string ErrorMessage { get; set; }
        public System.DateTime CreatedDate { get; set; }
        public bool IsSuccess { get; set; }
    }
}
