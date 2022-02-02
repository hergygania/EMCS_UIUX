using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Data.Domain.Extensions
{
    public class RateList
    {
        public string ServiceCode { get; set; }
        public string Origin { get; set; }
        public string Destination { get; set; }
        public string Rate { get; set; }
        public string CurrRate { get; set; }
        public decimal MinRate { get; set; }
        public string RA { get; set; }
        public string LeadTime { get; set; }
        public decimal CostRateIDR { get; set; }
    }
}
