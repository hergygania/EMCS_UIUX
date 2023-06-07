namespace App.Data.Domain
{
	using System.ComponentModel.DataAnnotations.Schema;

	public partial class SurveyDocument
	{
		[NotMapped]
		public string DocumentName { get; set; }
		[NotMapped]
		public string dml { get; set; }
	}
}
