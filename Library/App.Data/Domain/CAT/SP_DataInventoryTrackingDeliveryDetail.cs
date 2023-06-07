using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Data.Domain
{
    public class SP_DataInventoryTrackingDeliveryDetail
    {
        public string DA { get; set; }
        public DateTime tracking_date { get; set; }
        public string city { get; set; }
        public string tracking_status_id { get; set; }
        public string tracking_status_desc { get; set; }
    }
}
