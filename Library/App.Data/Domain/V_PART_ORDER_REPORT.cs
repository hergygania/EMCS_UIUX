namespace App.Data.Domain
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class V_PART_ORDER_REPORT
    {
        [Key]
        [Column(Order = 0)]
        [StringLength(50)]
        public string InvoiceNo { get; set; }

        [Key]
        [Column(Order = 1, TypeName = "date")]
        public DateTime InvoiceDate { get; set; }

        [Key]
        [Column(Order = 2)]
        [StringLength(7)]
        public string JCode { get; set; }

        [StringLength(100)]
        public string StoreNumber { get; set; }

        public decimal? TotalAmount { get; set; }

        public decimal? TotalFOB { get; set; }

        [Key]
        [Column(Order = 3)]
        public bool IsHazardous { get; set; }

        public decimal? ServiceCharges { get; set; }

        public decimal? CoreDeposit { get; set; }

        public decimal? OtherCharges { get; set; }

        public decimal? FreightCharges { get; set; }

        [StringLength(30)]
        public string ShippingIDASN { get; set; }

        [StringLength(10)]
        public string AgreementType { get; set; }

        [Key]
        [Column(Order = 4)]
        public byte VettingRoute { get; set; }

        [Column(TypeName = "date")]
        public DateTime? SurveyDate { get; set; }

        [Key]
        [Column(Order = 5)]
        public byte Status { get; set; }

        [Column(TypeName = "date")]
        public DateTime? HPLReceiptDate { get; set; }

        [Key]
        [Column(Order = 6)]
        public DateTime EntryDate { get; set; }

        [StringLength(3)]
        public string SOS { get; set; }

        [Key]
        [Column(Order = 7)]
        public DateTime ModifiedDate { get; set; }

        [Key]
        [Column(Order = 8)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public long PartsOrderID { get; set; }

        [Key]
        [Column(Order = 9)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int ShippingInstructionID { get; set; }

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
