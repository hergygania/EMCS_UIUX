using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Data.Domain.EMCS
{
   public class SPBlAwb
    {
        public long Id { get; set; }
        public string Number { get; set; }
        public DateTime? MasterBlDate { get; set; }
        public string ClNo { get; set; }
        public string HouseBlNumber { get; set; }
        public DateTime? HouseBlDate { get; set; }
        public string Publisher { get; set; }
        public long IdCl { get; set; }
 		public string StatusViewByUser { get; set; }
        public string AjuNumber { get; set; }
        public int PendingRFC { get; set; }
        public string RoleName { get; set; }
    }
}
