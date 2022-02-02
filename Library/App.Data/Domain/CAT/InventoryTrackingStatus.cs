namespace App.Data.Domain
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;
    [Table("cat.InventoryTrackingStatus")]
    public partial class InventoryTrackingStatus
    {
        [Key]
        public long ID { get; set; }
        public long HeaderID { get; set; }
        public string PartNumber { get; set; }
        public string StoreNumber { get; set; }
        public string SOS { get; set; }
        public string DocNumber { get; set; }
        public string UnitNumber { get; set; }
        public string SerialNumber { get; set; }
        public Nullable<DateTime> DocDate { get; set; }
        public string DocSales { get; set; }
        public string DocReturn { get; set; }
        public string DocWCSL { get; set; }
        public string CUSTOMER { get; set; }
        public string NewWO6F { get; set; }
        public string Notes { get; set; }
        public string RGNumber { get; set; }
        public string DANumber { get; set; }
        public string Status { get; set; }
        public Nullable<DateTime> LastUpdate { get; set; }
        public Int64? WIP_ID { get; set; }
    }
}
