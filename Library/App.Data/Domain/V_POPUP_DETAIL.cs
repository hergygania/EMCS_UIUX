using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Data.Domain
{
    public class V_POPUP_DETAIL
    {
        public string part_no { get; set; }
        public string case_number { get; set; }

        public string JCode { get; set; }
        public string LineItem { get; set; }

        public string sos { get; set; }

        public string type { get; set; }

        public string rfdcno { get; set; }

        public string qty_bo { get; set; }

        public string RCDCD { get; set; }

        public DateTime? doc_date { get; set; }

        public string RPORNE { get; set; }

        public string doc_number { get; set; }

        public string ORDSOS { get; set; }

        public string store_no { get; set; }

        public string cmnt1 { get; set; }
        public string order_class { get; set; }

        public string trilc { get; set; }

        public DateTime? invoice_date { get; set; }

        public string doc_invoice { get; set; }

        public DateTime? ackDate { get; set; }
				public DateTime? pupDate { get; set; }
				public DateTime? podDate { get; set; }
				public DateTime? receiveDate { get; set; }
        public string order_number { get; set; }
        public string xforno { get; set; }

        public string source { get; set; }


    }
}
