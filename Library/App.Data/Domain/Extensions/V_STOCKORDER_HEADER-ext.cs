using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Data.Domain
{
    public partial class V_STOCKORDER_HEADER
    {
        [NotMapped]
        public string sr_type { get; set; }

        [NotMapped]
        public string order_class { get; set; }

        [NotMapped]
        public string order_profile { get; set; }

        [NotMapped]
        public string OrderTypeCode { get; set; }

        [NotMapped]
        public string shp_type { get; set; }

        [NotMapped]
        public string agreement { get; set; }


        [NotMapped]
        public string filter_by { get; set; }

        [NotMapped]
        public string hub_id { get; set; }

        [NotMapped]
        public string area_id { get; set; }

        [NotMapped]
        public string filter_type { get; set; }

        [NotMapped]
        public IEnumerable<string> selStoreList_Nos { get; set; }

        [NotMapped]
        public string storeList_No { get; set; }

        [NotMapped]
        public DateTime? doc_date_start { get; set; }

        [NotMapped]
        public DateTime? doc_date_end { get; set; }

        public DateTime? doc_date { get; set; }

        [NotMapped]
        public string doc_date_s { get; set; }

        [NotMapped]
        public string case_no { get; set; }

        [NotMapped]
        public string part_number { get; set; }

        [NotMapped]
        public string part_desc { get; set; }

        [NotMapped]
        public string MaterialType { get; set; }

        [NotMapped]
        public string sos_group_type { get; set; }
        public DateTime? endDate { get; set; }
        public string origin { get; set; }
        public string destination { get; set; }
    }
}
