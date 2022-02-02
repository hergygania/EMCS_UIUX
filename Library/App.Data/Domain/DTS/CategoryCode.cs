namespace App.Data.Domain
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;
    [Table("dbo.CategoryCode")]

    public class CategoryCode
    {
        [Key, Column(Order = 0)]
        [StringLength(4)]
        public string Category { get; set; }
        [Key, Column(Order = 1)]
        [StringLength(4)]
        public string Code { get; set; }
        [StringLength(255)]
        public string Description1 { get; set; }
        [StringLength(255)]
        public string Description2 { get; set; }
        public int Ordering { get; set; }
        [StringLength(50)]
        public string CreateBy { get; set; }
        public DateTime? CreateDate { get; set; }
        [StringLength(50)]
        public string UpdateBy { get; set; }
        public DateTime? UpdateDate { get; set; }
    }
}