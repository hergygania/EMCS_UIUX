namespace App.Data.Domain
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;
    [Table("dbo.ShipmentOutbound")]

    public class ShipmentOutbound
    {
        [Key]
        public Int32 ID { get; set; }
        public string DA { get; set; }
        public string DI { get; set; }
        public DateTime? DIDate { get; set; }
        public string Origin { get; set; }
        public string Destination { get; set; }
        public string DeliveryContact { get; set; }
        public string Moda { get; set; }
        public string UnitModa { get; set; }
        public string UnitType { get; set; }
        public string Model { get; set; }
        public DateTime? ETD { get; set; }
        public DateTime? ATD { get; set; }
        public DateTime? ETA { get; set; }
        public DateTime? ATA { get; set; }
        public string Position { get; set; }
        public string OldPosition { get; set; }
        public string Status { get; set; }
        public string OldStatus { get; set; }
        public Decimal? Cost { get; set; }
        public DateTime? EntrySheetDate { get; set; }
        public DateTime? ReleaseDate { get; set; }
        public string ShipmentDoc { get; set; }
        public string ShipmentCost { get; set; }
        public string RejectReason { get; set; }
        public string SerialNumber { get; set; }
        public string ShipmentModa { get; set; }
        public string NoPol { get; set; }
        public string DriverName { get; set; }
        public string HPInlandFreight { get; set; }
        public string VesselName { get; set; }
        public string PIC { get; set; }
        public string HPSealandFreight { get; set; }
        public string NotesSAP { get; set; }
        public string Remarks { get; set; }
        public bool IsCKB { get; set; }
        public string CreateBy { get; set; }
        public DateTime? CreateDate { get; set; }
        public string UpdateBy { get; set; }
        public DateTime? UpdateDate { get; set; }
    }
}

