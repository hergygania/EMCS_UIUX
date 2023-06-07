using App.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace App.Web.Models
{
    public class ReportFilterDeliveryTrackingStatusView 
    {
        public DateTime? ETD { get; set; }
        public DateTime? ATD { get; set; }
        public DateTime? ETA { get; set; }
        public DateTime? ATA { get; set; }
        public List<Select2Result> ModaList { get; set; }
        public List<Select2Result> OriginList { get; set; }
        public List<Select2Result> DestinationList { get; set; }
        public List<Select2Result> StatusList { get; set; }
        public List<Select2Result> UnitTypeList { get; set; }
        public string NODA { get; set; }
    }
}