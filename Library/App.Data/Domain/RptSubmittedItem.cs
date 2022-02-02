namespace App.Data.Domain
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("PIS.RptSubmittedItem")]
    public partial class RptSubmittedItem
    {
        [Key]
        [Column(Order = 0)]
        public int sbmitm_ID { get; set; }

        [Key]
        [Column(Order = 1)]
        public DateTime sbmitm_CreatedOn { get; set; }

        [Key]
        [Column(Order = 2)]
        [StringLength(100)]
        public string sbmitm_CreatedBy { get; set; }

        [Key]
        [Column(Order = 3, TypeName = "date")]
        public DateTime sbmitm_UpdatedOn { get; set; }

        public int? sbmitm_HubID { get; set; }

        public int? sbmitm_AreaID { get; set; }

        [Column(TypeName = "numeric")]
        public decimal? sbmitm_TotalItem { get; set; }

        [Column(TypeName = "numeric")]
        public decimal? sbmitm_TotalValue { get; set; }

        [Column(TypeName = "numeric")]
        public decimal? sbmitm_TotalWeight { get; set; }

        [StringLength(3)]
        public string sbmitm_SOS { get; set; }

        [StringLength(3)]
        public string sbmitm_StoreNo { get; set; }

        [StringLength(50)]
        public string sbmitm_PartNumber { get; set; }

        [StringLength(50)]
        public string sbmitm_Description { get; set; }

        [StringLength(10)]
        public string sbmitm_Document { get; set; }

        [Column(TypeName = "date")]
        public DateTime? sbmitm_EntryDate { get; set; }

        [StringLength(3)]
        public string sbmitm_TRXCD1 { get; set; }

        public int? sbmitm_QTY { get; set; }

        public decimal? sbmitm_UNCS { get; set; }

        public decimal? sbmitm_GRSSWT { get; set; }

        [StringLength(100)]
        public string sbmitm_UpdatedBy { get; set; }
    }
}
