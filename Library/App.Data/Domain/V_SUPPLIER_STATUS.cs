using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Data.Domain
{
    public partial class V_SUPPLIER_STATUS
    {
        public int id { get; set; }
        public string case_number { get; set; }
        public string asn_number { get; set; }
        public string case_type { get; set; }

        public string case_desc { get; set; }

        public decimal? weight { get; set; }
        public string ship_via { get; set; }

        public string status_bo { get; set; }

        public string facility_bo { get; set; }
        public int? total { get; set; }
        public int? next_rcvd_qty { get; set; }

        public DateTime? nect_rcvd_date { get; set; }
        public int? total_bo { get; set; }
        public string freeze { get; set; }

    }

}
