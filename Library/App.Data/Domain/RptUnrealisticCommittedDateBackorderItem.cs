namespace App.Data.Domain
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("PIS.RptUnrealisticCommittedDateBackorderItem")]
    public partial class RptUnrealisticCommittedDateBackorderItem
    {
        [Key]
        public int ucdbi_ID { get; set; }

        [StringLength(5)]
        public string ucdbi_Store { get; set; }

        [StringLength(10)]
        public string ucdbi_RefDoc { get; set; }

        [StringLength(20)]
        public string ucdbi_CustPONo { get; set; }

        [StringLength(10)]
        public string ucdbi_DocDate { get; set; }

        [StringLength(20)]
        public string ucdbi_DocValue { get; set; }

        [Column(TypeName = "date")]
        public DateTime? ucdbi_CommittedDate { get; set; }

        [StringLength(20)]
        public string ucdbi_SOS { get; set; }

        [StringLength(20)]
        public string ucdbi_PartNo { get; set; }

        [StringLength(20)]
        public string ucdbi_OrderMethod { get; set; }

        [StringLength(50)]
        public string ucdbi_Facility { get; set; }

        [StringLength(10)]
        public string ucdbi_LeadTime { get; set; }

        [StringLength(10)]
        public string ucdbi_GapOfDate { get; set; }

        [StringLength(7)]
        public string ucdbi_CustID { get; set; }

        [StringLength(50)]
        public string ucdbi_CustName { get; set; }

        [StringLength(50)]
        public string ucdbi_StoreName { get; set; }

        public int? ucdbi_AreaID { get; set; }

        [StringLength(50)]
        public string ucdbi_AreaName { get; set; }

        public int? ucdbi_HubID { get; set; }

        [StringLength(50)]
        public string ucdbi_HubName { get; set; }

        public DateTime ucdbi_CreatedOn { get; set; }

        [Required]
        [StringLength(100)]
        public string ucdbi_CreatedBy { get; set; }

        public DateTime ucdbi_UpdatedOn { get; set; }

        [StringLength(100)]
        public string ucdbi_UpdatedBy { get; set; }

        public decimal? ucdbi_LineNo { get; set; }
    }
}
