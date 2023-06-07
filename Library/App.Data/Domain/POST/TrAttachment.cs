using System.ComponentModel.DataAnnotations.Schema;

namespace App.Data.Domain.POST
{
    [Table("dbo.TrAttachment")]
    public class TrAttachment
    {
        public long ID { get; set; }
        public string Code { get; set; }
        public string FileName { get; set; }
        public string FileNameOri { get; set; }
        public string CodeAttachment { get; set; }
        public string Path { get; set; }
        public System.DateTime UploadedDate { get; set; }
        public string UploadedBy { get; set; }
    }
}
