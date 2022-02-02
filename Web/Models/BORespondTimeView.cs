using System;

namespace App.Web.Models
{

    public class BORespondTimeView
    {
        public string PartNo { get; set; }
        public int? Quantity { get; set; }
        public string BinLoc { get; set; }
        public int? Weight { get; set; }
        public int? Length { get; set; }
        public int? Width { get; set; }
        public DateTime? PickupStartDate { get; set; }
        public DateTime? PickupEndDate { get; set; }
        public int? Height { get; set; }
    }
}