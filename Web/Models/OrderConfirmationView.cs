using System;
using System.Collections.Generic;
using System.Web.Mvc;

namespace App.Web.Models
{
    public class OrderConfirmationView
    {
        public int Id { get; set; }
        public string Store { get; set; }
        public string RefDoc { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public string DocValue { get; set; }
        public string CustId { get; set; }
        public string CustName { get; set; }
        public int? LineItemOrder { get; set; }
        public IEnumerable<SelectListItem> Customers { get; set; }
    }
}