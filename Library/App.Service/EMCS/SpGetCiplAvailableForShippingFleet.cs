using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Service.EMCS
{
   public  class SpGetCiplAvailableForShippingFleet
    {
        public long Id { get; set; }
        public long IdShippingFleet { get; set; }
        public long IdGr { get; set; }
        public long IdCipl { get; set; }
        public long IdCiplItem { get; set; }
    }
}
