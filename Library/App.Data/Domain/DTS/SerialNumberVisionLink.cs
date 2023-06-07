using System;
using System.Collections.Generic;

namespace App.Data.Domain
{
    public class SerialNumberVisionLink
    {
        public string serialNumber { get; set; }
        public string make { get; set; }
        public string model { get; set; }
        public DateTime? moduleTime { get; set; }
        public decimal longitude { get; set; }
        public decimal latitude { get; set; }
        public string Address { get; set; }
        public decimal smu { get; set; }
        public DateTime? ETL_DATE { get; set; }
        public DateTime? LocationReportTime { get; set; }

    }
}
