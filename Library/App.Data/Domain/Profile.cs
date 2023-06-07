namespace App.Data.Domain
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Profile")]
    public partial class Profile
    {
        [Key]
        [Column(Order = 0)]
        [StringLength(5)]
        public string ProfileNo { get; set; }

        [Key]
        [Column(Order = 1)]
        [StringLength(1)]
        public string OrderClass { get; set; }

        [StringLength(255)]
        public string Description { get; set; }

        public DateTime? EntryDate { get; set; }

        public DateTime? ModifiedDate { get; set; }

        [StringLength(20)]
        public string EntryBy { get; set; }

        [StringLength(20)]
        public string ModifiedBy { get; set; }
    }
}
