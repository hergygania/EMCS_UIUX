using System;
using System.Collections;
using App.Framework.Mvc.UI.Sorting;

namespace App.Framework.Mvc.UI
{
	public interface IPagination : IEnumerable
	{
		string Name { get; set; }

		int PageNumber { get; set; }

		int PageSize { get; set; }

		int TotalItems { get; }

		int TotalPages { get; }

		int FirstItem { get; }

		int LastItem { get; }

		bool HasPreviousPage { get; }

		bool HasNextPage { get; }

		SortOptions Sort { get; set; }

		JsonObject ToJsonObject();
	}
}
