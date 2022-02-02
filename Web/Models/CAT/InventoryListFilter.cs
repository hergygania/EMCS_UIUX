using App.Data.Domain;
using App.Data.Domain.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace App.Web.Models.CAT
{
    public class InventoryListFilter
    {
        public string ref_part_no { get; set; }
        public string alt_part_no { get; set; }
        public string comp_inv_no { get; set; }
        public string app_model { get; set; }        
        public string prefix { get; set; }
        public string smcs_code { get; set; }
        public string component { get; set; }
        public string mod { get; set; }
        public List<StoreList> store_list { get; set; }
        public string core_model { get; set; }
        public List<MasterSOS> sos_list { get; set; }
        public string family { get; set; }
        public string crc_tat { get; set; }
        public List<MasterSection> section_list { get; set; }
        public string surplus { get; set; }
        public List<MasterSOS> last_status_list { get; set; }

        //new Filter
        
        public string doctransfer { get; set; }
        public DateTime OriginalSchedule { get; set; }
        public string wcsl { get; set; }
        public string WO { get; set; }
        public string rebuildstatuscms { get; set; }
        public DateTime originalschedule { get; set; }
        public string unitno { get; set; }
        public string serialno { get; set; }
        public List<StoreList> location { get; set; }
        public string customer { get; set; }


        public int? StoreID { get; set; }
        public string SOSID { get; set; }
        public int? SectionID { get; set; }
        public string statusid { get; set; }
    }
}