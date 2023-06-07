namespace App.Data.Domain
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("PIS.RptDocumentAwaitingAck")]
    public partial class RptDocumentAwaitingAck
    {
        [Key]
        public int rack_ID { get; set; }

        [StringLength(5)]
        public string rack_Store { get; set; }

        [StringLength(10)]
        public string rack_RefDoc { get; set; }

        [StringLength(20)]
        public string rack_CustPONo { get; set; }

        [Column(TypeName = "date")]
        public DateTime? rack_DocDate { get; set; }

        [StringLength(7)]
        public string rack_CustID { get; set; }

        [StringLength(50)]
        public string rack_CustName { get; set; }

        [StringLength(50)]
        public string rack_StoreName { get; set; }

        public int? rack_HubID { get; set; }

        [StringLength(50)]
        public string rack_HubName { get; set; }

        public int? rack_AreaID { get; set; }

        [StringLength(50)]
        public string rack_AreaName { get; set; }

        public DateTime rack_CreatedOn { get; set; }

        [Required]
        [StringLength(100)]
        public string rack_CreatedBy { get; set; }

        public DateTime rack_UpdatedOn { get; set; }

        [StringLength(100)]
        public string rack_UpdatedBy { get; set; }

        [StringLength(20)]
        public string rack_DocValue { get; set; }
    }
}
