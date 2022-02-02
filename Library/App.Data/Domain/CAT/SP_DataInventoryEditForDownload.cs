using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Data.Domain
{
    public class SP_DataInventoryEditForDownload
    {
        public string KAL { get; set; }
        public string AlternetPartNumber { get; set; }
        public string LastStatus { get; set; }
        public string StoreNumber { get; set; }
        public string SOS { get; set; }
        public string Surplus { get; set; }
        public string UnitNumber { get; set; }
        public string EquipmentNumber { get; set; }
        public string CUSTOMER_ID { get; set; }
        public string DocDate { get; set; }



        public string DocDateTransfer { get; set; }
        public string DocTransfer { get; set; }
        public string NewWO6F { get; set; }
        public string MO { get; set; }

        public string DocSales { get; set; }
        public string DocReturn { get; set; }
        public string DocWCSL { get; set; }
    }
}
