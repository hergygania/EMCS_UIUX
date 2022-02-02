namespace WindowService.Library.Model
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("common.PartsOrder")]
    public partial class PartsOrder
    {
        public long PartsOrderID { get; set; }

        [Required]
        [StringLength(50)]
        public string InvoiceNo { get; set; }

        [Column(TypeName = "date")]
        public DateTime InvoiceDate { get; set; }

        [Required]
        [StringLength(7)]
        public string JCode { get; set; }

        [StringLength(100)]
        public string StoreNumber { get; set; }

        public int ShippingInstructionID { get; set; }

        public decimal? TotalAmount { get; set; }

        public decimal? TotalFOB { get; set; }

        public bool IsHazardous { get; set; }

        public decimal? ServiceCharges { get; set; }

        public decimal? CoreDeposit { get; set; }

        public decimal? OtherCharges { get; set; }

        public decimal? FreightCharges { get; set; }

        [StringLength(30)]
        public string ShippingIDASN { get; set; }

        [StringLength(20)]
        public string AgreementType { get; set; }

        public byte VettingRoute { get; set; }

        [Column(TypeName = "date")]
        public DateTime? SurveyDate { get; set; }

        public byte Status { get; set; }

        [StringLength(3)]
        public string SOS { get; set; }

        [Column(TypeName = "date")]
        public DateTime? HPLReceiptDate { get; set; }

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

        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public int isDisplay { get; set; }

        [StringLength(30)]
        public string DA { get; set; }
    }
}
