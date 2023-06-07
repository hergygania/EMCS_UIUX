using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Data.Domain
{
    [Table("pis.MasterCustomerGroup")]
    public partial class MasterCustomerGroup
    {
        public int ID { get; set; }
        public string CustomerID { get; set; }
        public string CustomerGroup { get; set; }
        public DateTime CreatedDate { get; set; }
        public Nullable<DateTime> UpdatedDate { get; set; }
    }
}
