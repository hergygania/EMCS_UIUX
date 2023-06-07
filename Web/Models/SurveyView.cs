using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using App.Data.Domain;

namespace App.Web.Models
{
	public class SurveyView
	{

		public string Freight { get; set; }
		public byte VettingRoute { get; set; }
		public int SurveyID { get; set; }
		public int? Id { get; set; }
		public string VRNo { get; set; }
		public string VONo { get; set; }
		public DateTime? DateSta { get; set; }
		public DateTime? DateFin { get; set; }
		public string mode { get; set; }
	}
}