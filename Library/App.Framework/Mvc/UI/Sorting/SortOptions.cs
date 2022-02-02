using System;

namespace App.Framework.Mvc.UI.Sorting
{
	[Serializable]
	public class SortOptions
	{
		public string SortBy { get; set; }
		public SortDirection SortOrder { get; set; }
	}
}
