namespace App.Data.Domain
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("PIS.RptSTRespondTime")]
    public partial class RptSTRespondTime
    {
        [Key]
        public int strsp_ID { get; set; }

        [StringLength(20)]
        public string strsp_PartNo { get; set; }

        [StringLength(50)]
        public string strsp_Description { get; set; }

        public int? strsp_PackageQty { get; set; }

        public int? strsp_Qty { get; set; }

        [StringLength(20)]
        public string strsp_Binloc { get; set; }

        public int? strsp_Weight { get; set; }

        public int? strsp_Length { get; set; }

        public int? strsp_Width { get; set; }

        public int? strsp_Height { get; set; }

        public DateTime? strsp_BOSubmissionDatetime { get; set; }

        [Column(TypeName = "date")]
        public DateTime? strsp_PickupDate { get; set; }

        public int? strsp_Leadtime { get; set; }

        public int? strsp_BM { get; set; }

        [StringLength(50)]
        public string strsp_StoreName { get; set; }

        public int? strsp_AreaID { get; set; }

        [StringLength(50)]
        public string strsp_AreaName { get; set; }

        public int? strsp_HubID { get; set; }

        [StringLength(50)]
        public string strsp_HubName { get; set; }

        public DateTime strsp_CreatedOn { get; set; }

        [Required]
        [StringLength(100)]
        public string strsp_CreatedBy { get; set; }

        public DateTime strsp_UpdatedOn { get; set; }

        [StringLength(100)]
        public string strsp_UpdatedBy { get; set; }
    }
}
