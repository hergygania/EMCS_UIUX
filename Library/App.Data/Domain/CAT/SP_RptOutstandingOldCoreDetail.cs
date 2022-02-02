using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Data.Domain
{
    public class SP_RptOutstandingOldCoreDetail
    {      
        public string Store { get; set; }
        public string SOS { get; set; }
        public string PartNo { get; set; }
        public string KAL { get; set; }
        public string Model { get; set; }
        public string Prefix { get; set; }
        public string Component { get; set; }

        public string UsedSN { get; set; }
        public string EquipmentNo { get; set; }
        public string Customer_Spuly { get; set; }
        public Nullable<DateTime> StoreSuppliedDate { get; set; }
        public string ReconditionedWO { get; set; }
        public string SaleDoc { get; set; }
        public string ReturnDoc { get; set; }
        public string WCSL { get; set; }

        public string PONumber { get; set; }
        public Nullable<DateTime> Schedule { get; set; }
        public string UnitNoSN { get; set; }
        public string SerialNo { get; set; }
        public string Location { get; set; }
        public string Customer { get; set; }

        public string Section { get; set; }
        public string RebuildOption { get; set; }
    }
}
