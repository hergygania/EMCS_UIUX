using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Data.Domain.Extensions
{
    public class InRateList
    {
        public string ServiceCode { get; set; }
        public string Origin { get; set; }
        public string Destination { get; set; }
        public decimal InRate { get; set; }
        public string CurrInRate { get; set; }
    }
}
