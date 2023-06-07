using App.Data.Caching;
using System;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Service.PartTracking
{
	public class V_GET_CUSTOMER_GROUP
	{
		private const string cacheName = "App.partTracking.V_CUSTOMER_GROUP";

		private readonly static ICacheManager _cacheManager = new MemoryCacheManager();

		public static List<Data.Domain.V_GET_CUSTOMER_GROUP> GetList()
		{
			string key = string.Format(cacheName);

			var list = _cacheManager.Get(key, () =>
			{
				using(var db = new Data.RepositoryFactory(new Data.EfDbContext()))
				{
					var tb = db.CreateRepository<Data.Domain.V_GET_CUSTOMER_GROUP>().Table.Select(e => e);
					return tb.ToList();
				}
			});

			return list;
		}


		public static IList<Data.Domain.V_GET_CUSTOMER_GROUP> GetList(string searchTerm)
		{
			var term = searchTerm != null ? searchTerm.Trim().ToLower() : "";

			using(var db = new Data.RepositoryFactory(new Data.EfDbContext()))
			{
				var data = db.DbContext.Database.SqlQuery<Data.Domain.V_GET_CUSTOMER_GROUP>("dbo.spGetCustomerGroupList @p0", searchTerm).ToList();
				data.Insert(0, new Data.Domain.V_GET_CUSTOMER_GROUP() { CUNM = "ALL" });
				return data;
			}
		}

		public static Data.Domain.V_GET_CUSTOMER_GROUP GetItem(string CUNO)
		{
			using(var db = new Data.RepositoryFactory(new Data.EfDbContext()))
			{
				string sql = "select CUNO,(CUNO + ' - ' + CUNM) AS CUNM,STCUNO as PRCUNO FROM [LIBJ21].CILNAME0 with(nolock) where CUNO={0}";
				//string sql = "select CUNO,(CUNO + ' - ' + CUNM) AS CUNM,STCUNO as PRCUNO FROM [EDW].[EDW_STG_DBS_DAILY].[LIBJ21].CILNAME0 with(nolock) where CUNO={0}";
				var data = db.DbContext.Database.SqlQuery<Data.Domain.V_GET_CUSTOMER_GROUP>(sql, CUNO).ToList();
				if(data.Count > 0)
					return data[0];
				else
					return new Data.Domain.V_GET_CUSTOMER_GROUP();
			}
		}
	}
}
