namespace App.Data.Domain
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("PIS.RptOrderConfirmation")]
    public partial class RptOrderConfirmation
    {
        [Key]
        public int ordcnf_ID { get; set; }

        [StringLength(5)]
        public string ordcnf_Store { get; set; }

        [StringLength(10)]
        public string ordcnf_RefDoc { get; set; }

        [StringLength(20)]
        public string ordcnf_CustPONo { get; set; }

        [Column(TypeName = "date")]
        public DateTime? ordcnf_DocDate { get; set; }

        [StringLength(20)]
        public string ordcnf_DocValue { get; set; }

        public int? ordcnf_LineItemOrder { get; set; }

        public int? ordcnf_LineItemBackorder { get; set; }

        [StringLength(7)]
        public string ordcnf_CustID { get; set; }

        [StringLength(50)]
        public string ordcnf_CustName { get; set; }

        [StringLength(50)]
        public string ordcnf_Remarks { get; set; }

        [StringLength(50)]
        public string ordncf_StoreName { get; set; }

        public int? ordncf_AreaID { get; set; }

        [StringLength(50)]
        public string ordncf_AreaName { get; set; }

        public int? ordncf_HubID { get; set; }

        [StringLength(50)]
        public string ordncf_HubName { get; set; }

        public DateTime ordcnf_CreatedOn { get; set; }

        [Required]
        [StringLength(100)]
        public string ordcnf_CreatedBy { get; set; }

        public DateTime ordcnf_UpdatedOn { get; set; }

        [StringLength(100)]
        public string ordcnf_UpdatedBy { get; set; }
    }
}
