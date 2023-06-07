namespace App.Data.Domain
{
	using System.ComponentModel.DataAnnotations.Schema;

	public partial class Survey
	{
		[NotMapped]
		public string CommodityCode { get; set; }
		[NotMapped]
		public string CommodityName { get; set; }
		[NotMapped]
		public byte VettingRoute { get; set; }
		[NotMapped]
		public string EmailKso { get; set; }
		[NotMapped]
		public string EmailVO { get; set; }
		[NotMapped]
		public string EmailRFI { get; set; }
	}
}
