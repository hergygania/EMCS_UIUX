using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Data.Domain
{
    public partial class RptSummaryPEXList
    {
        public string ref_part_no { get; set; }
        public string model { get; set; }
        public string component { get; set; }
        public string prefix { get; set; }
        public string scms { get; set; }
        public string mod { get; set; }
        public int total_availability { get; set; }
        public int allocated_oh { get; set; }
        public int allocated_st { get; set; }
        public int allocated_woc { get; set; }
        public int allocated_ttc { get; set; }
        public int allocated_sq { get; set; }
        public int allocated_wip { get; set; }
        public int allocated_jc { get; set; }
        public int free_allocation_oh { get; set; }
        public int free_allocation_st { get; set; }
        public int free_allocation_woc { get; set; }
        public int free_allocation_ttc { get; set; }
        public int free_allocation_sq { get; set; }
        public int free_allocation_wip { get; set; }
        public int free_allocation_jc { get; set; }
        public int total_allocation { get; set; }
        public int allocation_cycle1 { get; set; }
        public int allocation_cycle2 { get; set; }
        public int allocation_cycle3 { get; set; }
        public int allocation_cycle4 { get; set; }
        public int allocation_cycle5 { get; set; }
    }
}
