using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using App.Data.Domain;

namespace App.Web.Models
{
    public class StoreViewModel
    {
        public Store Store { get; set; }   
        public List<Hub> HubList { get; set; }
        public List<Area> AreaList { get; set; }
        public List<Region> RegionList { get; set; }
    }
}