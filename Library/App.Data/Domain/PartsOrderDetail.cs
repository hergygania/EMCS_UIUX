namespace App.Data.Domain
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("common.PartsOrderDetail")]
    public partial class PartsOrderDetail
    {
        [Key]
        public long DetailID { get; set; }

        public long PartsOrderID { get; set; }

        [Required]
        [StringLength(50)]
        public string InvoiceNo { get; set; }

        [Column(TypeName = "date")]
        public DateTime InvoiceDate { get; set; }

        [StringLength(30)]
        public string PrimPSO { get; set; }

        [StringLength(30)]
        public string CaseNo { get; set; }

        [StringLength(10)]
        public string PartsNumber { get; set; }

        [StringLength(10)]
        public string COO { get; set; }

        [StringLength(50)]
        public string COODescription { get; set; }

        public int? InvoiceItemNo { get; set; }

        [StringLength(30)]
        public string PartsDescriptionShort { get; set; }

        public int? InvoiceItemQty { get; set; }

        [StringLength(50)]
        public string CustomerReff { get; set; }

        public decimal? PartGrossWeight { get; set; }

        public decimal? ChargesDiscountAmount { get; set; }

        [StringLength(5)]
        public string BECode { get; set; }

        [StringLength(3)]
        public string OrderCLSCode { get; set; }

        public int? Profile { get; set; }

        public decimal? UnitPrice { get; set; }

        public int? OMID { get; set; }

        public DateTime EntryDate { get; set; }

        public DateTime ModifiedDate { get; set; }

        [Required]
        [StringLength(20)]
        public string EntryBy { get; set; }

        [Required]
        [StringLength(20)]
        public string ModifiedBy { get; set; }

        [Column(TypeName = "date")]
        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public DateTime? EntryDate_Date { get; set; }

        public byte Status { get; set; }

        public DateTime? EmailDate { get; set; }

        [StringLength(50)]
        public string Source { get; set; }

        public byte? ReturnToVendor { get; set; }

        public int? HSID { get; set; }

        public int? CommodityID { get; set; }

        public int? PartsID { get; set; }

        public string Remark { get; set; }

				//[DatabaseGenerated(DatabaseGeneratedOption.Computed)]
				//[StringLength(3)]
				//public string SOS { get; set; }

				//[DatabaseGenerated(DatabaseGeneratedOption.Computed)]
				//public int? isSurveyed { get; set; }
    }
}
