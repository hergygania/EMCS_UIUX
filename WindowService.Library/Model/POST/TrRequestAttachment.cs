namespace WindowService.Library.Model
{
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("dbo.TrRequestAttachment")]
    public class TrRequestAttachment
    {
        public long ID { get; set; }
        public long RequestID { get; set; }
        public long ItemID { get; set; }
        public string FileName { get; set; }
        public string FileNameOri { get; set; }
        public string CodeAttachment { get; set; }
        public string Path { get; set; }
        public System.DateTime UploadedDate { get; set; }
        public string UploadedBy { get; set; }
        public bool IsActive { get; set; }
        public string Name { get; set; }
        public string Progress { get; set; }
        public string DocType { get; set; }
        public bool IsRejected { get; set; }
        public bool IsApprove { get; set; }
        public string Notes { get; set; }
        public string QtyPartial { get; set; }
<<<<<<< HEAD
<<<<<<< HEAD
        public string FileNameKOFAX { get; set; }
=======
>>>>>>> 639d8d0 (Intial commit)
=======
        public string FileNameKOFAX { get; set; }
>>>>>>> 93c2efe ([U] Update from client's TFS)
    }
    

}
