namespace App.Data.Domain
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("PIS.RptVendorConstraint")]
    public partial class RptVendorConstraint
    {
        [Key]
        [Column(Order = 0)]
        public int vcon_ID { get; set; }

        [Key]
        [Column(Order = 1)]
        public DateTime vcon_CreatedOn { get; set; }

        [Key]
        [Column(Order = 2)]
        [StringLength(100)]
        public string vcon_CreatedBy { get; set; }

        [Key]
        [Column(Order = 3, TypeName = "date")]
        public DateTime vcon_UpdatedOn { get; set; }

        public int? vcon_HubID { get; set; }

        public int? vcon_AreaID { get; set; }

        [StringLength(3)]
        public string vcon_StoreNo { get; set; }

        [Column(TypeName = "numeric")]
        public decimal? vcon_TotalItem { get; set; }

        [Column(TypeName = "numeric")]
        public decimal? vcon_TotalValue { get; set; }

        [Column(TypeName = "numeric")]
        public decimal? vcon_TotalWeight { get; set; }

        [StringLength(50)]
        public string vcon_RPORNE { get; set; }

        [StringLength(3)]
        public string vcon_SOS { get; set; }

        [StringLength(50)]
        public string vcon_PartsNumber { get; set; }

        [StringLength(50)]
        public string vcon_Description { get; set; }

        [StringLength(50)]
        public string vcon_Document { get; set; }

        public int? vcon_QTY { get; set; }

        [Column(TypeName = "date")]
        public DateTime? vcon_OrderDate { get; set; }

        [Column(TypeName = "date")]
        public DateTime? vcon_InvoiceDate { get; set; }

        public decimal? vcon_UNCS { get; set; }

        public decimal? vcon_Weight { get; set; }

        [StringLength(10)]
        public string vcon_CaseNo { get; set; }

        [StringLength(50)]
        public string vcon_ASNNo { get; set; }

        [StringLength(50)]
        public string vcon_InvoiceNo { get; set; }

        [Column(TypeName = "date")]
        public DateTime? vcon_EntryDate { get; set; }

        [StringLength(100)]
        public string vcon_UpdatedBy { get; set; }
    }
}
