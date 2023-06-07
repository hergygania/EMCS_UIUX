using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Data.Domain
{
    public partial class RoleAccessMenu
    {
       [NotMapped]
        public string SelectedRole { get; set; }
        [NotMapped]
        public string SelectedMenu { get; set; }
    }
}
