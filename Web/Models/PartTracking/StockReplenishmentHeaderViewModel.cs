using App.Data.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace App.Web.Models
{
    public class StockReplenishmentHeaderViewModel: V_STOCKORDER_HEADER
    {
        public List<Hub> HubList { get; set; }
        public List<Area> AreaList { get; set; }

        public List<Store> StoreNumberList { get; set; }

        public List<OrderProfile> OrderProfileList { get; set; }

        public string param_rporne { get; set; }
        public string param_ordsos { get; set; }
        public string param_stno { get; set; }
        public string param_partNumber { get; set; }
        
    }
}