namespace App.Data.Domain
{
	using System.Collections.Generic;
	using System.ComponentModel.DataAnnotations.Schema;

	public class SurveyCollection
	{
		public Data.Domain.Survey survey { get; set; }
		public List<Data.Domain.SurveyDetail> surveyDetail { get; set; }
	}
}
