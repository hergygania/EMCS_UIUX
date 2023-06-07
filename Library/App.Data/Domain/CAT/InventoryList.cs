namespace App.Data.Domain
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;
    [Table("cat.Inventory")]
    public partial class InventoryList
    {
        [Key]
        public long ID { get; set; }
        public string KAL { get; set; }
        public string AlternetPartNumber { get; set; }  
        public string Component { get; set; }
        public string StoreNumber { get; set; }
        public string SOS { get; set; }
        public string CUSTOMER_ID { get; set; }
        public string Surplus { get; set; }
        public string UnitNumber { get; set; }
        public string EquipmentNumber { get; set; }
        public Nullable<DateTime> DocDate { get; set; }
        public string DocSales { get; set; }
        public string DocReturn { get; set; }
        public string DocWCSL { get; set; }
        public string DocTransfer { get; set; }
        public Nullable<DateTime> DocDateTransfer { get; set; }
        public string NewWO6F { get; set; }
        public string WO1K { get; set; }
        public string OldWO6F { get; set; }
        public string WO_CMS { get; set; }
        public string MO { get; set; }
        public string PONo { get; set; }
        public Nullable<int> WIP_ID { get; set; }
        public Nullable<DateTime> CRC_PCD { get; set; }
        public Nullable<DateTime> CRC_Completion { get; set; }
        public string DANumber { get; set; }
        public string RGNumber { get; set; }
        public string LastStatus { get; set; }
        public string RebuildStatus { get; set; }
        public string JobLoc { get; set; }
        public string JobCode { get; set; }
        public string ReturnAsZero { get; set; }
        public string TUID { get; set; }
        public string StandID { get; set; }
        public int Allocated { get; set; }
        public string LastDocNumber { get; set; }
        public string LastIdInventoryAdjustment { get; set; }
        public string Remarks { get; set; }
        public Nullable<DateTime> CreatedDate { get; set; }
        public Nullable<DateTime> UpdateDate { get; set; }
        
    }
                                                
}
