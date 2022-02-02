namespace App.Data.Domain
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("PIS.RptEmergencyOrder")]
    public partial class RptEmergencyOrder
    {
        [Key]
        public int emgor_ID { get; set; }

        [StringLength(5)]
        public string emgor_Store { get; set; }

        [StringLength(10)]
        public string emgor_RefDoc { get; set; }

        [StringLength(20)]
        public string emgor_CustPONo { get; set; }

        [Column(TypeName = "date")]
        public DateTime? emgor_DocDate { get; set; }

        [StringLength(20)]
        public string emgor_SOS { get; set; }

        [StringLength(20)]
        public string emgor_PartNo { get; set; }

        [StringLength(50)]
        public string emgor_Description { get; set; }

        public int? emgor_BackorderQty { get; set; }

        [StringLength(50)]
        public string emgor_Facility { get; set; }

        [StringLength(50)]
        public string emgor_Comments { get; set; }

        [StringLength(7)]
        public string emgor_CustID { get; set; }

        [StringLength(50)]
        public string emgor_CustName { get; set; }

        [StringLength(50)]
        public string emgor_StoreName { get; set; }

        public int? emgor_AreaID { get; set; }

        [StringLength(50)]
        public string emgor_AreaName { get; set; }

        public int? emgor_HubID { get; set; }

        [StringLength(50)]
        public string emgor_HubName { get; set; }

        public DateTime emgor_CreatedOn { get; set; }

        [Required]
        [StringLength(100)]
        public string emgor_CreatedBy { get; set; }

        public DateTime emgor_UpdatedOn { get; set; }

        [StringLength(100)]
        public string emgor_UpdatedBy { get; set; }

        [Column("emgor_LineNo ")]
        public decimal? emgor_LineNo_ { get; set; }
    }
}
