using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Data.Domain.Extensions
{
    public class UserAccessTable
    {
        public string UserID { get; set; }
        public string FullName { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string UserType { get; set; }
        public string Cust_Group_No { get; set; }
        public byte? Status { get; set; }
        public int? GroupID { get; set; }
        public int? LevelID { get; set; }
        public int? RoleID { get; set; }
        public string Position { get; set; }
        public DateTime? EntryDate { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public string EntryBy { get; set; }
        public string ModifiedBy { get; set; }
        public string SelectedGroup { get; set; }
        public string SelectedLevel { get; set; }
        public string SelectedRole { get; set; }
    }
}
