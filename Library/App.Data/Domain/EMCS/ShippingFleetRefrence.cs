using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Data.Domain.EMCS
{
    [Table("dbo.ShippingFleetRefrence")]
    public class ShippingFleetRefrence
    {
        
        [Key]
        public long  Id { get; set; }
        public long IdShippingFleet { get; set; }
        public long IdGr { get; set; }
        public long IdCipl { get; set; }
        public string DoNo { get; set; }
        public DateTime CreateDate { get; set; }
    }
}
