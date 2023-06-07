using System;

namespace App.Web.Models
{
    public class StoreGridViewModel
    {
        public int StoreID { get; set; }

        public string StoreNo { get; set; }

        public string Name { get; set; }
        public string PrevName { get; set; }


        public string Description { get; set; }

        public int? AreaID { get; set; }

        public int? HubID { get; set; }

        public DateTime? EntryDate { get; set; }

        public DateTime? ModifiedDate { get; set; }

        public string EntryBy { get; set; }

        public string ModifiedBy { get; set; }
        public string SelectedHub { get; set; }
        public string SelectedArea { get; set; }
    }
}