using System;

namespace App.Web.Models
{

    public class OutstandingBackorderItemView
    {
        public string RackStore { get; set; }
        public string PoNo { get; set; }
        public string RefDoc { get; set; }
        public string PartNo { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
    }
}