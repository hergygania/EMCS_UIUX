using System.ComponentModel.DataAnnotations.Schema;

namespace App.Data.Domain.POST
{
    [Table("dbo.MappingUploadItem")]
    public class MappingUploadItem
    {
        public long ID { get; set; }
        public long AttachmentID { get; set; }
        public long ItemID { get; set; }
        public System.DateTime CreatedDate { get; set; }
        public string CreatedBy { get; set; }
        public string Progress { get; set; }

    }
}
