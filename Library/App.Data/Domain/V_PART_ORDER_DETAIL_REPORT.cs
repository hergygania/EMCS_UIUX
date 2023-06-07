namespace App.Data.Domain
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class V_PART_ORDER_DETAIL_REPORT
    {
        [Key]
        [Column(Order = 0)]
        [StringLength(50)]
        public string InvoiceNo { get; set; }

        [Key]
        [Column(Order = 1, TypeName = "date")]
        public DateTime InvoiceDate { get; set; }

        [StringLength(30)]
        public string PrimPSO { get; set; }

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

        [Key]
        [Column(Order = 2)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public long DetailID { get; set; }

        [Key]
        [Column(Order = 3)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public long PartsOrderID { get; set; }

        [StringLength(30)]
        public string CaseNo { get; set; }

        [StringLength(3)]
        public string OrderCLSCode { get; set; }

        [Key]
        [Column(Order = 4)]
        public DateTime EntryDate { get; set; }

        [StringLength(3)]
        public string SOS { get; set; }

        public int? Profile { get; set; }

        public decimal? UnitPrice { get; set; }

        public int? OMID { get; set; }

        [StringLength(255)]
        public string StoreName { get; set; }

        [StringLength(3)]
        public string StoreID { get; set; }

        public int? HubID { get; set; }

        [StringLength(255)]
        public string HubName { get; set; }

        public int? AreaID { get; set; }

        [StringLength(255)]
        public string AreaName { get; set; }
    }
}
