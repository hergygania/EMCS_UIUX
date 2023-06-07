namespace App.Data.Domain
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("PIS.RptLongIntransit")]
    public partial class RptLongIntransit
    {
        [Key]
        [Column(Order = 0)]
        public int lint_ID { get; set; }

        [Key]
        [Column(Order = 1)]
        public DateTime lint_CreatedOn { get; set; }

        [Key]
        [Column(Order = 2)]
        [StringLength(100)]
        public string lint_CreatedBy { get; set; }

        [Key]
        [Column(Order = 3, TypeName = "date")]
        public DateTime lint_UpdatedOn { get; set; }

        public int? lint_HubID { get; set; }

        public int? lint_AreaID { get; set; }

        [Column(TypeName = "numeric")]
        public decimal? lint_TotalItem { get; set; }

        [Column(TypeName = "numeric")]
        public decimal? lint_TotalValue { get; set; }

        [Column(TypeName = "numeric")]
        public decimal? lint_TotalWeight { get; set; }

        [StringLength(3)]
        public string lint_SOS { get; set; }

        [StringLength(3)]
        public string lint_StoreNo { get; set; }

        [StringLength(50)]
        public string lint_PartsNumber { get; set; }

        [StringLength(50)]
        public string lint_Description { get; set; }

        [StringLength(50)]
        public string lint_Document { get; set; }

        public int? lint_QTY { get; set; }

        [Column(TypeName = "date")]
        public DateTime? lint_OrderDate { get; set; }

        [Column(TypeName = "date")]
        public DateTime? lint_InvoiceDate { get; set; }

        [Column(TypeName = "date")]
        public DateTime? lint_PODDate { get; set; }

        public decimal? lint_UNCS { get; set; }

        public decimal? lint_Weight { get; set; }

        [StringLength(10)]
        public string lint_CaseNo { get; set; }

        [StringLength(50)]
        public string lint_ASNNo { get; set; }

        [StringLength(50)]
        public string lint_InvoiceNo { get; set; }

        public double? lint_BM { get; set; }

        [Column(TypeName = "date")]
        public DateTime? lint_EntryDate { get; set; }

        [StringLength(100)]
        public string lint_UpdatedBy { get; set; }

        [StringLength(3)]
        public string lint_originID { get; set; }

        [StringLength(3)]
        public string lint_destinationID { get; set; }
    }
}
