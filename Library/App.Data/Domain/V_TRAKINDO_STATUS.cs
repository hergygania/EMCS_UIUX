using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Data.Domain
{
    public partial class V_TRAKINDO_STATUS
    {
        public int id { get; set; }
        public string store_number { get; set; }
        public string case_number { get; set; }
        public Decimal? order_qty { get; set; }
        public DateTime? order_date { get; set; }
        public Decimal? supply_qty { get; set; }
        public DateTime? supply_date { get; set; }
        public Decimal? bo_qty { get; set; }
        public string bo_to_fill { get; set; }
        public DateTime? bo_to_date { get; set; }
    }
}
