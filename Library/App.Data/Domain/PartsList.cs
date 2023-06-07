namespace App.Data.Domain
{
	using System;
	using System.Collections.Generic;
	using System.ComponentModel.DataAnnotations;
	using System.ComponentModel.DataAnnotations.Schema;
	using System.Data.Entity.Spatial;

	[Table("mp.PartsList")]
	public partial class PartsList
	{
		[Key]
		public int PartsID { get; set; }

		[Required]
		public string PartsNumber { get; set; }
        
        public string ManufacturingCode { get; set; }
        
		public string PartsName { get; set; }

		//[Column(TypeName = "text")]
		public string Description { get; set; }

		public int? OMID { get; set; }

        public DateTime? EntryDate { get; set; }

		public DateTime? ModifiedDate { get; set; }

        public Decimal? PPNBM { get; set; }

        public Decimal? Pref_Tarif { get; set; }

        [Column(TypeName = "text")]
        public string Description_Bahasa { get; set; }

        public Decimal? Add_Change { get; set; }
        
		public string EntryBy { get; set; }
        
		public string ModifiedBy { get; set; }

        public Nullable<byte> DeletionFlag { get; set; }

        //Add Remand Indicator, UTN and ChangedOMCode
        
        public bool RemandIndicator { get; set; }

        public bool UTN { get; set; }

        public string ChangedOMCode { get; set; }


        //[DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        //[Required]
        //[StringLength(9)]
        //public string PartsNumberReformat { get; set; }
    }
}
