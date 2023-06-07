namespace App.Data.Domain
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("PIS.RptDocumentHeld")]
    public partial class RptDocumentHeld
    {
        [Key]
        public int rhld_ID { get; set; }

        [StringLength(5)]
        public string rhld_Store { get; set; }

        [StringLength(10)]
        public string rhld_RefDoc { get; set; }

        [StringLength(20)]
        public string rhld_CustPONo { get; set; }

        [Column(TypeName = "date")]
        public DateTime? rhld_DocDate { get; set; }

        [StringLength(20)]
        public string rhld_DocValue { get; set; }

        [StringLength(7)]
        public string rhld_CustID { get; set; }

        [StringLength(50)]
        public string rhld_CustName { get; set; }

        [StringLength(50)]
        public string rhld_Remarks { get; set; }

        [StringLength(50)]
        public string rhld_StoreName { get; set; }

        public int? rhld_AreaID { get; set; }

        [StringLength(50)]
        public string rhld_AreaName { get; set; }

        public int? rhld_HubID { get; set; }

        [StringLength(50)]
        public string rhld_HubName { get; set; }

        public DateTime rhld_CreatedOn { get; set; }

        [Required]
        [StringLength(100)]
        public string rhld_CreatedBy { get; set; }

        public DateTime rhld_UpdatedOn { get; set; }

        [StringLength(100)]
        public string rhld_UpdatedBy { get; set; }
    }
}
