using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using App.Data.Domain.Extensions;
using System.ComponentModel.DataAnnotations;

namespace App.Web.Models.CAT
{
    public class ForecastAccuracyFilter
    {
        public List<App.Data.Domain.MasterCustomer> customer_list { get; set; }
        public List<StoreList> store_list { get; set; }

        public List<Month> month_list = new List<Month> {
            #region items
            new Month() { ID = 1, Name = "January" },
            new Month() { ID = 2, Name = "February" },
            new Month() { ID = 3, Name = "March" },
            new Month() { ID = 4, Name = "April" },
            new Month() { ID = 5, Name = "May" },
            new Month() { ID = 6, Name = "June" },
            new Month() { ID = 7, Name = "July" },
            new Month() { ID = 8, Name = "August" },
            new Month() { ID = 9, Name = "September" },
            new Month() { ID = 10, Name = "October" },
            new Month() { ID = 11, Name = "November" },
            new Month() { ID = 12, Name = "December" }
            #endregion
        };

        public int? month_id { get; set; }
        public int? year { get; set; }
        public string customer_id { get; set; }
        public string store_id { get; set; }
    }

    public class Month {
        public int ID { get; set; }
        public string Name { get; set; }
    }
}