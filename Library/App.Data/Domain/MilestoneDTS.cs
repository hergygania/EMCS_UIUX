using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Data.Domain
{
    public partial class MilestoneDTS
    {
        public string Milestone { get; set; }
        public string SalesOrder { get; set; }
        public string Workable { get; set; }
        public string Allocation { get; set; }
        public string RA { get; set; }
        public string OD { get; set; }
        public string Departure { get; set; }
        public string GI { get; set; }
        public string Arrival { get; set; }
    }
}
