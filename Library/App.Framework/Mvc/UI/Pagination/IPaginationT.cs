using System.Collections.Generic;
using System.Linq;

namespace App.Framework.Mvc.UI
{
	public interface IPagination<T> : IPagination, IEnumerable<T>
	{
    IQueryable<T> Query { get; }
		IList<ItemDescription> GetDescriptions();// { get; }
	}
}
