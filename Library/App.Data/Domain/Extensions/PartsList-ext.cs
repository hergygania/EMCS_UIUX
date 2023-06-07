namespace App.Data.Domain
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class PartsList
    {
        [NotMapped]
        public string OMCode { get; set; }
        [NotMapped]
        public IEnumerable<OrderMethod> OrderMethods { get; set; }
        [NotMapped]
        public bool ChangeOM { get; set; }
    }

    public partial class PartsListGroupByPartNumber
    {
        public int PartsID { get; set; }
        public string PartsNumber { get; set; }
        public string ManufacturingCode { get; set; }
    }

    public partial class SP_PartsList
    {
        public int PartsID { get; set; }
                          
        public string PartsNumber { get; set; }
                           
        public string ManufacturingCode { get; set; }
                           
        public string PartsName { get; set; }

        public string Description { get; set; }

        public byte Status { get; set; }

        public int? OMID { get; set; }

        public DateTime? EntryDate { get; set; }

        public DateTime? ModifiedDate { get; set; }

        public Decimal? PPNBM { get; set; }

        public Decimal? Pref_Tarif { get; set; }

        public string Description_Bahasa { get; set; }

        public Decimal? Add_Change { get; set; }

        public string EntryBy { get; set; }

        public string ModifiedBy { get; set; }

        public string OMCode { get; set; }
        
        public bool RemandIndicator { get; set; }

        public bool UTN { get; set; }

        public string ChangedOMCode { get; set; }
    }
}
