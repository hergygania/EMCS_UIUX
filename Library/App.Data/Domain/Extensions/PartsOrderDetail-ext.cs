namespace App.Data.Domain
{
	using System.ComponentModel.DataAnnotations.Schema;

	public partial class PartsOrderDetail
	{
	
		[NotMapped]
		public int ShippingInstructionID { get; set; }
		[NotMapped]
		public long? DANumber { get; set; }
		[NotMapped]
		public string AgreementType { get; set; }

		[NotMapped]
		public string JCode { get; set; }
		[NotMapped]
		public string StoreNumber { get; set; }
		[NotMapped]
		public string HSCode { get; set; }
		[NotMapped]
		public string HSDescription { get; set; }
		[NotMapped]
		public string PartsName { get; set; }
		[NotMapped]
		public string OMCode { get; set; }
		[NotMapped]
		public byte VettingRoute { get; set; }
		[NotMapped]
		public decimal TotalWeight { 
			get {return InvoiceItemQty.HasValue && PartGrossWeight.HasValue ? InvoiceItemQty.Value * PartGrossWeight.Value : 0;}
		}

	}
}
