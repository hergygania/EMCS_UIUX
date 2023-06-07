namespace WindowService.Library.Model
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("common.BOMIT")]
    public partial class BOMIT
    {
        public long BOMITID { get; set; }

        [StringLength(7)]
        public string JCode { get; set; }

        [StringLength(10)]
        public string PrimPSO { get; set; }

        [StringLength(100)]
        public string OrderReference { get; set; }

        [StringLength(2)]
        public string FMS { get; set; }

        [StringLength(10)]
        public string PartsNumber { get; set; }

        [StringLength(50)]
        public string PartsName { get; set; }

        public int? Pending { get; set; }

        [StringLength(2)]
        public string Class { get; set; }

        [StringLength(4)]
        public string TransportedThrough { get; set; }

        [Column(TypeName = "date")]
        public DateTime? OEDate { get; set; }

        public int? TotalMIT { get; set; }

        public int? NextReceiptQty { get; set; }

        [Column(TypeName = "date")]
        public DateTime? NextReceiptDate { get; set; }

        public int? TotalBO { get; set; }

        [StringLength(50)]
        public string FRZ { get; set; }

        public decimal? UnitWeight { get; set; }

        public decimal? EXT_WT { get; set; }

        public decimal? UNIT_DN { get; set; }

        public decimal? EXT_DN { get; set; }

        [StringLength(50)]
        public string AgreementType { get; set; }

        public int? BO_AGE { get; set; }

        [StringLength(50)]
        public string FU_DT_ORD { get; set; }

        [Column(TypeName = "date")]
        public DateTime? DueDate { get; set; }

        public DateTime EntryDate { get; set; }

        public DateTime ModifiedDate { get; set; }

        [Required]
        [StringLength(20)]
        public string EntryBy { get; set; }

        [Required]
        [StringLength(20)]
        public string ModifiedBy { get; set; }

        public DateTime? EmailDate { get; set; }

        [StringLength(50)]
        public string Source { get; set; }

        [Column(TypeName = "date")]
        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public DateTime? EntryDate_Date { get; set; }
    }
}
