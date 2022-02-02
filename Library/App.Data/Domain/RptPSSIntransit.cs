namespace App.Data.Domain
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("PIS.RptPSSIntransit")]
    public partial class RptPSSIntransit
    {
        [Key]
        public int pss_ID { get; set; }

        [StringLength(9)]
        public string pss_CaseNo { get; set; }

        [StringLength(18)]
        public string pss_ReffDoc { get; set; }

        [StringLength(18)]
        public string pss_Description { get; set; }

        public int? pss_ITQty { get; set; }

        public int? pss_Max { get; set; }

        public int? pss_Min { get; set; }

        [Column(TypeName = "date")]
        public DateTime? pss_OrderDate { get; set; }

        [StringLength(20)]
        public string pss_PartNo { get; set; }

        public int? pss_OnHand { get; set; }

        public int? pss_OnOrder { get; set; }

        public int? pss_InProcess { get; set; }

        [StringLength(20)]
        public string pss_SOS { get; set; }

        [StringLength(5)]
        public string pss_Store { get; set; }

        [StringLength(10)]
        public string pss_DocRef { get; set; }

        [StringLength(20)]
        public string pss_DA { get; set; }

        [Column(TypeName = "date")]
        public DateTime? pss_PUPDate { get; set; }

        [Column(TypeName = "date")]
        public DateTime? pss_PODDate { get; set; }

        [Column(TypeName = "date")]
        public DateTime? pss_StationOutbound { get; set; }

        [Column(TypeName = "date")]
        public DateTime? pss_TransitOutbound { get; set; }

        [Column(TypeName = "date")]
        public DateTime? pss_TransitInbound { get; set; }

        [StringLength(100)]
        public string pss_Origin { get; set; }

        [StringLength(10)]
        public string pss_OriginID { get; set; }

        [StringLength(100)]
        public string pss_Destination { get; set; }

        [StringLength(50)]
        public string pss_ServiceID { get; set; }

        [StringLength(20)]
        public string pss_LastStatus { get; set; }

        [StringLength(20)]
        public string pss_Status { get; set; }

        public int? pss_BM { get; set; }

        public int? pss_ELA { get; set; }

        public int? pss_OTIF { get; set; }

        [StringLength(50)]
        public string pss_StoreName { get; set; }

        public int? pss_AreaID { get; set; }

        [StringLength(50)]
        public string pss_AreaName { get; set; }

        public int? pss_HubID { get; set; }

        [StringLength(50)]
        public string pss_HubName { get; set; }

        public DateTime pss_CreatedOn { get; set; }

        [Required]
        [StringLength(100)]
        public string pss_CreatedBy { get; set; }

        public DateTime pss_UpdatedOn { get; set; }

        [StringLength(100)]
        public string pss_UpdatedBy { get; set; }
    }
}
