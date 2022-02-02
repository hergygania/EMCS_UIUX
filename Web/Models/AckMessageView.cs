
using System;

namespace App.Web.Models
{
    public class AckMessageView
    {
        public decimal? TotalItem { get; set; }
        public decimal? TotalValue { get; set; }
        public decimal? TotalWeight { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
    }
}