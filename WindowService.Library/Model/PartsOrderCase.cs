namespace WindowService.Library.Model
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("common.PartsOrderCase")]
    public partial class PartsOrderCase
    {
        [Key]
        public long CaseID { get; set; }

        public long? PartsOrderID { get; set; }

        [Required]
        [StringLength(50)]
        public string InvoiceNo { get; set; }

        [Column(TypeName = "date")]
        public DateTime InvoiceDate { get; set; }

        [Required]
        [StringLength(30)]
        public string CaseNo { get; set; }

        [StringLength(30)]
        public string ShippingIDASN { get; set; }

        [StringLength(30)]
        public string CaseType { get; set; }

        [StringLength(50)]
        public string CaseDescription { get; set; }

        public decimal? WeightKG { get; set; }

        public decimal? LengthCM { get; set; }

        public decimal? WideCM { get; set; }

        public decimal? HeightCM { get; set; }

        [StringLength(50)]
        public string RouteID { get; set; }

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
