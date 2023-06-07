namespace App.Data.Domain
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public class ShipmentInboundList
    {
        public string AjuNo { get; set; }
        public string MSONo { get; set; }
        public string PONo { get; set; }
        public string LoadingPort { get; set; }
        public string DischargePort { get; set; }
        public string Model { get; set; }
        public string ModelDescription { get; set; }
        public string Status { get; set; }
        public string SerialNumber { get; set; }
        public string Notes { get; set; }
        public string Position { get; set; }
        public string Remark { get; set; }
        public string ATACakung { get; set; }
        public string ETACakung { get; set; }
        public string ATAPort { get; set; }
        public string ETAPort { get; set; }
    }
}