using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Data.Domain
{
    public class TrackingStatusDetailCMS
    {
        public int WIP_ID { get; set; }
        public string CRCPromisedCompletionDate { get; set; }
        public string JobLoc { get; set; }
        public string JobCode { get; set; }
        public string ReturnAsZeroHour { get; set; }
        public string TUID { get; set; }
        public string WONumber { get; set; }
        public string WO_CMS { get; set; }
        public string DateReceived { get; set; }
        public string JobInstruction { get; set; }
        public string ComponentCondition { get; set; }
        public string StandID { get; set; }
        //public string EstimateCompletedDate { get; set; }
        public string Noted { get; set; }
        public string DANumber { get; set; }
        public string CRC_Completion { get; set; }
    }
}
