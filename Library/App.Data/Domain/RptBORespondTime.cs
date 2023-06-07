namespace App.Data.Domain
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("PIS.RptBORespondTime")]
    public partial class RptBORespondTime
    {
        [Key]
        public int borsp_ID { get; set; }

        [StringLength(20)]
        public string borsp_PartNo { get; set; }

        [StringLength(50)]
        public string borsp_Description { get; set; }

        public int? borsp_PackageQty { get; set; }

        public int? borsp_Qty { get; set; }

        [StringLength(20)]
        public string borsp_Binloc { get; set; }

        public int? borsp_Weight { get; set; }

        public int? borsp_Length { get; set; }

        public int? borsp_Width { get; set; }

        public int? borsp_Height { get; set; }

        public DateTime? borsp_BOSubmissionDatetime { get; set; }

        [Column(TypeName = "date")]
        public DateTime? borsp_PickupDate { get; set; }

        public int? borsp_Leadtime { get; set; }

        public int? borsp_BM { get; set; }

        [StringLength(50)]
        public string borsp_StoreName { get; set; }

        public int? borsp_AreaID { get; set; }

        [StringLength(50)]
        public string borsp_AreaName { get; set; }

        public int? borsp_HubID { get; set; }

        [StringLength(50)]
        public string borsp_HubName { get; set; }

        public DateTime borsp_CreatedOn { get; set; }

        [Required]
        [StringLength(100)]
        public string borsp_CreatedBy { get; set; }

        public DateTime borsp_UpdatedOn { get; set; }

        [StringLength(100)]
        public string borsp_UpdatedBy { get; set; }
    }
}
