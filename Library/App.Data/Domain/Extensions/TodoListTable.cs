using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Data.Domain
{
    public class ToDoListTable
    {
        public int ID { get; set; }
        public int Parent { get; set; }
        public string Name { get; set; }
        public string Url { get; set; }
        public string AREASTORE { get; set; }
        public int Value { get; set; }
        public DateTime? CreatedOn { get; set; }

        //public string Name { get; set; }
        //public int Value { get; set; }
        //public decimal? TotalValue { get; set; }
        //public decimal? TotalWeight { get; set; }

        //public string Url { get; set; }
        //public DateTime? CreatedOn { get; set; }
    }
}
