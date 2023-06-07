namespace App.Data.Domain
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("PIS.RptWHDocumentReprint")]
    public partial class RptWHDocumentReprint
    {
        [Key]
        public int whdocrep_ID { get; set; }

        [StringLength(20)]
        public string whdocrep_DocNo { get; set; }

        [StringLength(20)]
        public string whdocrep_SOS { get; set; }

        [StringLength(20)]
        public string whdocrep_PartNo { get; set; }

        [StringLength(50)]
        public string whdocrep_Description { get; set; }

        public int? whdocrep_PackageQty { get; set; }

        public int? whdocrep_Qty { get; set; }

        [StringLength(20)]
        public string whdocrep_Binloc { get; set; }

        public decimal? whdocrep_SellingPrice { get; set; }

        [Column(TypeName = "date")]
        public DateTime? whdocrep_ReprintDate { get; set; }

        [StringLength(20)]
        public string whdocrep_UserID { get; set; }

        [StringLength(50)]
        public string whdocrep_StoreName { get; set; }

        public int? whdocrep_AreaID { get; set; }

        [StringLength(50)]
        public string whdocrep_AreaName { get; set; }

        public int? whdocrep_HubID { get; set; }

        [StringLength(50)]
        public string whdocrep_HubName { get; set; }

        public DateTime whdocrep_CreatedOn { get; set; }

        [Required]
        [StringLength(100)]
        public string whdocrep_CreatedBy { get; set; }

        public DateTime whdocrep_UpdatedOn { get; set; }

        [StringLength(100)]
        public string whdocrep_UpdatedBy { get; set; }
    }
}
