using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Data.Domain.Extensions
{
    public class SurchargeList
    {
        public string ServiceCode { get; set; }
        public string Origin { get; set; }
        public string Destination { get; set; }
        public string Moda { get; set; }
        public string Surcharge { get; set; }
        public string Cost50 { get; set; }
        public string Cost100 { get; set; }
        public string Cost200 { get; set; }
    }
}
