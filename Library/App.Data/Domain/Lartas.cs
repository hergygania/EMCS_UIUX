namespace App.Data.Domain
{
	using System;
	using System.Collections.Generic;
	using System.ComponentModel.DataAnnotations;
	using System.ComponentModel.DataAnnotations.Schema;
	using System.Data.Entity.Spatial;

	[Table("mp.Lartas")]
	public partial class Lartas
	{
		[Key]
		[DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
		public int LartasID { get; set; }

		[Required]
		[StringLength(100)]
		public string Description { get; set; }

		public byte Status { get; set; }

		public DateTime? EntryDate { get; set; }

		public DateTime? ModifiedDate { get; set; }

		[StringLength(20)]
		public string EntryBy { get; set; }

		[StringLength(20)]
		public string ModifiedBy { get; set; }
	}
}
