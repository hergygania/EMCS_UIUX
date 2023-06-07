using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Data.Domain
{
    public partial class V_DetailDTS
    {
        public int ID { get; set; }
        public string NODI { get; set; }
        public string NODA { get; set; }
        public string SN { get; set; }
        public string Model { get; set; }
        public string Cost { get; set; }
        public string Origin { get; set; }
        public string Destination { get; set; }
        public Nullable<DateTime> ETD { get; set; }
        public Nullable<DateTime> ATD { get; set; }
        public Nullable<DateTime> ETA { get; set; }
        public Nullable<DateTime> ATA { get; set; }
        public string Status { get; set; }
        public string Type { get; set; }
        public string SalesOrderNumber { get; set; }
        public string BatchNumber { get; set; }
        public string OriginAddress { get; set; }
        public string OperatingPlan { get; set; }
        public string CostDelivery { get; set; }
        public string Remarks { get; set; }
        public string DestinationAddress { get; set; }
        public string PICDriver { get; set; }
        public string VendorName { get; set; }
        public string Route { get; set; }
    }
}
