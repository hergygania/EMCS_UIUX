namespace App.Data.Domain
{
	using System;
	using System.Collections.Generic;
	using System.ComponentModel.DataAnnotations;
	using System.ComponentModel.DataAnnotations.Schema;
	using System.Data.Entity.Spatial;

	[Table("imex.ShipmentManifest")]
	public partial class ShipmentManifest
	{
		[Key]
		public int ShipmentManifestID { get; set; }

		public int ShipmentID { get; set; }

		[Required]
		[StringLength(50)]
		public string ManifestNumber { get; set; }

		[Required]
		[StringLength(50)]
		public string ContainerNumber { get; set; }

		public DateTime EntryDate { get; set; }

		public DateTime ModifiedDate { get; set; }

		[Required]
		[StringLength(30)]
		public string EntryBy { get; set; }

		[Required]
		[StringLength(30)]
		public string ModifiedBy { get; set; }

		[Column(TypeName = "date")]
		[DatabaseGenerated(DatabaseGeneratedOption.Computed)]
		public DateTime? EntryDate_Date { get; set; }
	}
}
