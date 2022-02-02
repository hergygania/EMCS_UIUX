using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Data.Domain.Extensions
{
    public class menuTable
    {
        public int ID { get; set; }

        public int? ParentID { get; set; }

        public string Name { get; set; }

        public string URL { get; set; }

        public int? OrderNo { get; set; }

        public string Icon { get; set; }

        public bool IsDefault { get; set; }

        public bool IsActive { get; set; }

        public DateTime? EntryDate { get; set; }

        public DateTime? ModifiedDate { get; set; }

        public string EntryBy { get; set; }

        public string ModifiedBy { get; set; }

        public string SelectedParent { get; set; }
    }
}
