namespace App.Data.Domain
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("PIS.RptDocumentPendingRelease")]
    public partial class RptDocumentPendingRelease
    {
        [Key]
        public int rpnd_ID { get; set; }

        [StringLength(5)]
        public string rpnd_Store { get; set; }

        [StringLength(10)]
        public string rpnd_RefDoc { get; set; }

        [StringLength(20)]
        public string rpnd_CustPONo { get; set; }

        [Column(TypeName = "date")]
        public DateTime? rpnd_DocDate { get; set; }

        [StringLength(20)]
        public string rpnd_DocValue { get; set; }

        [StringLength(7)]
        public string rpnd_CustID { get; set; }

        [StringLength(50)]
        public string rpnd_CustName { get; set; }

        [StringLength(50)]
        public string rpnd_Remarks { get; set; }

        [StringLength(50)]
        public string rpnd_StoreName { get; set; }

        public int? rpnd_AreaID { get; set; }

        [StringLength(50)]
        public string rpnd_AreaName { get; set; }

        public int? rpnd_HubID { get; set; }

        [StringLength(50)]
        public string rpnd_HubName { get; set; }

        public DateTime rpnd_CreatedOn { get; set; }

        [Required]
        [StringLength(100)]
        public string rpnd_CreatedBy { get; set; }

        public DateTime rpnd_UpdatedOn { get; set; }

        [StringLength(100)]
        public string rpnd_UpdatedBy { get; set; }
    }
}
