namespace App.Data.Domain
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("PIS.RptBackorderSubmission")]
    public partial class RptBackorderSubmission
    {
        [Key]
        public int bcksms_ID { get; set; }

        [StringLength(5)]
        public string bcksms_Store { get; set; }

        [StringLength(10)]
        public string bcksms_RefDoc { get; set; }

        [StringLength(20)]
        public string bcksms_CustPONo { get; set; }

        [Column(TypeName = "date")]
        public DateTime? bcksms_DocDate { get; set; }

        [StringLength(20)]
        public string bcksms_SOS { get; set; }

        [StringLength(20)]
        public string bcksms_PartNo { get; set; }

        [StringLength(50)]
        public string bcksms_Description { get; set; }

        public int? bcksms_BackorderQty { get; set; }

        [StringLength(50)]
        public string bcksms_Facility { get; set; }

        [StringLength(20)]
        public string bcksms_TransferOrdNo { get; set; }

        [StringLength(20)]
        public string bcksms_FacOrdNo { get; set; }

        [StringLength(50)]
        public string bcksms_Comments { get; set; }

        [StringLength(7)]
        public string bcksms_CustID { get; set; }

        [StringLength(50)]
        public string bcksms_CustName { get; set; }

        [StringLength(50)]
        public string bcksms_StoreName { get; set; }

        public int? bcksms_AreaID { get; set; }

        [StringLength(50)]
        public string bcksms_AreaName { get; set; }

        public int? bcksms_HubID { get; set; }

        [StringLength(50)]
        public string bcksms_HubName { get; set; }

        public DateTime bcksms_CreatedOn { get; set; }

        [Required]
        [StringLength(100)]
        public string bcksms_CreatedBy { get; set; }

        public DateTime bcksms_UpdatedOn { get; set; }

        [StringLength(100)]
        public string bcksms_UpdatedBy { get; set; }

        public decimal? bcksms_LineNo { get; set; }
    }
}
