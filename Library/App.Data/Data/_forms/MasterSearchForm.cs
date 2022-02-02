using System;

namespace App.Domain
{
	public class MasterSearchForm
	{
		//srcBrnId
		public int? searchId { get; set; }
		public int? searchId2 { get; set; }
		public string searchCode { get; set; }
		public string searchName { get; set; }
		public byte? bStatus { get; set; }
		public byte? bFlag { get; set; }
		public string flag { get; set; }
		public DateTime? date1 { get; set; }
		public DateTime? date2 { get; set; }
	}

	//Extra classes to format the results the way the select2 dropdown wants them
	public class Select2PagedResult
	{
		public int Total { get; set; }
		public int page { get; set; }
		public string pagination { get; set; }
		public System.Collections.Generic.List<Select2Result> Results { get; set; }
	}

	public class Select2Result
	{
		public string id { get; set; }
		public string text { get; set; }
	}
	public class Select2Result2
	{
		public int id { get; set; }
		public string text { get; set; }
	}

	public class Select2Result3
	{
		public long id { get; set; }
		public string text { get; set; }
	}

	public class Select2Result4
	{
		public string id { get; set; }
		public string text { get; set; }
		public string extra { get; set; }
	}
}

namespace App.Domain.Master
{
	public class KeyValue
	{
		public Int16 KeyId { get; set; }
		public string Key { get; set; }
		public string Value { get; set; }
		public string Attr { get; set; }
	}
}

