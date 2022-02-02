namespace App.Data.Domain
{
	using System;
	using System.Collections.Generic;
	using System.ComponentModel.DataAnnotations;
	using System.ComponentModel.DataAnnotations.Schema;
	using System.Data.Entity.Spatial;

	[Table("imex.Shipment")]
	public partial class Shipment
	{
		[Key]
		public int ShipmentID { get; set; }

		[StringLength(30)]
		[Required]
		public string BLAWB { get; set; }

		[StringLength(50)]
		[Required]
		public string Vessel { get; set; }
		public int ShippingInstructionID { get; set; }
		public int LoadingPortID { get; set; }

		public int DestinationPortID { get; set; }

		[Column(TypeName = "date")]
		public DateTime ETD { get; set; }

		[Column(TypeName = "date")]
		public DateTime ETA { get; set; }

		[Column(TypeName = "date")]
		public DateTime? ATD { get; set; }

		public byte VettingRoute { get; set; }

		public byte Status { get; set; }

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
