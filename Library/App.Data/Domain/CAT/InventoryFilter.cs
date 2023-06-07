using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Data.Domain.CAT
{
   public class InventoryFilter
    {
        public string ref_part_no { get; set; }
        public string alt_part_no { get; set; }
        public string comp_inv_no { get; set; }
        public string app_model { get; set; }
        public string prefix { get; set; }
        public string smcs_code { get; set; }
        public string component { get; set; }
        public string mod { get; set; }
        public List<App.Data.Domain.Extensions.StoreList> store_list { get; set; }
        public string core_model { get; set; }
        public List<MasterSOS> sos_list { get; set; }
        public string family { get; set; }
        public string crc_tat { get; set; }
        public List<MasterSOS> section_list { get; set; }
        public string surplus { get; set; }
        public List<MasterSOS> last_status_list { get; set; }

        public string doctransfer { get; set; }
        public string wcsl { get; set; }
        public string WO { get; set; }
        public string rebuildstatuscms { get; set; }
        public DateTime? originalschedule { get; set; }
        public string unitno { get; set; }
        public string serialno { get; set; }
        public string location { get; set; }
        public string customer { get; set; }


        public int? StoreID { get; set; }
        public string SOSID { get; set; }
        public int? SectionID { get; set; }
        public string statusid { get; set; }
    }
}
