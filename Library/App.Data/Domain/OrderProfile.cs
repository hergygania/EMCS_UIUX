namespace App.Data.Domain
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("mp.OrderProfile")]
    public partial class OrderProfile
    {
        public int ID { get; set; }

        [StringLength(1)]
        public string OrderClass { get; set; }

        public int? ProfileNumber { get; set; }

        [Required]
        [StringLength(50)]
        public string ProfileDescription { get; set; }
    }
}
