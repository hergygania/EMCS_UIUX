namespace App.Data.Domain
{
	using System;
	using System.Collections.Generic;
	using System.ComponentModel.DataAnnotations;
	using System.ComponentModel.DataAnnotations.Schema;
	using System.Data.Entity.Spatial;

	[Table("imex.ShipmentManifestDetail")]
	public partial class ShipmentManifestDetail
	{
		[Key]
		public long DetailID { get; set; }
		public int ShipmentManifestID { get; set; }
		public int ShipmentID { get; set; }

		public long PartsOrderID { get; set; }
		public long CaseID { get; set; }

		[StringLength(5)]
		public string EndDestination { get; set; }
	}
}
