using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Data.Domain
{
    public partial class V_STOCKODER_DETAIL
    {
        public int id { get; set; }
        public string sos { get; set; }
        public string sosGroupType { get; set; }
        public string order_class { get; set; }
        public string trilc { get; set; }
        public string part_no { get; set; }
        public string part_desc { get; set; }
        public string om { get; set; }
        public string asn_number { get; set; }
        public string invoice_no { get; set; }
        public string doc_line { get; set; }
        public string doc_number { get; set; }
        public string doc_date { get; set; }
        public string doc_status { get; set; }
        public int doc_status_num { get; set; }
        public DateTime? invoice_date { get; set; }
        public DateTime? pupDate { get; set; }
        public DateTime? podDate { get; set; }
        public DateTime? receiveDate { get; set; }
        public string case_number { get; set; }

        public string source { get; set; }

        public string ack_status { get; set; }

        public string ack_status_msg { get; set; }

        public string qty { get; set; }

        public DateTime? eta { get; set; }

        public DateTime? ata { get; set; }
        public DateTime? Etl_Date { get; set; }

        public string comm_code { get; set; }

        public string weight { get; set; }

        public decimal? dn { get; set; }

        public decimal? ext_dn { get; set; }

        public string JCode { get; set; }

        public string RCDCD { get; set; }

        public string doc_type { get; set; }
        public int? progress { get; set; }
        public decimal? percentage { get; set; }
        public int sum_bm { get; set; }
        public int sum_actual { get; set; }
        public int delay { get; set; }
        public int Sequence { get; set; }
        public int total { get; set; }
    }
}
