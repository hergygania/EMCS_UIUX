namespace App.Data.Domain
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;
    [Table("cat.SOS")]
    public partial class MasterSOS
    {
        [Key]
        public int ID { get; set; }

        [Required]
        [Range(1, 99999, ErrorMessage = "SOS length must be below or equal 5 digits")]
        public int SOS { get; set; }

        [Required]
        [StringLength(200)]
        public string Description { get; set; }
        public bool IsActive { get; set; }
        public DateTime? EntryDate { get; set; }
        public DateTime? ModifiedDate { get; set; }

        [StringLength(50)]
        public string EntryBy { get; set; }

        [StringLength(50)]
        public string ModifiedBy { get; set; }
    }
}
