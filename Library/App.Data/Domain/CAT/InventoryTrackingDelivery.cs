namespace App.Data.Domain
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;
    [Table("cat.InventoryTrackingDelivery")]
    public partial class InventoryTrackingDelivery
    {
        [Key]
        public int ID { get; set; }
        public int InventoryID { get; set; }
        [StringLength(50)]
        public string DANo { get; set; }

        public DateTime Date { get; set; }
        [StringLength(50)]
        public string Destination { get; set; }
        [StringLength(50)]
        public string Reference { get; set; }
    }
}
