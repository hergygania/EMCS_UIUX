namespace App.Data.Domain
{
	using System;
	using System.Collections.Generic;
	using System.ComponentModel.DataAnnotations;
	using System.ComponentModel.DataAnnotations.Schema;
	using System.Data.Entity.Spatial;

	[Table("imex.Survey")]
	public partial class Survey
	{
		[Key]
		public int SurveyID { get; set; }

		[StringLength(50)]
		public string VRNo { get; set; }

		[Column(TypeName = "date")]
		public DateTime VRDate { get; set; }

		public int CommodityID { get; set; }
		public byte Freight { get; set; }

		[StringLength(50)]
		public string VONo { get; set; }

		[Column(TypeName = "date")]
		public DateTime? RFIDate { get; set; }

		[Column(TypeName = "date")]
		public DateTime? SurveyDate { get; set; }

		[Column(TypeName = "date")]
		public DateTime? SurveyDone { get; set; }

		public string Email { get; set; }
		public string eLS { get; set; }

		public DateTime? FDSubmit { get; set; }
		public DateTime? PaymentDate { get; set; }
		[StringLength(50)]
		public string PaymentInvoiceNo { get; set; }
		public string Remark { get; set; }
		public byte Status { get; set; }

		public DateTime EntryDate { get; set; }

		public DateTime ModifiedDate { get; set; }

		[Required]
		[StringLength(30)]
		public string EntryBy { get; set; }

		[Required]
		[StringLength(30)]
		public string ModifiedBy { get; set; }

		[Column(TypeName = "date")]
		[DatabaseGenerated(DatabaseGeneratedOption.Computed)]
		public DateTime? EntryDate_Date { get; set; }

		[Column(TypeName = "date")]
		[DatabaseGenerated(DatabaseGeneratedOption.Computed)]
		public DateTime? ModifiedDate_Date { get; set; }

        [StringLength(20)]
        public string ManufacturingCode { get; set; }

        public Nullable<Int32> SurveyLocationId { get; set; }
    }
}
