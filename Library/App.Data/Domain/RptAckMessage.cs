namespace App.Data.Domain
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("PIS.RptAckMessage")]
    public partial class RptAckMessage
    {
        [Key]
        [Column(Order = 0)]
        public int ackm_ID { get; set; }

        [Key]
        [Column(Order = 1)]
        public DateTime ackm_CreatedOn { get; set; }

        [Key]
        [Column(Order = 2)]
        [StringLength(100)]
        public string ackm_CreatedBy { get; set; }

        [Key]
        [Column(Order = 3, TypeName = "date")]
        public DateTime ackm_UpdatedOn { get; set; }

        public int? ackm_HubID { get; set; }

        public int? ackm_AreaID { get; set; }

        [StringLength(3)]
        public string ackm_StoreNo { get; set; }

        [Column(TypeName = "numeric")]
        public decimal? ackm_TotalItem { get; set; }

        [Column(TypeName = "numeric")]
        public decimal? ackm_TotalValue { get; set; }

        [Column(TypeName = "numeric")]
        public decimal? ackm_TotalWeight { get; set; }

        [StringLength(3)]
        public string ackm_SOS { get; set; }

        [StringLength(50)]
        public string ackm_PartsNumber { get; set; }

        [StringLength(50)]
        public string ackm_Description { get; set; }

        [StringLength(10)]
        public string ackm_Document { get; set; }

        public decimal? ackm_UNCS { get; set; }

        public decimal? ackm_GRSSWT { get; set; }

        [StringLength(100)]
        public string ackm_Message { get; set; }

        [StringLength(100)]
        public string ackm_UpdatedBy { get; set; }
    }
}
