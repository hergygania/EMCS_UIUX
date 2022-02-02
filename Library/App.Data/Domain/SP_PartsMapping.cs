namespace App.Data.Domain
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class SP_PartsMapping
    {
        public int PartsMappingID { get; set; }

        public int PartsId { get; set; }

        public int HSId { get; set; }

        public byte Status { get; set; }

        public DateTime? EntryDate { get; set; }

        public DateTime? ModifiedDate { get; set; }

        public string ManufacturingCode { get; set; }

        public string EntryBy { get; set; }

        public string ModifiedBy { get; set; }

        public string ActionUser { get; set; }

        public string Source { get; set; }

        public string HSCode { get; set; }

        public string HSDescription { get; set; }

        public string HSCodeCap { get; set; }

        public string PartsNumber { get; set; }

        public string PartsName { get; set; }

        public string PartsNameCap { get; set; }

        public string OMCode { get; set; }

        public Decimal? PPNBM { get; set; }

        public Decimal? Pref_Tarif { get; set; }

        public string Description_Bahasa { get; set; }

        public Decimal? Add_Change { get; set; }

        public Decimal? BeaMasuk { get; set; }
    }
}
