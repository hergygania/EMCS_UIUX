namespace App.Data.Domain
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("PIS.RptPODtoBOFill")]
    public partial class RptPODtoBOFill
    {
        [Key]
        public int podbo_ID { get; set; }

        [StringLength(20)]
        public string podbo_DocNo { get; set; }

        [StringLength(20)]
        public string podbo_SOS { get; set; }

        [StringLength(20)]
        public string podbo_PartNo { get; set; }

        [StringLength(50)]
        public string podbo_Description { get; set; }

        public int? podbo_PackageQty { get; set; }

        public int? podbo_Qty { get; set; }

        public decimal? podbo_SellingPrice { get; set; }

        public int? podbo_Weight { get; set; }

        public int? podbo_Length { get; set; }

        public int? podbo_Width { get; set; }

        public int? podbo_Height { get; set; }

        public int? podbo_PoDDate { get; set; }

        public DateTime? podbo_ReceiptDateTime { get; set; }

        [StringLength(20)]
        public string podbo_UserID { get; set; }

        public int? podbo_Leadtime { get; set; }

        public int? podbo_BM { get; set; }

        [StringLength(50)]
        public string podbo_StoreName { get; set; }

        public int? podbo_AreaID { get; set; }

        [StringLength(50)]
        public string podbo_AreaName { get; set; }

        public int? podbo_HubID { get; set; }

        [StringLength(50)]
        public string podbo_HubName { get; set; }

        public DateTime podbo_CreatedOn { get; set; }

        [Required]
        [StringLength(100)]
        public string podbo_CreatedBy { get; set; }

        public DateTime podbo_UpdatedOn { get; set; }

        [StringLength(100)]
        public string podbo_UpdatedBy { get; set; }
    }
}
