using App.Data.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace App.Web.Models
{
    public class CustomerOrderViewModel : V_CLIENTORDER_HEADER
    {
        public List<V_GET_CUSTOMER> CustomerList { get; set; }
    }
}