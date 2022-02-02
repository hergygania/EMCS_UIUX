using System;

namespace App.Data.Domain.Extensions
{
    public  class StoreTable
    {
        public int StoreID { get; set; }

        public string StoreNo { get; set; }

        public string Plant { get; set; }

        public string Name { get; set; }

        public string PrevName { get; set; }

        public string JCode { get; set; }

        public string Description { get; set; }

        public int? AreaID { get; set; }

        public int? HubID { get; set; }

        public int? RegionID { get; set; }

        public int? TimeZone { get; set; }

        public string TimeZoneSelect { get; set; }

        public string C3LC { get; set; }

        public DateTime? EntryDate { get; set; }

        public DateTime? ModifiedDate { get; set; }

        public string EntryBy { get; set; }

        public string ModifiedBy { get; set; }
        public string SelectedHub { get; set; }
        public string SelectedArea { get; set; }
        public string SelectedRegion { get; set; }
    }
}