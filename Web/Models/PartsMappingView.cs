using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using App.Data.Domain;

namespace App.Web.Models
{
	public class PartsMappingView
	{

		public Data.Domain.PartsMapping partsMapping
		{
			get;
			set;
		}

		public int PartsMappingID { get; set; }
		public int Status { get; set; }
        public bool IsNullHSCode { get; set; }
        public string HSDescription { get; set; }
		public string PartsName { get; set; }

        public string ManufacturingCode { get; set; }
        public Decimal? PPNBM { get; set; }
        public Decimal? Pref_Tarif { get; set; }
        public string Description_Bahasa { get; set; }
        public Decimal? Add_Change { get; set; }
		
		public IEnumerable<string> selPartsList_Ids { get; set; }
		public IEnumerable<string> selPartsList_Names { get; set; }
		public IEnumerable<string> selHSCodeList_Ids { get; set; }
		public IEnumerable<string> selHSCodeLists_Names { get; set; }
		public IEnumerable<string> selOrderMethods { get; set; }
        public int offset { get; set; }
        public int limit { get; set; }
        public int totalcount { get; set; }
		public MultiSelectList OrderMethodsList { get; set; }
	}
}