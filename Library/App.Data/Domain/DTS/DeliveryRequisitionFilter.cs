using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Data.Domain.DTS
{
    public class DeliveryRequisitionFilter
    {
        public string searchName { get; set; }
        public string IdString { get; set; }
        public bool requestor { get; set; }
        public string status { get; set; }
        public string today  { get; set; }
        public List<FilterColumn> filterColumns { get; set; }
    }
}
