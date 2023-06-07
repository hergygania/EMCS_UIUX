namespace App.Data.Domain
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("PIS.RptDocumentNonConsolidateInvoice")]
    public partial class RptDocumentNonConsolidateInvoice
    {
        [Key]
        public int rncinv_ID { get; set; }

        [StringLength(5)]
        public string rncinv_Store { get; set; }

        [StringLength(10)]
        public string rncinv_RefDoc { get; set; }

        [StringLength(20)]
        public string rncinv_CustPONo { get; set; }

        [Column(TypeName = "date")]
        public DateTime? rncinv_DocDate { get; set; }

        [StringLength(20)]
        public string rncinv_DocValue { get; set; }

        [StringLength(7)]
        public string rncinv_CustID { get; set; }

        [StringLength(50)]
        public string rncinv_CustName { get; set; }

        [StringLength(50)]
        public string rncinv_StoreName { get; set; }

        public int? rncinv_AreaID { get; set; }

        [StringLength(50)]
        public string rncinv_AreaName { get; set; }

        public int? rncinv_HubID { get; set; }

        [StringLength(50)]
        public string rncinv_HubName { get; set; }

        public DateTime rncinv_CreatedOn { get; set; }

        [Required]
        [StringLength(100)]
        public string rncinv_CreatedBy { get; set; }

        public DateTime rncinv_UpdatedOn { get; set; }

        [StringLength(100)]
        public string rncinv_UpdatedBy { get; set; }
    }
}
