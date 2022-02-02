namespace App.Data.Domain
{
	using System.ComponentModel.DataAnnotations.Schema;

	public partial class PartsOrderCase
	{		
		[NotMapped]
		public string JCode { get; set; }
		[NotMapped]
		public string StoreNumber { get; set; }
		[NotMapped]
		public string AgreementType { get; set; }
		[NotMapped]
		public string DA { get; set; }
		[NotMapped]
		public string dml { get; set; }	
	}
}
