using System;

namespace App.Web.Models
{
    public class PODtoBOFillView
    {
        public string DocNo { get; set; }
        public string PartNo { get; set; }
        public string Sos { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public int? Quantity { get; set; }
    }
}