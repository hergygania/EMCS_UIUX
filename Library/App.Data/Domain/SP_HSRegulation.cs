namespace App.Data.Domain
{
    using System;
    public partial class SP_HSRegulation
    {
        public int HSID { get; set; }

        public decimal? BeaMasuk { get; set; }

        public string UnitBeaMasuk { get; set; }

        public string HSCode { get; set; }
                            
        public string HSCodeReformat { get; set; }

        public string HSDescription { get; set; }

        public byte Status { get; set; }

        public string OMCode { get; set; }

        public DateTime? EntryDate { get; set; }

        public DateTime? ModifiedDate { get; set; }

        public string EntryBy { get; set; }

        public string ModifiedBy { get; set; }
    }
}
