namespace App.Data.Domain
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("PIS.RptDocumentSuspend")]
    public partial class RptDocumentSuspend
    {
        [Key]
        public int rspnd_ID { get; set; }

        [StringLength(5)]
        public string rspnd_Store { get; set; }

        [StringLength(10)]
        public string rspnd_RefDoc { get; set; }

        [StringLength(20)]
        public string rspnd_CustPONo { get; set; }

        [Column(TypeName = "date")]
        public DateTime? rspnd_DocDate { get; set; }

        [StringLength(20)]
        public string rspnd_DocValue { get; set; }

        [StringLength(7)]
        public string rspnd_CustID { get; set; }

        [StringLength(50)]
        public string rspnd_CustName { get; set; }

        [StringLength(50)]
        public string rspnd_StoreName { get; set; }

        public int? rspnd_AreaID { get; set; }

        [StringLength(50)]
        public string rspnd_AreaName { get; set; }

        public int? rspnd_HubID { get; set; }

        [StringLength(50)]
        public string rspnd_HubName { get; set; }

        public DateTime rspnd_CreatedOn { get; set; }

        [Required]
        [StringLength(100)]
        public string rspnd_CreatedBy { get; set; }

        public DateTime rspnd_UpdatedOn { get; set; }

        [StringLength(100)]
        public string rspnd_UpdatedBy { get; set; }
    }
}
