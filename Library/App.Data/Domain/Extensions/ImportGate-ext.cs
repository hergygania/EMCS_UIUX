using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace App.Data.Domain
{
	public partial class ImportGate
	{


		[NotMapped]
		public IEnumerable<FreightPort> SeaPorts { get; set; }
		[NotMapped]
		public IEnumerable<FreightPort> AirPorts { get; set; }

		[NotMapped]
		public string SeaPortCode { get; set; }

		[NotMapped]
		public string AirPortCode { get; set; }
		[NotMapped]
		public bool SelectedStatus { get; set; }
	}
}