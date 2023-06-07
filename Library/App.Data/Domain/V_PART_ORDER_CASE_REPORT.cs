namespace App.Data.Domain
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class V_PART_ORDER_CASE_REPORT
    {
        [Key]
        [Column(Order = 0)]
        [StringLength(50)]
        public string InvoiceNo { get; set; }

        [Key]
        [Column(Order = 1, TypeName = "date")]
        public DateTime InvoiceDate { get; set; }

        [StringLength(30)]
        public string ShippingIDASN { get; set; }

        [Key]
        [Column(Order = 2)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public long CaseID { get; set; }

        [Key]
        [Column(Order = 3)]
        [StringLength(30)]
        public string CaseNo { get; set; }

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

        public long? PartsOrderID { get; set; }

        [Key]
        [Column(Order = 4)]
        public DateTime EntryDate { get; set; }

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
