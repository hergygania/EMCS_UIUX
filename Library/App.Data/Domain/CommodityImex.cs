namespace App.Data.Domain
{
	using System;
	using System.ComponentModel.DataAnnotations;
	using System.ComponentModel.DataAnnotations.Schema;

	[Table("mp.CommodityImex")]
	public partial class CommodityImex
	{
		[Key]
		public int ID { get; set; }

		[Required]
		[StringLength(10)]
		public string CommodityCode { get; set; }

		[Required]
		[StringLength(50)]
		public string CommodityName { get; set; }

		public byte Status { get; set; }
		public DateTime? EntryDate { get; set; }

		public DateTime? ModifiedDate { get; set; }

		[StringLength(20)]
		public string EntryBy { get; set; }

		[StringLength(20)]
		public string ModifiedBy { get; set; }
	}
}
