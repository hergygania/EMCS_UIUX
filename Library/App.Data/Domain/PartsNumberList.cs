namespace App.Data.Domain
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("pis.PartsNumberList")]
    public partial class PartsNumberList
    {
        [Key]
        public int PartsID { get; set; }
        public string PartsNumber { get; set; }
        public string PartsName { get; set; }
        public Nullable<int> OMID { get; set; }
        public string OMCode { get; set; }
        public string ManufacturingCode { get; set; }
        public string Description { get; set; }
        public string Description_Bahasa { get; set; }
        public decimal Pref_Tarif { get; set; }
        public decimal PPNBM { get; set; }
        public decimal Add_Change { get; set; }
        public byte DeletionFlag { get; set; }
        public string EntryBy { get; set; }
        public string ModifiedBy { get; set; }
        public DateTime? EntryDate { get; set; }
        public DateTime? ModifiedDate { get; set; }

        //Add Remand Indicator, Change OM add
        //public bool? RemandIndicator { get; set; }
        public bool RemandIndicator { get; set; }
        public bool UTN { get; set; }
        public bool ChangeOM { get; set; }
        public string ChangedOMCode { get; set; }


    }
}
