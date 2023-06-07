namespace App.Data.Domain
{
	using System.ComponentModel.DataAnnotations.Schema;

	public partial class CommodityMapping
	{
		[NotMapped]
		public string CommodityCode { get; set; }
		[NotMapped]
		public string CommodityName { get; set; }
		[NotMapped]
		public string CommodityCap { get; set; }
		[NotMapped]
		public string HSCode { get; set; }
		[NotMapped]
		public string HSDescription { get; set; }
		[NotMapped]
		public string HSCodeCap { get; set; }
	}

	public partial class CommodityImex
	{
		[NotMapped]
		public bool hasChild { get; set; }
	}
}
