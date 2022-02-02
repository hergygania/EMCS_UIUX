using System;
using System.Collections.Generic;
using System.Web.Mvc;

namespace App.Web.Models
{
    public class CounterPerformanceView
    {
        public int Id { get; set; }
        public string Store { get; set; }
        public string RefDoc { get; set; }
        public int LineItemCount { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public string UserId { get; set; }
        public string UserName { get; set; }
        public IEnumerable<SelectListItem> Users { get; set; }
    }
}