namespace App.Data.Domain
{
	using System.ComponentModel.DataAnnotations.Schema;

	public partial class PartsOrder
	{
		[NotMapped]
		public string ShippingInstruction { get; set; }
		[NotMapped]
		public string Email { get; set; }

	}
}
