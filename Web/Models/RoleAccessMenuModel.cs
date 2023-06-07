using App.Data.Domain;
using App.Data.Domain.Extensions;
using App.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace App.Web.Models
{
    public class RoleAccessMenuModel
    {
        public List<RoleAccessDetailsMenu> RoleAccessDetailsMenu { get; set; }
        public List<Select2Result> SelectMenu { get; set; }
        public List<Select2Result> SelectRole { get; set; }
    }
}