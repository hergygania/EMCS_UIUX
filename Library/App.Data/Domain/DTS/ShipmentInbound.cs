namespace App.Data.Domain
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;
    [Table("dbo.ShipmentInbound")]
    public class ShipmentInbound
    {
        [StringLength(50)]
        public string AjuNo { get; set; }
        [StringLength(50)]
        public string MSONo { get; set; }
        [Key]
        [Required]
        [StringLength(50)]
        public string PONo { get; set; }

        public DateTime? PODate { get; set; }
        [StringLength(50)]
        public string LoadingPort { get; set; }
        [StringLength(100)]
        public string DischargePort { get; set; }
        [StringLength(100)]
        public string Model { get; set; }
        [StringLength(255)]
        public string ModelDescription { get; set; }
        [StringLength(50)]
        public string Status { get; set; }
        [StringLength(50)]
        public string SerialNumber { get; set; }
        [StringLength(255)]
        public string Notes { get; set; }
        [StringLength(50)]
        public string Position { get; set; }
        [StringLength(50)]
        public string BatchNumber { get; set; }
        [StringLength(255)]
        public string Remark { get; set; }
        public DateTime? ATACakung { get; set; }
        public DateTime? ETACakung { get; set; }
        public DateTime? ATAPort { get; set; }
        public DateTime? ETAPort { get; set; }
        public bool IsCkb { get; set; }
        [StringLength(50)]
        public string CreateBy { get; set; }
        public DateTime? CreateDate { get; set; }
        [StringLength(50)]
        public string UpdateBy { get; set; }
        public DateTime? UpdateDate { get; set; }
        public string Plant { get; set; }
    }
}