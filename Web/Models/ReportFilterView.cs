using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using App.Data.Domain;
using App.Data.Domain.Extensions;
using App.Domain;

namespace App.Web.Models
{
    public class ReportFilterView
    {
        public string GroupType { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public string StoreNumber { get; set; }
        public string[] CustomerId { get; set; }
        public IEnumerable<SelectListItem> Customers { get; set; }
        public List<Select2Result> HubList { get; set; }
        public string FilterBy { get; set; }
        public List<Select2Result> AreaList { get; set; }
        public List<Select2Result> StoreNumberList { get; set; }
    }
}