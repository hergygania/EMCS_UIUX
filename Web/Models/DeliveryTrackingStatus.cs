using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace App.Web.Models
{
    public class DeliveryTrackingStatus
    {
        public int ID { get; set; }

        public int Moda { get; set; }

        public string Unit_Moda { get; set; }

        public string From { get; set; }

        public string To { get; set; }

        public string NODA { get; set; }

        public string NODI { get; set; }

        public int Unit_Type { get; set; }

        public string Model { get; set; }

        public string BatchNumber { get; set; }

        public string SN { get; set; }

        public DateTime ETD { get; set; }

        public DateTime ATD { get; set; }

        public DateTime ETA { get; set; }

        public DateTime ATA { get; set; }

        public int Status { get; set; }

        public string Cost { get; set; }

        public string Currency { get; set; }

        public string Ship_Doc { get; set; }

        public string Ship_Cost { get; set; }

        public string Entry_Sheet { get; set; }

        public string No_PI { get; set; }

        public string Reject { get; set; }

        public string Remarks { get; set; }

        public DateTime EntryDate { get; set; }

        public string EntryBy { get; set; }

        public string PICDriver { get; set; }

        public string VendorName { get; set; }

        public string OperatingPlan { get; set; }

        public string CostDelivery { get; set; }

        public string OutboundDelivery { get; set; }

        public string SalesOrderNumber { get; set; }
    }
}