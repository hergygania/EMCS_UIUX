using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Data.Domain.Extensions
{
    public class RoleAccessDetailsMenu
    {
        public int ID { get; set; }
        public int ParentID { get; set; }
        public Nullable<int> RoleID { get; set; }
        public string Name { get; set; }
        public string URL { get; set; }
        public string icon { get; set; }
        public Nullable<int> OrderNo { get; set; }
        public Nullable<bool> IsDefault { get; set; }
        //public string state { get; set; }
        public Nullable<bool> IsRead { get; set; }
        public Nullable<bool> IsCreated { get; set; }
        public Nullable<bool> IsUpdated { get; set; }
        public Nullable<bool> IsDeleted { get; set; }
        public virtual ICollection<RoleAccessDetailsMenu> children { get; set; }
    }
}
