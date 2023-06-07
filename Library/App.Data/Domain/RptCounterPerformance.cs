namespace App.Data.Domain
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("PIS.RptCounterPerformance")]
    public partial class RptCounterPerformance
    {
        [Key]
        public int ctprf_ID { get; set; }

        [StringLength(5)]
        public string ctprf_Store { get; set; }

        [StringLength(10)]
        public string ctprf_RefDoc { get; set; }

        [Column(TypeName = "date")]
        public DateTime? ctprf_DocDate { get; set; }

        public int? ctprf_LineItemCount { get; set; }

        [StringLength(20)]
        public string ctprf_DBSUserID { get; set; }

        [StringLength(20)]
        public string ctprf_DBSUserName { get; set; }

        [StringLength(50)]
        public string ctprf_StoreName { get; set; }

        public int? ctprf_AreaID { get; set; }

        [StringLength(50)]
        public string ctprf_AreaName { get; set; }

        public int? ctprf_HubID { get; set; }

        [StringLength(50)]
        public string ctprf_HubName { get; set; }

        public DateTime ctprf_CreatedOn { get; set; }

        [Required]
        [StringLength(100)]
        public string ctprf_CreatedBy { get; set; }

        public DateTime ctprf_UpdatedOn { get; set; }

        [StringLength(100)]
        public string ctprf_UpdatedBy { get; set; }
    }
}
