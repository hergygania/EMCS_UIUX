namespace App.Data.Domain.EMCS
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("dbo.NotificationQueue")]
    public class NotificationQueue
    {
        [Key]
        public long Id { get; set; }
        public string Module { get; set; }
        public long RequestId { get; set; }
        public string NotificationTo { get; set; }
        public string NotificationSubject { get; set; }
        public string NotificationContent { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public string UpdatedBy { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public bool IsRead { get; set; }
        public bool IsDelete { get; set; }
    }
                                                
}
