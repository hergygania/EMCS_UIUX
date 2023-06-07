using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using App.Data.Domain;
using System.Web.Mvc;

namespace App.Web.Models
{
    public class OrderThruCounterHeaderViewModel : V_CUSTORDER_HEADER
    {
        public List<Hub> HubList { get; set; }
        public List<Area> AreaList { get; set; }

        public List<Store> StoreNumberList { get; set; }

        public List<V_MODEL_LIST> ModelList { get; set; }

        public List<V_GET_CUSTOMER_GROUP> CustomerGroupList { get; set; }
        public List<V_GET_CUSTOMER> CustomerList { get; set; }


    }
}