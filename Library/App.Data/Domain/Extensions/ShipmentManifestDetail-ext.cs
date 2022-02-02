namespace App.Data.Domain
{
	using System;
	using System.ComponentModel.DataAnnotations.Schema;

	public partial class ShipmentManifestDetail
    {
			[NotMapped]
			public string BLAWB { get; set; }
			[NotMapped]
			public string Vessel { get; set; }
			[NotMapped]
			public string ManifestNumber { get; set; }
			[NotMapped]
			public string InvoiceNo { get; set; }
			[NotMapped]
			public DateTime InvoiceDate { get; set; }
			[NotMapped]
			public string AgreementType { get; set; }
			[NotMapped]
			public string CaseNo { get; set; }
			[NotMapped]
			public string CaseType { get; set; }
			[NotMapped]
			public string CaseDescription { get; set; }
			[NotMapped]
			public decimal? WeightKG { get; set; }
			[NotMapped]
			public decimal? LengthCM { get; set; }
			[NotMapped]
			public decimal? WideCM { get; set; }
			[NotMapped]
			public decimal? HeightCM { get; set; }
			[NotMapped]
			public string JCode { get; set; }
			[NotMapped]
			public string StoreNumber { get; set; }
			[NotMapped]
			public string DA { get; set; }
			[NotMapped]
			public string PartsNumber { get; set; }
			[NotMapped]
			public string PartsDescriptionShort { get; set; }
			[NotMapped]
			public int? InvoiceItemQty { get; set; }

			[NotMapped]
			public DateTime ETD { get; set; }
			[NotMapped]
			public DateTime ETA { get; set; }
			[NotMapped]
			public DateTime? ATD { get; set; }
			[NotMapped]
			public string LoadingPortDesc { get; set; }
			[NotMapped]
			public string DestinationPortDesc { get; set; }

			[NotMapped]
			public DateTime? ModifiedDate { get; set; }
			[NotMapped]
			public string ModifiedBy { get; set; }

			[NotMapped]
			public decimal totPackage { get; set; }
			
		}
}
