namespace App.Data.Domain
{
	using System.ComponentModel.DataAnnotations.Schema;

	public partial class ShipmentManifest
	{
		[NotMapped]
		public int totPackage { get; set; }

		[NotMapped]
		public string dml { get; set; }
	}
}
