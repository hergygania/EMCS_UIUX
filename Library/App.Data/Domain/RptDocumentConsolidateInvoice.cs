namespace App.Data.Domain
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("PIS.RptDocumentConsolidateInvoice")]
    public partial class RptDocumentConsolidateInvoice
    {
        [Key]
        public int rcinv_ID { get; set; }

        [StringLength(5)]
        public string rcinv_Store { get; set; }

        [StringLength(10)]
        public string rcinv_RefDoc { get; set; }

        [StringLength(20)]
        public string rcinv_CustPONo { get; set; }

        [Column(TypeName = "date")]
        public DateTime? rcinv_DocDate { get; set; }

        [StringLength(20)]
        public string rcinv_DocValue { get; set; }

        [StringLength(7)]
        public string rcinv_CustID { get; set; }

        [StringLength(50)]
        public string rcinv_CustName { get; set; }

        [StringLength(50)]
        public string rcinv_StoreName { get; set; }

        public int? rcinv_AreaID { get; set; }

        [StringLength(50)]
        public string rcinv_AreaName { get; set; }

        public int? rcinv_HubID { get; set; }

        [StringLength(50)]
        public string rcinv_HubName { get; set; }

        public DateTime rcinv_CreatedOn { get; set; }

        [Required]
        [StringLength(100)]
        public string rcinv_CreatedBy { get; set; }

        public DateTime rcinv_UpdatedOn { get; set; }

        [StringLength(100)]
        public string rcinv_UpdatedBy { get; set; }
    }
}
