
namespace App.Framework.Mvc.UI
{
	public static class PaginationQueryManager
	{
		/// <summary>
		/// Template Factory of IPaginationQuery<>
		/// </summary>
		static class InternalPaginationQueryProvider<T> where T : class
		{
			private static IPaginationQuery<T> s_paginationQuery = null;

			public static void SetQuery(IPaginationQuery<T> paginationQuery)
			{
				s_paginationQuery = paginationQuery;
			}

			public static IPaginationQuery<T> GetOrCreate()
			{
				if (s_paginationQuery == null)
				{
					s_paginationQuery = new DefaultPaginationQuery<T>();
				}

				return s_paginationQuery;
			}
		}

		static PaginationQueryManager()
		{
			Register(new DataRowPaginationQuery());
			Register(new ObjectPaginationQuery());
		}

		public static void Register<T>(IPaginationQuery<T> paginationQuery) where T : class
		{
			InternalPaginationQueryProvider<T>.SetQuery(paginationQuery);
		}

		public static IPaginationQuery<T> Get<T>() where T : class
		{
			return InternalPaginationQueryProvider<T>.GetOrCreate();
		}
	}
}
