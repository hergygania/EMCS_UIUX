namespace App.Data.Domain
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("PIS.RptPartReleaseInformation")]
    public partial class RptPartReleaseInformation
    {
        [Key]
        public int rpartrel_ID { get; set; }

        [StringLength(5)]
        public string rpartrel_Store { get; set; }

        [StringLength(10)]
        public string rpartrel_RefDoc { get; set; }

        [StringLength(20)]
        public string rpartrel_CustPONo { get; set; }

        [Column(TypeName = "date")]
        public DateTime? rpartrel_DocDate { get; set; }

        [StringLength(20)]
        public string rpartrel_SOS { get; set; }

        [StringLength(20)]
        public string rpartrel_PartNo { get; set; }

        [StringLength(50)]
        public string rpartrel_Description { get; set; }

        public int? rpartrel_TotalOrderQty { get; set; }

        public int? rpartrel_ShippedQty { get; set; }

        public int? rpartrel_BackorderQty { get; set; }

        public int? rpartrel_PartialShippedQty { get; set; }

        [StringLength(7)]
        public string rpartrel_CustID { get; set; }

        [StringLength(50)]
        public string rpartrel_CustName { get; set; }

        [StringLength(50)]
        public string rpartrel_StoreName { get; set; }

        public int? rpartrel_AreaID { get; set; }

        [StringLength(50)]
        public string rpartrel_AreaName { get; set; }

        public int? rpartrel_HubID { get; set; }

        [StringLength(50)]
        public string rpartrel_HubName { get; set; }

        public DateTime rpartrel_CreatedOn { get; set; }

        [Required]
        [StringLength(100)]
        public string rpartrel_CreatedBy { get; set; }

        public DateTime rpartrel_UpdatedOn { get; set; }

        [StringLength(100)]
        public string rpartrel_UpdatedBy { get; set; }
    }
}
