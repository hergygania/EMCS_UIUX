using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Data.Domain
{
    public partial class RptOutstandingOldCoreSummaryList
    {
        public string Store { get; set; }
        //public string Location { get; set; }
        public int OutOldCore0day { get; set; }
        public int OutOldCore11day { get; set; }
        public int OutOldCore21day { get; set; }

        //public int StoreID { get; set; }
        //public int LocationID { get; set; }
    }
}
