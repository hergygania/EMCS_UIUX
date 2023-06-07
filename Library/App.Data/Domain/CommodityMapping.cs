namespace App.Data.Domain
{
	using System;
	using System.ComponentModel.DataAnnotations;
	using System.ComponentModel.DataAnnotations.Schema;

	[Table("imex.CommodityMapping")]
	public partial class CommodityMapping
	{
		[Key]
		public int MappingID { get; set; }

		public int CommodityID { get; set; }

		public int HSId { get; set; }

		public byte Status { get; set; }

		public DateTime? EntryDate { get; set; }

		public DateTime? ModifiedDate { get; set; }

		[StringLength(20)]
		public string EntryBy { get; set; }

		[StringLength(20)]
		public string ModifiedBy { get; set; }
	}
}
