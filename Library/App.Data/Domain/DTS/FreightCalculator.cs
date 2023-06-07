namespace App.Data.Domain
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;
    [Table("dbo.FreightEstimateCalculator")]
    public class FreightCalculator
    {
        public Int64 ID { get; set; }
        [StringLength(100)]
        public string Origin { get; set; }
        [StringLength(100)]
        public string Area { get; set; }
        [StringLength(100)]
        public string Provinsi { get; set; }
        [StringLength(100)]
        public string KabupatenKota { get; set; }
        [StringLength(100)]
        public string Model { get; set; }
        public decimal Value { get; set; }
        [StringLength(50)]
        public string CreateBy { get; set; }
        public DateTime? CreateDate { get; set; }
        [StringLength(50)]
        public string UpdateBy { get; set; }
        public DateTime? UpdateDate { get; set; }
    }
}