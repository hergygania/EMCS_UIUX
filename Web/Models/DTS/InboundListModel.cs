using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace App.Web.Models.DTS
{
    public class InboundListModel
    {
        public string AjuNo { get; set; }
        public string MSONo { get; set; }
        public string PONo { get; set; }
        public string LoadingPort { get; set; }
        public string DischargePort { get; set; }
        public string Model { get; set; }
        public string ModelDescription { get; set; }
        public string Status { get; set; }
        public string SerialNumber { get; set; }
        public string ETAPort { get; set; }
        public string ETACakung { get; set; }
        public string ATAPort { get; set; }
        public string ATACakung { get; set; }
        public string Notes { get; set; }
    }
}