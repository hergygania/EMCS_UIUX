using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace App.Web.Models
{
    public class STRespondTimeView
    {
        public string PartNo { get; set; }
        public string BinLoc { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public int? Bm { get; set; }
    }
}