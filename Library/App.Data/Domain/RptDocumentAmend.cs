namespace App.Data.Domain
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("PIS.RptDocumentAmend")]
    public partial class RptDocumentAmend
    {
        [Key]
        public int amdoc_ID { get; set; }

        [StringLength(20)]
        public string amdoc_Type { get; set; }

        [StringLength(20)]
        public string amdoc_ST1 { get; set; }

        [StringLength(20)]
        public string amdoc_QYHNDST1 { get; set; }

        [StringLength(20)]
        public string amdoc_TAST1 { get; set; }

        [StringLength(20)]
        public string amdoc_StatusStockST1 { get; set; }

        [StringLength(20)]
        public string amdoc_STNo { get; set; }

        [StringLength(20)]
        public string amdoc_DocNo { get; set; }

        [StringLength(20)]
        public string amdoc_SOS { get; set; }

        [StringLength(20)]
        public string amdoc_PartNo { get; set; }

        public int? amdoc_Qty1 { get; set; }

        [StringLength(20)]
        public string amdoc_StatusStockSTNo { get; set; }

        [StringLength(20)]
        public string amdoc_LSACJ8 { get; set; }

        [StringLength(20)]
        public string amdoc_QYHND { get; set; }

        [StringLength(20)]
        public string amdoc_QYOR { get; set; }

        [StringLength(20)]
        public string amdoc_QYPCS { get; set; }

        [StringLength(20)]
        public string amdoc_RTQYPC { get; set; }

        [StringLength(50)]
        public string amdoc_Note { get; set; }

        [StringLength(20)]
        public string amdoc_UserID { get; set; }

        [StringLength(20)]
        public string amdoc_PXQY2 { get; set; }

        [StringLength(50)]
        public string amdoc_StoreName { get; set; }

        public int? amdoc_AreaID { get; set; }

        [StringLength(50)]
        public string amdoc_AreaName { get; set; }

        public int? amdoc_HubID { get; set; }

        [StringLength(50)]
        public string amdoc_HubName { get; set; }

        public DateTime amdoc_CreatedOn { get; set; }

        [Required]
        [StringLength(100)]
        public string amdoc_CreatedBy { get; set; }

        public DateTime amdoc_UpdatedOn { get; set; }

        [StringLength(100)]
        public string amdoc_UpdatedBy { get; set; }
    }
}
