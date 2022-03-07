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
<<<<<<< HEAD
<<<<<<< HEAD
        public string typesearch { get; set; }
=======
>>>>>>> 639d8d0 (Intial commit)
=======
        public string typesearch { get; set; }
>>>>>>> 93c2efe ([U] Update from client's TFS)
        public List<FilterColumn> filterColumns { get; set; }
    }
}
