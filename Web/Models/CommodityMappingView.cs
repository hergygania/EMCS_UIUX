using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using App.Data.Domain;

namespace App.Web.Models
{
	public class CommodityMappingView
	{

		public Data.Domain.CommodityMapping CommodityMapping
		{
			get;
			set;
		}

		public int MappingID { get; set; }
		public int Status { get; set; }
		public string CommodityName { get; set; }
		public string HSDescription { get; set; }
		public IEnumerable<string> selHSCodeList_Ids { get; set; }
		public IEnumerable<string> selHSCodeLists_Names { get; set; }
	}
}