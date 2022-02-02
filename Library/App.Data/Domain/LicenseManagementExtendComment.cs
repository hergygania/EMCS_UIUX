namespace App.Data.Domain
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("imex.LicenseManagementExtendComment")]
    public partial class LicenseManagementExtendComment
    {
        [Key]
        public int CommentID { get; set; }
				public int LicenseManagementID { get; set; }

        public int? ExtendID { get; set; }

        [Column(TypeName = "text")]
        [Required]
        public string Comment { get; set; }

        public DateTime? EntryDate { get; set; }

        [StringLength(20)]
        public string EntryBy { get; set; }
    }
}
