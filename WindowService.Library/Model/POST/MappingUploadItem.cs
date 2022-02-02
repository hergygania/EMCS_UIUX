namespace WindowService.Library.Model
{
    using System.ComponentModel.DataAnnotations.Schema;

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
