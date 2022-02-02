namespace App.Data.Domain
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("common.JobFlag")]
    public partial class JobFlag
    {
        [Key]
        public int JobID { get; set; }

        [Required]
        [StringLength(50)]
        public string JobName { get; set; }

        public byte Status { get; set; }

        public DateTime? LastTimeRun { get; set; }

        [Column(TypeName = "date")]
        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public DateTime? LastTimeRunDate { get; set; }
    }
}
