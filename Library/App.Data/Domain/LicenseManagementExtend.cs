namespace App.Data.Domain
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("imex.LicenseManagementExtend")]
    public partial class LicenseManagementExtend
    {
        [Key]
        public int ExtendID { get; set; }

        public int LicenseManagementID { get; set; }

        [Column(TypeName = "date")]
        public DateTime ApplyDate { get; set; }

        [Column(TypeName = "date")]
        public DateTime NewReleaseDate { get; set; }

        [Column(TypeName = "date")]
        public DateTime NewExpiredDate { get; set; }

        [StringLength(50)]
        public string NewValidity { get; set; }

        [StringLength(50)]
        public string NewQuota { get; set; }

        [Column(TypeName = "text")]
        public string Requirement { get; set; }

        public DateTime? EntryDate { get; set; }

        public DateTime? ModifiedDate { get; set; }

        [StringLength(20)]
        public string EntryBy { get; set; }

        [StringLength(20)]
        public string ModifiedBy { get; set; }
    }

    public partial class LicenseManagementExtend
    {
        [NotMapped]
        public string RegulationCode { get; set; }

        [NotMapped]
        public string HSCode { get; set; }

        [NotMapped]
        public string PartNumber { get; set; }
    }
}
