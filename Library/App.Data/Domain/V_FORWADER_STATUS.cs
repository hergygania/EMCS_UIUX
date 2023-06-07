using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Data.Domain
{
    public partial class V_FORWADER_STATUS
    {
        public int id { get; set; }
        public string case_number { get; set; }
        public string da_number { get; set; }

        public string last_location { get; set; }

        public string origin { get; set; }

        public string destination { get; set; }

        public string service_type { get; set; }

        public string status { get; set; }

        public DateTime? status_date { get; set; }

    }
}
