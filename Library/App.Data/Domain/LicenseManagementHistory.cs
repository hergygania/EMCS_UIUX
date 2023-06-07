namespace App.Data.Domain
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("imex.LicenseManagementHistory")]
    public partial class LicenseManagementHistory
    {
        [Key]
        public int HistoryID { get; set; }

        public int LicenseManagementID { get; set; }

        public int GroupID { get; set; }
        public int PortsID { get; set; }
        public string Ports { get; set; }

        [Required]
        [StringLength(100)]
        public string Description { get; set; }

        [Required]
        [StringLength(50)]
        public string LicenseNumber { get; set; }

        [Column(TypeName = "date")]
        public DateTime? ReleaseDate { get; set; }

        [Column(TypeName = "date")]
        public DateTime? ExpiredDate { get; set; }

        [StringLength(50)]
        public string Validity { get; set; }

        [StringLength(50)]
        public string Quota { get; set; }

        public byte Status { get; set; }

        public DateTime? EntryDate { get; set; }

        public DateTime? ModifiedDate { get; set; }

        [StringLength(20)]
        public string EntryBy { get; set; }

        [StringLength(20)]
        public string ModifiedBy { get; set; }
    }
}
