namespace App.Data.Domain
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("PIS.RptDocumentReprint")]
    public partial class RptDocumentReprint
    {
        [Key]
        public int docrep_ID { get; set; }

        [StringLength(5)]
        public string docrep_Store { get; set; }

        [StringLength(10)]
        public string docrep_RefDoc { get; set; }

        [StringLength(20)]
        public string docrep_CustPONo { get; set; }

        [Column(TypeName = "date")]
        public DateTime? docrep_DocDate { get; set; }

        [StringLength(20)]
        public string docrep_DocValue { get; set; }

        [StringLength(7)]
        public string docrep_CustID { get; set; }

        [StringLength(50)]
        public string docrep_CustName { get; set; }

        public int? docrep_PrintCount { get; set; }

        [StringLength(50)]
        public string docrep_Remark { get; set; }

        [StringLength(50)]
        public string docrep_StoreName { get; set; }

        public int? docrep_AreaID { get; set; }

        [StringLength(50)]
        public string docrep_AreaName { get; set; }

        public int? docrep_HubID { get; set; }

        [StringLength(50)]
        public string docrep_HubName { get; set; }

        public DateTime docrep_CreatedOn { get; set; }

        [Required]
        [StringLength(100)]
        public string docrep_CreatedBy { get; set; }

        public DateTime docrep_UpdatedOn { get; set; }

        [StringLength(100)]
        public string docrep_UpdatedBy { get; set; }
    }
}
