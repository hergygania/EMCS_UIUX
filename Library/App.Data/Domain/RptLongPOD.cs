namespace App.Data.Domain
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("PIS.RptLongPOD")]
    public partial class RptLongPOD
    {
        [Key]
        [Column(Order = 0)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int lpod_ID { get; set; }

        [Key]
        [Column(Order = 1)]
        public DateTime lpod_CreatedOn { get; set; }

        [Key]
        [Column(Order = 2)]
        [StringLength(100)]
        public string lpod_CreatedBy { get; set; }

        [Key]
        [Column(Order = 3, TypeName = "date")]
        public DateTime lpod_UpdatedOn { get; set; }

        public int? lpod_HubID { get; set; }

        public int? lpod_AreaID { get; set; }

        [Column(TypeName = "numeric")]
        public decimal? lpod_TotalItem { get; set; }

        [Column(TypeName = "numeric")]
        public decimal? lpod_TotalValue { get; set; }

        [Column(TypeName = "numeric")]
        public decimal? lpod_TotalWeight { get; set; }

        [StringLength(3)]
        public string lpod_StoreNo { get; set; }

        [StringLength(3)]
        public string lpod_SOS { get; set; }

        [StringLength(50)]
        public string lpod_PartsNumber { get; set; }

        [StringLength(50)]
        public string lpod_Description { get; set; }

        [StringLength(50)]
        public string lpod_Document { get; set; }

        public int? lpod_QTY { get; set; }

        [Column(TypeName = "date")]
        public DateTime? lpod_OrderDate { get; set; }

        [Column(TypeName = "date")]
        public DateTime? lpod_InvoiceDate { get; set; }

        [Column(TypeName = "date")]
        public DateTime? lpod_PODDate { get; set; }

        [Column(TypeName = "date")]
        public DateTime? lpod_RGDate { get; set; }

        public decimal? lpod_UNCS { get; set; }

        public decimal? lpod_Weight { get; set; }

        [StringLength(10)]
        public string lpod_CaseNo { get; set; }

        [StringLength(50)]
        public string lpod_ASNNo { get; set; }

        [StringLength(50)]
        public string lpod_InvoiceNo { get; set; }

        [Column(TypeName = "date")]
        public DateTime? lpod_EntryDate { get; set; }

        [StringLength(100)]
        public string lpod_UpdatedBy { get; set; }
    }
}
