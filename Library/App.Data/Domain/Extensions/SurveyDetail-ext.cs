using System;
using System.ComponentModel.DataAnnotations.Schema;
namespace App.Data.Domain
{

	public partial class SurveyDetail
	{
		[NotMapped]
		public string InvoiceNo { get; set; }
		[NotMapped]
		public DateTime InvoiceDate { get; set; }
		[NotMapped]
		public string PrimPSO { get; set; }
		[NotMapped]
		public string PartsNumber { get; set; }
		[NotMapped]
		public int? InvoiceItemQty { get; set; }
		[NotMapped]
		public decimal? PartGrossWeight { get; set; }
		[NotMapped]
		public decimal TotalWeight
		{
			get { return InvoiceItemQty.HasValue && PartGrossWeight.HasValue ? InvoiceItemQty.Value * PartGrossWeight.Value : 0; }
		}
		[NotMapped]
		public string HSCode { get; set; }
		[NotMapped]
		public string HSDescription { get; set; }
		[NotMapped]
		public string PartsName { get; set; }
		[NotMapped]
		public string OMCode { get; set; }		
		[NotMapped]
		public string COODescription { get; set; }
		[NotMapped]
		public string dml { get; set; }
        [NotMapped]
        public string CaseNo { get; set; }

    }
}
