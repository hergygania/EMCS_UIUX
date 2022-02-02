namespace App.Data.Domain
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("PIS.RptUnrealisticCommittedDateExstock")]
    public partial class RptUnrealisticCommittedDateExstock
    {
        [Key]
        public int urcdx_ID { get; set; }

        [StringLength(5)]
        public string urcdx_Store { get; set; }

        [StringLength(10)]
        public string urcdx_RefDoc { get; set; }

        [StringLength(20)]
        public string urcdx_CustPONo { get; set; }

        [StringLength(10)]
        public string urcdx_DocDate { get; set; }

        [StringLength(20)]
        public string urcdx_DocValue { get; set; }

        [Column(TypeName = "date")]
        public DateTime? urcdx_CommittedDate { get; set; }

        [StringLength(50)]
        public string urcdx_GapOfDate { get; set; }

        [StringLength(7)]
        public string urcdx_CustID { get; set; }

        [StringLength(50)]
        public string urcdx_CustName { get; set; }

        [StringLength(50)]
        public string urcdx_StoreName { get; set; }

        public int? urcdx_AreaID { get; set; }

        [StringLength(50)]
        public string urcdx_AreaName { get; set; }

        public int? urcdx_HubID { get; set; }

        [StringLength(50)]
        public string urcdx_HubName { get; set; }

        public DateTime urcdx_CreatedOn { get; set; }

        [Required]
        [StringLength(100)]
        public string urcdx_CreatedBy { get; set; }

        public DateTime urcdx_UpdatedOn { get; set; }

        [StringLength(100)]
        public string urcdx_UpdatedBy { get; set; }
    }
}
