using System.Collections.Generic;
using System.Data;
using System.Data.Linq;
using System.Linq;
using App.Framework.Mvc.UI.Sorting;

namespace App.Framework.Mvc.UI
{
	public static class PaginationHelper
  {
    #region Pagination<T>
    public static IPagination<T> AsPagination<T>(this IEnumerable<T> source)
			where T : class
		{
			return AsPagination(source, null);
		}

		public static IPagination<T> AsPagination<T>(this IEnumerable<T> source, string name)
			where T : class
		{
			return AsPagination(source, name, null, 0, 0);
		}

		public static IPagination<T> AsPagination<T>(this IEnumerable<T> source, int pageNumber)
			where T : class
		{
			return AsPagination(source, pageNumber, 0);
		}

		public static IPagination<T> AsPagination<T>(this IEnumerable<T> source, SortOptions shortOption, int pageNumber)
			where T : class
		{
			return AsPagination(source, shortOption, pageNumber, 0);
		}

		public static IPagination<T> AsPagination<T>(this IEnumerable<T> source, int pageNumber, int pageSize)
			where T : class
		{
			return AsPagination(source, null, pageNumber, pageSize);
		}

		public static IPagination<T> AsPagination<T>(this IEnumerable<T> source, SortOptions shortOption, int pageNumber, int pageSize)
			where T : class
		{
			return AsPagination(source, null, shortOption, pageNumber, pageSize);
		}

		public static IPagination<T> AsPagination<T>(this IEnumerable<T> source, string name, SortOptions shortOption, int pageNumber, int pageSize)
			where T :class 
		{
			return new LazyPagination<T>(source.AsQueryable(), name, shortOption, pageNumber, pageSize);
		}
		#endregion

	}
}
