using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace App.Data.Domain.EMCS
{
    [Table("dbo.ShippingFleetItem")]
    public class ShippingFleetItem
    {
        [Key]
        public long Id { get; set; }
        public long IdShippingFleet { get; set; }
        public long IdGr { get; set; }
        public string IdCipl { get; set; }
        public long IdCiplItem { get; set; }


    }
}


