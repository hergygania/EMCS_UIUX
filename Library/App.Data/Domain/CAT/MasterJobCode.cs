namespace App.Data.Domain
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;
    [Table("cat.JobCode")]
    public partial class MasterJobCode
    {
        public int ID { get; set; }

        [Required]
        [StringLength(50)]
        public string JobCode { get; set; }

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
