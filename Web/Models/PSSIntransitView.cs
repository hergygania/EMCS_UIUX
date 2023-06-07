using System;
using System.Collections.Generic;
using System.Web.Mvc;

namespace App.Web.Models
{
    public class PSSIntransitView
	{
        public string CaseNo { get; set; }
        public string ReffDoc { get; set; }
        public string PartNo { get; set; }
        public string Sos { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public int? Quantity { get; set; }
    }
}