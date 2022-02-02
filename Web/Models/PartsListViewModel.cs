using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using App.Data.Domain;

namespace App.Web.Models
{
    public class PartsListViewModel
    {
        public PartsList PartsList { get; set; }
        public IEnumerable<OrderMethod> OrderMethods { get; set; }
        public bool SelectedStatus { get; set; }
    }
}