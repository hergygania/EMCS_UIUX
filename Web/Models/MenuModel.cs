using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using App.Data.Domain;
using App.Domain;

namespace App.Web.Models
{
    public class MenuModel
    {
        public Menu Menu { get; set; }
        //public List<Menu> MenuList { get; set; }
        public List<Select2Result> MenuList { get; set; }
    }
}