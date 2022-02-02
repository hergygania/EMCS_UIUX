using System.Collections;

namespace App.Framework.Mvc.UI
{
  public interface IFilter<F> where F : class, new()
	{
		F Filter { get; set; }
	}

	public interface IData<I>
	{
		IPagination<I> Pagination { get; set; }
	}

	public interface IPaginationFilter
	{
		object Filter { get; set; }
		IEnumerable Pagination { get; set; }
	}

	public class PaginationFilter<F, I> : IPaginationFilter, IFilter<F>, IData<I>
		where F : class, new()
	{
		public PaginationFilter() : this(true) { }
 
		public PaginationFilter(bool init)
		{
			if (init) Filter = new F();
		}

		public F Filter { get; set; }

		public IPagination<I> Pagination { get; set; } 

		object IPaginationFilter.Filter
		{
			get
			{
				return Filter;
			}
			set
			{
				Filter = (F)value;
			}
		}

		IEnumerable IPaginationFilter.Pagination
		{
			get
			{
				return Pagination;
			}
			set
			{
        Pagination =  (IPagination<I>)value;
			}
		}
  }
}
