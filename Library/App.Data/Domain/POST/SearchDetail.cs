using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Data.Domain.POST
{
    public class SearchDetail
    {
        public string Item { get; set; }

        public string Status { get; set; }

        public string StartDateDeliveryDate { get; set; }

        public string EndDateDeliveryDate { get; set; }

        public string Destination { get; set; }
    }
}
