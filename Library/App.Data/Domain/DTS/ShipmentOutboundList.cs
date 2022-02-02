namespace App.Data.Domain
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public class ShipmentOutboundList
    {
        public string DA { get; set; }
        public string DI { get; set; }
        public string Origin { get; set; }
        public string Destination { get; set; }
        public string Moda { get; set; }
        public string UnitModa { get; set; }
        public string UnitType { get; set; }
        public string Model { get; set; }
        public string ETD { get; set; }
        public string ATD { get; set; }
        public string ETA { get; set; }
        public string ATA { get; set; }
        public string Position { get; set; }
        public string Status { get; set; }
        public string SerialNumber { get; set; }
        public string Remarks { get; set; }
        public bool IsCKB { get; set; }
    }
}

