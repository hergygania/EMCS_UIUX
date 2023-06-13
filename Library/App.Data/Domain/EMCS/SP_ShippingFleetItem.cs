using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Data.Domain.EMCS
{
    public class SP_ShippingFleetItem
    {
        public long Id { get; set; }
        public long IdShippingFleet { get; set; }
        public long IdGr { get; set; }
        public long IdCipl { get; set; }
        public long IdCiplItem { get; set; }
    }
}
