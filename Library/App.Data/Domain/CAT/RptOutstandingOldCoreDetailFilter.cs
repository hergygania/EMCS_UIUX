using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Data.Domain.CAT
{
   public class RptOutstandingOldCoreDetailFilter
    {
        public string store_id { get; set; }
        public string sos_id { get; set; }
        public string part_no { get; set; }
        public string kal { get; set; }
        public string model { get; set; }
        public string component { get; set; }
        public string sn_unit { get; set; }
        public string customer_id { get; set; }
        public string prefix { get; set; }
        public string store_supplied_date_start { get; set; }
        public string store_supplied_date_end { get; set; }
        public string reconditioned_wo { get; set; }
        public string doc_c { get; set; }
        public string doc_r { get; set; }
        public string wcsl { get; set; }
        public string DateFilter { get; set; }
    }
}
