using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Web.Mvc;
using App.Framework.Mvc.UI;
using App.Framework.Mvc.UI.Sorting;
using System.Collections.Specialized;

namespace App.Framework.Mvc
{

	public partial class BaseController
	{
		public virtual PaginationFilterManager  Paginator
		{
			get;
			private set;
		}

		//Actualy we don't need this property
		public PaginationFilterManager PaginatorBoot
		{
			get;
			private set;
		}

		/// <summary>
		/// Belum dibuat factory atau configurasinya untuk extension
		/// </summary>
		/// <param name="configuration"></param>
		/// <param name="storageKey"></param>
		/// <returns></returns>
		public virtual PaginationFilterManager CreatePaginationManager(PaginationConfiguration configuration = null, string storageKey = null)
		{
			return new PaginationFilterManager(this, configuration, storageKey);
		}

		#region Pagination

		/// <summary>
		/// History class for PaginationFilter object which used for pagination manage  
		/// </summary>
		[Serializable]
		public class PaginationState 
		{
			public IPaginationFilter PaginationFilter { get; set; }
			public int PageNumber { get; set; }
			public int PageSize { get; set; }
			public SortOptions Sort { get; set; }

		}

		/// <summary>
		/// Configuration class which used for Pagination variable 
		/// </summary>
		public class PaginationConfiguration
		{
			public string Key { get; set; }
			public string Page { get; set; }
			public string SortBy { get; set; }
			public string SortOrder { get; set; }
			public string SortOrderDesc { get; set; }
			public string PageSize { get; set; }
		}

		public class PaginationFilterManager  
		{
			private PaginationFilterManager m_globalSessionManager = null;
			private IDictionary<string, PaginationState> m_storage = null;

			#region Ctors
			public PaginationFilterManager(BaseController controller2) : this(controller2, null, null) { }

			public PaginationFilterManager(BaseController controller2, PaginationConfiguration configuration) : this(controller2, configuration, null) { }

			public PaginationFilterManager(BaseController controller2, string storagekey) : this(controller2, null, storagekey) { }

			public PaginationFilterManager(BaseController controller2, PaginationConfiguration configuration, string storageKey)
			{
				Controller = controller2;
				Configuration = configuration != null ? configuration : CreateDefaultConfiguration();
				PaginationsKey = string.IsNullOrEmpty(storageKey) ? CreateDefaultPaginationStateKey(Controller.GetType().FullName) : CreateDefaultPaginationStateKey(storageKey);
			}
			#endregion

			#region Properties
			public PaginationConfiguration Configuration { get; private set; }

			public PaginationFilterManager Global
			{
				get
				{
					if (!this.PaginationsKey.EqualsTo(CreateDefaultPaginationStateKey(GlobalPaginationKey)))
					{
						if (m_globalSessionManager == null)
						{
							m_globalSessionManager = CreateGlobalPaginaton();
						}
						return m_globalSessionManager;
					}

					return this;
				}
			}

			protected virtual IDictionary<string, PaginationState> Storage
			{
				get
				{
					if (m_storage == null)
					{
						m_storage = Controller.Session[PaginationsKey] as IDictionary<string, PaginationState>;

						if (m_storage == null)
						{
							m_storage = new Dictionary<string, PaginationState>();
							Controller.Session[PaginationsKey] = m_storage;
						}
					}

					return m_storage;
				}
			}

			protected BaseController Controller { get; set; }

			protected string PaginationsKey { get; private set; }

			protected virtual string GlobalPaginationKey
			{
				get
				{
					return "paginationfiltermanager.global.session.key";
				}
			}
			#endregion

			#region DataTable
			public IData<DataRow> Manage(string key, Func<DataTable> factory, NameValueCollection parameters = null)
			{
				return Manage<object, DataRow>(key, o => factory().AsEnumerable().AsPagination(key), parameters/*, null*/);
			}

			public PaginationFilter<F, DataRow> Manage<F>(string key, Func<F, DataTable> factory, NameValueCollection parameters = null) where F : class, new()
			{
				return Manage<F, DataRow>(key, f => factory(f).AsEnumerable().AsPagination(key), parameters/*, null*/);
			}
			#endregion

			#region IEnumerable<T>
			public IData<I> Manage<I>(string key, Func<IEnumerable<I>> factory, NameValueCollection parameters = null)
				where I : class
			{
				return Manage<object, I>(key, o => factory().AsPagination<I>(key), parameters/*, null*/);
			}

			public PaginationFilter<F, I> Manage<F, I>(string key, Func<F, IEnumerable<I>> factory, NameValueCollection parameters = null) 
				where F : class, new()
				where I : class
			{
				return Manage<F, I>(key, f => factory(f).AsPagination<I>(key), parameters/*, null*/);
			}
			#endregion

			/// <summary>
			///Determines whether the Paginator contains an PaginationFilter<F, I> with the specified key. 
			/// </summary>
			/// <param name="key">The key of the PaginationFilter to search.</param>
			/// <returns>true if the Paginator contains an PaginationFilter<F, I> with the key; otherwise, false </returns>
			/// <exception cref="System.ArgumentNullException">key is null.</exception>  
			public bool Exists(string key)
			{
				return Storage.ContainsKey(key);
			}

			/// <summary>
			/// Gets the value of filter object from PaginationFilter<F,I> associated with the specified key.
			/// </summary>
			/// <typeparam name="F">The type of the elements of Filter</typeparam>
			/// <param name="key">The key of the PaginationFilter<F,I> to search which contains the filter object.</param>
			/// <returns>Filter object</returns>
			/// <exception cref="System.ArgumentNullException">key is null.</exception>
			public F GetFilter<F>(string key) where F : class, new()
			{
				var state = GetPaginationState(key);
				return state == null ? null : (F)state.PaginationFilter.Filter;
			}

			/// <summary>
			/// Gets PaginationFilter<F,I> object associated with the specified key.
			/// </summary>
			/// <typeparam name="F">The type of the elements of Filter</typeparam>
			///	<typeparam name="I">The type of the elements of IPagination<I></typeparam>
			/// <param name="key">The key of the PaginationFilter<F,I> to search.</param>
			/// <returns>Filter object</returns>
			/// <exception cref="System.ArgumentNullException">key is null.</exception>
			public PaginationFilter<F, I> Get<F, I>(string key) where F : class, new()
			{
				var state = GetPaginationState(key);
				return state == null ? null : (PaginationFilter<F, I>)state.PaginationFilter;
			}

			public F GetCreateFilter<F>(string key) where F : class, new()
			{
				F filter = GetFilter<F>(key);
				if (filter == null)
				{
					filter = Activator.CreateInstance<F>();
					Controller.UpdateModel(filter);
				}
				return filter;
			}

			/// <summary>
			/// Get IPagination<I> object from PaginationFilter<F,I> associated with the specified key.
			/// </summary>
			/// <typeparam name="I">The type of the elements of IPagination<I></typeparam>
			/// <param name="key">The key of the PaginationFilter<F,I> which contains IPagination<I> object.</param>
			/// <returns>App.Framework.Mvc.IPagination<I> with the specified key.</returns>
			/// <exception cref="System.ArgumentNullException">key is null.</exception> 
			public IPagination<I> GetPagination<I>(string key)
			{
				var result = GetPaginationState(key);
				return result == null ? null : (IPagination<I>)result.PaginationFilter.Pagination;
			}

			/// <summary>
			/// Remove all PaginationFilters
			/// </summary>
			public void Clear()
			{
				Storage.Clear();
			}

			/// <summary>
			/// Remove PaginationFilter with the specified key from the current controller
			/// </summary>
			/// <param name="key">The key of the PaginationFilter to remove.</param>
			/// <exception cref="System.ArgumentNullException">key is null.</exception> 
			public void Remove(string key)
			{
				if (Storage.ContainsKey(key)) Storage.Remove(key);
			}

			/// <summary>
			/// Remove Pagination from PaginationFilter object with the specified key
			/// </summary>
			/// <param name="key">The key of the PaginationFilter<F, I> which the IPagination object will be removed.</param>
			/// <exception cref="System.ArgumentNullException">key is null.</exception>
			public void RemovePagination(string key)
			{
				if (Storage.ContainsKey(key)) Storage[key].PaginationFilter.Pagination = null;
			}

			#region ManagePagination
			public virtual PaginationFilter<F, I> Manage<F, I>(string key, Func<F, IPagination<I>> factory, NameValueCollection parameters/*, string[] properties*/) where F : class, new()
			{
				parameters = parameters ?? GetDefaultParameters();

				var currentPagination = parameters[Configuration.Key];

				int pageNumber;

				if (!Int32.TryParse(parameters[Configuration.Page], out pageNumber))
				{
					pageNumber = 1;
				}

				var sortBy = parameters[Configuration.SortBy];
				SortOptions sortOptions = null;

				if (!string.IsNullOrEmpty(sortBy))
				{
					var sortDirection = SortDirection.Ascending;

					if (parameters[Configuration.SortOrder].StartsWithTo(Configuration.SortOrderDesc))
					{
						sortDirection = SortDirection.Descending;
					}

					sortOptions = new SortOptions { SortBy = sortBy, SortOrder = sortDirection };
				}

				var state = GetPaginationState(key).NullToDefault();
				var result = state.PaginationFilter as PaginationFilter<F, I>;

				if (string.IsNullOrEmpty(currentPagination) || key.EqualsTo(currentPagination))
				{
					int pageSize;

					if (!Int32.TryParse(parameters[Configuration.PageSize], out pageSize))
					{
						pageSize = 0;
					}

					if (result == null || result.Filter == null || result.Pagination == null)
					{
						var isRemovePaginationList = false;

						if (result == null)
						{
							result = new PaginationFilter<F, I>(false);
						}
						else
						{
							isRemovePaginationList = result.Pagination == null;

							if (isRemovePaginationList)
							{
								pageSize = state.PageSize;
								pageNumber = state.PageNumber;
								sortOptions = state.Sort;
							}
						}

						if (result.Filter == null)
						{
							result.Filter = Activator.CreateInstance<F>();
							//Controller.UpdateModel(result.Filter);
							UpdateModel(result.Filter); 
						}

						result.Pagination = factory(result.Filter);

						if (isRemovePaginationList)
						{
							result.Pagination.PageNumber = pageNumber;
							result.Pagination.Sort = sortOptions;
						}
					}
					else
					{

						if (pageNumber > 0)
						{
							result.Pagination.PageNumber = pageNumber;
						}

						if (sortOptions != null)
						{
							result.Pagination.Sort = sortOptions;
						}
					}

					if (pageSize != 0)
					{
						result.Pagination.PageSize = pageSize;
					}

					state.PaginationFilter = result;
					state.Sort = result.Pagination.Sort;
					state.PageSize = result.Pagination.PageSize;
					state.PageNumber = result.Pagination.PageNumber;
					SavePaginationState(key, state);
				}
				return result;
			}

			#endregion

			private NameValueCollection GetDefaultParameters()
			{
				if (Controller.Request.QueryString.Count > 0) return Controller.Request.QueryString;
				return Controller.Request.Form;
			}

			protected void UpdateModel<F>(F model) where F : class
			{
				Controller.UpdateModel(model); 
			}

			/// <summary>
			/// Create Key for a PaginationFilter 
			/// </summary>
			/// <param name="key">The key for a PaginationFilter object</param>
			/// <exception cref="System.ArgumentNullException">key is null.</exception>
			protected virtual string CreateDefaultPaginationStateKey(string key)
			{
				return "Pagination." + key.ValidateNullEmptyArg("Key is empty or null");
			}

			protected virtual PaginationFilterManager CreateGlobalPaginaton()
			{
				return Controller.CreatePaginationManager(Configuration, GlobalPaginationKey);
			}

			private PaginationConfiguration CreateDefaultConfiguration()
			{
				return new PaginationConfiguration { Key = "key", Page = "page", PageSize = "pagesize", SortBy = "sortby", SortOrder = "sortorder", SortOrderDesc = "desc" };
			}

			#region Get Set Pagination Key

			/// <summary>
			/// Gets the PaginationState with the specified key.
			/// </summary>
			/// <param name="key">The key of the PaginationState to get.</param>
			/// <returns>The PaginationState with the specified key.</returns>
			///<exception cref="System.ArgumentNullException">Key is null.</exception>
			protected PaginationState GetPaginationState(string key)
			{  
				return Storage.ContainsKey(key) ? Storage[key] : null;
			}

			protected void SavePaginationState(string key, PaginationState state)
			{
				Storage[key] = state;
			}

			#endregion

		}

		#endregion
	}
}
