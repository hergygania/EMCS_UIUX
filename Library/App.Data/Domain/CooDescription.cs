namespace App.Data.Domain
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("mp.CooDescription")]
    public partial class CooDescription
    {
        [Key]
        [StringLength(5)]
        public string COO { get; set; }

        [StringLength(50)]
        public string Description { get; set; }
    }
}
