namespace App.Data.Domain
{
	using System;
	using System.Collections.Generic;
	using System.ComponentModel.DataAnnotations;
	using System.ComponentModel.DataAnnotations.Schema;
	using System.Data.Entity.Spatial;

	[Table("imex.PartsMappingHistory")]
	public partial class PartsMappingHistory
	{
		[Key]
		[DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
		public int HistoryID { get; set; }

		public int PartsMappingID { get; set; }

		public int PartsId { get; set; }

		public int HSId { get; set; }

		public byte Status { get; set; }

		public DateTime? EntryDate { get; set; }

		public DateTime? ModifiedDate { get; set; }

        [StringLength(10)]
        public string ManufacturingCode { get; set; }

		[StringLength(20)]
		public string EntryBy { get; set; }

		[StringLength(20)]
		public string ModifiedBy { get; set; }

		[Required]
		[StringLength(50)]
		public string ActionUser { get; set; }

		[Required]
		[StringLength(100)]
		public string Source { get; set; }
	}
}
