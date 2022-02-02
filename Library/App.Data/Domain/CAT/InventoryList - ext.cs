namespace App.Data.Domain
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;
    public partial class InventoryList
    {
        [NotMapped]
        public List<InventoryAllocation> InventoryAllocation { get; set; }
    }
}
