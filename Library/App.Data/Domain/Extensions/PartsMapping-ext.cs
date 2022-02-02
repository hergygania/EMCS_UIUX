namespace App.Data.Domain
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class PartsMapping
    {
			[NotMapped]
			public string HSCode { get; set; }			
			[NotMapped]
			public string HSDescription { get; set; }
			[NotMapped]
			public string HSCodeCap { get; set; }
			[NotMapped]
			public string PartsNumber { get; set; }

			[NotMapped]
			public string PartsName { get; set; }
			[NotMapped]
			public string PartsNameCap { get; set; }
			[NotMapped]
			public string OMCode { get; set; }
            [NotMapped]
            public Decimal? PPNBM { get; set; }
            [NotMapped]
            public Decimal? Pref_Tarif { get; set; }
            [NotMapped]
            public string Description_Bahasa { get; set; }
            [NotMapped]
            public Decimal? Add_Change { get; set; }

    }
}
