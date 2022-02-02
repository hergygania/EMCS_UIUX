using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Data.Domain.DTS
{
    public class DeliveryInstructionFilter
    {
        public string searchName { get; set; }
        public string IdString { get; set; }
        public bool requestor { get; set; }
        public bool mode { get; set; }
    }
}
