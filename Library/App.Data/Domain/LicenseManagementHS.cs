namespace App.Data.Domain
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("imex.LicenseManagementHS")]
    public partial class LicenseManagementHS
    {
        [Key]
        public Int64 ID { get; set; }
        public int LicenseID { get; set; }

        [Required]
        [StringLength(100)]
        public string RegulationCode { get; set; }

        [Required]
        [StringLength(100)]
        public string HSCode { get; set; }

        public DateTime? EntryDate { get; set; }

        [StringLength(20)]
        public string EntryBy { get; set; }

        public DateTime? ModifiedDate { get; set; }

        [StringLength(20)]
        public string ModifiedBy { get; set; }

        [NotMapped]
        public string LicenseNumber { get; set; }
    }
}
