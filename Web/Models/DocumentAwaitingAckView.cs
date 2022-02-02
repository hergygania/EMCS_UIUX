﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace App.Web.Models
{
    public class DocumentAwaitingAckView
    {
        public int Id { get; set; }
        public string Store { get; set; }
        public string RefDoc { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public string CustId { get; set; }
        public string CustName { get; set; }
        public IEnumerable<SelectListItem> Customers { get; set; }
    }
}
