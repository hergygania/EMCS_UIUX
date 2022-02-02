namespace App.Data.Domain
{
	using System;
	using System.Collections.Generic;
	using System.ComponentModel.DataAnnotations;
	using System.ComponentModel.DataAnnotations.Schema;
	using System.Data.Entity.Spatial;

	[Table("mp.AgreementType")]
	public partial class AgreementTypes
	{
		[Key]
		public int AgreementTypeID { get; set; }

		[Required]
		[StringLength(20)]
		public string AgreementType { get; set; }
		public string Description { get; set; }

	}
}
