using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace App.Web.Models
{
    public class listLeadTime
    {
        public int id { get; set; }
        public string STNO { get; set; }
        public string STNAME { get; set; }
        public string FILTERTYPE { get; set; }
        public string LEADTIME { get; set; }
        public string PICKUPTIME1 { get; set; }
        public string PICKUPTIME2 { get; set; }
        public bool ISACTIVE { get; set; }
    }

    public class getStoreNo
    {
        public string StoreNo { get; set; }
        public string Name { get; set; }
    }

    public class addLeadTime
    {
        public int id { get; set; }
        public string storeID { get; set; }
        public string filter_type { get; set; }
        public string leadTime { get; set; }
        public string pickUpTime1 { get; set; }
        public string pickUpTime2 { get; set; }
    }
}