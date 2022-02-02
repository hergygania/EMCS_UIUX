namespace App.Data.Domain
{
	using System;
	using System.Collections.Generic;
	using System.ComponentModel.DataAnnotations;
	using System.ComponentModel.DataAnnotations.Schema;
	using System.Data.Entity.Spatial;

	[Table("mp.FreightPort")]
	public partial class FreightPort
	{
		[Key]
		public int PortID { get; set; }

		[Required]
		[StringLength(10)]
		public string PortCode { get; set; }

		[StringLength(100)]
		public string PortName { get; set; }

		[StringLength(100)]
		public string Description { get; set; }

		public byte Status { get; set; }

		public bool IsSeaFreight { get; set; }

		public DateTime? EntryDate { get; set; }

		public DateTime? ModifiedDate { get; set; }

		[StringLength(20)]
		public string EntryBy { get; set; }

		[StringLength(20)]
		public string ModifiedBy { get; set; }
	}
}
