namespace App.Data.Domain
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public class ShipmentOutboundListData
    {
        public string DA { get; set; }
        public string DI { get; set; }
        public DateTime? DIDate { get; set; }
        public string Origin { get; set; }
        public string Destination { get; set; }
        public string Moda { get; set; }
        public string UnitModa { get; set; }
        public string UnitType { get; set; }
        public string Model { get; set; }
        public DateTime? ETD { get; set; }
        public DateTime? ATD { get; set; }
        public DateTime? ETA { get; set; }
        public DateTime? ATA { get; set; }
        public string Position { get; set; }
        public string Status { get; set; }
        public string SerialNumber { get; set; }
        public string NoPol { get; set; }
        public string DriverName { get; set; }
        public string HPInlandFreight { get; set; }
        public string VesselName { get; set; }
        public string PIC { get; set; }
        public string HPSealandFreight { get; set; }
        public string NotesSAP { get; set; }
        public string Remarks { get; set; }
        public bool IsCKB { get; set; }
    }
}

