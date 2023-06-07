using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Data.Domain.DTS
{
    public class ReportDeliveryRequisitionFilter
    {
        public string Activity { get; set; }
        public string Status { get; set; }
        public List<FilterColumn> filterColumns { get; set; }
    }
}
