namespace App.Data.Domain
{
	using System.ComponentModel.DataAnnotations.Schema;

	public partial class Shipment
	{
		[NotMapped]
		public string LoadingPortDesc { get; set; }
		[NotMapped]
		public string DestinationPortDesc { get; set; }
		[NotMapped]
		public int totManifest { get; set; }
		[NotMapped]
		public int totPackage { get; set; }
		[NotMapped]
		public bool IsSeaFreight { get; set; }
		[NotMapped]
		public string Freight { get{return IsSeaFreight ? "Sea" : "Air"; }}
		[NotMapped]
		public string Email { get; set; }

	}
}
