using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Data.Domain
{
    public class SP_DataInventory
    {
        public long ID { get; set; }
        public string KAL { get; set; }
        public string RefPartNo { get; set; }
        public string AlternetPartNumber { get; set; }
        public string ComponentInventoryNumber { get; set; }
        public string ApplicableModel { get; set; }
        public string Prefix { get; set; }
        public string SMCSCode { get; set; }
        public string Component { get; set; }
        public string MOD { get; set; }
        public string StoreID { get; set; }
        public string StoreNumber { get; set; }
        public string Location { get; set; }
        public string CoreModel { get; set; }
        public string SOS { get; set; }
        public string Family { get; set; }
        public string CRCTAT { get; set; }
        public string Section { get; set; }
        public string RGNumber { get; set; }
        public string LastDocNumber { get; set; }
        public string DocSales { get; set; }
        public string DocDate { get; set; }
        public string DocTransfer { get; set; }
        public string DocDateTransfer { get; set; }
        public string UnitNumber { get; set; }
        public string EquipmentNumber { get; set; }
        public string LastStatus { get; set; }
        public string PONumber { get; set; }
        public string Schedule { get; set; }
        public string Surplus { get; set; }
        public string Customer { get; set; }
        public string NewWO6F { get; set; }
        public string DocWCSL { get; set; }
        public string WO1K { get; set; }
        public string OldWO6F { get; set; }
        public string WO_CMS { get; set; }
        public string MO { get; set; }
        public string RebuildStatus { get; set; }
        public string Remarks { get; set; }
        public string CRC_PCD { get; set; }
        public string DocReturn { get; set; }
        public string TUID { get; set; }
        public string CUSTOMER_ID { get; set; }
    }
}
