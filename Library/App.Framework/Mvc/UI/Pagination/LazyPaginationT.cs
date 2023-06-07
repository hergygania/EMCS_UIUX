using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using App.Framework.Mvc.UI.Sorting;


namespace App.Framework.Mvc.UI
{
	public class LazyPagination<T> : IPagination<T>
		where T : class
	{
		#region Fields
		public const int DefaultPageSize = 15;
		bool m_needSort = false;
		private SortOptions m_sortOption = new SortOptions();
		private IList<ItemDescription> m_itemDesctiptions = null;
		#endregion

		#region Constructors
		public LazyPagination(IQueryable<T> query)
			: this(query, null, null, 0, 0)
		{
		}

		public LazyPagination(string name, IQueryable<T> query)
			: this(query, name, null, 0, 0)
		{
		}

		public LazyPagination(IQueryable<T> query, SortOptions shortOptions, int pageNumber, int pageSize)
			: this(query, null, shortOptions, pageNumber, pageSize)
		{
		}

		public LazyPagination(IQueryable<T> query, string name, SortOptions shortOptions, int pageNumber, int pageSize)
		{
			Name = name;
			Query = query;
			PageNumber = pageNumber < 1 ? 1 : pageNumber;
			PageSize = pageSize < 1 ? DefaultPageSize : pageSize;
			Sort = shortOptions ?? new SortOptions();
		}
		#endregion

		#region Properties

		public string Name { get; set; }

		public int TotalItems
		{
			get { return Query.Count(); }
		}

		public int TotalPages
		{
			get { return (int)Math.Ceiling(((double)TotalItems) / PageSize); }
		}

		public int FirstItem
		{
			get { return ((PageNumber - 1) * PageSize) + 1; }
		}

		public int LastItem
		{
			get
			{
				int o;
				var r = Math.DivRem(TotalItems, PageSize, out o);
				return FirstItem + (o == 0 ? PageSize : o) - 1;
			}
		}

		public bool HasPreviousPage
		{
			get { return PageNumber > 1; }
		}

		public bool HasNextPage
		{
			get { return PageNumber < TotalPages; }
		}

		public int PageSize { get; set; }

		public int PageNumber { get; set; }

		public SortOptions Sort 
		{
			get
			{
				return m_sortOption; 
			}
			set
			{
				if (m_sortOption != null && value != null)
				{
					if (!m_sortOption.SortBy.EqualsTo(value.SortBy) || m_sortOption.SortOrder != value.SortOrder)
					{
						m_needSort = true; 
					}
				}
				m_sortOption = value;
			}
		}

		public IQueryable<T> Query
		{
			get;
			private set;
		}

		public virtual IList<ItemDescription> GetDescriptions()
		{
			if (m_itemDesctiptions == null)
				m_itemDesctiptions = PaginationQueryManager.Get<T>().GetDescriptions(Query);

			return m_itemDesctiptions;
		}
		#endregion

		#region Methods

		IEnumerator IEnumerable.GetEnumerator()
		{
			return this.GetEnumerator();
		}

		public IEnumerator<T> GetEnumerator()
		{
			foreach (var item in ExecuteQuery())
			{
				yield return item;
			}
		}

		protected virtual IQueryable<T> ExecuteQuery()
		{
			int numberToSkip = (PageNumber - 1) * PageSize;

			if(m_needSort)
			{
				Query = PaginationQueryManager.Get<T>().OrderBy(Query, Sort);
				m_needSort = false;
			}
			return Query.Skip(numberToSkip).Take(PageSize);
		}

		#endregion

		#region Json
		public JsonObject ToJsonObject()
		{
			var rawJson = PaginationQueryManager.Get<T>().ToJsonRawObject(this);
			return new JsonPageData((PageNumber * PageSize) - PageSize, TotalItems, 0, null, rawJson, Sort.SortBy, Sort.SortOrder.ToString());
		}
		#endregion
	}
}