using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace App.Web.Models
{
    public class updDoc
    {
        public string docNumber { get; set; }
        public string order_status { get; set; }
    }

    public class getCekDate
    {
        public int count { get; set; }
    }
}