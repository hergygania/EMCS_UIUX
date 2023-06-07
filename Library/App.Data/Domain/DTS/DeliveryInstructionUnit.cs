namespace App.Data.Domain
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;
    [Table("dbo.DeliveryInstructionUnit")]
    public class DeliveryInstructionUnit
    {
        public Int64 ID { get; set; }
        public Int64 HeaderID { get; set; }

        [StringLength(255)]
        public string Model { get; set; }
        [StringLength(255)]
        public string SerialNumber { get; set; }
        [StringLength(50)]
        public string Batch { get; set; }

        [StringLength(50)]
        public string CreateBy { get; set; }
        public DateTime? CreateDate { get; set; }
        [StringLength(50)]
        public string UpdateBy { get; set; }
        public DateTime? UpdateDate { get; set; }
        public Decimal? FreightCost { get; set; }
    }
}