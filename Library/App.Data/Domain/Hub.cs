namespace App.Data.Domain
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Hub")]
    public partial class Hub
    {
        public int HubID { get; set; }

        [StringLength(255)]
        public string Name { get; set; }

        [Column(TypeName = "text")]
        public string Description { get; set; }

        public DateTime? EntryDate { get; set; }

        public DateTime? ModifiedDate { get; set; }

        [StringLength(20)]
        public string EntryBy { get; set; }



        [StringLength(20)]
        public string ModifiedBy { get; set; }
    }
}
