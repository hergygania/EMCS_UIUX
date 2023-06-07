using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Data.Domain
{
    public class SP_TrackingStatusDetailWOC
    {
        public string DocSales { get; set; }
        public string UsedSN { get; set; }
        public string EquipmentNo { get; set; }
        public string Customer { get; set; }
        public string StoreSuppliedDate { get; set; }
        public string TUID { get; set; }
        public string WONumber { get; set; }
        public string ElapsedCustomerDate { get; set; }
        public string StandID { get; set; }
        public string CRRDateIssued { get; set; }
        public string ElapsedDay { get; set; }
        public string Status { get; set; }
        public string CRRCreated { get; set; }
        public string RemarkReceived { get; set; }
        public string RemarkFollowUpCRR { get; set; }
    }
}
