using System;
using System.ComponentModel.DataAnnotations.Schema;
namespace App.Data.Domain
{
    public partial class UserAccess
    {
        [NotMapped]
        public string RoleAccess { get; set; }
        [NotMapped]
        public string RoleAccessMode { get; set; }
        [NotMapped]
        public string Cust_Group_NoCap { get; set; }
        [NotMapped]
        public string PasswordNew { get; set; }
        [NotMapped]
        public int LoginCount { get; set; }
        [NotMapped]
        public DateTime? LastLoginTime { get; set; }
        [NotMapped]
        public string SelectedGroup { get; set; }

        [NotMapped]
        public string SelectedLevel { get; set; }
        [NotMapped]
        public string SelectedRole { get; set; }
    }
}
