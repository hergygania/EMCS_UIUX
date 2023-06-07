using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Data.Domain
{
    public class SP_TrackingStatusDetailTTC
    {
        public string DocReturn { get; set; }
        public string WCSL { get; set; }
        public string WONumber { get; set; }
        public string ReconditionNo { get; set; }
        public string DANumber { get; set; }
        public string ComponentID { get; set; }
        public string StandID { get; set; }
        public DateTime? CRRDateIssued { get; set; }
        public int ElapsedDay { get; set; }
        public string Status { get; set; }
        public string CRRCreated { get; set; }
        public string RemarkReceived { get; set; }
        public string RemarkFollowUpCRR { get; set; }
    }
}
