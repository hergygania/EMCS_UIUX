using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Data.Domain.Extensions
{
    public class SupplyDetail
    {
        public int ID { get; set; }
        public int ParentID { get; set; }
        public string Name { get; set; }
        public string Url { get; set; }
        public string HUBSTORE { get; set; }
        public int Value { get; set; }
        public virtual ICollection<SupplyDetail> children { get; set; }
    }
}
