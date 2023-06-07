using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Data.Domain.DTS
{
    public class InboundEvizFilter
    {
        public string SalesModel { get; set; }
        public string SerialNumber { get; set; }
        public string ShipSourceName { get; set; }

        public string StartDate { get; set; }
        public string EndDate { get; set; }

        public string ETAStartDate { get; set; }
        public string ETAEndDate { get; set; }
    }
}
