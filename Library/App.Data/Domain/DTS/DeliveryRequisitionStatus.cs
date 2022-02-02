namespace App.Data.Domain
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;
    [Table("dbo.DeliveryRequisitionStatus")]
    public class DeliveryRequisitionStatus
    {
        [Key]
        public long ID { get; set; }
        public long HeaderID { get; set; }
        [StringLength(50)]
        public string RefNo { get; set; }
        public long RefItemId { get; set; }

        [StringLength(4)]
        public string Action { get; set; }
        [StringLength(4)]
        public string Status { get; set; }
        [StringLength(100)]
        public string Position { get; set; }
        [StringLength(255)]
        public string Notes { get; set; }
        [StringLength(1000)]
        public string Attachment1 { get; set; }
        [StringLength(1000)]
        public string Attachment2 { get; set; }
        [StringLength(1000)]
        public string LogDescription { get; set; }

        public DateTime? EstTimeDeparture { get; set; }
        public DateTime? EstTimeArrival { get; set; }
        public DateTime? ActTimeDeparture { get; set; }
        public DateTime? ActTimeArrival { get; set; }


        [StringLength(50)]
        public string CreateBy { get; set; }
        public DateTime? CreateDate { get; set; }
        [StringLength(50)]
        public string UpdateBy { get; set; }
        public DateTime? UpdateDate { get; set; }
    }
}