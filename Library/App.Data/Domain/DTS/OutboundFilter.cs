using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Data.Domain.DTS
{
    public class OutboundFilter
    {
        public string IdString { get; set; }
        public string DANumber { get; set; }
        public string DINumber { get; set; }
        public string SerialNumber { get; set; }
        public string Moda { get; set; }
        public string UnitType { get; set; }
        public string Model { get; set; }
        public string Destination { get; set; }
        public string Origin { get; set; }
        public string Status { get; set; }
        public string Position { get; set; }
    }
}
