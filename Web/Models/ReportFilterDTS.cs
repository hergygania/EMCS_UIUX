using App.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace App.Web.Models
{
    public class ReportFilterDTS
    {
        public string SerialNumber { get; set; }

        public string OriginID { get; set; }

        public string DestinationID { get; set; }

        public List<Select2Result> Origin { get; set; }

        public DateTime? EstDepature { get; set; }

        public DateTime? EstArrival { get; set; }

        public string OutboundDelivery { get; set; }

        public string SalesOrderNumber { get; set; }

        public string Model { get; set; }

        public List<Select2Result> Destination { get; set; }

        public DateTime? ActualDepature { get; set; }

        public DateTime? ActualArrival { get; set; }
    }
}