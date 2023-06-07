namespace App.Data.Domain
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Store")]
    public partial class Store
    {
        //[Key]
        public int StoreID { get; set; }

        [StringLength(255)]
        public string StoreNo { get; set; }

        [StringLength(255)]
        public string Name { get; set; }

        [StringLength(255)]
        public string Plant { get; set; }

        [StringLength(255)]
        public string PrevName { get; set; }

        [StringLength(5)]
        public string JCode { get; set; }

        [Column(TypeName = "text")]
        public string Description { get; set; }

        public int? AreaID { get; set; }

        public int? HubID { get; set; }

        public int? RegionID { get; set; }

        public int? TimeZone { get; set; }

        [Column("3LC")]
        [StringLength(255)]
        public string C3LC { get; set; }

        public DateTime? EntryDate { get; set; }

        public DateTime? ModifiedDate { get; set; }

        [StringLength(20)]
        public string EntryBy { get; set; }

        [StringLength(20)]
        public string ModifiedBy { get; set; }
    }
}
